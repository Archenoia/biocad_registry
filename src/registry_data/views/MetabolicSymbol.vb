Imports System.Runtime.CompilerServices
Imports registry_data.biocad_registryModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Module MetabolicSymbol

    ''' <summary>
    ''' register metabolite symbol and make updates of the reaction hashcode
    ''' </summary>
    ''' <param name="registry"></param>
    <Extension>
    Public Sub RegisterMetabolicSymbols(registry As biocad_registry)
        Dim page_size As Integer = 3000
        Dim role = (registry.MetabolicSubstrateRole.id, registry.MetabolicProductRole.id)

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data As reaction() = registry.reaction _
                .limit(offset, page_size) _
                .select(Of reaction)

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"process metabolic symbol data page {page}".info
            End If

            Dim set_hash = registry.reaction.open_transaction

            For Each reaction As reaction In page_data
                ' re-calculate the reaction hashcode
                Dim key = registry _
                    .RegisterMetabolicSymbols(reaction, role) _
                    .CalculateReactionHashCode(reaction.ec_number)

                If key.hashcode <> reaction.hashcode OrElse key.topology_key <> reaction.topology_key Then
                    Call set_hash.add(registry.reaction _
                        .where(field("id") = reaction.id) _
                        .save_sql(field("hashcode") = key.hashcode,
                                  field("topology_key") = key.topology_key))
                End If
            Next

            Call set_hash.commit()
        Next
    End Sub

    <Extension>
    Private Function RegisterMetabolicSymbols(registry As biocad_registry, reaction As reaction, role As (left As UInteger, right As UInteger)) As List(Of (UInteger, UInteger))
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim metabolites As metabolic_network() = registry.metabolic_network _
            .where(field("reaction_id") = reaction.id,
                   field("role").in({role.left, role.right})) _
            .select(Of metabolic_network)
        Dim links As New List(Of (UInteger, UInteger))
        Dim check_duplicated = False
        Dim duplicates As IGrouping(Of String, metabolic_network)() = metabolites _
            .GroupBy(Function(a) a.role & "-" & a.symbol_id) _
            .ToArray

        For Each link As IGrouping(Of String, metabolic_network) In duplicates
            If link.Count > 1 Then
                Dim removes = link.OrderBy(Function(a) a.id).Skip(1).ToArray
                check_duplicated = True
                For Each item As metabolic_network In link
                    Call registry.metabolic_network.where(field("id") = item.id).delete()
                Next
            End If
        Next

        If check_duplicated Then
            metabolites = registry.metabolic_network _
                .where(field("reaction_id") = reaction.id,
                       field("role").in({role.left, role.right})) _
                .select(Of metabolic_network)
        End If

        For Each meta_edge As metabolic_network In metabolites
            Dim metab As metabolites = registry.metabolites _
                .where(field("id") = meta_edge.species_id) _
                .find(Of metabolites)

            If metab IsNot Nothing Then
                Dim meta_id As UInteger = If(metab.main_id > 0, metab.main_id, metab.id)

                If meta_id <> metab.id Then
                    Call registry.metabolic_network.where(field("id") = meta_edge.id).save(field("species_id") = meta_id)
                End If

                Call links.Add((meta_edge.role, meta_id))
            Else
                Call links.Add((meta_edge.role, 0))
            End If

            ' current no symbol mapping, required of the re-mapping and
            ' then try to run this function again
            If metab Is Nothing Then
                Continue For
            Else
                If registry.SymbolRegister(meta:=metab) Is Nothing Then
                    Call $"make register of '{metab.name}' error!".warning
                End If
            End If
        Next

        Return links
    End Function

    <Extension>
    Public Sub UpdateMetaboliteSymbolName(registry As biocad_registry)
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim page_size As Integer = 10000

        For page As Integer = 1 To Integer.MaxValue
            Dim page_data = registry.registry_resolver _
                .where(field("type") = metabolite_type) _
                .limit((page - 1) * page_size, page_size) _
                .select(Of biocad_registryModel.registry_resolver)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            Dim update As CommitTransaction = registry.registry_resolver.open_transaction

            For Each batch In page_data.SplitIterator(100)
                Dim namedata = registry.registry_resolver _
                    .left_join("metabolites") _
                    .on((field("metabolites.id") = field("symbol_id")) And (field("type") = metabolite_type)) _
                    .where(field("`registry_resolver`.id").in(From s In batch Select s.id)) _
                    .select(Of MetaboliteSymbol)("`registry_resolver`.id", "name")

                For Each symbol In namedata
                    Call update.add(registry.registry_resolver.where(field("id") = symbol.id).save_sql(field("register_name") = symbol.name.makeSymbol))
                Next
            Next

            Call update.commit()
        Next
    End Sub

    Private Class MetaboliteSymbol

        ''' <summary>
        ''' register id
        ''' </summary>
        ''' <returns></returns>
        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property name As String

    End Class
End Module
