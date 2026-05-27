Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.Chemoinformatics.Metabolite
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data
Imports registry_data.biocad_registryModel

Public Module MetaboliteData

    <Extension>
    Public Sub SaveDbLinks(registry As biocad_registry,
                           meta As MetaInfo,
                           m As metabolites,
                           db_source As UInteger,
                           Optional saveID As Boolean = False,
                           Optional trans As CommitTransaction = Nothing)

        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim metabolite_type As UInteger = vocabulary.metabolite_type
        Dim updates As New List(Of FieldAssert)
        Dim xrefs As CrossReference.xref = meta.xref
        Dim pubchem_cid As String = Strings.Trim(xrefs.pubchem).int_id
        Dim chebi_id As String = Strings.Trim(xrefs.chebi).int_id
        Dim auto_commit As Boolean = trans Is Nothing

        ' transaction of registry.db_xrefs
        If auto_commit Then
            trans = registry.db_xrefs.open_transaction.ignore
        End If

        If m.pubchem_cid = 0 AndAlso (Not pubchem_cid Is Nothing) AndAlso Not pubchem_cid.IsPattern("0+") Then
            updates.Add(field("pubchem_cid") = pubchem_cid)
        End If
        If m.chebi_id = 0 AndAlso (Not chebi_id Is Nothing) AndAlso Not chebi_id.IsPattern("0+") Then
            updates.Add(field("chebi_id") = chebi_id)
        End If
        If m.hmdb_id.StringEmpty AndAlso Not xrefs.HMDB.StringEmpty Then
            updates.Add(field("hmdb_id") = xrefs.HMDB)
        End If
        If m.kegg_id.StringEmpty AndAlso Not xrefs.KEGG.StringEmpty Then
            updates.Add(field("kegg_id") = xrefs.KEGG)
        End If
        If m.lipidmaps_id.StringEmpty AndAlso Not xrefs.lipidmaps.StringEmpty Then
            updates.Add(field("lipidmaps_id") = xrefs.lipidmaps)
        End If
        If m.mesh_id.StringEmpty AndAlso Not xrefs.MeSH.StringEmpty Then
            updates.Add(field("mesh_id") = xrefs.MeSH)
        End If
        If m.wikipedia.StringEmpty AndAlso Not xrefs.Wikipedia.StringEmpty Then
            updates.Add(field("wikipedia") = xrefs.Wikipedia)
        End If
        If m.cas_id.StringEmpty AndAlso Not xrefs.CAS.DefaultFirst.StringEmpty Then
            Dim main_cas_id As String = xrefs.CAS _
                .DefaultFirst _
                .StringSplit("[,;]", True) _
                .First _
                .Split _
                .First

            updates.Add(field("cas_id") = main_cas_id)
        End If
        If m.biocyc.StringEmpty AndAlso Not xrefs.MetaCyc.StringEmpty Then
            updates.Add(field("biocyc") = xrefs.MetaCyc)
        End If
        If m.drugbank_id.StringEmpty AndAlso Not xrefs.DrugBank.StringEmpty Then
            updates.Add(field("drugbank_id") = xrefs.DrugBank)
        End If

        If updates.Any Then
            Call trans.add(registry.metabolites.ignore.where(field("id") = m.id).save_sql(updates.ToArray))
        End If

        If (Not pubchem_cid.StringEmpty) AndAlso Not pubchem_cid.IsPattern("0+") Then
            Dim data = {field("db_source") = db_source,
                             field("db_name") = vocabulary.db_pubchem,
                             field("db_xref") = pubchem_cid,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id}

            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If (Not chebi_id.StringEmpty) AndAlso Not chebi_id.IsPattern("0+") Then
            chebi_id = $"ChEBI:{chebi_id}"

            Dim data = {field("db_source") = db_source,
                             field("db_name") = vocabulary.db_chebi,
                             field("db_xref") = chebi_id,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id}

            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.HMDB.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_hmdb,
                             field("db_xref") = xrefs.HMDB,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }

            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.lipidmaps.StringEmpty Then
            Dim data = {
                field("db_source") = db_source,
                             field("db_name") = vocabulary.db_lipidmaps,
                             field("db_xref") = xrefs.lipidmaps,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }

            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.KEGG.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_kegg,
                             field("db_xref") = xrefs.KEGG,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }

            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.MeSH.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_mesh,
                             field("db_xref") = xrefs.MeSH,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }
            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.Wikipedia.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_wikipedia,
                             field("db_xref") = xrefs.Wikipedia,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }
            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.MetaCyc.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_biocyc,
                             field("db_xref") = xrefs.MetaCyc,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }
            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.DrugBank.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_drugbank,
                             field("db_xref") = xrefs.DrugBank,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }
            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If Not xrefs.metlin.StringEmpty Then
            Dim data = {
            field("db_source") = db_source,
                             field("db_name") = vocabulary.db_metlin,
                             field("db_xref") = xrefs.metlin,
                             field("type") = metabolite_type,
                             field("obj_id") = m.id
            }
            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If
        If saveID Then
            Dim data = {
                 field("db_source") = db_source,
                                  field("db_name") = db_source,
                                  field("db_xref") = meta.ID,
                                  field("type") = metabolite_type,
                                  field("obj_id") = m.id
            }

            If auto_commit Then
                trans.ignore.add(data)
            Else
                trans.add(registry.db_xrefs.ignore.add_sql(data))
            End If
        End If

        For Each id As String In xrefs.CAS.SafeQuery
            If id.StringEmpty Then
                Continue For
            End If

            For Each split As String In id.StringSplit("[,;]\s*")
                Dim data = {
                   field("db_source") = db_source,
                         field("db_name") = vocabulary.db_cas,
                         field("db_xref") = split,
                         field("type") = metabolite_type,
                         field("obj_id") = m.id
                }

                If auto_commit Then
                    trans.ignore.add(data)
                Else
                    trans.add(registry.db_xrefs.ignore.add_sql(data))
                End If
            Next
        Next

        If auto_commit Then
            Call trans.commit()
        End If
    End Sub

    <Extension>
    Private Function int_id(id As String) As String
        If Not id.StringEmpty(, True) Then
            id = id.Match("\d+")
        End If
        If id = "" Then
            Return Nothing
        Else
            Return id
        End If
    End Function

    <Extension>
    Private Function MakeNameSearch(registry As biocad_registry, meta As MetaInfo, mass_filter As FieldAssert) As metabolites
        ' not working as expected
        ' m = registry.FindByName({meta.name, meta.IUPACName}.JoinIterates(meta.synonym), exact_mass)
        Dim hashset As String() = meta.synonym _
            .JoinIterates({meta.name, meta.IUPACName}) _
            .Where(Function(str) Not str.StringEmpty(, True)) _
            .Select(Function(str) str.ToLower.MD5) _
            .ToArray
        Dim m As metabolites = registry.metabolites _
            .where(field("hashcode").in(hashset), mass_filter) _
            .order_by("id") _
            .find(Of metabolites)
        Dim metabolite_type As String = registry.biocad_vocabulary.metabolite_type

        If m Is Nothing Then
            Dim hit = registry.synonym _
                .left_join("metabolites") _
                .on(field("`metabolites`.id") = field("obj_id") And field("type") = metabolite_type) _
                .where(mass_filter, field("`synonym`.hashcode").in(hashset)) _
                .group_by("obj_id") _
                .order_by("count(*)", desc:=True) _
                .find(Of NameHit)("obj_id AS id", "COUNT(*) AS size")

            If hit IsNot Nothing Then
                m = registry.metabolites.where(field("id") = hit.id).find(Of metabolites)
            End If
        Else
            Call m.ToString.debug
        End If

        Return m
    End Function

    <Extension>
    Private Function MakePrimaryKeySearch(registry As biocad_registry, meta As MetaInfo, mass_filter As FieldAssert) As metabolites
        Dim m As metabolites = Nothing

        If m Is Nothing AndAlso Not meta.xref.KEGG.StringEmpty Then
            m = registry.metabolites.where(field("kegg_id") = meta.xref.KEGG, mass_filter).find(Of metabolites)
        End If
        If m Is Nothing AndAlso Not meta.xref.HMDB.StringEmpty Then
            m = registry.metabolites.where(field("hmdb_id") = meta.xref.HMDB, mass_filter).find(Of metabolites)
        End If
        If m Is Nothing AndAlso Not meta.xref.lipidmaps.StringEmpty Then
            m = registry.metabolites.where(field("lipidmaps_id") = meta.xref.lipidmaps, mass_filter).find(Of metabolites)
        End If

        Return m
    End Function

    <Extension>
    Public Function FindMolecule(registry As biocad_registry, meta As MetaInfo, Optional primaryKey As String = Nothing,
                                 Optional nameSearch As Boolean = False,
                                 Optional preferNameSearch As Boolean = False,
                                 Optional source_db As UInteger = 0) As metabolites

        Dim pubchem_cid As String = Strings.Trim(meta.xref.pubchem).int_id
        Dim chebi_id As String = Strings.Trim(meta.xref.chebi).int_id
        Dim name As String = Strings.Trim(meta.name)
        Dim hashcode As String = name.ToLower.MD5
        Dim exact_mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)
        Dim mass_filter As FieldAssert

        If exact_mass < 0 Then
            exact_mass = 0
            mass_filter = field("exact_mass") = 0
        Else
            mass_filter = field("exact_mass").between(exact_mass - 1, exact_mass + 1)
        End If

        Dim m As metabolites = Nothing

        If Not primaryKey Is Nothing Then
            m = registry.metabolites _
                .where(field(primaryKey) = meta.ID, mass_filter) _
                .find(Of metabolites)
        End If

        If exact_mass > 1 AndAlso m Is Nothing Then
            If preferNameSearch AndAlso nameSearch Then
                m = registry.MakeNameSearch(meta, mass_filter)

                If m Is Nothing Then
                    m = registry.MakePrimaryKeySearch(meta, mass_filter)
                End If
            Else
                m = registry.MakePrimaryKeySearch(meta, mass_filter)

                If m Is Nothing AndAlso nameSearch Then
                    m = registry.MakeNameSearch(meta, mass_filter)
                End If
            End If
        End If

        If m Is Nothing Then
            Dim main_cas_id As String = meta.xref.CAS _
                .DefaultFirst _
                .StringSplit("\s*[,;]\s*", True) _
                .FirstOrDefault

            If main_cas_id IsNot Nothing Then
                main_cas_id = main_cas_id.Split.First
            End If

            Call registry.metabolites.add(
                field("name") = name,
                field("hashcode") = hashcode,
                field("formula") = If(meta.formula, ""),
                field("exact_mass") = exact_mass,
                field("cas_id") = main_cas_id,
                field("pubchem_cid") = pubchem_cid,
                field("chebi_id") = chebi_id,
                field("hmdb_id") = meta.xref.HMDB,
                field("lipidmaps_id") = meta.xref.lipidmaps,
                field("kegg_id") = meta.xref.KEGG,
                field("biocyc") = meta.xref.MetaCyc,
                field("mesh_id") = meta.xref.MeSH,
                field("wikipedia") = meta.xref.Wikipedia,
                field("drugbank_id") = meta.xref.DrugBank,
                field("note") = meta.description
            )

            If Not primaryKey Is Nothing Then
                m = registry.metabolites.where(field(primaryKey) = meta.ID, mass_filter) _
                    .order_by("id", desc:=True) _
                    .find(Of metabolites)
            End If

            ' 20260106 fix for the non-primary key database
            ' exmple as refmet is missing from the master list
            ' so m is always nothing
            ' try to find by name at here
            If m Is Nothing Then
                m = registry.metabolites _
                    .where(mass_filter, field("name") = name) _
                    .order_by("id", desc:=True) _
                    .find(Of metabolites)
            End If
        Else
            If m.note.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("note") = meta.description)
            End If
            If m.name.StringEmpty(, True) Then
                registry.metabolites.where(field("id") = m.id).save(field("name") = meta.name)
            End If
        End If

        If TypeOf meta Is MetaLib Then
            Dim zh_name As String() = {DirectCast(meta, MetaLib).zh_name}
            Dim trans = registry.synonym.open_transaction

            If zh_name.TryCount > 0 Then
                ' 20260417
                ' update zh_name if database record is empty
                If m.name_zh.StringEmpty(, True) Then
                    Call trans.add(registry.metabolites.where(field("id") = m.id).save_sql(field("name_zh") = zh_name.FirstOrDefault))
                End If

                If source_db > 0 Then
                    For Each name_zh As String In zh_name
                        Call trans.add(
                            field("obj_id") = m.id,
                            field("type") = registry.biocad_vocabulary.metabolite_type,
                            field("db_source") = source_db,
                            field("synonym") = name_zh,
                            field("hashcode") = name_zh.ToLower.MD5,
                            field("lang") = "zh"
                        )
                    Next
                End If
            End If

            Call trans.commit()
        End If

        Return m
    End Function

    <Extension>
    Public Function FindByName(registry As biocad_registry, names As IEnumerable(Of String), exact_mass As Double) As metabolites
        Dim hits As New List(Of NameHit)
        Dim metabolite_type As UInteger = New biocad_vocabulary(registry).metabolite_type

        For Each name As String In names
            If name.StringEmpty(, True) Then
                Continue For
            Else
                name = name _
                    .Replace("-", " ") _
                    .Replace("+", " ") _
                    .Replace("""", " ") _
                    .Replace("'", " ") _
                    .Replace("*", " ") _
                    .Replace(">", " ") _
                    .Replace("<", " ") _
                    .Replace("~", " ") _
                    .Replace("(", " ") _
                    .Replace(")", " ") _
                    .Replace("`", " ") _
                    .Trim
            End If

            Dim fulltext As String = match("synonym").against(name, booleanMode:=False).ToString
            Dim top = registry.synonym _
                .left_join("metabolites").on((field("`metabolites`.id") = field("obj_id")) And (field("type") = metabolite_type)) _
                .where(field("exact_mass").between(exact_mass - 1, exact_mass + 1), expr(fulltext)) _
                .group_by("`metabolites`.id") _
                .order_by($"count(*) * SUM({fulltext})", desc:=True) _
                .find(Of NameHit)("metabolites.id", "COUNT(*) AS size", $"count(*) * SUM({fulltext}) as score")

            If top IsNot Nothing Then
                Call hits.Add(top)
            End If
        Next

        If hits.Any Then
            Dim top_id = hits _
                .GroupBy(Function(a) a.id) _
                .OrderByDescending(Function(a) Aggregate i In a Into Sum(i.size)) _
                .First

            Return registry.metabolites.where(field("id") = top_id.Key).find(Of metabolites)
        Else
            Return Nothing
        End If
    End Function

    Private Class NameHit

        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property size As Long
        <DatabaseField> Public Property score As Double

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

    End Class
End Module
