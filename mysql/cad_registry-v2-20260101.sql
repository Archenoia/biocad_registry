CREATE DATABASE  IF NOT EXISTS `cad_registry` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cad_registry`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 192.168.3.48    Database: cad_registry
-- ------------------------------------------------------
-- Server version	9.7.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ '67642701-4d4a-11f1-ab5f-e23ecd42db51:1-5540203';

--
-- Table structure for table `compartment_enrich`
--

DROP TABLE IF EXISTS `compartment_enrich`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compartment_enrich` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `metabolite_id` int unsigned NOT NULL,
  `location_id` int unsigned NOT NULL,
  `evidence` varchar(1024) DEFAULT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_link` (`metabolite_id`,`location_id`),
  KEY `metabolite_data_idx` (`metabolite_id`),
  KEY `location_data_idx` (`location_id`)
) ENGINE=InnoDB AUTO_INCREMENT=70545 DEFAULT CHARSET=utf8mb3 COMMENT='metabolite ~ compartment location association';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `compartment_location`
--

DROP TABLE IF EXISTS `compartment_location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compartment_location` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(512) NOT NULL COMMENT 'symbol name that used in virtual cell model',
  `zh_name` varchar(45) DEFAULT NULL,
  `fullname` varchar(1024) NOT NULL COMMENT 'full name of this location',
  `membrane` tinyint(1) NOT NULL DEFAULT '0',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` mediumtext COMMENT 'description note about this cellular location',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  FULLTEXT KEY `search_text` (`fullname`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=397 DEFAULT CHARSET=utf8mb3 COMMENT='[cellular entity model] cellular location name';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `complex`
--

DROP TABLE IF EXISTS `complex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `complex` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `protein_id` int unsigned NOT NULL COMMENT 'id of the protein complex entity which is defined inside protein',
  `compound_id` int unsigned NOT NULL COMMENT 'the one of the compound of this protein complex entity, compound could be metabolite, gene(rna) and another protein ',
  `compound_type` int unsigned NOT NULL COMMENT 'the entity type of this compound object',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_compound` (`protein_id`,`compound_id`,`compound_type`),
  KEY `metabolite_compound_idx` (`compound_id`),
  KEY `protein_complex_idx` (`protein_id`),
  KEY `entity_namespace_idx` (`compound_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='components for used for protein complex';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `conserved_domain`
--

DROP TABLE IF EXISTS `conserved_domain`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `conserved_domain` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `protein_id` int unsigned NOT NULL COMMENT 'id reference to the protein data object',
  `domain_id` int unsigned NOT NULL COMMENT 'id reference to the protein domain ontology table',
  `left` int unsigned NOT NULL COMMENT 'start location of the sequence',
  `right` int unsigned NOT NULL COMMENT 'end location on the protein sequence',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `unique_hit` (`protein_id`,`domain_id`,`left`)
) ENGINE=InnoDB AUTO_INCREMENT=1320439 DEFAULT CHARSET=utf8mb3 COMMENT='protein domain data ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `conserved_operon`
--

DROP TABLE IF EXISTS `conserved_operon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `conserved_operon` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL COMMENT 'the conserved operon name',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='conserved gene operon cluster across multiple species';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `db_xrefs`
--

DROP TABLE IF EXISTS `db_xrefs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `db_xrefs` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `obj_id` int unsigned NOT NULL COMMENT 'object reference id to the metabolites/genes/protein/compartment location/reaction table',
  `type` int unsigned NOT NULL COMMENT 'entity type of the object that associated with this database corss reference id',
  `db_name` int unsigned NOT NULL COMMENT 'database name of the db_xref id',
  `db_xref` varchar(500) NOT NULL COMMENT 'database cross reference id',
  `db_source` int unsigned NOT NULL COMMENT 'source trace of this relation ship where it comes from',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_xref` (`obj_id`,`type`,`db_name`,`db_xref`,`db_source`),
  KEY `source_name_idx` (`db_source`),
  KEY `entity_namespace_idx` (`type`),
  KEY `dbname_idx` (`db_name`),
  KEY `entity_metabolite_idx` (`obj_id`),
  KEY `find_by_xref` (`db_name`,`db_xref`,`type`),
  KEY `find_by_object` (`type`,`obj_id`),
  KEY `search_xref_word` (`db_xref`)
) ENGINE=InnoDB AUTO_INCREMENT=16230458 DEFAULT CHARSET=utf8mb3 COMMENT='database cross reference of the model objects ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dois`
--

DROP TABLE IF EXISTS `dois`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dois` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `doi` varchar(255) NOT NULL,
  `title` mediumtext NOT NULL,
  `journal` varchar(255) NOT NULL,
  `year` int unsigned NOT NULL DEFAULT '0',
  `pmid` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `doi_UNIQUE` (`doi`),
  FULLTEXT KEY `search_text` (`title`,`note`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `enzyme`
--

DROP TABLE IF EXISTS `enzyme`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `enzyme` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `enzyme_class` int unsigned NOT NULL DEFAULT '0' COMMENT 'First digit, Main enzyme class (EC X), Indicates the broad category of reaction catalyzed (e.g., oxidation-reduction, transfer, hydrolysis).',
  `sub_class` int unsigned NOT NULL DEFAULT '0' COMMENT 'the second digit, Specifies the type of reaction within the main class, often based on the type of bond or chemical group involved.',
  `sub_category` int unsigned NOT NULL DEFAULT '0' COMMENT 'the third digit, Further defines the reaction mechanism or the specific type of chemical transformation.',
  `enzyme_number` int unsigned NOT NULL DEFAULT '0' COMMENT 'the fourth digit, enzyme serial number, the identifier in this class, Uniquely identifies the specific enzyme that catalyzes a reaction within the given subclass.',
  `recommended_name` varchar(255) NOT NULL,
  `hashcode` char(32) NOT NULL COMMENT 'md5 hashcode of the lower recommended_name string',
  `systematic_name` mediumtext,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `check_ecnumber` (`enzyme_class`,`sub_class`,`sub_category`,`enzyme_number`),
  KEY `find_name_hash` (`hashcode`),
  FULLTEXT KEY `search_text` (`recommended_name`,`systematic_name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=8055 DEFAULT CHARSET=utf8mb3 COMMENT='a table of the metabolic enzyme data model';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `gene`
--

DROP TABLE IF EXISTS `gene`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gene` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL COMMENT 'gene name',
  `function` mediumtext COMMENT 'gene product function description text',
  `gene_cluster` int unsigned NOT NULL DEFAULT '0' COMMENT 'id reference to the conserved operon table, to indicates that this gene model is belongs to which conserved gene clusters. default zero means this gene not belongs to any cluster',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `cluster_info_idx` (`gene_cluster`),
  KEY `name_hit` (`name`),
  FULLTEXT KEY `search_text` (`function`,`note`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='[cellular entity model] a table of the reference gene model, just defined the gene object at here';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `gene_ontology`
--

DROP TABLE IF EXISTS `gene_ontology`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gene_ontology` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `gene_id` int unsigned NOT NULL,
  `ontology_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_feature` (`gene_id`,`ontology_id`),
  KEY `ontology_term_idx` (`ontology_id`),
  KEY `gene_data_idx` (`gene_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `kinetics_law`
--

DROP TABLE IF EXISTS `kinetics_law`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kinetics_law` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(64) NOT NULL,
  `db_source` int unsigned NOT NULL,
  `ec_number` varchar(16) NOT NULL COMMENT 'ec number of this kinetics law(or enzyme protein)',
  `enzyme_id` varchar(64) NOT NULL COMMENT 'id reference to the protein model table',
  `enzyme_name` varchar(255) NOT NULL DEFAULT '-',
  `lambda` varchar(4096) NOT NULL COMMENT 'the math expression for this kinetics law, usually be the Michaelis-Menten equation: v = vmax * S / (km + S)',
  `parameters` json NOT NULL COMMENT 'parameter values for the lambda expression, a key-value pair tuple json data, store the kinetics constant at here, example as there is a kinetics lambda expression: v = vmax * S / (km + S), parameter value json could be {vmax: 1000, S: registry_resolver_id, km: 500}, where number in parameter value is a constant value that detected from the experiment and S is the substrate, value is the model registry resolver id, could be unify mapping to metabolite or other biomolecule entity object',
  `metabolic_node` int unsigned NOT NULL COMMENT 'metabolic reaction id, mount this enzyme kinetics law model to a metabolic reaction',
  `buffer` varchar(4096) DEFAULT NULL COMMENT 'experiment condition description',
  `ph` double NOT NULL COMMENT 'experiment ph value',
  `temperature` double NOT NULL COMMENT 'experiment temperature value',
  `pubmed_id` varchar(45) NOT NULL DEFAULT '-',
  `pdb_data` int unsigned NOT NULL COMMENT 'structure data of this kinetics law: molecule docking visual of the enzyme with the substrate molecule',
  `raw` json DEFAULT NULL COMMENT 'the rawdata json of this kinetics law model',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `enzyme_protein_idx` (`enzyme_id`),
  KEY `metabolic_netnode_idx` (`metabolic_node`),
  KEY `docking_result_idx` (`pdb_data`),
  KEY `find_by_id` (`db_xref`,`db_source`),
  FULLTEXT KEY `search_text` (`buffer`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=35204 DEFAULT CHARSET=utf8mb3 COMMENT='enzyme kinetics law, the kinetics law has a unify unit at here, all kinetics law in this table its parameter value must be converted to the standard unit before it save into this table: standard unit of time is second(s), velocity standard unit is mmol/L/s, Km standard unit is (mM)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `metabolic_network`
--

DROP TABLE IF EXISTS `metabolic_network`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metabolic_network` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `reaction_id` int unsigned NOT NULL COMMENT 'reference to the reaction model node',
  `factor` int unsigned NOT NULL COMMENT 'stoichiometric number',
  `species_id` int unsigned NOT NULL COMMENT 'molecule entity id, reference to the registry resolver table, unify reference to metabolite id, gene(rna) id, or protein id',
  `symbol_id` varchar(45) NOT NULL,
  `role` int unsigned NOT NULL COMMENT 'role of this species, vocabulary term reference id, vocabulary term could be substrate, product, enzyme, inhibitor',
  `compartment_id` int unsigned NOT NULL COMMENT 'compartment location of this species compound',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `reaction_model_idx` (`reaction_id`),
  KEY `role_term_idx` (`role`),
  KEY `location_info_idx` (`compartment_id`),
  KEY `registry_model_idx` (`species_id`),
  KEY `symbol_index` (`symbol_id`),
  KEY `filter_network` (`role`,`species_id`)
) ENGINE=InnoDB AUTO_INCREMENT=564184 DEFAULT CHARSET=utf8mb3 COMMENT='metabolic reaction network';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `metabolite_class`
--

DROP TABLE IF EXISTS `metabolite_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metabolite_class` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `metabolite_id` int unsigned NOT NULL,
  `class_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_class` (`metabolite_id`,`class_id`),
  KEY `metabolite_info_idx` (`metabolite_id`),
  KEY `class_info_idx` (`class_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1145148 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `metabolite_thermo`
--

DROP TABLE IF EXISTS `metabolite_thermo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metabolite_thermo` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `env_id` int unsigned NOT NULL,
  `metabolite_id` int unsigned NOT NULL,
  `delta_gf0` double NOT NULL,
  `delta_gf0_error` double NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`env_id`,`metabolite_id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='thermo data of metabolites';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `metabolites`
--

DROP TABLE IF EXISTS `metabolites`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metabolites` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `main_id` int unsigned NOT NULL DEFAULT '0' COMMENT 'processing of the possible duplicated name metabolites',
  `name` varchar(2048) NOT NULL,
  `name_zh` varchar(255) DEFAULT NULL COMMENT 'the metabolite chinese name',
  `hashcode` char(32) NOT NULL COMMENT 'md5 checksum of the tolower(name)',
  `formula` varchar(128) NOT NULL,
  `exact_mass` double unsigned NOT NULL DEFAULT '0',
  `cas_id` varchar(32) DEFAULT NULL COMMENT 'cas registry number',
  `pubchem_cid` int unsigned DEFAULT NULL COMMENT 'pubchem cid(not sid)',
  `chebi_id` int unsigned DEFAULT NULL COMMENT 'chebi id',
  `hmdb_id` varchar(24) DEFAULT NULL COMMENT 'hmdb id',
  `lipidmaps_id` varchar(16) DEFAULT NULL COMMENT 'lipidmaps id',
  `kegg_id` varchar(6) DEFAULT NULL COMMENT 'kegg compound id',
  `drugbank_id` varchar(45) DEFAULT NULL,
  `biocyc` varchar(1024) DEFAULT NULL COMMENT 'biocyc database id',
  `mesh_id` varchar(1024) DEFAULT NULL COMMENT 'ncbi pubmed mesh id',
  `wikipedia` varchar(1024) DEFAULT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COMMENT 'the metabolite description text, extract from the pubmed articles or genertaed via LLMs',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `find_name` (`hashcode`),
  KEY `find_formula` (`formula`),
  KEY `filter_mass` (`exact_mass`),
  KEY `find_cas_id` (`cas_id`),
  KEY `find_pubchem` (`pubchem_cid`),
  KEY `find_chebi` (`chebi_id`),
  KEY `find_hmdb` (`hmdb_id`),
  KEY `find_lipidmaps` (`lipidmaps_id`),
  KEY `find_kegg` (`kegg_id`),
  KEY `find_biocyc` (`biocyc`),
  KEY `find_mesh` (`mesh_id`),
  KEY `find_wiki` (`wikipedia`),
  KEY `find_zh_name` (`name_zh`),
  FULLTEXT KEY `search_text` (`name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=2129800 DEFAULT CHARSET=utf8mb3 COMMENT='[cellular entity model][entity instance] a set of reference metabolites, template based on the www.metabolomicsworkbench.org refmet dataset';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `molecular_networking`
--

DROP TABLE IF EXISTS `molecular_networking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `molecular_networking` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `u` int unsigned NOT NULL COMMENT 'id of the molecule data, reference to the metabolites table, nucleotide table, protein data table based on the value of type field',
  `v` int unsigned NOT NULL COMMENT 'id of the molecule data, reference to the metabolites table, nucleotide table, protein data table based on the value of type field',
  `similarity` double NOT NULL COMMENT 'cosine similarity value between two embedding vector',
  `type` int unsigned NOT NULL COMMENT 'the molecule type of this network edge, value could be metabolite vs metabolite, gene vs gene, protein vs protein',
  `add_time` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `metabolite_network_idx` (`u`,`v`),
  KEY `molecule_type_idx` (`type`),
  KEY `sort_smilarity` (`type`,`similarity` DESC)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='networking result based on the biological molecule fingerprint embedding vector similarity networking method';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `motif`
--

DROP TABLE IF EXISTS `motif`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `motif` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL COMMENT 'motif name',
  `family` varchar(45) NOT NULL COMMENT 'motif family name',
  `domain` varchar(45) NOT NULL DEFAULT 'bacteria' COMMENT 'bacteria, plant, animal, fungi',
  `pwm` longtext NOT NULL COMMENT 'pre-computed motif pwm matrix, base64 encoded of double[][] matrix, network byte order',
  `width` int NOT NULL,
  `alphabets` varchar(45) NOT NULL DEFAULT 'ACGT' COMMENT 'alphabets string for make decode of the pwm data',
  `background` json DEFAULT NULL COMMENT 'the background freqnency of the alphabets',
  `logo` longtext COMMENT 'data uri of the motif logo image encoded in base64 string',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `family_index` (`family`),
  KEY `name_search` (`name`),
  FULLTEXT KEY `text_search` (`name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=3879 DEFAULT CHARSET=utf8mb3 COMMENT='alphabets for nucleotide seuqnece is ACGT';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ncbi_taxonomy`
--

DROP TABLE IF EXISTS `ncbi_taxonomy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ncbi_taxonomy` (
  `id` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
  `GTDB_id` int unsigned DEFAULT NULL,
  `name` varchar(1024) NOT NULL COMMENT 'taxonomy name',
  `zh_name` varchar(255) DEFAULT NULL COMMENT 'chinese name translation of this node name',
  `rank` int unsigned NOT NULL COMMENT 'rank level of this taxonomy node, rank name is reference to the vocabulary term table',
  `ancestor` int unsigned NOT NULL DEFAULT '0' COMMENT 'ancestor taxonomy node id, this value for root node is zero',
  `childs` json NOT NULL COMMENT 'json encode of the child nodes tax id vector, default empty should be []',
  `num_childs` int NOT NULL DEFAULT '0' COMMENT 'number of the direct child of this taxonomy node it have, leaf node is zero',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `taxonomy_tree_idx` (`ancestor`),
  KEY `rank_name_idx` (`rank`),
  KEY `find_by_scientific_name` (`name`),
  KEY `find_by_zh_name` (`zh_name`),
  KEY `sort_time` (`add_time`),
  KEY `gtdb_index` (`GTDB_id`),
  FULLTEXT KEY `search_text` (`name`,`note`,`zh_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='[entity instance] the ncbi taxonomy tree data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `nucleotide_data`
--

DROP TABLE IF EXISTS `nucleotide_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nucleotide_data` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `source_id` varchar(45) DEFAULT NULL COMMENT 'db_xref id of this nucleotide sequence in its source database, example as ncbi genbank accession id',
  `source_db` int unsigned NOT NULL COMMENT 'source database name, id reference to vocabulary table',
  `name` varchar(45) DEFAULT NULL COMMENT 'name of this nucleotide object(gene or motif site)',
  `function` varchar(2048) DEFAULT NULL COMMENT 'gene product function description text',
  `is_motif` tinyint unsigned NOT NULL DEFAULT '0' COMMENT 'boolean type, 1 for true, means is a motif site, 0 for false, means is a gene object instance, default is zero (gene object)',
  `left` int unsigned NOT NULL COMMENT 'left location on the chromosome',
  `strand` char(1) NOT NULL DEFAULT '+' COMMENT 'value is +/-, means forward or reversed strand',
  `operon_id` int unsigned NOT NULL DEFAULT '0' COMMENT 'id of the operon instance object, default zero means current nucleotide object not belongs to any operon',
  `model_id` int unsigned NOT NULL COMMENT 'id reference to the gene model table or motif model table',
  `organism_source` int unsigned NOT NULL COMMENT 'ncbi taxonomy id',
  `sequence` longtext COMMENT 'nucleotide sequence in upper case',
  `checksum` char(32) DEFAULT NULL COMMENT 'md5 checksum of the sequence',
  `fingerprint` longtext COMMENT ' embedding vector of this sequence, base64 encoded double array, network byte order',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `gene_model_idx` (`model_id`),
  KEY `dbname_idx` (`source_db`),
  KEY `taxonomy_source_idx` (`organism_source`),
  KEY `operon_data_idx` (`operon_id`),
  KEY `find_locus` (`source_id`,`source_db`)
) ENGINE=InnoDB AUTO_INCREMENT=1992559 DEFAULT CHARSET=utf8mb3 COMMENT='[entity instance] gene object instance, nucleotide sequence region instance, used for build a local blastn database, or TFBS motif database';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ontology`
--

DROP TABLE IF EXISTS `ontology`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ontology` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `term_id` varchar(128) NOT NULL COMMENT 'ontology term id',
  `term` varchar(2048) NOT NULL COMMENT 'term name',
  `term_zh` varchar(255) DEFAULT NULL COMMENT 'chinease name translation of the term field',
  `ontology_id` int unsigned NOT NULL COMMENT 'external ontology database name, example as gene_ontology for gene_data, protein_ontology for protein_data, chebi ontology for metabolites',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COMMENT 'term description text',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_term` (`term_id`,`ontology_id`),
  KEY `ontology_db_idx` (`ontology_id`),
  FULLTEXT KEY `search_text` (`term`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=266673 DEFAULT CHARSET=utf8mb3 COMMENT='ontology terms, classification system';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ontology_relation`
--

DROP TABLE IF EXISTS `ontology_relation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ontology_relation` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `term_id` int unsigned NOT NULL COMMENT 'current ontology term node',
  `is_a` int unsigned NOT NULL COMMENT 'id reference to its parent node',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_relation` (`term_id`,`is_a`),
  KEY `term_node_idx` (`term_id`),
  KEY `parent_node_idx` (`is_a`)
) ENGINE=InnoDB AUTO_INCREMENT=282337 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `operon`
--

DROP TABLE IF EXISTS `operon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operon` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `conserved_operon` int unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `conserved_info_idx` (`conserved_operon`),
  FULLTEXT KEY `search_text` (`name`,`note`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='operon instance objects';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `organism_source`
--

DROP TABLE IF EXISTS `organism_source`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `organism_source` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `metabolite_id` int unsigned NOT NULL COMMENT 'reference to the metabolite information',
  `taxname` varchar(255) NOT NULL,
  `organism_id` int unsigned NOT NULL COMMENT 'organism source its ncbi taxonomy id',
  `evidence` varchar(255) NOT NULL DEFAULT '-' COMMENT 'usually be the article DOI id/ncbi pubmed id ',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `check_unique` (`metabolite_id`,`evidence`,`taxname`),
  KEY `metabolite_data_idx` (`metabolite_id`),
  KEY `organism_info_idx` (`organism_id`)
) ENGINE=InnoDB AUTO_INCREMENT=881293 DEFAULT CHARSET=utf8mb3 COMMENT='metabolite organism source report, or the lcms experiment annotation result based on the experiment sample information associated with the organism information ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `organism_traits`
--

DROP TABLE IF EXISTS `organism_traits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `organism_traits` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `tax_id` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
  `traits_id` int unsigned NOT NULL,
  `unit` varchar(64) NOT NULL,
  `consensus_value` varchar(255) NOT NULL,
  `min` double DEFAULT NULL,
  `median` double DEFAULT NULL,
  `mean` double DEFAULT NULL,
  `max` double DEFAULT NULL,
  `discrete_values` json DEFAULT NULL,
  `databases` json DEFAULT NULL,
  `ontology_ids` json DEFAULT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `traits_index` (`traits_id`),
  KEY `ncbi_index` (`tax_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='metaTraits dataset';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pathway`
--

DROP TABLE IF EXISTS `pathway`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pathway` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `accession_id` varchar(255) NOT NULL,
  `db_source` int unsigned NOT NULL,
  `name` varchar(1024) NOT NULL COMMENT 'the pathway name',
  `type` varchar(45) DEFAULT NULL,
  `taxid` int unsigned NOT NULL DEFAULT '0',
  `dois` json DEFAULT NULL COMMENT 'a json array of the doi reference of this pathway',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COMMENT 'pathway description text',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `id_index` (`db_source`,`accession_id`),
  KEY `id_search` (`accession_id`),
  KEY `filter_tax` (`taxid`),
  KEY `find_by_name` (`name`),
  FULLTEXT KEY `search_text` (`name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=249688 DEFAULT CHARSET=utf8mb3 COMMENT='pathway view(a collection of reaction models) and biological event view.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pathway_network`
--

DROP TABLE IF EXISTS `pathway_network`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pathway_network` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `pathway_id` int unsigned NOT NULL,
  `model_id` int unsigned NOT NULL,
  `symbol_id` varchar(64) NOT NULL,
  `class_id` int unsigned NOT NULL COMMENT 'data type of the reference model object',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_link` (`pathway_id`,`model_id`,`class_id`),
  KEY `pathway_info_idx` (`pathway_id`),
  KEY `biological_events_idx` (`model_id`),
  KEY `link2` (`pathway_id`,`symbol_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5667852 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pdb`
--

DROP TABLE IF EXISTS `pdb`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pdb` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `pdb_id` varchar(8) NOT NULL DEFAULT 'NA' COMMENT 'the id of this pdb structre data, reference to the pdb database, this field could be NA if this structre data is generated via molecule docking',
  `title` mediumtext,
  `data` longtext NOT NULL COMMENT 'base64 encoded pdb text data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  FULLTEXT KEY `search_text` (`title`,`note`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='molecule 3d structure data, store data in pdb data format';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `protein`
--

DROP TABLE IF EXISTS `protein`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `protein` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(45) NOT NULL,
  `name` varchar(128) NOT NULL COMMENT 'protein name',
  `template` int unsigned NOT NULL DEFAULT '0' COMMENT 'reference to the gene model table id that gene object that could be used as the template for produce this protein, default value is zero means this protein is artificial designed or it is a protein complex object',
  `pdb_data` int unsigned NOT NULL COMMENT 'reference to the table of pdb structure data, avaible for the protein complex object',
  `function` mediumtext COMMENT 'protein function description text',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` mediumtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `gene_source_idx` (`template`),
  KEY `pdb_data_idx` (`pdb_data`),
  KEY `name_hit` (`name`),
  KEY `xref_index` (`db_xref`),
  FULLTEXT KEY `search_text` (`function`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=28133 DEFAULT CHARSET=utf8mb3 COMMENT='[cellular entity model] a table of the reference protein models, just defined the protein model information at here, usually be the KO/COG orthology data model, includes metabolic enzyme and other biological protein models';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `protein_cluster`
--

DROP TABLE IF EXISTS `protein_cluster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `protein_cluster` (
  `query_id` int unsigned NOT NULL COMMENT 'id reference to the protein fasta table',
  `hit_id` int unsigned NOT NULL COMMENT 'id reference to the protein fasta table',
  `identities` float unsigned NOT NULL COMMENT 'protein sequence similarity score value',
  `mis_match` int unsigned NOT NULL,
  `gap_open` int unsigned NOT NULL,
  `q_start` int unsigned NOT NULL,
  `q_end` int unsigned NOT NULL,
  `s_start` int unsigned NOT NULL,
  `s_end` int unsigned NOT NULL,
  `e_value` double NOT NULL,
  `bit_score` float NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`query_id`,`hit_id`),
  UNIQUE KEY `diamond_idx_edge` (`query_id`,`hit_id`),
  KEY `rank_score` (`identities` DESC),
  KEY `filter_edges` (`query_id`,`identities`,`e_value`),
  KEY `filter_edges2` (`hit_id`,`identities`,`e_value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='protein diamond blastp search result, diamond commandline: diamond blastp -d proteins -q proteins.fasta -o protein_clusters.tsv  --very-sensitive  --threads 96  --outfmt 6 qtitle stitle pident length mismatch gapopen qstart qend sstart send evalue bitscore  --max-target-seqs 0   -e 1';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `protein_data`
--

DROP TABLE IF EXISTS `protein_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `protein_data` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `source_id` varchar(45) NOT NULL COMMENT 'id in source database, example as uniprot id',
  `source_db` int unsigned NOT NULL COMMENT 'vocabulary term id to the source database name',
  `name` varchar(45) NOT NULL COMMENT 'protein name',
  `function` varchar(2048) NOT NULL COMMENT 'the protein function description',
  `gene_id` int unsigned NOT NULL COMMENT 'reference to the gene instance id(not gene model table)',
  `cluster_id` int unsigned NOT NULL DEFAULT '0' COMMENT 'protein cluster id',
  `protein_id` int unsigned NOT NULL DEFAULT '0' COMMENT 'id reference to the reference protein table(protein model table)',
  `ncbi_taxid` int unsigned NOT NULL DEFAULT '0',
  `sequence` longtext NOT NULL COMMENT 'protein sequence, should be in upper case',
  `checksum` char(32) NOT NULL COMMENT 'md5 hash checksum of the protein sequence',
  `fingerprint` longtext COMMENT ' embedding vector of this sequence, base64 encoded double array, network byte order',
  `pdb_data` int unsigned NOT NULL DEFAULT '0' COMMENT 'reference to the pdb table, used for the web visualization of this protein object',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `translate_from_idx` (`gene_id`),
  KEY `protein_model_idx` (`protein_id`),
  KEY `dbname_idx` (`source_db`),
  KEY `pdb_datavalue_idx` (`pdb_data`),
  KEY `find_by_source_id` (`source_db`,`ncbi_taxid`,`source_id`),
  KEY `query_db_locus` (`source_id`,`source_db`),
  KEY `finter_organism` (`ncbi_taxid`),
  KEY `filter_cluster` (`cluster_id`),
  FULLTEXT KEY `search_text` (`function`)
) ENGINE=InnoDB AUTO_INCREMENT=4066538 DEFAULT CHARSET=utf8mb3 ROW_FORMAT=DYNAMIC COMMENT='[entity instance] the instance of the protein with sequence data, used for build a local blastp database';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `protein_ontology`
--

DROP TABLE IF EXISTS `protein_ontology`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `protein_ontology` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `protein_id` int unsigned NOT NULL,
  `ontology_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_features` (`protein_id`,`ontology_id`),
  KEY `protein_data_idx` (`protein_id`),
  KEY `ontology_term_idx` (`ontology_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reaction`
--

DROP TABLE IF EXISTS `reaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reaction` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(255) NOT NULL,
  `db_source` int unsigned NOT NULL,
  `hashcode` char(32) DEFAULT NULL COMMENT 'md5 hashcode of the reaction enzyme ec number tagged with the topological network hashcode',
  `topology_key` char(32) DEFAULT NULL COMMENT 'metabolite  topological network hashcode: hashcode of the [species_id] in metabolic network table, due to the reason of role maybe swap between different reaction entry but the site metabolite keeps identitical, so this hashcode just used the metabolits species id sort in asc order to calculate, just reflects the network topology feature, no reaction direction information in this hashcode',
  `main_id` int unsigned NOT NULL DEFAULT '0',
  `name` varchar(8192) NOT NULL COMMENT 'name of this reaction',
  `ec_number` varchar(20) DEFAULT NULL COMMENT 'ec number of this reaction, value could be null(means is a chemical reaction that not required of enzyme)',
  `equation` varchar(8192) NOT NULL COMMENT 'equation string of this reaction',
  `obsolete` int NOT NULL DEFAULT '0' COMMENT 'mark this reaction pending to be deleted',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `find_reference` (`db_xref`,`db_source`),
  KEY `hash_index` (`hashcode`),
  KEY `topo_index` (`topology_key`),
  FULLTEXT KEY `search_text` (`name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=104116 DEFAULT CHARSET=utf8mb3 COMMENT='[biological process model] biological reaction/chemical reaction model';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reaction_thermo`
--

DROP TABLE IF EXISTS `reaction_thermo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reaction_thermo` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `env_id` int unsigned NOT NULL COMMENT 'id of the thermo environment',
  `rxn_id` int unsigned NOT NULL COMMENT 'the biological reaction id',
  `delta_rg0` double NOT NULL,
  `delta_rg0_error` double NOT NULL,
  `direction_status` varchar(50) NOT NULL DEFAULT 'forward' COMMENT 'Forward/Reverse/Reversible',
  `fba_lb` double NOT NULL,
  `fba_ub` double NOT NULL,
  `error_msg` text,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`env_id`,`rxn_id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `idx_direction` (`direction_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='thermo data of the biological reactions';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `refseq`
--

DROP TABLE IF EXISTS `refseq`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `refseq` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `assembly_accession` varchar(45) NOT NULL,
  `assembly_level` varchar(45) DEFAULT NULL,
  `bioproject` varchar(45) NOT NULL,
  `biosample` varchar(45) DEFAULT NULL,
  `group` varchar(45) DEFAULT NULL,
  `taxid` int unsigned NOT NULL,
  `species_taxid` int unsigned NOT NULL,
  `organism_name` varchar(255) DEFAULT NULL,
  `infraspecific_name` varchar(255) DEFAULT NULL,
  `ftp_path` mediumtext NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `find_asm_id` (`assembly_accession`),
  KEY `search_tax1` (`taxid`),
  KEY `search_tax2` (`species_taxid`),
  KEY `filter_tax_group` (`group`),
  KEY `filter_level` (`assembly_level`),
  FULLTEXT KEY `search_taxname` (`organism_name`)
) ENGINE=InnoDB AUTO_INCREMENT=492871 DEFAULT CHARSET=utf8mb3 COMMENT='imports of assembly_summary_refseq.txt';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `registry_resolver`
--

DROP TABLE IF EXISTS `registry_resolver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `registry_resolver` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `register_name` varchar(255) NOT NULL COMMENT 'symbol name of the model object',
  `symbol_id` int unsigned NOT NULL COMMENT 'symbol model table id, could be reference to metabolites, gene, protein, reaction and cellular compartment location table',
  `type` int unsigned NOT NULL COMMENT 'the symbol model type, reference to vocabulary term table',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` mediumtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_symbol` (`register_name`,`symbol_id`,`type`),
  UNIQUE KEY `register_name_UNIQUE` (`register_name`,`type`),
  KEY `register_namespace_idx` (`type`),
  KEY `metabolite_reference_idx` (`symbol_id`),
  KEY `find_name` (`register_name`)
) ENGINE=InnoDB AUTO_INCREMENT=400536 DEFAULT CHARSET=utf8mb3 COMMENT='a unify symbol mapping inside the registry database system';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `regulatory_network`
--

DROP TABLE IF EXISTS `regulatory_network`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `regulatory_network` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `regulator` int unsigned NOT NULL COMMENT 'TF regulator protein instance',
  `motif_site` int unsigned NOT NULL COMMENT 'motif site instance',
  `effector_name` varchar(128) DEFAULT NULL,
  `effector` int unsigned NOT NULL COMMENT 'metabolite effector of the TF, id reference to the metabolites table',
  `effects` double NOT NULL COMMENT 'the regulation effects, positive value means activator, negative value means inhibitor',
  `db_source` int unsigned NOT NULL COMMENT 'database source of this binding relationship',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_TRN` (`regulator`,`motif_site`,`db_source`,`effector`),
  KEY `motif_site_info_idx` (`motif_site`),
  KEY `protein_info_idx` (`regulator`),
  KEY `metabolite_info_idx` (`effector`),
  KEY `db_name_idx` (`db_source`),
  FULLTEXT KEY `search_text` (`note`)
) ENGINE=InnoDB AUTO_INCREMENT=6254 DEFAULT CHARSET=utf8mb3 COMMENT='TF to motif binding relationship';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `struct_data`
--

DROP TABLE IF EXISTS `struct_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `struct_data` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `metabolite_id` int unsigned NOT NULL COMMENT 'id reference to the metabolites table',
  `checksum` char(32) NOT NULL COMMENT 'md5 checksum of the smiles string',
  `smiles` longtext NOT NULL COMMENT 'the molecule structure data in SMILES string format',
  `fingerprint` longtext COMMENT ' embedding vector of this smiles structure sequence, base64 encoded double array, network byte order',
  `pdb_data` int unsigned NOT NULL DEFAULT '0' COMMENT '3d structre data in pdb format, transform from the SMILES string to pdb data via RDkit software or other program, used for data visualization on web',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `metabolite_info_idx` (`metabolite_id`),
  KEY `pdb_modelvalue_idx` (`pdb_data`)
) ENGINE=InnoDB AUTO_INCREMENT=1711681 DEFAULT CHARSET=utf8mb3 COMMENT='the metabolite molecule structre data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `subcellular_location`
--

DROP TABLE IF EXISTS `subcellular_location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subcellular_location` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `protein_id` int unsigned NOT NULL COMMENT 'protein object instance id',
  `location_id` int unsigned NOT NULL COMMENT 'cellular compartment location id',
  `evidence` varchar(1024) NOT NULL COMMENT 'usually be the article doi id or pubmed id',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `compartment_term_idx` (`location_id`),
  KEY `prot_obj_idx` (`protein_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1607563 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `synonym`
--

DROP TABLE IF EXISTS `synonym`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `synonym` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `obj_id` int unsigned NOT NULL COMMENT 'object reference id to the metabolites/genes/protein/compartment location/reaction table',
  `type` int unsigned NOT NULL COMMENT 'entity type of the object that associated with this synonym name',
  `db_source` int unsigned NOT NULL COMMENT 'database source of this synonym name',
  `synonym` longtext NOT NULL COMMENT 'synonym name',
  `hashcode` char(32) NOT NULL COMMENT 'md5 checksum of the tolower( synonym field)',
  `lang` varchar(8) NOT NULL COMMENT 'language of this name, default is ''en'' english, value could be en - english, zh - chinese',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_name` (`obj_id`,`type`,`db_source`,`hashcode`,`lang`),
  KEY `dbname_idx` (`db_source`),
  KEY `entity_namespace_idx` (`type`),
  KEY `find_name` (`lang`,`hashcode`),
  KEY `fast_hash_search` (`obj_id`,`hashcode`,`type`),
  KEY `entity_metabolite_idx` (`obj_id`,`type`),
  KEY `obj_hash_query` (`type`,`hashcode`),
  FULLTEXT KEY `search_text` (`synonym`)
) ENGINE=InnoDB AUTO_INCREMENT=10457963 DEFAULT CHARSET=utf8mb3 COMMENT='synonyms, alias names of the model objects';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `thermo_environment`
--

DROP TABLE IF EXISTS `thermo_environment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `thermo_environment` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'the environment id',
  `ph` double unsigned NOT NULL DEFAULT '7',
  `temperature` double NOT NULL COMMENT 'temperature(K)',
  `ionic_strength` double NOT NULL COMMENT 'ionic strength of the solution system',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
  UNIQUE KEY `uk_env_params` (`ph`,`temperature`,`ionic_strength`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='thermo environment for the biological reactor system';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `topic`
--

DROP TABLE IF EXISTS `topic`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `topic` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `topic_id` int unsigned NOT NULL,
  `type` int unsigned NOT NULL DEFAULT '0' COMMENT 'default type id is zero, means the model object is registry resolved model. otherwise is the object instance id in this registry',
  `model_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_link` (`topic_id`,`model_id`),
  KEY `registry_model_idx` (`model_id`),
  KEY `topic_term_idx` (`topic_id`),
  KEY `filter_class_type` (`type`)
) ENGINE=InnoDB AUTO_INCREMENT=13880869 DEFAULT CHARSET=utf8mb3 COMMENT='topic about the biological model';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `vocabulary`
--

DROP TABLE IF EXISTS `vocabulary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vocabulary` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `category` varchar(64) NOT NULL DEFAULT 'NA' COMMENT 'term category name, value could be registry model type, external database, metabolic edge type',
  `term` varchar(255) NOT NULL COMMENT 'vocabulary term',
  `term_zh` varchar(255) DEFAULT NULL COMMENT 'the chinese translation of the term name',
  `parent_id` int unsigned NOT NULL DEFAULT '0' COMMENT 'ontology parent is of this vocabylary term, reference to the id of another vocabulary term that could be used as the ancestor of this term',
  `color` varchar(8) NOT NULL DEFAULT '#000000' COMMENT 'html color code for make render of this vocabulary term display on web',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` mediumtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `search_term` (`category`,`term`),
  KEY `ontology_tree_idx` (`parent_id`),
  FULLTEXT KEY `search_text` (`note`)
) ENGINE=InnoDB AUTO_INCREMENT=1217 DEFAULT CHARSET=utf8mb3 COMMENT='vocabulary term inside the registry database';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'cad_registry'
--

--
-- Dumping routines for database 'cad_registry'
--
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-23 23:13:07
