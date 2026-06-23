// export R# source type define for javascript/typescript language
//
// package_source=biocad_registry

declare namespace biocad_registry {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
   */
   function __push_compound_metadata(biocad_registry: any, compound: any, mol: any): object;
   /**
   */
   function cast_kegg_reaction(rxn: any): object;
   /**
   */
   function check_metabolite(biocad_registry: any, compound: any): object;
   /**
   */
   function check_metabolite_synonym(biocad_registry: any, compound: any): object;
   /**
   */
   function enzyme_regulation(biocad_registry: any): object;
   /**
   */
   function export_enzymatic(biocad_registry: any): object;
   /**
     * @param fasta default value Is ``true``.
   */
   function export_genomics_fasta(biocad_registry: any, parent_taxname: any, fasta?: any): object;
   /**
   */
   function export_reactionLinks(biocad_registry: any): object;
   /**
   */
   function find_molecule(biocad_registry: any, meta: any, xref_id: any): object;
   /**
   */
   function gene_term(biocad_registry: any): object;
   /**
     * @param repo_dir default value Is ``./``.
   */
   function get_genbank(asm_id: any, repo_dir?: any): object;
   /**
     * @param repo_dir default value Is ``./``.
   */
   function get_genbank_ftp(ncbi: any, url: any, repo_dir?: any): object;
   /**
     * @param dbname default value Is ``null``.
   */
   function get_molecule_by_dbxref(registry: any, db_xref: any, dbname?: any): object;
   /**
   */
   function imports_chebi(biocad_registry: any, chebi: any): object;
   module imports_genebank {
      /**
      */
      function obsolete(biocad_registry: any, genebank: any): object;
   }
   /**
   */
   function imports_genomic_refseq(biocad_registry: any, gbff: any): object;
   /**
   */
   function imports_kegg(biocad_registry: any, kegg: any): object;
   /**
   */
   function imports_kegg_reaction(biocad_registry: any, kegg: any): object;
   /**
   */
   function imports_metacyc(biocad_registry: any, metacyc: any): object;
   /**
     * @param fast_check default value Is ``false``.
   */
   function imports_odor(biocad_registry: any, pubchem: any, fast_check?: any): object;
   /**
   */
   function imports_pubchem(biocad_registry: any, pubchem: any): object;
   /**
     * @param rhea default value Is ``./rhea.rdf``.
   */
   function imports_rhea(biocad_registry: any, rhea?: any): object;
   /**
     * @param fast_check default value Is ``false``.
   */
   function imports_uniprot_obsolete(biocad_registry: any, uniprot: any, fast_check?: any): object;
   /**
   */
   function link_gene_proteins(biocad_registry: any): object;
   /**
   */
   function link_kinetics(registry: any): object;
   /**
   */
   function link_reaction_enzymes(biocad_registry: any): object;
   /**
     * @param refresh default value Is ``false``.
   */
   function link_reaction_metabolites(biocad_registry: any, refresh?: any): object;
   /**
   */
   function load_biocyc_compounds(biocad_registry: any, metacyc: any): object;
   /**
   */
   function load_biocyc_genes(biocad_registry: any, metacyc: any): object;
   /**
   */
   function load_biocyc_proteins(biocad_registry: any, metacyc: any): object;
   /**
   */
   function load_biocyc_reactions(biocad_registry: any, metacyc: any): object;
   /**
   */
   function map_ncbi_gtdb_taxonomy(registry: any, ncbi_tax: any, GTDB_tax: any): object;
   /**
   */
   function metabolite_term(biocad_registry: any): object;
   /**
   */
   function molecule_entity(biocad_registry: any): object;
   /**
   */
   function molecule_terms(biocad_registry: any): object;
   /**
     * @param host default value Is ``localhost``.
     * @param port default value Is ``3306``.
     * @param dbname default value Is ``cad_registry``.
   */
   function open_registry(user: any, passwd: any, host?: any, port?: any, dbname?: any): object;
   /**
   */
   function protein_term(biocad_registry: any): object;
   /**
   */
   function push_reaction(biocad_registry: any, reaction: any, source_db: any): object;
   /**
   */
   function query_refseq(registry: any, taxname: any): object;
   /**
   */
   function reaction_model(biocad_registry: any): object;
   /**
   */
   function rna_term(biocad_registry: any): object;
   /**
   */
   function save_nucleotide_embedding(biocad_registry: any, mol_id: any, dnaseq: any, sgt: any, Nucleotide_graph: any): object;
}
