Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions
Imports BioNovoGene.BioDeep.Chemistry
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports BioNovoGene.BioDeep.Chemoinformatics.Metabolite
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Text.Xml.Models
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports PlantToolKit
Imports registry_data
Imports registry_data.biocad_registryModel
Imports registry_exports
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.Motif
Imports SMRUCC.genomics.Assembly.KEGG
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.Uniprot.XML
Imports SMRUCC.genomics.ComponentModel.Annotation
Imports SMRUCC.genomics.Data
Imports SMRUCC.genomics.Data.BioCyc
Imports SMRUCC.genomics.Data.Regprecise
Imports SMRUCC.genomics.Data.Rhea
Imports SMRUCC.genomics.Data.SABIORK
Imports SMRUCC.genomics.Data.SABIORK.SBML
Imports SMRUCC.genomics.Data.SABIORK.TabularDump
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports sbXML = SMRUCC.genomics.Model.SBML.Level3.XmlFile(Of SMRUCC.genomics.Data.SABIORK.SBML.SBMLReaction)

<Package("registry")>
Public Module registry

    <ExportAPI("save_uniprot")>
    Public Function saveUniprot(registry As biocad_registry, <RRawVectorArgument> uniprot As Object,
                                Optional process_none_enzyme As Boolean = True,
                                Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of entry)(uniprot, env)

        If pull.isError Then
            Return pull.getError
        End If

        Call registry.importsUniProt(pull.populates(Of entry)(env), process_none_enzyme)

        Return Nothing
    End Function

    <ExportAPI("imports_struct_domains")>
    Public Function imports_struct_domains(registry As biocad_registry, <RRawVectorArgument> uniprot As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of entry)(uniprot, env)

        If pull.isError Then
            Return pull.getError
        End If

        Call registry.SaveInterProDomain(pull.populates(Of entry)(env))

        Return Nothing
    End Function

    <ExportAPI("save_genbank")>
    Public Function save_genbank(registry As biocad_registry,
                                 <RRawVectorArgument>
                                 genbank As Object,
                                 Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of GBFF.File)(genbank, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each gb_asm As GBFF.File In pull.populates(Of GBFF.File)(env)
            Call registry.SaveGenBank(gb_asm)
        Next

        Return Nothing
    End Function

    <ExportAPI("make_genbank_dbxrefs")>
    Public Function save_genbank_xrefs(registry As biocad_registry,
                                       <RRawVectorArgument>
                                       genbank As Object,
                                       Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of GBFF.File)(genbank, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each gb_asm As GBFF.File In pull.populates(Of GBFF.File)(env)
            Call registry.SaveDbXrefs(gb_asm)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_mona")>
    Public Function imports_mona(registry As biocad_registry, <RRawVectorArgument> mona As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of SpectraSection)(mona, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each chunk As SpectraSection() In pull.populates(Of SpectraSection)(env).SplitIterator(5000)
            Call MoNADatabase.MakeImports(registry, chunk)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_spectraverse")>
    Public Function imports_spectraverse(registry As biocad_registry, <RRawVectorArgument> spectraverse As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of spectraverse)(spectraverse, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each chunk As spectraverse() In pull.populates(Of spectraverse)(env).SplitIterator(5000)
            Call MoNADatabase.MakeImports(registry, chunk)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_pubchem")>
    Public Function imports_pubchem(registry As biocad_registry, <RRawVectorArgument> pubchem As Object,
                                    Optional skip_prefix As UInteger = 0,
                                    Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of PugViewRecord)(pubchem, env)

        If pull.isError Then
            Return pull.getError
        ElseIf skip_prefix > 0 Then
            Call VBDebugger.EchoLine($"all metabolite with int id prefix char value less than {skip_prefix} will be ignored.")
        End If

        Dim chunks = pull.populates(Of PugViewRecord)(env).Where(Function(c) Not c Is Nothing) _
            .Where(Function(c)
                       ' 20260211 id is order by string
                       ' example as 1 100 11000 1113333
                       ' so we just test the first char of the int id
                       Return Integer.Parse(c.RecordNumber.First) >= skip_prefix
                   End Function) _
            .SplitIterator(1000)
        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim db_pubchem As UInteger = vocabulary.db_pubchem

        For Each block As PugViewRecord() In chunks
            For Each meta As MetaInfo In TqdmWrapper.Wrap(block _
                .Select(Function(c)
                            Try
                                Return c.GetMetaInfo
                            Catch ex As Exception
                                Call App.LogException(ex)
                                Return Nothing
                            End Try
                        End Function) _
                .Where(Function(m) m IsNot Nothing) _
                .ToArray
            )
                ' 不信任pubchem id的映射结果，在这里直接设置kegg_id来避免直接通过pubchem id查找到结果
                Dim m As metabolites = registry.FindMolecule(meta, "kegg_id", nameSearch:=True)

                If m Is Nothing Then
                    Continue For
                End If

                Call registry.SaveDbLinks(meta, m, db_pubchem)
                Call registry.SaveStructureData(m, meta.xref.SMILES)
                Call registry.SaveSynonyms(m, meta.EnumerateAllNames, db_pubchem)
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_lotus")>
    Public Function imports_lotus(registry As biocad_registry, <RRawVectorArgument> lotus As Object, Optional env As Environment = Nothing)
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of LOTUS.NaturalProduct)(lotus, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim lotus_db As UInteger = registry.biocad_vocabulary.GetDatabaseResource("LOTUS NaturalProduct").id

        For Each block As LOTUS.NaturalProduct() In pull.populates(Of LOTUS.NaturalProduct)(env).SplitIterator(5000)
            For Each np In TqdmWrapper.Wrap(block)
                Dim metab As MetaLib = np.CreateMetabolite
                Dim m As metabolites = registry.FindMolecule(metab, "kegg_id", nameSearch:=True)

                If m Is Nothing Then
                    Continue For
                End If

                Call registry.SaveStructureData(m, metab.xref.SMILES)
                Call registry.SaveMetaboliteClass(m, lotus_db, (metab.super_class, metab.class, Nothing, Nothing), np.lotus_id)
                Call registry.SaveSynonyms(m, metab.EnumerateAllNames, lotus_db)
                Call registry.SaveDbLinks(metab, m, lotus_db, saveID:=True)
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_coconut")>
    Public Function imports_coconut(registry As biocad_registry, <RRawVectorArgument> coconut As Object, Optional env As Environment = Nothing)
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of Coconut.CoconutNPTable)(coconut, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim coconut_db As UInteger = registry.biocad_vocabulary.GetDatabaseResource("Coconut NaturalProduct").id

        For Each block As Coconut.CoconutNPTable() In pull.populates(Of Coconut.CoconutNPTable)(env).SplitIterator(5000)
            For Each np In TqdmWrapper.Wrap(block)
                Dim metab As MetaLib = np.GetMetaboliteData
                Dim m As metabolites = registry.FindMolecule(metab, "kegg_id", nameSearch:=True, preferNameSearch:=True)

                If m Is Nothing Then
                    Continue For
                End If

                Call registry.SaveStructureData(m, metab.xref.SMILES)
                Call registry.SaveMetaboliteClass(m, coconut_db, (metab.super_class, metab.class, Nothing, Nothing), np.identifier)
                Call registry.SaveSynonyms(m, metab.EnumerateAllNames, coconut_db)
                Call registry.SaveDbLinks(metab, m, coconut_db, saveID:=True)

                Dim source As CommitTransaction = registry.organism_source.ignore.open_transaction

                For Each taxname As String In np.organisms.SafeQuery
                    Dim tax = registry.GetTaxonomy(taxname)
                    Dim taxid As UInteger = If(tax Is Nothing, 0, tax.id)
                    Dim check = registry.organism_source _
                        .where(field("metabolite_id") = m.id,
                               field("taxname") = taxname,
                               field("evidence") = np.identifier) _
                        .find(Of biocad_registryModel.organism_source)

                    If check Is Nothing Then
                        Call source.ignore.add(
                            field("metabolite_id") = m.id,
                            field("organism_id") = taxid,
                            field("taxname") = taxname,
                            field("evidence") = np.identifier,
                            field("note") = taxname & " - " & np.np_classifier_pathway
                        )
                    End If
                Next

                Call source.commit()
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_kegg_reactions")>
    Public Function imports_kegg_reactions(registry As biocad_registry, <RRawVectorArgument> kegg As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of DBGET.bGetObject.Reaction)(kegg, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim models As SMRUCC.genomics.ComponentModel.EquaionModel.Reaction() = pull _
            .populates(Of DBGET.bGetObject.Reaction)(env) _
            .Where(Function(r) Not r.ID.StringEmpty(, True)) _
            .Select(Function(r)
                        Return SMRUCC.genomics.ComponentModel.EquaionModel.Reaction.FromKeggReaction(r)
                    End Function) _
            .ToArray

        Call registry.importsReactions(models, db_name:="kegg")

        Return Nothing
    End Function

    <ExportAPI("imports_rhea")>
    Public Function imports_rhea(registry As biocad_registry, <RRawVectorArgument> rhea As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of SMRUCC.genomics.ComponentModel.EquaionModel.Reaction)(rhea, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim models = pull.populates(Of SMRUCC.genomics.ComponentModel.EquaionModel.Reaction)(env).ToArray

        Call registry.importsReactions(models, db_name:="Rhea")

        Return Nothing
    End Function

    <ExportAPI("imports_metacyc_reactions")>
    Public Function imports_metacyc_reactions(registry As biocad_registry, metacyc As Workspace) As Object
        Call BioCycDatabase.ImportsMetaCycReactions(registry, metacyc)
        Return Nothing
    End Function

    <ExportAPI("imports_metacyc_compounds")>
    Public Function imports_metacyc(registry As biocad_registry, metacyc As Workspace) As Object
        Call BioCycDatabase.ImportsMetaCyc(registry, metacyc)
        Return Nothing
    End Function

    ''' <summary>
    ''' Save RegPrecise regulation network
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="genomes"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("save_TRN")>
    Public Function save_regprecise(registry As biocad_registry, <RRawVectorArgument> genomes As Object, Optional env As Environment = Nothing) As Object
        Dim database As pipeline = pipeline.TryCreatePipeline(Of BacteriaRegulome)(genomes, env)

        If database.isError Then
            Return database.getError
        End If

        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim db_regprecise As UInteger = vocabulary.GetDatabaseResource("RegPrecise").id
        Dim db_genbank As UInteger = vocabulary.db_genbank
        Dim TRN As CommitTransaction = registry.regulatory_network.ignore.open_transaction

        For Each genome As BacteriaRegulome In TqdmWrapper.Wrap(database.populates(Of BacteriaRegulome)(env).ToArray)
            Dim taxid As UInteger = genome.genome.taxonomyId

            For Each TF As SMRUCC.genomics.Data.Regprecise.Regulator In genome.regulome.AsEnumerable
                Dim motifPlaceholder As motif = registry.motif.where(field("name") = TF.regulog.name).find(Of motif)("id")

                If motifPlaceholder Is Nothing Then
                    Continue For
                End If

                Dim effects As Double = 1

                If Not TF.regulationMode.StringEmpty Then
                    If TF.regulationMode.StartsWith("repressor") Then
                        If Strings.InStr(TF.regulationMode, "activator") Then
                            effects = -0.5
                        Else
                            effects = -1
                        End If
                    Else
                        If Strings.InStr(TF.regulationMode, "repressor") Then
                            effects = 0.5
                        Else
                            effects = 1
                        End If
                    End If
                End If

                For Each tag As NamedValue In TF.locus_tags.SafeQuery
                    Dim protTF As protein_data = registry.protein_data _
                        .where(field("source_id") = tag.name,
                               field("source_db") = db_genbank,
                               field("ncbi_taxid") = taxid) _
                        .find(Of protein_data)("id", "ncbi_taxid")

                    If protTF IsNot Nothing Then
                        Call TRN.add(
                            field("regulator") = protTF.id,
                            field("motif_site") = motifPlaceholder.id,
                            field("effector_name") = TF.effector,
                            field("effector") = 0,
                            field("effects") = effects,
                            field("db_source") = db_regprecise,
                            field("note") = TF.regulationMode & " - " & TF.biological_process.JoinBy("; ")
                        )
                    End If
                Next
            Next
        Next

        Call TRN.commit()

        Return Nothing
    End Function

    <ExportAPI("update_logo")>
    Public Function update_logo(registry As biocad_registry) As Object
        For Each model As motif In TqdmWrapper.Wrap(registry.motif.select(Of motif)("id", "name"))
            Dim sites = registry.nucleotide_data _
                .where(field("is_motif") <> 0,
                       field("model_id") = model.id) _
                .select(Of nucleotide_data)("source_id", "sequence")
            Dim fq As FastaSeq() = sites.Select(Function(si) New FastaSeq(si.sequence, si.source_id)).ToArray

            If fq.Length < 3 Then
                Continue For
            End If

            Dim gibbs As New GibbsSampler(fq, fq.Average(Function(si) si.Length) * 0.85)
            Dim motifdata As MSAMotif = gibbs.find
            Dim motif As MotifPWM = motifdata.CreateMotif

            Call registry.UpdateLogo(model, motif)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_sbml_reactions")>
    Public Function imports_sbml_reactions(registry As biocad_registry, sbml As sbXML, Optional env As Environment = Nothing)
        ' imports compound at first
        For Each sp In sbml.model.listOfSpecies

        Next

        ' imports reaction data
        For Each rxn As SBMLReaction In sbml.model.listOfReactions.AsEnumerable

        Next

        Return Nothing
    End Function

    Public Iterator Function ParseMetaIDs(json As String) As IEnumerable(Of UInteger)
        Dim args As Dictionary(Of String, String) = json.LoadJSON(Of Dictionary(Of String, String))

        For Each val As String In args.Values
            Dim prefix = val.Match("\d+ [-] ")

            If prefix <> "" AndAlso val.StartsWith(prefix) Then
                Yield UInteger.Parse(val.Split("-"c).First.Trim)
            End If
        Next
    End Function

    <Extension>
    Public Function MountReactionModel(registry As biocad_registry, rxn As (KEGGReactionId$, Ec_number$), substrates As UInteger()) As biocad_registryModel.reaction
        Dim kegg_rxn As String = rxn.KEGGReactionId
        Dim reaction As biocad_registryModel.reaction = Nothing
        Dim left_role As UInteger = registry.MetabolicSubstrateRole
        Dim right_role As UInteger = registry.MetabolicProductRole
        Dim kegg_db As UInteger = registry.biocad_vocabulary.db_kegg

        If Not kegg_rxn.StringEmpty Then
            reaction = registry.reaction _
                .where(field("db_xref") = kegg_rxn,
                       field("db_source") = kegg_db) _
                .find(Of biocad_registryModel.reaction)
        End If
        If reaction Is Nothing AndAlso Not substrates.IsNullOrEmpty Then
            Dim reactions = registry.metabolic_network _
                .left_join("reaction") _
                .on(field("`reaction`.id") = field("`metabolic_network`.reaction_id")) _
                .where(field("role") = left_role,
                       field("species_id").in(substrates)) _
                .select(Of biocad_registryModel.reaction)("reaction.*")
            Dim ec As ECNumber = ECNumber.ValueParser(rxn.Ec_number)

            If reactions.Any Then
                Dim enzyme_groups = reactions.GroupBy(Function(a) If(a.ec_number, "")).ToArray
                Dim top_group = enzyme_groups _
                    .OrderByDescending(Function(a)
                                           Dim rxn_ec As ECNumber = ECNumber.ValueParser(a.Key)

                                           If ec IsNot Nothing AndAlso rxn_ec IsNot Nothing Then
                                               Return ECNumber.MatchScore(rxn_ec, ec)
                                           End If

                                           Return 0
                                       End Function) _
                    .First

                reaction = top_group.First
            End If
        End If

        Return reaction
    End Function

    <ExportAPI("update_kinetics_registry")>
    Public Function update_kinetics_registry(registry As biocad_registry)
        Dim page_size As Integer = 1000
        Dim sabiork As UInteger = registry.biocad_vocabulary.GetDatabaseResource("SABIO-RK").id

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data = registry.kinetics_law.limit(offset, page_size).select(Of kinetics_law)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each law As kinetics_law In TqdmWrapper.Wrap(page_data)
                Dim rxn As EnzymeCatalystKineticLaw = JsonParser.Parse(law.raw).CreateObject(Of EnzymeCatalystKineticLaw)
                Dim kinetics_id = rxn.SabiorkId
                Dim find_law = registry.kinetics_law _
                      .where(field("db_xref") = kinetics_id,
                             field("db_source") = sabiork) _
                      .find(Of biocad_registryModel.kinetics_law)

                If find_law Is Nothing Then
                    Continue For
                End If

                Dim subsList As Dictionary(Of String, NamedVector(Of String)) = rxn.substrates
                Dim prodList As Dictionary(Of String, NamedVector(Of String)) = rxn.products
                Dim args As String = registry.arguments_json((rxn.parameters, rxn.uniprot_id, rxn.enzyme, subsList, prodList))
                Dim reaction = registry.MountReactionModel((rxn.KEGGReactionId, rxn.Ec_number), ParseMetaIDs(args).ToArray)

                Call registry.kinetics_law.where(field("id") = law.id).save(
                     field("parameters") = args,
                     field("metabolic_node") = If(reaction Is Nothing, 0, reaction.id)
                )
            Next
        Next

        Return Nothing
    End Function

    <Extension>
    Private Function arguments_json(registry As biocad_registry,
                                    rxn As (
                                        parameters As Dictionary(Of String, String),
                                        uniprot_id As String(),
                                        enzyme As Dictionary(Of String, String),
                                        substrates As Dictionary(Of String, NamedVector(Of String)),
                                        products As Dictionary(Of String, NamedVector(Of String)))) As String

        Dim args As New Dictionary(Of String, String)(rxn.parameters)
        Dim uniprot_id = rxn.uniprot_id
        Dim enzymes = rxn.enzyme
        Dim left = registry.IndexMetabolites(rxn.substrates)
        Dim right = registry.IndexMetabolites(rxn.products)

        For Each id As String In args.Keys
            Dim key As String = args(id)

            If Left.ContainsKey(key) AndAlso Left(key) IsNot Nothing Then
                args(id) = Left(key).id & " - " & Left(key).name
            End If

            If enzymes.ContainsKey(key) Then
                args(id) = $"enzyme - {uniprot_id.DefaultFirst}"
            End If
        Next

        Return args.GetJson(simpleDict:=True)
    End Function

    <Extension>
    Private Function IndexMetabolites(registry As biocad_registry, list As Dictionary(Of String, NamedVector(Of String))) As Dictionary(Of String, metabolites)
        Return list _
            .ToDictionary(Function(a) a.Key,
                          Function(a)
                              Return registry.FindSymbol(a.Value.name, a.Value.vector)
                          End Function)
    End Function

    <ExportAPI("imports_sabiork")>
    Public Function imports_enzyme_kinetics(registry As biocad_registry, <RRawVectorArgument> xmlfiles As Object,
                                            Optional updates As Boolean = False,
                                            Optional env As Environment = Nothing) As Object

        Dim sabiork As UInteger = registry.biocad_vocabulary.GetDatabaseResource("SABIO-RK").id

        For Each block As String() In CLRVector.asCharacter(xmlfiles).SplitIterator(1000)
            For Each file As String In TqdmWrapper.Wrap(block)
                Dim doc As SbmlDocument = SbmlDocument.LoadDocument(file)

                If doc Is Nothing OrElse doc.empty Then
                    Continue For
                End If

                Dim dml As CommitTransaction = registry.kinetics_law.open_transaction

                For Each rxn As EnzymeCatalystKineticLaw In ModelHelper.CreateKineticsData(doc).Select(Function(r) r.Item2)
                    Dim kinetics_id As String = rxn.SabiorkId
                    Dim find_law As kinetics_law = registry.kinetics_law _
                        .where(field("db_xref") = kinetics_id,
                               field("db_source") = sabiork) _
                        .find(Of biocad_registryModel.kinetics_law)

                    If Not find_law Is Nothing Then
                        If Not updates Then
                            Continue For
                        End If
                    End If

                    Dim args As String = registry.arguments_json((rxn.parameters, rxn.uniprot_id, rxn.enzyme, rxn.substrates, rxn.products))
                    Dim uniprot_id = rxn.uniprot_id
                    Dim enzymes = rxn.enzyme
                    Dim reaction = registry.MountReactionModel((rxn.KEGGReactionId, rxn.Ec_number), ParseMetaIDs(args).ToArray)

                    If find_law IsNot Nothing AndAlso updates Then
                        Call dml.add(registry.kinetics_law.where(field("id") = find_law.id).save_sql(
                            field("ec_number") = rxn.Ec_number,
                            field("enzyme_id") = If(uniprot_id.IsEmptyStringVector(True), "-", $"uniprot:{uniprot_id.JoinBy(",")}"),
                            field("enzyme_name") = If(enzymes.IsNullOrEmpty, "-", enzymes.First.Value),
                            field("lambda") = rxn.lambda,
                            field("parameters") = args,
                            field("metabolic_node") = If(reaction Is Nothing, 0, reaction.id),
                            field("buffer") = rxn.buffer,
                            field("ph") = rxn.PH,
                            field("temperature") = rxn.temperature,
                            field("pubmed_id") = If(rxn.PubMed.DefaultFirst, "-"),
                            field("pdb_data") = 0,
                            field("note") = rxn.reaction,
                            field("raw") = rxn.GetJson(simpleDict:=True)
                        ))
                    Else
                        registry.kinetics_law.add(
                            field("db_xref") = kinetics_id,
                            field("db_source") = sabiork,
                            field("ec_number") = rxn.Ec_number,
                            field("enzyme_id") = If(uniprot_id.IsEmptyStringVector(True), "-", $"uniprot:{uniprot_id.JoinBy(",")}"),
                            field("enzyme_name") = If(enzymes.IsNullOrEmpty, "-", enzymes.First.Value),
                            field("lambda") = rxn.lambda,
                            field("parameters") = args,
                            field("metabolic_node") = If(reaction Is Nothing, 0, reaction.id),
                            field("buffer") = rxn.buffer,
                            field("ph") = rxn.PH,
                            field("temperature") = rxn.temperature,
                            field("pubmed_id") = If(rxn.PubMed.DefaultFirst, "-"),
                            field("pdb_data") = 0,
                            field("note") = rxn.reaction,
                            field("raw") = rxn.GetJson(simpleDict:=True)
                        )
                    End If
                Next

                Call dml.commit()
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("import_brenda_enzymes")>
    Public Function import_brenda_enzymes(registry As biocad_registry, <RRawVectorArgument> brenda As Object, Optional env As Environment = Nothing)
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of BrendaEnzymeData)(brenda, env)

        If pull.isError Then
            Return pull.getError
        End If

        Return registry.MakeImports(pull.populates(Of BrendaEnzymeData)(env))
    End Function

    <ExportAPI("import_refseq")>
    Public Function import_refseq(registry As biocad_registry, <RRawVectorArgument> refseq As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of GenBankAssemblyIndex)(refseq, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim trans As CommitTransaction = registry.refseq.ignore.open_transaction

        For Each seq As GenBankAssemblyIndex In pull.populates(Of GenBankAssemblyIndex)(env)
            Dim asm_id As String = seq.assembly_accession.Split("."c).First
            Dim check = registry.refseq.where(field("assembly_accession") = asm_id).find(Of biocad_registryModel.refseq)

            If check Is Nothing Then
                Call trans.add(
                    field("assembly_accession") = asm_id,
                    field("assembly_level") = seq.assembly_level,
                    field("bioproject") = seq.bioproject,
                    field("biosample") = seq.biosample,
                    field("group") = seq.group,
                    field("taxid") = seq.taxid,
                    field("species_taxid") = seq.species_taxid,
                    field("organism_name") = seq.organism_name,
                    field("infraspecific_name") = seq.infraspecific_name,
                    field("ftp_path") = seq.ftp_path
                )
            End If
        Next

        Call trans.commit()

        Return Nothing
    End Function

    <ExportAPI("ontology_translation")>
    Public Sub OntologyTranslation(registry As biocad_registry, ontology As String)
        Call Translations.TranslateOntology(registry, ontology)
    End Sub

    <ExportAPI("metabolite_translation")>
    Public Sub MetaboliteTranslation(registry As biocad_registry, ontology As String)
        Call Translations.TranslateMetaboliteName(registry, ontology)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="motifs"></param>
    ''' <param name="tf">PlantTFDB TF fasta sequence collection</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("imports_planttfdb")>
    Public Function imports_planttfdb(registry As biocad_registry,
                                      <RRawVectorArgument> motifs As Object,
                                      <RRawVectorArgument> tf As Object,
                                      Optional taxname As String = Nothing,
                                      Optional env As Environment = Nothing)

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of MotifPWM)(motifs, env)
        Dim tfPull As IEnumerable(Of FastaSeq) = pipHelper.GetFastaSeq(tf, env)

        If pull.isError Then
            Return pull.getError
        ElseIf tfPull Is Nothing Then
            Return Nothing
        End If

        Dim planttfdb As UInteger = registry.biocad_vocabulary.GetDatabaseResource("PlantTFDB").id
        Dim tfSet = tfPull _
            .Select(Function(fa) (fa, tf:=New TFInfo(fa.Title))) _
            .GroupBy(Function(a) a.tf.gene_id) _
            .ToDictionary(Function(a)
                              Return a.Key
                          End Function,
                          Function(grp)
                              Return grp.First
                          End Function)
        Dim tax As biocad_registryModel.ncbi_taxonomy = Nothing

        If Not taxname.StringEmpty Then
            tax = registry.GetTaxonomy(taxname)
        End If

        For Each seq In TqdmWrapper.Wrap(tfSet.Values)
            Dim prot = registry.protein_data.where(field("source_id") = seq.tf.protein_id, field("source_db") = planttfdb).find(Of protein_data)

            If prot Is Nothing Then
                registry.protein_data.add(
                    field("source_id") = seq.tf.protein_id,
                    field("source_db") = planttfdb,
                    field("name") = seq.tf.family,
                    field("function") = seq.tf.description,
                    field("gene_id") = 0,
                    field("cluster_id") = 0,
                    field("protein_id") = 0,
                    field("ncbi_taxid") = If(tax, registry.GetTaxonomy(seq.tf.species)).id,
                    field("sequence") = seq.fa.SequenceData,
                    field("checksum") = Strings.UCase(seq.fa.SequenceData).MD5,
                    field("pdb_data") = 0
                )
                prot = registry.protein_data _
                    .where(field("source_id") = seq.tf.protein_id,
                           field("source_db") = planttfdb) _
                    .order_by("id", desc:=True) _
                    .find(Of protein_data)
            End If

            seq.fa.Headers = New String() {prot.id}
        Next

        Const suffix_pattern As String = "((_g)|(\.g))$"

        For Each motif As MotifPWM In pull.populates(Of MotifPWM)(env)
            Dim tf_id As String = Regex.Replace(motif.name.Split.ElementAt(1), suffix_pattern, String.Empty)
            Dim matrix_id As String = motif.name.Split.ElementAt(2)
            Dim reg_tf = tfSet(tf_id)
            Dim regulon = matrix_id & " - " & reg_tf.tf.species
            Dim model As motif = registry.motif _
                .where(field("name") = regulon) _
                .find(Of motif)

            If model Is Nothing Then
                registry.motif.add(
                    field("name") = regulon,
                    field("family") = reg_tf.tf.family,
                    field("pwm") = "",
                    field("width") = 0,
                    field("note") = reg_tf.ToString & vbCrLf & vbCrLf & motif.note
                )
                model = registry.motif _
                    .where(field("name") = regulon) _
                    .order_by("id", desc:=True) _
                    .find(Of motif)
            End If

            If Not model Is Nothing Then
                Dim site_pattern As String = motif.site_pattern
                Dim site = registry.nucleotide_data _
                    .where(field("source_id") = tf_id & ":" & matrix_id,
                           field("source_db") = planttfdb) _
                    .find(Of nucleotide_data)

                If site Is Nothing Then
                    registry.nucleotide_data.add(
                       field("source_id") = tf_id & ":" & matrix_id,
                           field("source_db") = planttfdb,
                           field("name") = reg_tf.tf.family,
                           field("is_motif") = 1,
                           field("left") = 0,
                           field("strand") = "+",
                           field("operon_id") = 0,
                           field("model_id") = model.id,
                           field("organism_source") = If(tax, registry.GetTaxonomy(reg_tf.tf.species)).id,
                           field("sequence") = site_pattern,
                           field("checksum") = Strings.UCase(site_pattern).MD5
                   )
                    site = registry.nucleotide_data _
                        .where(field("source_id") = tf_id & ":" & matrix_id,
                               field("source_db") = planttfdb) _
                        .order_by("id", desc:=True) _
                        .find(Of nucleotide_data)
                End If

                If registry.regulatory_network _
                    .where(field("regulator") = reg_tf.fa.Headers(0),
                           field("motif_site") = model.id) _
                    .find(Of regulatory_network) Is Nothing Then

                    Call registry.regulatory_network.add(
                        field("regulator") = reg_tf.fa.Headers(0),
                           field("motif_site") = model.id,
                           field("effector_name") = "",
                           field("effector") = 0,
                           field("effects") = 1,
                           field("db_source") = planttfdb,
                           field("note") = reg_tf.tf.ToString & " - " & matrix_id
                    )
                End If

                model.name = reg_tf.tf.family
                registry.UpdateLogo(model, motif)
            End If
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_chineseName")>
    Public Function imports_chineseName(registry As biocad_registry,
                                        <RRawVectorArgument(TypeCodes.integer)> id As Object,
                                        <RRawVectorArgument(TypeCodes.string)> names As Object,
                                        Optional env As Environment = Nothing)

        Dim uid As Long() = CLRVector.asLong(id)
        Dim zh_names As String() = CLRVector.asCharacter(names)
        Dim updates As CommitTransaction = registry.metabolites.open_transaction

        For i As Integer = 0 To uid.Length - 1
            Dim m = registry.metabolites.where(field("id") = uid(i)).find(Of metabolites)

            If Not m Is Nothing Then
                If m.name_zh.StringEmpty(True, True) Then
                    Call updates.add(registry.metabolites.where(field("id") = uid(i)).save_sql(field("name_zh") = zh_names(i)))
                End If
            End If
        Next

        Call updates.commit()

        Return Nothing
    End Function

    ''' <summary>
    ''' a general function for make metabolites data imports
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="metab"></param>
    ''' <param name="db_name"></param>
    ''' <param name="check_id"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("imports_metab_repo")>
    <RApiReturn(TypeCodes.string)>
    Public Function imports_metab_repo(registry As biocad_registry, <RRawVectorArgument> metab As Object, db_name As String,
                                       Optional check_id As Boolean = True,
                                       Optional env As Environment = Nothing) As Object

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of MetaInfo)(metab, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim db_key As UInteger = registry.biocad_vocabulary.GetDatabaseResource(db_name)
        Dim mapping As list = list.empty

        For Each batch As MetaInfo() In pull.populates(Of MetaInfo)(env).SplitIterator(1000)
            Dim trans As CommitTransaction = registry.metabolites.open_transaction

            For Each meta As MetaInfo In batch
                Try
                    If check_id Then
                        Dim check_xref = registry.db_xrefs _
                            .where(field("type") = registry.biocad_vocabulary.metabolite_type,
                                   field("db_name") = db_key,
                                   field("db_xref") = meta.ID) _
                            .find(Of db_xrefs)

                        If check_xref IsNot Nothing Then
                            mapping.add(meta.ID, "BioCAD" & check_xref.obj_id.ToString.PadLeft(11, "0"))
                            Continue For
                        End If
                    End If

                    Dim m As metabolites = registry.FindMolecule(meta,
                                                                 primaryKey:=Nothing,
                                                                 nameSearch:=True,
                                                                 preferNameSearch:=True,
                                                                 source_db:=db_key)

                    Call registry.SaveStructureData(m, meta.xref.SMILES, commit:=trans)
                    Call registry.SaveSynonyms(m, meta.synonym.JoinIterates({meta.name, meta.IUPACName}).Distinct, db_key, trans:=trans)
                    Call registry.SaveDbLinks(meta, m, db_key, saveID:=True, trans:=trans)
                    Call mapping.add(meta.ID, "BioCAD" & m.id.ToString.PadLeft(11, "0"))
                Catch ex As Exception

                End Try
            Next

            Call trans.commit()
        Next

        Return mapping
    End Function
End Module
