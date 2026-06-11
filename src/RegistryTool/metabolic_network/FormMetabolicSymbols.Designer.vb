Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMetabolicSymbols
    Inherits DocumentWindow

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        AdvancedDataGridViewSearchToolBar1 = New Galaxy.Data.TableSheet.AdvancedDataGridViewSearchToolBar()
        AdvancedDataGridView1 = New Galaxy.Data.TableSheet.AdvancedDataGridView()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        ViewToolStripMenuItem = New ToolStripMenuItem()
        CType(AdvancedDataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' AdvancedDataGridViewSearchToolBar1
        ' 
        AdvancedDataGridViewSearchToolBar1.AllowMerge = False
        AdvancedDataGridViewSearchToolBar1.GripStyle = ToolStripGripStyle.Hidden
        AdvancedDataGridViewSearchToolBar1.Location = New Point(0, 0)
        AdvancedDataGridViewSearchToolBar1.MaximumSize = New Size(0, 27)
        AdvancedDataGridViewSearchToolBar1.MinimumSize = New Size(0, 27)
        AdvancedDataGridViewSearchToolBar1.Name = "AdvancedDataGridViewSearchToolBar1"
        AdvancedDataGridViewSearchToolBar1.RenderMode = ToolStripRenderMode.Professional
        AdvancedDataGridViewSearchToolBar1.Size = New Size(800, 27)
        AdvancedDataGridViewSearchToolBar1.TabIndex = 1
        AdvancedDataGridViewSearchToolBar1.Text = "AdvancedDataGridViewSearchToolBar1"
        ' 
        ' AdvancedDataGridView1
        ' 
        AdvancedDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        AdvancedDataGridView1.ContextMenuStrip = ContextMenuStrip1
        AdvancedDataGridView1.Dock = DockStyle.Fill
        AdvancedDataGridView1.FilterAndSortEnabled = True
        AdvancedDataGridView1.FilterStringChangedInvokeBeforeDatasourceUpdate = True
        AdvancedDataGridView1.Location = New Point(0, 27)
        AdvancedDataGridView1.MaxFilterButtonImageHeight = 23
        AdvancedDataGridView1.MultiSelect = False
        AdvancedDataGridView1.Name = "AdvancedDataGridView1"
        AdvancedDataGridView1.ReadOnly = True
        AdvancedDataGridView1.RightToLeft = RightToLeft.No
        AdvancedDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        AdvancedDataGridView1.Size = New Size(800, 423)
        AdvancedDataGridView1.SortStringChangedInvokeBeforeDatasourceUpdate = True
        AdvancedDataGridView1.TabIndex = 2
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {ViewToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(100, 26)
        ' 
        ' ViewToolStripMenuItem
        ' 
        ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        ViewToolStripMenuItem.Size = New Size(99, 22)
        ViewToolStripMenuItem.Text = "View"
        ' 
        ' FormMetabolicSymbols
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(AdvancedDataGridView1)
        Controls.Add(AdvancedDataGridViewSearchToolBar1)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        Name = "FormMetabolicSymbols"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        TabPageContextMenuStrip = DockContextMenuStrip1
        Text = "Metabolic Symbols Table"
        CType(AdvancedDataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents AdvancedDataGridViewSearchToolBar1 As Galaxy.Data.TableSheet.AdvancedDataGridViewSearchToolBar
    Friend WithEvents AdvancedDataGridView1 As Galaxy.Data.TableSheet.AdvancedDataGridView
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
End Class
