// export R# package module type define for javascript/typescript language
//
//    imports "models" from "biocad_registry";
//
// ref=biocadRegistry.registry_models@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace models {
   /**
   */
   function build_marine_nps(registry: object): ;
   /**
   */
   function build_microbial_nps(registry: object): ;
   /**
   */
   function build_plantnp_library(registry: object): ;
   /**
     * @param batch_size default value Is ``10000000``.
     * @param env default value Is ``null``.
   */
   function diamond_transaction(blastp: any, dir: string, batch_size?: object, env?: object): any;
   /**
     * @param check_unique default value Is ``false``.
     * @param batch_size default value Is ``500000``.
     * @param block_size default value Is ``100000``.
     * @param env default value Is ``null``.
   */
   function imports_diamond(registry: object, blastp: any, check_unique?: boolean, batch_size?: object, block_size?: object, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_interprodb(registry: object, interpro: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_metatraits(registry: object, metatraits: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_pathways(registry: object, pathways: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function link_prot_ko(registry: object, kofamscan: any, env?: object): any;
   /**
     * @param cutoff default value Is ``65``.
     * @param eval_cutoff default value Is ``1E-05``.
   */
   function make_protein_clusters(registry: object, cutoff?: number, eval_cutoff?: number): any;
   /**
    * register metabolite symbol and make updates of the reaction hashcode
    * 
    * 
     * @param registry -
   */
   function register_metabolic_symbols(registry: object): ;
   /**
   */
   function resolve_metabolite_alias(registry: object): ;
   /**
    * make updates of the compartment location metadata
    * 
    * 
     * @param registry -
     * @param locations -
   */
   function update_location(registry: object, locations: object): ;
   /**
    * Fill metabolite species id inside metabolic network
    * 
    * 
     * @param registry -
   */
   function update_metabolic_network(registry: object): ;
   /**
   */
   function update_symbolname(registry: object): ;
}
