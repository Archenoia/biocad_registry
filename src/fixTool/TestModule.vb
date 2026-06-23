' ============================================================
'  测试模块：验证 MetaboliteNameResolver 算法
'  使用 Troxerutin 同义名集合作为测试用例
' ============================================================
Imports BioNovoGene.BioDeep.Chemistry.MetaLib

Module TestModule

    Sub Main1()
        ' 测试数据：Troxerutin 的同义名集合（来自多个公共数据库）
        Dim synonyms As New List(Of String) From {
            "Troxarutin",
            "Troxarutin",
            "Troxarutin-ratiopharm",
            "Troxarutin-ratiopharm",
            "troxerutin",
            "Troxerutin",
            "Troxerutin",
            "Troxerutin",
            "Troxerutin",
            "TROXERUTIN",
            "Troxerutin (85%)",
            "Troxerutin (85%)",
            "Troxerutin (INN)",
            "Troxerutin (INN)",
            "TROXERUTIN [EP MONOGRAPH]",
            "Troxerutin [INN:BAN:DCF]",
            "TROXERUTIN [INN]",
            "TROXERUTIN [MART.]",
            "TROXERUTIN [MI]",
            "TROXERUTIN [WHO-DD]",
            "TROXERUTIN EP MONOGRAPH",
            "TROXERUTIN INN",
            "Troxerutin INN:BAN:DCF",
            "TROXERUTIN MART.",
            "TROXERUTIN WHO-DD",
            "Troxerutin-ratiopharm",
            "Troxerutin-ratiopharm",
            "Troxerutin, European Pharmacopoeia (EP) Reference Standard",
            "Troxerutin,(S)",
            "Troxerutina",
            "Troxerutina [INN-Spanish]",
            "Troxerutina INN-Spanish",
            "Troxerutine",
            "Troxerutine",
            "Troxerutine",
            "Troxerutine",
            "Troxerutine [INN-French]",
            "Troxerutine INN-French"
        }

        ' 实例化解析器并执行解析
        Dim resolver As New MetaboliteNameResolver()
        Dim bestName As String = resolver.ResolveBestName(synonyms)

        ' 输出结果
        Console.WriteLine("====================================================")
        Console.WriteLine("  代谢物名称解析器 - 测试")
        Console.WriteLine("====================================================")
        Console.WriteLine("输入同义名数量 : " & synonyms.Count.ToString())
        Console.WriteLine("解析结果       : " & bestName)
        Console.WriteLine("期望结果       : Troxerutin")
        Console.WriteLine("----------------------------------------------------")
        If bestName = "Troxerutin" Then
            Console.WriteLine("测试结果       : 通过 [OK]")
        Else
            Console.WriteLine("测试结果       : 失败 [FAIL]")
        End If
        Console.WriteLine("====================================================")
        Console.ReadLine()
    End Sub

End Module
