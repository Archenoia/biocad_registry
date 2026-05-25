// export R# package module type define for javascript/typescript language
//
//    imports "registry" from "biocad_registry";
//
// ref=biocadRegistry.registry@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace registry {
   /**
     * @param env default value Is ``null``.
   */
   function import_brenda_enzymes(registry: object, brenda: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function import_refseq(registry: object, refseq: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_chineseName(registry: object, id: any, names: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_coconut(registry: object, coconut: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_kegg_reactions(registry: object, kegg: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_lotus(registry: object, lotus: any, env?: object): any;
   /**
    * a general function for make metabolites data imports
    * 
    * 
     * @param registry -
     * @param metab -
     * @param db_name -
     * @param check_id -
     * 
     * + default value Is ``true``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function imports_metab_repo(registry: object, metab: any, db_name: string, check_id?: boolean, env?: object): any;
   /**
   */
   function imports_metacyc_compounds(registry: object, metacyc: object): any;
   /**
   */
   function imports_metacyc_reactions(registry: object, metacyc: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_mona(registry: object, mona: any, env?: object): any;
   /**
    * 
    * 
     * @param registry -
     * @param motifs -
     * @param tf PlantTFDB TF fasta sequence collection
     * @param taxname 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function imports_planttfdb(registry: object, motifs: any, tf: any, taxname?: string, env?: object): any;
   /**
     * @param skip_prefix default value Is ``0``.
     * @param env default value Is ``null``.
   */
   function imports_pubchem(registry: object, pubchem: any, skip_prefix?: object, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_rhea(registry: object, rhea: any, env?: object): any;
   /**
     * @param updates default value Is ``false``.
     * @param env default value Is ``null``.
   */
   function imports_sabiork(registry: object, xmlfiles: any, updates?: boolean, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_sbml_reactions(registry: object, sbml: object, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_spectraverse(registry: object, spectraverse: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_struct_domains(registry: object, uniprot: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function make_genbank_dbxrefs(registry: object, genbank: any, env?: object): any;
   /**
   */
   function metabolite_translation(registry: object, ontology: string): ;
   /**
   */
   function ontology_translation(registry: object, ontology: string): ;
   /**
     * @param env default value Is ``null``.
   */
   function save_genbank(registry: object, genbank: any, env?: object): any;
   /**
    * Save RegPrecise regulation network
    * 
    * 
     * @param registry -
     * @param genomes -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function save_TRN(registry: object, genomes: any, env?: object): any;
   /**
     * @param process_none_enzyme default value Is ``true``.
     * @param env default value Is ``null``.
   */
   function save_uniprot(registry: object, uniprot: any, process_none_enzyme?: boolean, env?: object): any;
   /**
   */
   function update_kinetics_registry(registry: object): any;
   /**
   */
   function update_logo(registry: object): any;
}
