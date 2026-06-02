require(biocad_registry);

imports "registry" from "biocad_registry";

let biocad_registry = open_registry("root", 123456, host ="192.168.3.48");
let names = read.csv("Z:\anno_lib.csv", row.names = NULL, check.names = FALSE);

print(names);

biocad_registry |> imports_chineseName(id = names$id,
                                    names = names$name_zh);