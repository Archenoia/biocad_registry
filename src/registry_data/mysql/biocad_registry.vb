Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports registry_data.biocad_registryModel

Public Class biocad_registry : Inherits biocad_registryModel.db_mysql

    Public ReadOnly Property metabolites As TableModel(Of biocad_registryModel.metabolites)
        Get
            Return m_metabolites
        End Get
    End Property

    Public ReadOnly Property vocabulary As TableModel(Of biocad_registryModel.vocabulary)
        Get
            Return m_vocabulary
        End Get
    End Property

    Public ReadOnly Property db_xrefs As TableModel(Of biocad_registryModel.db_xrefs)
        Get
            Return m_db_xrefs
        End Get
    End Property

    Public ReadOnly Property dois As TableModel(Of dois)
        Get
            Return m_dois
        End Get
    End Property

    Public ReadOnly Property ontology As TableModel(Of biocad_registryModel.ontology)
        Get
            Return m_ontology
        End Get
    End Property

    Public ReadOnly Property ontology_relation As TableModel(Of biocad_registryModel.ontology_relation)
        Get
            Return m_ontology_relation
        End Get
    End Property

    Public ReadOnly Property metabolite_class As TableModel(Of biocad_registryModel.metabolite_class)
        Get
            Return m_metabolite_class
        End Get
    End Property

    Public ReadOnly Property synonym As TableModel(Of biocad_registryModel.synonym)
        Get
            Return m_synonym
        End Get
    End Property

    Public ReadOnly Property struct_data As TableModel(Of biocad_registryModel.struct_data)
        Get
            Return m_struct_data
        End Get
    End Property

    Public ReadOnly Property conserved_domain As TableModel(Of conserved_domain)
        Get
            Return m_conserved_domain
        End Get
    End Property

    Public ReadOnly Property topic As TableModel(Of biocad_registryModel.topic)
        Get
            Return m_topic
        End Get
    End Property

    Public ReadOnly Property registry_resolver As TableModel(Of biocad_registryModel.registry_resolver)
        Get
            Return m_registry_resolver
        End Get
    End Property

    Public ReadOnly Property motif As TableModel(Of biocad_registryModel.motif)
        Get
            Return m_motif
        End Get
    End Property

    Public ReadOnly Property enzyme As TableModel(Of biocad_registryModel.enzyme)
        Get
            Return m_enzyme
        End Get
    End Property

    Public ReadOnly Property regulatory_network As TableModel(Of biocad_registryModel.regulatory_network)
        Get
            Return m_regulatory_network
        End Get
    End Property

    Public ReadOnly Property nucleotide_data As TableModel(Of biocad_registryModel.nucleotide_data)
        Get
            Return m_nucleotide_data
        End Get
    End Property

    Public ReadOnly Property protein_data As TableModel(Of biocad_registryModel.protein_data)
        Get
            Return m_protein_data
        End Get
    End Property

    Public ReadOnly Property protein_cluster As TableModel(Of protein_cluster)
        Get
            Return m_protein_cluster
        End Get
    End Property

    Public ReadOnly Property compartment_location As TableModel(Of biocad_registryModel.compartment_location)
        Get
            Return m_compartment_location
        End Get
    End Property

    Public ReadOnly Property compartment_enrich As TableModel(Of biocad_registryModel.compartment_enrich)
        Get
            Return m_compartment_enrich
        End Get
    End Property

    Public ReadOnly Property subcellular_location As TableModel(Of biocad_registryModel.subcellular_location)
        Get
            Return m_subcellular_location
        End Get
    End Property

    Public ReadOnly Property protein As TableModel(Of biocad_registryModel.protein)
        Get
            Return m_protein
        End Get
    End Property

    Public ReadOnly Property reaction As TableModel(Of biocad_registryModel.reaction)
        Get
            Return m_reaction
        End Get
    End Property

    Public ReadOnly Property pathway As TableModel(Of pathway)
        Get
            Return m_pathway
        End Get
    End Property

    Public ReadOnly Property pathway_network As TableModel(Of pathway_network)
        Get
            Return m_pathway_network
        End Get
    End Property

    Public ReadOnly Property metabolic_network As TableModel(Of biocad_registryModel.metabolic_network)
        Get
            Return m_metabolic_network
        End Get
    End Property

    Public ReadOnly Property ncbi_taxonomy As TableModel(Of biocad_registryModel.ncbi_taxonomy)
        Get
            Return m_ncbi_taxonomy
        End Get
    End Property

    Public ReadOnly Property organism_source As TableModel(Of biocad_registryModel.organism_source)
        Get
            Return m_organism_source
        End Get
    End Property

    Public ReadOnly Property organism_traits As TableModel(Of biocad_registryModel.organism_traits)
        Get
            Return m_organism_traits
        End Get
    End Property

    Public ReadOnly Property kinetics_law As TableModel(Of biocad_registryModel.kinetics_law)
        Get
            Return m_kinetics_law
        End Get
    End Property

    Public ReadOnly Property refseq As TableModel(Of biocad_registryModel.refseq)
        Get
            Return m_refseq
        End Get
    End Property

    Public ReadOnly Property biocad_vocabulary As biocad_vocabulary

    ReadOnly ncbi_tax As New Dictionary(Of String, biocad_registryModel.ncbi_taxonomy)
    ReadOnly ncbi_taxset As New Dictionary(Of UInteger, biocad_registryModel.ncbi_taxonomy)

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
        Me.biocad_vocabulary = New biocad_vocabulary(Me)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetTaxonomy(name As String) As biocad_registryModel.ncbi_taxonomy
        Return ncbi_tax.ComputeIfAbsent(
            name,
            lazyValue:=Function(key)
                           Return m_ncbi_taxonomy _
                               .where(field("name") = key) _
                               .find(Of biocad_registryModel.ncbi_taxonomy)
                       End Function)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetTaxonomy(taxid As UInteger) As biocad_registryModel.ncbi_taxonomy
        Return ncbi_taxset.ComputeIfAbsent(
            taxid,
            lazyValue:=Function(id)
                           Return ncbi_taxonomy _
                               .where(field("id") = taxid) _
                               .find(Of ncbi_taxonomy)
                       End Function)
    End Function

    ''' <summary>
    ''' Check of the target <paramref name="taxid"/> is belongs to the given <paramref name="ancestor_id"/>.
    ''' </summary>
    ''' <param name="taxid"></param>
    ''' <param name="ancestor_id"></param>
    ''' <returns></returns>
    Public Function CheckLineage(taxid As UInteger, ancestor_id As UInteger) As Boolean
        Dim tax As ncbi_taxonomy = GetTaxonomy(taxid)

        If tax Is Nothing Then
            Return False
        End If

        Do While tax.ancestor > 0
            If ancestor_id = tax.ancestor Then
                Return True
            Else
                tax = GetTaxonomy(tax.ancestor)

                If tax Is Nothing Then
                    Return False
                End If
            End If
        Loop

        Return False
    End Function

    ''' <summary>
    ''' Check <paramref name="id"/> could be mapping to <paramref name="main_id"/>
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="main_id"></param>
    ''' <returns></returns>
    Public Function CheckIdAlias(id As UInteger, main_id As UInteger) As Boolean
        Dim m = metabolites.where(field("id") = id).find(Of metabolites)

        If m Is Nothing Then
            Return False
        End If

        Do While m.main_id > 0
            If main_id = m.main_id Then
                Return True
            Else
                m = metabolites _
                    .where(field("id") = m.main_id) _
                    .find(Of metabolites)

                If m Is Nothing Then
                    Return False
                End If
            End If
        Loop

        Return False
    End Function
End Class