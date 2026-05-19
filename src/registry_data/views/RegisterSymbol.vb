Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Text
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Module RegisterSymbol

    ReadOnly greekAlphabet As Dictionary(Of String, String) = GreekAlphabets.lower _
        .ReverseMaps _
        .ToDictionary(Function(a) "&" & a.Key & ";",
                      Function(a)
                          Return a.Value
                      End Function)

    <Extension>
    Public Function makeSymbol(name As String) As String
        Dim symbol As String = name _
            .StringReplace("[<][/]?\s*sub[>]", "") _
            .StringReplace("[<][/]?\s*sup[>]", "") _
            .StringReplace("[<][/]?\s*i[>]", "")

        symbol = CleanName(Strings.Trim(symbol)) _
            .Replace("(", "_") _
            .Replace(")", "_") _
            .Replace("""", "_") _
            .Replace("'", "_") _
            .Replace("!", "_") _
            .Replace("?", "_") _
            .Replace("\", "_") _
            .Replace("/", "_") _
            .StringReplace("\s+", "_") _
            .StringReplace("[_-]{2,}", "_") _
            .Trim("_"c, ","c)

        symbol = symbol.Replace("[_", "[").Replace("_]", "]")
        symbol = symbol.Replace("{_", "{").Replace("_}", "}")

        symbol = symbol.StringReplace(",[\-_]+", ",")
        symbol = symbol.StringReplace("[\-_]+,", ",")
        symbol = symbol.StringReplace(",{2,}", ",")

        Return symbol
    End Function

    Public Function CleanName(name As String) As String
        For Each alphabet In greekAlphabet
            name = name.Replace(alphabet.Key, alphabet.Value)
            name = name.StringReplace(alphabet.Key.Replace("&", "[&]"), alphabet.Value)
        Next

        name = name _
            .StringReplace("_{2,}", "_") _
            .StringReplace("[-]{2,}", "-") _
            .StringReplace(",{2,}", ",") _
            .StringReplace("['""]{2,}", "'") _
            .StringReplace("\s{2,}", " ") _
            .StringReplace("[.,;][-]", "-") _
            .StringReplace("[.,;]_", "_")

        name = name.StringReplace("[&]plusmn;", "±")
        name = name.StringReplace("[&]#39;", "'")
        name = name.StringReplace("[&]alpha$", "α")
        name = name.StringReplace("[&]beta$", "β")
        name = name.StringReplace("-alpha-", "-α-")
        name = name.StringReplace("-beta-", "-β-")
        name = name.Replace("&#39", "")
        name = name.StringReplace("\s{2,}", " ").Trim

        Return name
    End Function

    ''' <summary>
    ''' get an existed register symbol for the target metabolite
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="meta_id"></param>
    ''' <returns>
    ''' this function will returns nothing if the symbol is not found
    ''' </returns>
    <Extension>
    Public Function GetMetaboliteModel(registry As biocad_registry, meta_id As UInteger) As registry_resolver
        Return registry.registry_resolver _
            .where(field("symbol_id") = meta_id,
                   field("type") = registry.biocad_vocabulary.metabolite_type) _
            .find(Of registry_resolver)
    End Function

    <Extension>
    Public Function MetaboliteScore(m As metabolites) As Double
        Dim score As Double = 0

        If m.exact_mass > 0 Then score += 1
        If m.pubchem_cid > 0 Then score += 1
        If m.chebi_id > 0 Then score += 1

        If Not m.kegg_id.StringEmpty Then score += 1
        If Not m.hmdb_id.StringEmpty Then score += 1
        If Not m.biocyc.StringEmpty Then score += 1
        If Not m.cas_id.StringEmpty Then score += 1
        If Not m.drugbank_id.StringEmpty Then score += 1
        If Not m.lipidmaps_id.StringEmpty Then score += 1
        If Not m.wikipedia.StringEmpty Then score += 1
        If Not m.mesh_id.StringEmpty Then score += 1

        Return score / m.id
    End Function

    ''' <summary>
    ''' make register of the metabolite symbol inside the biocad registry and then returns the new symbol
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="meta"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' this function will try to find the existsed symbol with the same name, if not found then will try to make a new one.
    ''' 
    ''' 目前我尝试基于下面的vb.net的函数来实现如下所示的代谢物符号更新流程：  
    ''' 
    ''' 首先针对代谢物基于其名称创建一个变量符号名称symbol_name，然后基于symbol_name查找目前的注册表中是否存在。以及检查当前代谢物其id以及其main_id对应的符号是否存在。
    ''' 由于在我的代谢物表中会存在同名并且相同化学式的代谢物数据，所以针对这种情况，我为代谢物对象添加了main_id映射，从同名的代谢物中选取一个作为主代谢物，其他的作为次要的代谢物，对次要代谢物的main_id赋值，将其映射到主代谢。
    ''' 
    ''' 所以在这里我同时检查了当前代谢物的id对应的符号是否存在，以及尝试通过main_Id来检查是否可能存在主代谢物对应的符号。
    ''' 由于registry_resolver表里面的register_name变量符号必须是唯一的，并且代谢物实体在这个注册表中的记录也应该是唯一的。所以我尝试通过下面的更新逻辑来尝试保持这种唯一性的约束： 
    ''' 
    ''' 当通过symbol_name检查得到的check变量存在的时候，假若check变量对应的代谢物的id大于当前代谢物的id或者check变量的代谢物不存在，则将对应的符号的id映射到当前的代谢物id上，并在更新映射之前需要删除掉当前的代谢物的在注册表中所有的通过代谢物id关联的符号记录
    ''' 当通过symbol_name检查得到的check变量不存在的时候，则metabolite_id对应的符号存在的时候，更新metabolite_id对应的符号的register_name，同时在这里也要删除当前代谢物id对应的符号映射记录
    ''' </remarks>
    <Extension>
    Public Function SymbolRegister(registry As biocad_registry, meta As metabolites) As registry_resolver
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim metabolite_id As UInteger = If(meta.main_id > 0, meta.main_id, meta.id)
        ' create symbol name
        Dim symbol_name As String = meta.name.makeSymbol
        ' then check of current symbol name
        Dim check As registry_resolver = registry.registry_resolver _
            .where(field("register_name") = symbol_name,
                   field("type") = metabolite_type) _
            .find(Of registry_resolver)
        ' check of current metabolite its symbol is existsed or not
        Dim current As registry_resolver = registry.registry_resolver _
            .where(field("symbol_id") = meta.id, field("type") = metabolite_type) _
            .find(Of registry_resolver)
        Dim current_main As registry_resolver = registry.registry_resolver.where(field("symbol_id") = meta.main_id, field("type") = metabolite_type).find(Of registry_resolver)

        If check IsNot Nothing Then
            Dim check_id As UInteger = check.symbol_id
            Dim check_main_mapping = registry.metabolites.where(field("id") = check_id).find(Of metabolites)

            If check_main_mapping Is Nothing Then
                ' metabolite data is missing from table
                ' current symbol name reference to an invalid registry symbol
                check_id = 0
            Else
                If check_main_mapping.main_id > 0 Then
                    check_id = check_main_mapping.main_id
                End If
            End If

            ' symbol is already existsed
            If check_id = 0 OrElse (check_id <> metabolite_id AndAlso metabolite_id < check_id) Then
                ' removes current id mapping
                If current IsNot Nothing Then
                    Call registry.registry_resolver.where(field("id") = current.id).delete()
                End If
                If current_main IsNot Nothing Then
                    Call registry.registry_resolver.where(field("id") = current_main.id).delete()
                End If

                ' make updates of the current symbol name id mapping
                Call registry.registry_resolver _
                        .where(field("id") = check.id) _
                        .save(field("symbol_id") = metabolite_id)

                Return GetMetaboliteModel(registry, metabolite_id)
            End If

            Return check
        Else
            If metabolite_id <> meta.id AndAlso current IsNot Nothing Then
                ' removes current metabolite id mapping
                ' use the main metabolite id mapping
                Call registry.registry_resolver.where(field("id") = current.id).delete()
            End If

            If current_main Is Nothing Then
                ' make a new symbol register inside the registry system
                Call registry.registry_resolver.add(
                    field("register_name") = symbol_name,
                    field("type") = metabolite_type,
                    field("symbol_id") = metabolite_id
                )
            Else
                Call registry.registry_resolver _
                        .where(field("id") = current_main.id) _
                        .save(field("register_name") = symbol_name)
            End If
        End If

        ' get register symbol by its metabolite id
        Return GetMetaboliteModel(registry, metabolite_id)
    End Function
End Module
