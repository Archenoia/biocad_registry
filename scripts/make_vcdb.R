require(biocad_registry);

imports "exports" from "biocad_registry";

let biocad_registry = open_registry("root", 123456, host ="192.168.3.48");
let repo = relative_work("./localdb");

biocad_registry |> exports::virtualcell_componentdb(repo,

    tfbs = FALSE,
    tf = FALSE,
    cc = FALSE,
    ec = FALSE,
    rxn = TRUE,
    metab = TRUE

);