require(biocad_registry);

imports "models" from "biocad_registry";

let registry = open_registry("root", 123456, host ="192.168.3.48");

# Fill metabolite species id inside metabolic network
registry |> update_metabolic_network();
# registry |> register_metabolic_symbols();