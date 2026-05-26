Imports registry_data
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data.biocad_registryModel

Module Module1

    ReadOnly names_test As String =
        <names>
            Activated tRNA
        (-)-3-exo-Hydroxycamphor
        (-)-Maackiain-3-O-glucosyl-6''-O-malonate
        (1~{S},2'~{S},4~{S},6'~{R},8~{R},10~{S},10'~{S},11~{R},11'~{S},12~{S},13~{R},16~{R},22'~{R},23'~{R},26'~{S},27'~{R},31'~{S},33'~{R},34'~{R},36'~{R},37'~{R})-27'-[(2~{R},3~{S})-3-[(1~{R})-1,2-dimethylpropyl]oxiran-2-yl]-2',11,17',23',27',36',37'-heptahydroxy-10',36'-bis(hydroxymethyl)-16-(2-methoxyethyl)-34'-methyl-spiro[17-azatetracyclo[11.3.1.1^{1,4}.0^{8,12}]octadec-6-yne-10,12'-3,4-dithia-14-azanonacyclo[21.15.3.1^{11,14}.1^{15,19}.0^{1,34}.0^{6,11}.0^{22,26}.0^{22,31}.0^{33,41}]tritetraconta-15,17,19(42),29,40-pentaene]-39',43'-dione
(&amp;plusmn;)-JWH 018 N-(3-hydroxypentyl) metabolite
(&amp;PLUSMN;)-Clopidogrel
Genistein 4&#39;,7-O-diglucuronide
</names>

    Sub cleanMoNAName()
        ' "; LC-tDDA; CE20"
        ' "; LC-tDDA; CE10"
        ' "; LC-tDDA; CE30"
        ' "; LC-tDDA; CE40"
        ' ; AIF; CE30; CorrDec
        ' "; AIF; CE20; CorrDec"
        ' "; AIF; CE10; CorrDec"
        ' "; AIF; CE40; CorrDec"
        ' "; AIF; CE0; CorrDec"
        ' ; AIF; CE0; MS2Dec
        ' ; AIF; CE10; MS2Dec
        ' ; AIF; CE20; MS2Dec
        ' ; AIF; CE30; MS2Dec
        ' ; AIF; CE40; MS2Dec
        ' #
        Dim strip_part As String = " #"
        Dim names = registry.metabolites.where(field("name").instr(strip_part)).select(Of metabolites)
        Dim update = registry.metabolites.open_transaction

        For Each name In names
            Call update.add(registry.metabolites.where(field("id") = name.id).save_sql(field("name") = name.name.Replace(strip_part, "").Trim))
        Next

        Call update.commit()
    End Sub

    Sub clean__major()
        Dim names = registry.metabolites.where(field("name").instr("_major")).select(Of metabolites)
        Dim update = registry.metabolites.open_transaction

        For Each name In names
            If name.name.EndsWith("_major") Then
                Call update.add(registry.metabolites.where(field("id") = name.id).save_sql(field("name") = name.name.Substring(0, name.name.Length - 6).Trim))
            End If
        Next

        Call update.commit()
    End Sub

    Sub cleanNameTest()
        For Each name As String In names_test.LineTokens.Select(AddressOf Strings.Trim)
            Call Console.WriteLine(name)
            Call Console.WriteLine(RegisterSymbol.CleanName(name))
        Next
    End Sub

    Sub migrantTopicToInternalMetaboliteID()
        Dim page_size = 5000

        For page As Integer = 1 To Integer.MaxValue
            Dim pagedata = registry.topic _
                .left_join("registry_resolver") _
                .on(field("`registry_resolver`.id") = field("model_id")) _
                .where(field("`topic`.type") = 0,
                       field("`registry_resolver`.type") = registry.biocad_vocabulary.metabolite_type) _
                .limit((page - 1) * page_size, page_size) _
                .select(Of InternalTopicLink)("`topic`.id AS topic_link", "`symbol_id` AS metabolite_id")

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            Dim update As CommitTransaction = registry.topic.open_transaction

            For Each item In pagedata
                Call update.add(registry.topic.where(field("id") = item.topic_link).ignore.save_sql(field("type") = registry.biocad_vocabulary.metabolite_type, field("model_id") = item.metabolite_id))
            Next

            Call update.commit()
        Next
    End Sub
End Module

Public Class InternalTopicLink

    <DatabaseField> Public Property topic_link As UInteger
    <DatabaseField> Public Property metabolite_id As UInteger

End Class