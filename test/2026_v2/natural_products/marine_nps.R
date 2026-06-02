require(biocad_registry);

imports "models" from "biocad_registry";

let registry = open_registry("root", 123456, host ="192.168.3.48");

build_marine_nps(registry);