require(biocad_registry);

imports "registry" from "biocad_registry";

let registry = open_registry("root", 123456, host ="192.168.3.48");

update_kinetics_registry(registry);