Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Model
Imports SMRUCC.genomics.Interops.NBCR.MEME_Suite
Imports SMRUCC.genomics.SequenceModel.FASTA

''' <summary>
''' Make exports of the component models for run virtual cell annotations
''' </summary>
Public Class ExportVirtualCellModels

    ReadOnly registry As biocad_registry
    ReadOnly repo As String
    ReadOnly vocabulary As biocad_vocabulary
    ReadOnly model As ProteinModel

    Sub New(registry As biocad_registry, repo As String)
        Me.vocabulary = registry.biocad_vocabulary
        Me.repo = repo
        Me.registry = registry
        Me.model = New ProteinModel(registry)
    End Sub

    Public Sub ExportLocations()
        Dim locs = registry.registry_resolver _
            .where(field("type") = vocabulary.GetRegistryEntity(biocad_vocabulary.EntitySubcellularLocation).id) _
            .select(Of registry_resolver)
        Dim models As WebJSON.CellularLocation() = locs _
            .SafeQuery _
            .Select(Function(cc)
                        Return New WebJSON.CellularLocation With {
                            .id = cc.symbol_id,
                            .symbol = cc.register_name
                        }
                    End Function) _
            .ToArray

        Call models.GetJson.SaveTo($"{repo}/subcellular.json")
    End Sub

    Public Sub ExportEnzymeDb()
        Using text As New StreamWriter($"{repo}/ec_numbers.fasta".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportEnzyme(model), filterEmpty:=True)
        End Using
    End Sub

    Public Sub ExportSubcellularLocationDb()
        Using text As New StreamWriter($"{repo}/subcellular.fasta".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportCellularLocation, filterEmpty:=True)
        End Using
    End Sub

    Public Sub ExportMoleculeData()
        Using jsonl As New System.IO.StreamReader($"{repo}/metabolic_network.jsonl".Open(System.IO.FileMode.Open, doClear:=False, [readOnly]:=True))
            Dim json_str As Value(Of String) = ""
            Dim mols As New Dictionary(Of String, metabolites)

            Using molecules As New System.IO.StreamWriter($"{repo}/molecules.jsonl".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
                Do While (json_str = jsonl.ReadLine) IsNot Nothing
                    Dim model As WebJSON.Reaction = json_str.LoadJSON(Of WebJSON.Reaction)

                    For Each cid As UInteger In From c As WebJSON.Substrate
                                                In model.left.JoinIterates(model.right)
                                                Select c.molecule_id
                                                Distinct

                        Call mols.ComputeIfAbsent(cid.ToString, Function(id) MakeQueryMetabolite(id, molecules))
                    Next
                Loop
            End Using

            ' export table
            Call mols.Values.SaveTo($"{repo}/molecules.csv")
        End Using
    End Sub

    Private Function MakeQueryMetabolite(id As UInteger, jsonl As System.IO.StreamWriter) As metabolites
        Dim metabolite_type As UInteger = vocabulary.metabolite_type
        Dim meta As metabolites = registry.metabolites.where(field("id") = id).find(Of metabolites)

        If meta Is Nothing Then
            Return Nothing
        End If

        Dim xrefs As New List(Of WebJSON.DBXref)
        Dim symbol As registry_resolver = registry.registry_resolver _
            .where(field("symbol_id") = meta.id,
                   field("type") = metabolite_type) _
            .find(Of registry_resolver)

        If meta.pubchem_cid > 0 Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "PubChem", .xref_id = "PubChem:" & meta.pubchem_cid})
        If meta.chebi_id > 0 Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "ChEBI", .xref_id = "ChEBI:" & meta.chebi_id})

        If Not meta.cas_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "CAS", .xref_id = meta.cas_id})
        If Not meta.kegg_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "KEGG", .xref_id = meta.kegg_id})
        If Not meta.hmdb_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "HMDB", .xref_id = meta.hmdb_id})
        If Not meta.lipidmaps_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "LipidMaps", .xref_id = meta.lipidmaps_id})
        If Not meta.biocyc.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "BioCyc", .xref_id = meta.biocyc})
        If Not meta.mesh_id.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "MeSH", .xref_id = meta.mesh_id})
        If Not meta.wikipedia.StringEmpty(, True) Then Call xrefs.Add(New WebJSON.DBXref With {.dbname = "Wikipedia", .xref_id = meta.wikipedia})

        Dim model As New WebJSON.Molecule With {
            .id = "BioCAD" & id.ToString.PadLeft(11, "0"c),
            .name = meta.name,
            .formula = meta.formula,
            .db_xrefs = xrefs.ToArray,
            .symbol = If(symbol Is Nothing, Nothing, symbol.register_name)
        }

        meta.note = model.symbol

        Call jsonl.WriteLine(model.GetJson)

        Return meta
    End Function

    Public Sub ExportReactionPool()
        ' load all metabolic reaction hash key
        Dim topology_keys As String() = registry.reaction _
            .where(field("hashcode").char_length > 0, field("topology_key").char_length > 0) _
            .distinct _
            .project(Of String)("topology_key")
        Dim role_left As UInteger = registry.MetabolicSubstrateRole.id
        Dim role_right As UInteger = registry.MetabolicProductRole.id
        Dim ec_type As UInteger = vocabulary.db_ECNumber
        Dim reaction_type As UInteger = vocabulary.reaction_type

        Using json As New System.IO.StreamWriter($"{repo}/metabolic_network.jsonl".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            For Each topo_key As String In TqdmWrapper.Wrap(topology_keys)
                Dim rxnSet As reaction() = registry.reaction _
                    .where(field("topology_key") = topo_key) _
                    .select(Of reaction)
                Dim enzymatic As reaction() = rxnSet.Where(Function(r) Not r.ec_number.StringEmpty(, True)).ToArray
                Dim rxn As reaction = enzymatic.FirstOrDefault
                Dim db_xrefs As String()

                ' this network node is a non-enzymatic reaction
                If rxn Is Nothing Then
                    ' non-enzymatic reaction template
                    rxn = rxnSet.First
                    db_xrefs = rxnSet _
                        .Select(Function(r) r.db_xref) _
                        .Distinct _
                        .ToArray
                Else
                    db_xrefs = enzymatic _
                        .Select(Function(r) r.db_xref) _
                        .Distinct _
                        .ToArray
                End If

                ' get substrate/products list
                Dim species As metabolic_network() = registry.metabolic_network _
                    .where(field("reaction_id") = rxn.id,
                           field("role").in({role_left, role_right})) _
                    .select(Of metabolic_network)
                Dim left = castModel(From c As metabolic_network In species Where c.role = role_left).ToArray
                Dim right = castModel(From c As metabolic_network In species Where c.role = role_right).ToArray

                ' 20260415 skip any gapped reaction
                If left.IsNullOrEmpty OrElse
                    left.Any(Function(a) a.molecule_id = 0) OrElse
                    right.IsNullOrEmpty OrElse
                    right.Any(Function(a) a.molecule_id = 0) Then

                    Continue For
                End If

                ' get ec number from the db_xrefs table
                Dim ec_number As String() = registry.db_xrefs _
                    .where(field("type") = reaction_type,
                           field("db_name") = ec_type,
                           field("obj_id") = rxn.id) _
                    .distinct _
                    .project(Of String)("db_xref")

                ec_number = ec_number _
                    .JoinMany(enzymatic _
                        .Select(Function(r)
                                    Return r.ec_number.StringSplit("\s*[,;]\s*", trimTrailingEmptyStrings:=True)
                                End Function)) _
                    .Distinct _
                    .ToArray

                Dim model As New WebJSON.Reaction With {
                    .guid = topo_key,
                    .name = rxn.name,
                    .reaction = rxn.equation,
                    .left = left,
                    .right = right,
                    .law = laws(ec_number, .left).ToArray,
                    .db_xrefs = db_xrefs
                }

                Call json.WriteLine(model.GetJson)
            Next
        End Using
    End Sub

    Private Iterator Function laws(ec_number As IEnumerable(Of String), left As WebJSON.Substrate()) As IEnumerable(Of WebJSON.LawData)
        For Each ec As String In ec_number.SafeQuery
            Yield New WebJSON.LawData With {
                .metabolite_id = "BioCAD" & left(0).molecule_id.ToString.PadLeft(11, "0"c),
                .lambda = "(Vmax * S)/(Km + S)",
                .params = New Dictionary(Of String, String) From {
                    {"Km", 1},
                    {"Vmax", 100},
                    {"S", .metabolite_id}
                },
                .ec_number = ec
            }
        Next
    End Function

    Private Iterator Function castModel(nodes As IEnumerable(Of metabolic_network)) As IEnumerable(Of WebJSON.Substrate)
        For Each c As metabolic_network In nodes _
            .GroupBy(Function(v) v.species_id) _
            .Select(Function(v) v.First)

            Yield New WebJSON.Substrate With {
                .factor = c.factor,
                .location = c.compartment_id,
                .molecule_id = c.species_id
            }
        Next
    End Function

    ReadOnly networkOrder As New NetworkByteOrderBuffer

    Private Function ParseMotif(motif As motif) As Probability
        Dim alphabets As String = If(motif.alphabets.StringEmpty(, True), "ACGT", motif.alphabets)
        Dim vec As Double()() = networkOrder _
            .ParseDouble(motif.pwm, zip:=NetworkByteOrderBuffer.Compression.none) _
            .SplitIterator(alphabets.Length) _
            .ToArray
        Dim bg As Dictionary(Of String, Double) = Nothing

        If motif.background.StringEmpty(, True) Then
            bg = New Dictionary(Of String, Double) From {
                {"A", 0.25},
                {"C", 0.25},
                {"G", 0.25},
                {"T", 0.25}
            }
        Else
            Dim bg_vec As Double() = motif.background.LoadJSON(Of Double())
            bg = New Dictionary(Of String, Double)

            For i As Integer = 0 To alphabets.Length - 1
                bg.Add(alphabets(i).ToString, bg_vec(i))
            Next
        End If

        Dim pwm As New Probability With {
            .name = $"PWM{motif.id.ToHexString} {motif.family} [{motif.name}]",
            .region = vec _
                .Select(Function(r, i)
                            Return New Residue(r, alphabets, i)
                        End Function) _
                .ToArray,
            .pvalue = 0.05,
            .score = 1,
            .background = bg
        }

        Return pwm
    End Function

    Public Sub ExportMotifSites()
        Dim dir As String = $"{repo}/motifs/"
        Dim page_size As Integer = 100
        Dim domain_motifs As New Dictionary(Of String, Dictionary(Of String, List(Of Probability)))

        For page As Integer = 1 To Integer.MaxValue
            Dim motif_data As motif() = registry.motif _
                .where(field("width") > 0) _
                .limit((page - 1) * page_size, page_size) _
                .select(Of motif)("id", "name", "family", "pwm", "width", "domain", "alphabets", "background")

            If motif_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each motif As motif In motif_data
                Dim pwm As Probability = ParseMotif(motif)

                If Not domain_motifs.ContainsKey(motif.domain) Then
                    domain_motifs.Add(motif.domain, New Dictionary(Of String, List(Of Probability)))
                End If

                Dim family_motifs = domain_motifs(motif.domain)

                If Not family_motifs.ContainsKey(motif.family) Then
                    Call family_motifs.Add(motif.family, New List(Of Probability))
                End If

                Call family_motifs(motif.family).Add(pwm)
            Next
        Next

        For Each domain In domain_motifs
            Dim family_motifs As Dictionary(Of String, List(Of Probability)) = domain.Value

            For Each family_key As String In family_motifs.Keys
                Call MemeWriter.WriteMemeFormat(family_motifs(family_key), $"{dir}/{domain.Key}/{family_key.NormalizePathString(False)}.meme")
            Next
        Next
    End Sub

    Public Sub ExportTFDb()
        Using text As New StreamWriter($"{repo}/TF.fasta".Open(System.IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Dim page_size As Integer = 1000

            For page As Integer = 1 To Integer.MaxValue
                Dim tf_prots = registry.regulatory_network _
                    .left_join("protein_data").on(field("`protein_data`.id") = field("regulator")) _
                    .left_join("motif").on(field("`motif`.id") = field("`regulatory_network`.motif_site")) _
                    .limit((page - 1) * page_size, page_size) _
                    .select(Of protein_data)("protein_data.id",
    "motif.name as `function`",
    "family as name",
    "motif_site AS gene_id",
    "cluster_id",
    "protein_id",
    "sequence",
    "protein_data.`function` as fingerprint",
    "domain as checksum")

                If tf_prots.IsNullOrEmpty Then
                    Exit For
                End If

                Call text.Add(From tf As protein_data
                              In tf_prots
                              Let tf_id As String = $"{tf.name}|PWM{tf.gene_id.ToHexString} {tf.checksum} [{tf.function}] {tf.fingerprint}"
                              Select New FastaSeq(tf.sequence, title:=tf_id))
            Next
        End Using
    End Sub

End Class
