Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.ComponentModel
Imports SMRUCC.genomics.ComponentModel.EquaionModel

Public Module ImportsReaction

    Private Function CCNameMapping(cc As String) As String
        Select Case UCase(cc)
            Case "", "CCO-CYTOSOL", "NIL", "CCO-IN" : Return "Cytoplasm"
            Case "CCO-OUT" : Return "Extracellular"
            Case "CCI-PM-BAC-NEG-GN", "CCO-PM-BAC-NEG", "CCO-MEMBRANE", "CCO-PM-BAC-ACT", "CCO-MIDDLE" : Return "Membrane"
            Case Else
                Throw New NotImplementedException(cc)
        End Select
    End Function

    ''' <summary>
    ''' Just make updates of the metabolic network species id
    ''' </summary>
    ''' <param name="registry"></param>
    <Extension>
    Public Sub UpdateMetabolicNetwork(registry As biocad_registry)
        Dim page_size As Integer = 3000
        Dim role = {registry.MetabolicSubstrateRole.id, registry.MetabolicProductRole.id}

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data = registry.metabolic_network _
                .where(field("role").in(role)) _
                .limit(offset, page_size) _
                .select(Of metabolic_network)

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"update metabolic network of page {page}".debug
            End If

            Dim updates As CommitTransaction = registry.metabolic_network.open_transaction
            Dim q As FieldAssert

            For Each link As metabolic_network In page_data
                If link.symbol_id.StringEmpty() Then
                    Continue For
                End If

                If link.symbol_id.IsPattern("[CG]\d{5}") Then
                    q = field("kegg_id") = link.symbol_id
                ElseIf link.symbol_id.IsPattern("ChEBI[:]\d+") Then
                    q = field("chebi_id") = UInteger.Parse(link.symbol_id.Match("\d+"))
                ElseIf link.symbol_id.IsPattern("\d+[-]\d+[-]\d+") Then
                    q = field("cas_id") = link.symbol_id
                Else
                    q = field("biocyc") = link.symbol_id
                End If

                Dim m As metabolites = registry.metabolites _
                    .where(q) _
                    .order_by("id") _
                    .find(Of metabolites)("id", "main_id")

                If Not m Is Nothing Then
                    If m.main_id > 0 Then
                        Call updates.add(registry.metabolic_network.where(field("id") = link.id).save_sql(field("species_id") = m.main_id))
                    ElseIf m.id <> link.species_id Then
                        Call updates.add(registry.metabolic_network.where(field("id") = link.id).save_sql(field("species_id") = m.id))
                    End If
                End If
            Next

            Call updates.commit()
        Next
    End Sub

    ''' <summary>
    ''' left
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <returns></returns>
    <Extension>
    Public Function MetabolicSubstrateRole(registry As biocad_registry) As vocabulary
        Return registry.biocad_vocabulary.GetVocabulary("Metabolic Role", "Substrate")
    End Function

    ''' <summary>
    ''' right
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <returns></returns>
    <Extension>
    Public Function MetabolicProductRole(registry As biocad_registry) As vocabulary
        Return registry.biocad_vocabulary.GetVocabulary("Metabolic Role", "Product")
    End Function

    <Extension>
    Public Sub importsReactions(registry As biocad_registry, reactions As IEnumerable(Of EquaionModel.Reaction), db_name As String)
        Dim db_source As UInteger = registry.biocad_vocabulary.GetDatabaseResource(db_name).id
        Dim ec_num As UInteger = registry.biocad_vocabulary.db_ECNumber
        Dim dblinks As CommitTransaction = registry.db_xrefs.ignore.open_transaction
        Dim entityType As UInteger = registry.biocad_vocabulary.reaction_type
        Dim role_substrate As UInteger = registry.MetabolicSubstrateRole.id
        Dim role_product As UInteger = registry.MetabolicProductRole.id
        Dim network As CommitTransaction = registry.metabolic_network.ignore.open_transaction
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim dbList As UInteger() = {registry.biocad_vocabulary.db_kegg, registry.biocad_vocabulary.db_chebi, registry.biocad_vocabulary.db_biocyc}
        Dim compartmentIndex As New Dictionary(Of String, biocad_registryModel.compartment_location)
        Dim pool As EquaionModel.Reaction() = reactions.ToArray
        Dim enrich = registry.compartment_enrich.ignore.open_transaction

        For Each rxn As EquaionModel.Reaction In TqdmWrapper.Wrap(pool)
            For Each cpd In rxn.equation.Reactants
                Call compartmentIndex _
                    .ComputeIfAbsent(If(cpd.Compartment, ""),
                                     Function(cc)
                                         cc = CCNameMapping(cc)
                                         Dim cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         If cc_obj Is Nothing Then
                                             registry.compartment_location.add(
                                                field("name") = cc,
                                                field("fullname") = cc
                                             )
                                             cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         End If

                                         Return cc_obj
                                     End Function)
            Next
            For Each cpd In rxn.equation.Products
                Call compartmentIndex _
                    .ComputeIfAbsent(If(cpd.Compartment, ""),
                                     Function(cc)
                                         cc = CCNameMapping(cc)
                                         Dim cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         If cc_obj Is Nothing Then
                                             registry.compartment_location.add(
                                                field("name") = cc,
                                                field("fullname") = cc
                                             )
                                             cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         End If

                                         Return cc_obj
                                     End Function)
            Next
        Next

        For Each rxn As EquaionModel.Reaction In TqdmWrapper.Wrap(pool)
            Dim find As biocad_registryModel.reaction = registry.reaction _
                .where(field("db_source") = db_source,
                       field("db_xref") = rxn.entry) _
                .find(Of biocad_registryModel.reaction)

            If find Is Nothing Then
                Call registry.reaction.add(
                    field("db_source") = db_source,
                    field("db_xref") = rxn.entry,
                    field("hashcode") = "",
                    field("name") = If(rxn.definition.StringEmpty, rxn.equation.ToString, rxn.definition),
                    field("ec_number") = rxn.enzyme.DefaultFirst,
                    field("equation") = rxn.equation.ToString,
                    field("note") = rxn.comment
                )
                find = registry.reaction _
                    .where(field("db_source") = db_source,
                           field("db_xref") = rxn.entry) _
                    .find(Of biocad_registryModel.reaction)
            End If

            If find.name.StringEmpty(, True) Then
                registry.reaction.where(field("id") = find.id).save(field("name") = find.equation)
            End If

            For Each num As String In rxn.enzyme
                Call dblinks.ignore.add(
                    field("obj_id") = find.id,
                    field("type") = entityType,
                    field("db_name") = ec_num,
                    field("db_xref") = num,
                    field("db_source") = db_source
                )
            Next

            Call dblinks.ignore.add(
                field("obj_id") = find.id,
                field("type") = entityType,
                field("db_name") = db_source,
                field("db_xref") = rxn.entry,
                field("db_source") = db_source
            )

            For Each link In rxn.db_xrefs.SafeQuery
                Dim source_name As UInteger
                Dim xref_id As String = link.text

                Select Case LCase(link.name)
                    Case "kegg.reaction" : source_name = registry.biocad_vocabulary.db_kegg
                    Case "reactome" : source_name = registry.biocad_vocabulary.GetDatabaseResource("Reactome").id
                    Case "biocyc"
                        source_name = registry.biocad_vocabulary.db_biocyc
                        xref_id = Strings.Trim(xref_id.GetTagValue(":").Value)
                    Case "macie" : source_name = registry.biocad_vocabulary.GetDatabaseResource("MACiE").id
                    Case "obo"
                        source_name = registry.biocad_vocabulary.GetDatabaseResource("Gene Ontology").id
                        xref_id = Strings.Trim(xref_id.Replace("_", ":"))
                    Case Else
                        Throw New NotImplementedException(link.name)
                End Select

                Call dblinks.ignore.add(
                    field("obj_id") = find.id,
                    field("type") = entityType,
                    field("db_name") = source_name,
                    field("db_xref") = xref_id,
                    field("db_source") = db_source
                )
            Next

            Dim eq = rxn.equation
            Dim links As New List(Of (role_id As UInteger, species_id As UInteger))

            For Each sp As SideCompound In rxn.compounds
                Dim role As UInteger = If(sp.side = "left", role_substrate, role_product)
                Dim factor = If(role = role_substrate, eq.Reactants, eq.Products).KeyItem(sp.compound.entry)

                If factor Is Nothing Then
                    factor = If(role <> role_substrate, eq.Reactants, eq.Products).KeyItem(sp.compound.entry)
                End If

                factor = If(factor, New DefaultTypes.CompoundSpecieReference)

                Dim metab = registry.db_xrefs _
                    .where(field("type") = metabolite_type,
                           field("db_name").in(dbList),
                           field("db_xref") = sp.compound.entry) _
                    .order_by("obj_id") _
                    .find(Of biocad_registryModel.db_xrefs)
                Dim loc As biocad_registryModel.compartment_location = compartmentIndex _
                    .ComputeIfAbsent(If(factor.Compartment, ""),
                                     Function(cc)
                                         cc = CCNameMapping(cc)
                                         Return registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                     End Function)

                Call network.ignore.add(
                    field("reaction_id") = find.id,
                    field("factor") = If(factor.Stoichiometry.IsNaNImaginary OrElse factor.Stoichiometry = 0.0, 1, factor.Stoichiometry),
                    field("species_id") = If(metab Is Nothing, 0, metab.obj_id),
                    field("symbol_id") = sp.compound.entry,
                    field("role") = role,
                    field("compartment_id") = loc.id,
                    field("note") = sp.ToString
                )

                If metab IsNot Nothing Then
                    Call links.Add((role, metab.obj_id))
                Else
                    Call links.Add((role, 0))
                End If

                If metab IsNot Nothing AndAlso compartmentIndex.Count > 0 AndAlso compartmentIndex.Keys.First <> "" Then
                    Call enrich.ignore.add(
                        field("metabolite_id") = metab.obj_id,
                        field("location_id") = loc.id,
                        field("evidence") = rxn.entry
                    )
                End If
            Next

            If links.All(Function(a) a.Item2 > 0) Then
                Dim key = links.CalculateReactionHashCode(ec_number:=find.ec_number)

                registry.reaction _
                    .where(field("id") = find.id) _
                    .save(field("hashcode") = key.hashcode,
                          field("topology_key") = key.topology_key)
            End If
        Next

        Call dblinks.commit()
        Call network.commit()
        Call enrich.commit()
    End Sub

    ''' <summary>
    ''' a set of tuple of [role_id, obj_id]
    ''' </summary>
    ''' <param name="links">no needs to sort order before, this function will sort the order automatically</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' this function will sort by role_id first, then sort by symbol_id, then build hashcode
    ''' </remarks>
    <Extension>
    Public Function CalculateReactionHashCode(links As IEnumerable(Of (role_id As UInteger, species_id As UInteger)), ec_number As String) As (hashcode As String, topology_key As String)
        Dim roles = links.GroupBy(Function(a) a.role_id).ToArray
        Dim topo As String = roles _
            .OrderBy(Function(a)
                         ' 20260416 ignores of the reaction direction
                         ' just reflects the network topology connection
                         Return a.Sum(Function(s) s.species_id)
                     End Function) _
            .Select(Function(a)
                        Return a.OrderBy(Function(s) s.species_id) _
                            .Select(Function(s) s.species_id) _
                            .JoinBy(",")
                    End Function) _
            .JoinBy("+") _
            .MD5
        Dim rxn_key As String = $"{topo} [{ec_number}]".MD5

        Return (rxn_key, topo)
    End Function
End Module
