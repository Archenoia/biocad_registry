Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

''' <summary>
''' A tabular data of the smiles structure of the metabolic compound object
''' </summary>
Public Class SMILESData

    <DatabaseField> Public Property id As String
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property exact_mass As Double
    <DatabaseField> Public Property xref_id As String
    <DatabaseField> Public Property smiles As String

End Class
