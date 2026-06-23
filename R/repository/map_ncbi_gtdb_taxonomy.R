const map_ncbi_gtdb_taxonomy = function(registry, ncbi_tax, GTDB_tax) {
    let ncbi_taxtree = registry |> table("ncbi_taxonomy");

    for(let map in tqdm(as.list(data.frame(ncbi_tax = ncbi_tax, GTDB_tax = GTDB_tax), byrow = TRUE))) {
        ncbi_taxtree 
        |> where(id = map$ncbi_tax) 
        |> save(GTDB_id = map$GTDB_tax )
        ;
    }
}