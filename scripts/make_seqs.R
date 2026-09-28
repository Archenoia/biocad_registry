require(biocad_registry);

imports "exports" from "biocad_registry";
imports "bioseq.fasta" from "seqtoolkit";

let file = open.fasta(here("species.faa"), read = FALSE);
let biocad_registry = open_registry("root", 123456, host ="192.168.3.48");

export_species_sequence( biocad_registry, file);
close(file);
