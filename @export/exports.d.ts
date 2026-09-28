// export R# package module type define for javascript/typescript language
//
//    imports "exports" from "biocad_registry";
//
// ref=biocadRegistry.exports@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace exports {
   /**
    * Export smiles table for run molecule strucutre analysis
    * 
    * 
     * @param registry -
     * @param dbname -
     * 
     * + default value Is ``null``.
     * @param topic 
     * + default value Is ``null``.
   */
   function export_smiles_data(registry: object, dbname?: string, topic?: string): object;
   /**
   */
   function export_species_sequence(registry: object, file: object): any;
   /**
     * @param dbname default value Is ``null``.
   */
   function metabolite_table(registry: object, dbname?: string): any;
   /**
    * Make exports of the component models for run virtual cell annotations
    * 
    * 
     * @param registry -
     * @param repo -
     * @param tfbs 
     * + default value Is ``true``.
     * @param tf 
     * + default value Is ``true``.
     * @param cc 
     * + default value Is ``true``.
     * @param ec 
     * + default value Is ``true``.
     * @param rxn 
     * + default value Is ``true``.
     * @param metab 
     * + default value Is ``true``.
   */
   function virtualcell_componentdb(registry: object, repo: string, tfbs?: boolean, tf?: boolean, cc?: boolean, ec?: boolean, rxn?: boolean, metab?: boolean): any;
}
