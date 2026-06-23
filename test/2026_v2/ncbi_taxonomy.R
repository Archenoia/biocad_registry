require(biocad_registry);

imports "setup" from "biocad_registry";
imports "taxonomy_kit" from "metagenomics_kit";

let registry = open_registry("root", 123456, host ="192.168.3.48");
# let ncbi_tax = Ncbi.taxonomy_tree("U:\metagenomics_LLMs\taxdmp_2025-12-01");
let tax_map = read.table("C:\Users\Administrator\Downloads\NCBI2GTDB_lineage.tsv", header = TRUE);

# setup::setup_taxonomy(registry,ncbi_tax);

print(tax_map, max.print = 6);
stop();

map_ncbi_gtdb_taxonomy(registry, ncbi_tax, GTDB_tax);