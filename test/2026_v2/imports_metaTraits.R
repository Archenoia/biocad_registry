require(biocad_registry);
require(GCModeller);

imports "models" from "biocad_registry";
imports "microbiome" from "metagenomics_kit";

let registry = open_registry("root", 123456, host ="192.168.3.48");
let traisdata = load.meta_traits("C:\Users\Administrator\Downloads\ncbi_species_summary_no_predictions.tsv");

registry |> imports_metatraits(traisdata );