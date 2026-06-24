require(biocad_registry);
require(GCModeller);

imports "models" from "biocad_registry";
imports "microbiome" from "metagenomics_kit";

let registry = open_registry("root", 123456, host ="192.168.3.48");

for(file in ["C:\Users\Administrator\Downloads\ncbi_species_summary_no_predictions.tsv"
"C:\Users\Administrator\Downloads\ncbi_genus_summary_all.tsv"
"C:\Users\Administrator\Downloads\ncbi_family_summary_all.tsv"
"C:\Users\Administrator\Downloads\ncbi_species_summary_all.tsv"
]) {

    let traisdata = load.meta_traits(file);

    registry |> imports_metatraits(traisdata );
}

