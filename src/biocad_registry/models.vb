
Imports System.ComponentModel
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem.ExtensionModels
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Workbench
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Analysis.Metagenome.MetaFunction.metaTraits
Imports SMRUCC.genomics.Analysis.SequenceTools.HMMER
Imports SMRUCC.genomics.Analysis.SequenceTools.HMMER.InterPro.Xml
Imports SMRUCC.genomics.ComponentModel.Annotation
Imports SMRUCC.genomics.Interops.NCBI.Extensions
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

<Package("models")>
<RTypeExport("cad_registry", GetType(biocad_registry))>
Public Module registry_models

    Friend Sub Main()
    End Sub

    ''' <summary>
    ''' make updates of the compartment location metadata
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="locations"></param>
    <ExportAPI("update_location")>
    Public Sub update_location(registry As biocad_registry, locations As dataframe)
        Dim name As String() = CLRVector.asCharacter(locations!name)
        Dim zh_names As String() = CLRVector.asCharacter(locations!zh_name)
        Dim membrane As Integer() = CLRVector.asInteger(locations!membrane)
        Dim updates As CommitTransaction = registry.compartment_location.ignore.open_transaction
        Dim term_location As UInteger = registry.biocad_vocabulary.GetRegistryEntity(biocad_vocabulary.EntitySubcellularLocation).id

        For i As Integer = 0 To locations.nrows - 1
            Dim loc As compartment_location = registry.compartment_location _
                .where(field("name") = name(i)) _
                .find(Of compartment_location)

            If Not loc Is Nothing Then
                Dim term As String = loc.name.Replace(" ", "_")

                Call updates.add(registry.compartment_location _
                            .where(field("id") = loc.id) _
                            .save_sql(field("zh_name") = zh_names(i),
                                      field("membrane") = membrane(i)))

                If registry.registry_resolver _
                    .where(field("symbol_id") = loc.id,
                           field("type") = term_location,
                           field("register_name") = term) _
                    .find(Of registry_resolver) Is Nothing Then

                    Call updates.add(registry.registry_resolver.add_sql(
                        field("symbol_id") = loc.id,
                        field("type") = term_location,
                        field("register_name") = term
                    ))
                End If
            End If
        Next

        Call updates.commit()
    End Sub

    <ExportAPI("imports_metatraits")>
    Public Function imports_metatraits(registry As biocad_registry, <RRawVectorArgument> metatraits As Object, Optional env As Environment = Nothing)
        Dim traits As PipeIterator(Of metaTraitData) = pipeline.Stream(Of metaTraitData)(metatraits, env)

        If traits.isError Then
            Return traits.getError
        End If

        Dim metaTraits_id As UInteger = registry.biocad_vocabulary.GetDatabaseResource("metaTraits")

        For Each genome In TqdmWrapper.Wrap(traits.getData)

        Next

        Return 0
    End Function

    ''' <summary>
    ''' Fill metabolite species id inside metabolic network
    ''' </summary>
    ''' <param name="registry"></param>
    <ExportAPI("update_metabolic_network")>
    Public Sub update_metabolic_network(registry As biocad_registry)
        Call registry.UpdateMetabolicNetwork
    End Sub

    ''' <summary>
    ''' register metabolite symbol and make updates of the reaction hashcode
    ''' </summary>
    ''' <param name="registry"></param>
    <ExportAPI("register_metabolic_symbols")>
    Public Sub register_metabolic_symbols(registry As biocad_registry)
        Call registry.RegisterMetabolicSymbols
    End Sub

    <ExportAPI("update_symbolname")>
    Public Sub update_symbolname(registry As biocad_registry)
        Call registry.UpdateMetaboliteSymbolName
    End Sub

    <ExportAPI("resolve_metabolite_alias")>
    Public Sub update_metabolite_models(registry As biocad_registry)
        Call registry.MetaboliteLinks(check_hash:=True)
    End Sub

    <ExportAPI("build_plantnp_library")>
    Public Sub build_plantnp(registry As biocad_registry)
        Call TopicViews.PlantNP(registry)
    End Sub

    <ExportAPI("build_microbial_nps")>
    Public Sub build_fermertation_np(registry As biocad_registry)
        Call TopicViews.MicrobialNP(registry)
    End Sub

    <ExportAPI("build_marine_nps")>
    Public Sub build_marineNPs(registry As biocad_registry)
        Call TopicViews.MarineNPs(registry)
    End Sub

    <ExportAPI("imports_pathways")>
    Public Function imports_pathways(registry As biocad_registry, <RRawVectorArgument> pathways As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of PathwayGraph)(pathways, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim vocabulary As biocad_vocabulary = registry.biocad_vocabulary
        Dim metab_class = vocabulary.metabolite_type
        Dim enzyme_class = vocabulary.db_ECNumber
        Dim prot_class = vocabulary.protein_data
        Dim db_uniprot = vocabulary.db_uniprot

        For Each pwy As PathwayGraph In TqdmWrapper.Wrap(pull.populates(Of PathwayGraph)(env).ToArray)
            Dim source_db As UInteger = vocabulary.GetDatabaseResource(pwy.source).id
            Dim check = registry.pathway _
                .where(field("db_source") = source_db,
                       field("accession_id") = pwy.pwacc) _
                .find(Of biocad_registryModel.pathway)
            Dim links As CommitTransaction = registry.pathway_network.ignore.open_transaction

            If check Is Nothing Then
                Call registry.pathway.add(
                    field("db_source") = source_db, field("accession_id") = pwy.pwacc,
                    field("name") = If(pwy.name, pwy.pwacc),
                    field("type") = pwy.pwtype,
                    field("taxid") = CInt(Val(pwy.taxid)),
                    field("dois") = pwy.dois.SafeQuery.Where(Function(did) Not did.StringEmpty(, True)).GetJson,
                    field("note") = pwy.citations.SafeQuery.Where(Function(ct) Not ct.StringEmpty(, True)).JoinBy("; ")
                )
                check = registry.pathway _
                    .where(field("db_source") = source_db,
                           field("accession_id") = pwy.pwacc) _
                    .order_by("id", desc:=True) _
                    .find(Of biocad_registryModel.pathway)
            End If

            If check Is Nothing Then
                Throw New InvalidOperationException("create new pathway model error: " & registry.pathway.GetLastErrorMessage)
            End If

            For Each cid As String In pwy.cids.SafeQuery
                If cid.StringEmpty(, True) Then
                    Continue For
                End If

                Dim check_link = registry.pathway_network _
                    .where(field("pathway_id") = check.id,
                           field("symbol_id") = cid,
                           field("class_id") = metab_class) _
                    .find(Of pathway_network)

                If check_link Is Nothing Then
                    Dim metab = registry.metabolites.where(field("pubchem_cid") = cid).find(Of metabolites)

                    Call links.add(
                        field("pathway_id") = check.id,
                        field("symbol_id") = cid,
                        field("class_id") = metab_class,
                        field("note") = pwy.name & $" [{If(metab Is Nothing, $"CID:{cid}", metab.name)}]",
                        field("model_id") = If(metab Is Nothing, 0, metab.id)
                    )
                End If
            Next

            For Each ec_id As String In pwy.ecs.SafeQuery
                If ec_id.StringEmpty(, True) Then
                    Continue For
                End If

                Dim check_link = registry.pathway_network _
                    .where(field("pathway_id") = check.id,
                           field("symbol_id") = ec_id,
                           field("class_id") = enzyme_class) _
                    .find(Of pathway_network)

                If check_link Is Nothing Then
                    Dim ecnum As ECNumber = ECNumber.ValueParser(ec_id)
                    Dim enz = registry.enzyme _
                        .where(field("enzyme_class") = CInt(ecnum.type),
                               field("sub_class") = ecnum.subType,
                               field("sub_category") = ecnum.subCategory,
                               field("enzyme_number") = ecnum.serialNumber) _
                        .find(Of biocad_registryModel.enzyme)

                    Call links.add(
                        field("pathway_id") = check.id,
                        field("symbol_id") = ec_id,
                        field("class_id") = enzyme_class,
                        field("note") = pwy.name & $" [{ec_id}]",
                        field("model_id") = If(enz Is Nothing, 0, enz.id)
                    )
                End If
            Next

            For Each prot_id As String In pwy.protacxns.SafeQuery
                If prot_id.StringEmpty(, True) Then
                    Continue For
                End If

                Dim check_link = registry.pathway_network _
                    .where(field("pathway_id") = check.id,
                           field("symbol_id") = prot_id,
                           field("class_id") = prot_class) _
                    .find(Of pathway_network)

                If check_link Is Nothing Then
                    Dim uniprot = registry.db_xrefs _
                        .where(field("type") = prot_class,
                               field("db_name") = db_uniprot,
                               field("db_xref") = prot_id) _
                        .find(Of biocad_registryModel.db_xrefs)

                    Call links.add(
                        field("pathway_id") = check.id,
                        field("symbol_id") = prot_id,
                        field("class_id") = prot_class,
                        field("note") = pwy.name & $" [{prot_id}]",
                        field("model_id") = If(uniprot Is Nothing, 0, uniprot.obj_id)
                    )
                End If
            Next

            Call links.commit()
        Next

        Return Nothing
    End Function

    <ExportAPI("diamond_transaction")>
    Public Function diamond_transaction(<RRawVectorArgument> blastp As Object, dir As String,
                                        Optional batch_size As Integer = 10000000,
                                        Optional env As Environment = Nothing)

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of DiamondAnnotation)(blastp, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim part As i32 = 1

        For Each block As DiamondAnnotation() In pull.populates(Of DiamondAnnotation)(env).SplitIterator(batch_size)
            Call block _
                .CreateDiamondDumpData _
                .DumpLargeTransaction(path:=$"{dir}/diamond_{++part}.sql", distinct:=False)
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_diamond")>
    Public Function imports_diamond(registry As biocad_registry, <RRawVectorArgument> blastp As Object,
                                    Optional check_unique As Boolean = False,
                                    Optional batch_size As Integer = 500000,
                                    Optional block_size As Integer = 100000,
                                    Optional env As Environment = Nothing)

        Dim pull As pipeline = pipeline.TryCreatePipeline(Of DiamondAnnotation)(blastp, env)

        If pull.isError Then
            Return pull.getError
        End If

        For Each block As DiamondAnnotation() In pull.populates(Of DiamondAnnotation)(env).SplitIterator(batch_size)
            Dim insert As CommitTransaction = registry.protein_cluster.open_transaction(blockSize:=block_size)

            For Each hit As DiamondAnnotation In block
                Dim qid As UInteger = hit.QseqId.Split("|"c).Last
                Dim sid As UInteger = hit.SseqId.Split("|"c).Last
                Dim make_insert As Boolean = True

                If check_unique Then
                    make_insert = registry.protein_cluster _
                        .where(field("query_id") = qid,
                               field("hit_id") = sid) _
                        .find(Of biocad_registryModel.protein_cluster) Is Nothing
                End If

                If make_insert Then
                    Call insert.add(
                        field("query_id") = qid,
                           field("hit_id") = sid,
                           field("identities") = hit.Pident,
                           field("mis_match") = hit.Mismatch,
                           field("gap_open") = hit.GapOpen,
                           field("q_start") = hit.QStart,
                           field("q_end") = hit.QEnd,
                           field("s_start") = hit.SStart,
                           field("s_end") = hit.SEnd,
                           field("e_value") = hit.EValue,
                           field("bit_score") = hit.BitScore
                    )
                End If
            Next

            Call insert.commit()
        Next

        Return Nothing
    End Function

    <ExportAPI("make_protein_clusters")>
    Public Function make_protein_clusters(registry As biocad_registry, Optional cutoff As Double = 65, Optional eval_cutoff As Double = 0.00001)
        Dim page_size As Integer = 10000
        Dim n_clusters As Integer = 1

        ' 定义每批次处理的最大ID数量，防止SQL语句过长
        Const BATCH_SIZE As Integer = 500

        ' UPDATE `cad_registry`.`protein_data` SET `cluster_id` = '0'

        Call $"make protein cluster with identities cutoffs > {cutoff} and e-value < {eval_cutoff}".debug

        For page As Integer = 1 To Integer.MaxValue
            Dim offset = (page - 1) * page_size
            Dim protein_ids As UInteger() = registry.protein_cluster _
                .limit(offset, page_size) _
                .distinct _
                .project(Of UInteger)("query_id")

            If protein_ids.IsNullOrEmpty Then
                Exit For
            End If

            For Each cluster_key As UInteger In protein_ids
                Dim protein As protein_data = registry.protein_data _
                    .where(field("id") = cluster_key) _
                    .find(Of protein_data)("id", "cluster_id", "`function`")

                If protein.cluster_id > 0 Then
                    ' 如果已经被归簇，跳过
                    Continue For
                Else
                    ' 设置为当前簇的根
                    Call registry.protein_data _
                        .where(field("id") = cluster_key) _
                        .save(field("cluster_id") = cluster_key)
                End If

                ' 2. BFS 搜索队列
                Dim queue As New List(Of UInteger) From {cluster_key}
                Dim visited As New HashSet(Of UInteger) From {cluster_key}

                Do While queue.Count > 0
                    ' 取出当前层级的 ID 列表
                    Dim current_batch_ids = queue.Take(BATCH_SIZE).ToList()
                    queue = queue.Skip(BATCH_SIZE).AsList()

                    ' 3. 查找邻居 (优化：只查 ID)
                    ' 注意：这里假设 protein_cluster 包含双向数据
                    ' 建议添加 index: (query_id, identities, hit_id)
                    ' 3. 双向查询邻居
                    ' 3.1 查询hit方向的邻居
                    Dim neighbor_ids_from_hit = registry.protein_cluster _
                        .left_join("`protein_data` as pd1") _
                        .on(field("`pd1`.id") = field("hit_id")) _
                        .where(field("query_id").in(current_batch_ids),
                               field("identities") > cutoff,
                               field("e_value") < eval_cutoff,
                               field("`pd1`.cluster_id") = 0) _
                        .project(Of UInteger)("`pd1`.id")

                    ' 3.2 查询query方向的邻居
                    Dim neighbor_ids_from_query = registry.protein_cluster _
                        .left_join("`protein_data` as pd2") _
                        .on(field("`pd2`.id") = field("query_id")) _
                        .where(field("hit_id").in(current_batch_ids),
                               field("identities") > cutoff,
                               field("e_value") < eval_cutoff,
                               field("`pd2`.cluster_id") = 0) _
                        .project(Of UInteger)("`pd2`.id")

                    ' 合并并去重
                    Dim neighbor_ids = neighbor_ids_from_hit _
                        .Union(neighbor_ids_from_query) _
                        .Distinct() _
                        .Where(Function(id) Not visited.Contains(id)) _
                        .ToArray()

                    If Not neighbor_ids.IsNullOrEmpty Then
                        For Each block As UInteger() In neighbor_ids.SplitIterator(BATCH_SIZE, echo:=False)
                            ' 4. 批量更新归簇 (分批更新防止SQL过长)
                            ' 这里简化逻辑，实际建议对 neighbor_ids 也分批 Update
                            registry.protein_data _
                                .where(field("id").in(block)) _
                                .save(field("cluster_id") = cluster_key)
                        Next

                        ' 5. 将新发现的邻居加入队列，继续向外扩展
                        ' 加入队列（去重）
                        For Each new_neighbor As UInteger In neighbor_ids
                            If Not visited.Contains(new_neighbor) Then
                                visited.Add(new_neighbor)
                                queue.Add(new_neighbor)
                            End If
                        Next

                        Call $"[protein_cluster {n_clusters}: {protein.function}] join {neighbor_ids.Length} cluster neighbors, queue {queue.Count} cluster members!".debug
                    End If
                Loop

                n_clusters += 1
            Next
        Next

        Return Nothing
    End Function

    <ExportAPI("link_prot_ko")>
    Public Function link_ko(registry As biocad_registry, <RRawVectorArgument> kofamscan As Object, Optional env As Environment = Nothing)
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of KOFamScan)(kofamscan, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim protein_type As UInteger = registry.biocad_vocabulary.protein_type
        Dim kegg As UInteger = registry.biocad_vocabulary.db_kegg

        For Each block In pull.populates(Of KOFamScan)(env).Where(Function(a) a.flag).SplitIterator(10000)
            Dim update As CommitTransaction = registry.protein_data.open_transaction

            Try
                For Each prot As KOFamScan In TqdmWrapper.Wrap(block)
                    Dim prot_id As UInteger = UInteger.Parse(prot.gene_name.Split("|"c).Last)
                    Dim ko = registry.db_xrefs _
                        .where(field("type") = protein_type,
                               field("db_name") = kegg,
                               field("db_xref") = prot.KO) _
                        .find(Of biocad_registryModel.db_xrefs)

                    If Not ko Is Nothing Then
                        Call update.add(registry.protein_data.where(field("id") = prot_id).save_sql(field("protein_id") = ko.obj_id))
                    End If
                Next
            Catch ex As Exception

            End Try

            Call update.commit()
        Next

        Return Nothing
    End Function

    <ExportAPI("imports_interprodb")>
    Public Function imports_interprodb(registry As biocad_registry, <RRawVectorArgument> interpro As Object, Optional env As Environment = Nothing) As Object
        Dim pull As pipeline = pipeline.TryCreatePipeline(Of Interpro)(interpro, env)

        If pull.isError Then
            Return pull.getError
        End If

        Dim db_interpro As UInteger = registry.biocad_vocabulary.GetDatabaseResource("InterPro").id
        Dim insert As CommitTransaction = registry.ontology.open_transaction

        For Each term As Interpro In TqdmWrapper.Wrap(pull.populates(Of Interpro)(env).ToArray)
            Dim check = registry.ontology.where(field("term_id") = term.id, field("ontology_id") = db_interpro).find(Of ontology)

            If check Is Nothing Then
                Call insert.add(
                    field("term_id") = term.id,
                    field("term") = term.name,
                     field("ontology_id") = db_interpro,
                     field("note") = If(term.abstract Is Nothing, "", term.abstract.ToString)
                )
            End If
        Next

        Call insert.commit()

        Return Nothing
    End Function
End Module
