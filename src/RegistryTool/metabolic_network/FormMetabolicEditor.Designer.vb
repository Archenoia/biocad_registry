Imports System.ComponentModel
Imports Galaxy.Workbench.DockDocument

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMetabolicEditor
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
        components = New Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(FormMetabolicEditor))
        SplitContainer1 = New SplitContainer()
        DataGridView1 = New DataGridView()
        Column11 = New DataGridViewTextBoxColumn()
        Column1 = New DataGridViewTextBoxColumn()
        Column10 = New DataGridViewTextBoxColumn()
        Column2 = New DataGridViewTextBoxColumn()
        Column3 = New DataGridViewTextBoxColumn()
        SplitContainer2 = New SplitContainer()
        DataGridView2 = New DataGridView()
        Column4 = New DataGridViewTextBoxColumn()
        Column5 = New DataGridViewTextBoxColumn()
        Column6 = New DataGridViewTextBoxColumn()
        Column7 = New DataGridViewTextBoxColumn()
        Column8 = New DataGridViewTextBoxColumn()
        Column9 = New DataGridViewTextBoxColumn()
        Column12 = New DataGridViewTextBoxColumn()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        OpenToolStripMenuItem = New ToolStripMenuItem()
        CopySymbolNameToolStripMenuItem = New ToolStripMenuItem()
        GroupBox1 = New GroupBox()
        Button5 = New Button()
        ListBox1 = New ListBox()
        Label4 = New Label()
        Button4 = New Button()
        Button3 = New Button()
        TextBox3 = New TextBox()
        Label3 = New Label()
        Button2 = New Button()
        TextBox2 = New TextBox()
        Label2 = New Label()
        Button1 = New Button()
        TextBox1 = New TextBox()
        Label1 = New Label()
        ToolStrip1 = New ToolStrip()
        ToolStripLabel1 = New ToolStripLabel()
        ToolStripButton1 = New ToolStripButton()
        ToolStripLabel2 = New ToolStripLabel()
        ToolStripTextBox1 = New ToolStripTextBox()
        ToolStripButton2 = New ToolStripButton()
        ToolStripButton3 = New ToolStripButton()
        ToolStripSeparator1 = New ToolStripSeparator()
        ToolStripLabel3 = New ToolStripLabel()
        ToolStripTextBox2 = New ToolStripTextBox()
        ToolStripButton4 = New ToolStripButton()
        ToolStripSeparator2 = New ToolStripSeparator()
        ToolStripLabel4 = New ToolStripLabel()
        ToolStripComboBox1 = New ToolStripComboBox()
        ToolStripSeparator3 = New ToolStripSeparator()
        ToolStripButton5 = New ToolStripButton()
        ToolStripSeparator4 = New ToolStripSeparator()
        ToolStripLabel5 = New ToolStripLabel()
        ToolStripButton6 = New ToolStripButton()
        ToolStripButton7 = New ToolStripButton()
        ContextMenuStrip2 = New ContextMenuStrip(components)
        CopyEquationStringToolStripMenuItem = New ToolStripMenuItem()
        CType(SplitContainer1, ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(DataGridView1, ISupportInitialize).BeginInit()
        CType(SplitContainer2, ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        CType(DataGridView2, ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        GroupBox1.SuspendLayout()
        ToolStrip1.SuspendLayout()
        ContextMenuStrip2.SuspendLayout()
        SuspendLayout()
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 25)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(DataGridView1)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(SplitContainer2)
        SplitContainer1.Size = New Size(1394, 796)
        SplitContainer1.SplitterDistance = 472
        SplitContainer1.TabIndex = 1
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.BackgroundColor = Color.WhiteSmoke
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Columns.AddRange(New DataGridViewColumn() {Column11, Column1, Column10, Column2, Column3})
        DataGridView1.ContextMenuStrip = ContextMenuStrip2
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Window
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle1
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.Location = New Point(0, 0)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersWidth = 80
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(1394, 472)
        DataGridView1.TabIndex = 0
        ' 
        ' Column11
        ' 
        Column11.HeaderText = "database"
        Column11.Name = "Column11"
        Column11.ReadOnly = True
        ' 
        ' Column1
        ' 
        Column1.HeaderText = "name"
        Column1.Name = "Column1"
        Column1.ReadOnly = True
        Column1.Width = 200
        ' 
        ' Column10
        ' 
        Column10.HeaderText = "ec_number"
        Column10.Name = "Column10"
        Column10.ReadOnly = True
        ' 
        ' Column2
        ' 
        Column2.HeaderText = "equation"
        Column2.Name = "Column2"
        Column2.ReadOnly = True
        Column2.Width = 400
        ' 
        ' Column3
        ' 
        Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column3.HeaderText = "note"
        Column3.Name = "Column3"
        Column3.ReadOnly = True
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.Location = New Point(0, 0)
        SplitContainer2.Name = "SplitContainer2"
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(DataGridView2)
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(GroupBox1)
        SplitContainer2.Size = New Size(1394, 320)
        SplitContainer2.SplitterDistance = 751
        SplitContainer2.TabIndex = 1
        ' 
        ' DataGridView2
        ' 
        DataGridView2.AllowUserToAddRows = False
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView2.Columns.AddRange(New DataGridViewColumn() {Column4, Column5, Column6, Column7, Column8, Column9, Column12})
        DataGridView2.ContextMenuStrip = ContextMenuStrip1
        DataGridView2.Dock = DockStyle.Fill
        DataGridView2.Location = New Point(0, 0)
        DataGridView2.Name = "DataGridView2"
        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView2.Size = New Size(751, 320)
        DataGridView2.TabIndex = 0
        ' 
        ' Column4
        ' 
        Column4.HeaderText = "id"
        Column4.Name = "Column4"
        Column4.ReadOnly = True
        ' 
        ' Column5
        ' 
        Column5.HeaderText = "db_xref"
        Column5.Name = "Column5"
        Column5.ReadOnly = True
        ' 
        ' Column6
        ' 
        Column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column6.HeaderText = "name"
        Column6.Name = "Column6"
        Column6.ReadOnly = True
        ' 
        ' Column7
        ' 
        Column7.HeaderText = "formula"
        Column7.Name = "Column7"
        Column7.ReadOnly = True
        ' 
        ' Column8
        ' 
        Column8.HeaderText = "mass"
        Column8.Name = "Column8"
        Column8.ReadOnly = True
        ' 
        ' Column9
        ' 
        Column9.HeaderText = "role"
        Column9.Name = "Column9"
        Column9.ReadOnly = True
        ' 
        ' Column12
        ' 
        Column12.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Column12.HeaderText = "symbol_name"
        Column12.Name = "Column12"
        Column12.ReadOnly = True
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {OpenToolStripMenuItem, CopySymbolNameToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(191, 48)
        ' 
        ' OpenToolStripMenuItem
        ' 
        OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        OpenToolStripMenuItem.Size = New Size(190, 22)
        OpenToolStripMenuItem.Text = "Open Metabolite Data"
        ' 
        ' CopySymbolNameToolStripMenuItem
        ' 
        CopySymbolNameToolStripMenuItem.Name = "CopySymbolNameToolStripMenuItem"
        CopySymbolNameToolStripMenuItem.Size = New Size(190, 22)
        CopySymbolNameToolStripMenuItem.Text = "Copy Symbol Name"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(Button5)
        GroupBox1.Controls.Add(ListBox1)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(Button4)
        GroupBox1.Controls.Add(Button3)
        GroupBox1.Controls.Add(TextBox3)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(Button2)
        GroupBox1.Controls.Add(TextBox2)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Controls.Add(TextBox1)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Dock = DockStyle.Fill
        GroupBox1.Location = New Point(0, 0)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(639, 320)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "Edit Reaction Data"
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(361, 82)
        Button5.Name = "Button5"
        Button5.Size = New Size(64, 24)
        Button5.TabIndex = 12
        Button5.Text = "Edit"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' ListBox1
        ' 
        ListBox1.FormattingEnabled = True
        ListBox1.Location = New Point(105, 82)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(250, 79)
        ListBox1.TabIndex = 11
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(7, 87)
        Label4.Name = "Label4"
        Label4.Size = New Size(92, 15)
        Label4.TabIndex = 10
        Label4.Text = "EC Number List:"
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(448, 284)
        Button4.Name = "Button4"
        Button4.Size = New Size(97, 24)
        Button4.TabIndex = 9
        Button4.Text = "LLMs Text"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(563, 284)
        Button3.Name = "Button3"
        Button3.Size = New Size(64, 24)
        Button3.TabIndex = 8
        Button3.Text = "Save"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(105, 167)
        TextBox3.Multiline = True
        TextBox3.Name = "TextBox3"
        TextBox3.ScrollBars = ScrollBars.Both
        TextBox3.Size = New Size(522, 111)
        TextBox3.TabIndex = 7
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(63, 167)
        Label3.Name = "Label3"
        Label3.Size = New Size(36, 15)
        Label3.TabIndex = 6
        Label3.Text = "Note:"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(361, 51)
        Button2.Name = "Button2"
        Button2.Size = New Size(64, 24)
        Button2.TabIndex = 5
        Button2.Text = "Save"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(105, 53)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(250, 23)
        TextBox2.TabIndex = 4
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(28, 56)
        Label2.Name = "Label2"
        Label2.Size = New Size(71, 15)
        Label2.TabIndex = 3
        Label2.Text = "EC Number:"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(563, 21)
        Button1.Name = "Button1"
        Button1.Size = New Size(64, 24)
        Button1.TabIndex = 2
        Button1.Text = "Save"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(105, 22)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(440, 23)
        TextBox1.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(57, 30)
        Label1.Name = "Label1"
        Label1.Size = New Size(42, 15)
        Label1.TabIndex = 0
        Label1.Text = "Name:"
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.Items.AddRange(New ToolStripItem() {ToolStripLabel1, ToolStripButton1, ToolStripLabel2, ToolStripTextBox1, ToolStripButton2, ToolStripButton3, ToolStripSeparator1, ToolStripLabel3, ToolStripTextBox2, ToolStripButton4, ToolStripSeparator2, ToolStripLabel4, ToolStripComboBox1, ToolStripSeparator3, ToolStripButton5, ToolStripSeparator4, ToolStripLabel5, ToolStripButton6, ToolStripButton7})
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(1394, 25)
        ToolStrip1.TabIndex = 2
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' ToolStripLabel1
        ' 
        ToolStripLabel1.Name = "ToolStripLabel1"
        ToolStripLabel1.Size = New Size(97, 22)
        ToolStripLabel1.Text = "Navigation Page:"
        ' 
        ' ToolStripButton1
        ' 
        ToolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), Image)
        ToolStripButton1.ImageTransparentColor = Color.Magenta
        ToolStripButton1.Name = "ToolStripButton1"
        ToolStripButton1.Size = New Size(23, 22)
        ToolStripButton1.Text = "Previous"
        ' 
        ' ToolStripLabel2
        ' 
        ToolStripLabel2.Name = "ToolStripLabel2"
        ToolStripLabel2.Size = New Size(33, 22)
        ToolStripLabel2.Text = "Page"
        ' 
        ' ToolStripTextBox1
        ' 
        ToolStripTextBox1.Name = "ToolStripTextBox1"
        ToolStripTextBox1.Size = New Size(50, 25)
        ToolStripTextBox1.Text = "1"
        ' 
        ' ToolStripButton2
        ' 
        ToolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), Image)
        ToolStripButton2.ImageTransparentColor = Color.Magenta
        ToolStripButton2.Name = "ToolStripButton2"
        ToolStripButton2.Size = New Size(23, 22)
        ToolStripButton2.Text = "Goto"
        ' 
        ' ToolStripButton3
        ' 
        ToolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), Image)
        ToolStripButton3.ImageTransparentColor = Color.Magenta
        ToolStripButton3.Name = "ToolStripButton3"
        ToolStripButton3.Size = New Size(23, 22)
        ToolStripButton3.Text = "Next"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(6, 25)
        ' 
        ' ToolStripLabel3
        ' 
        ToolStripLabel3.Name = "ToolStripLabel3"
        ToolStripLabel3.Size = New Size(118, 22)
        ToolStripLabel3.Text = "Open Reaction By ID:"
        ' 
        ' ToolStripTextBox2
        ' 
        ToolStripTextBox2.Name = "ToolStripTextBox2"
        ToolStripTextBox2.Size = New Size(100, 25)
        ' 
        ' ToolStripButton4
        ' 
        ToolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton4.Image = CType(resources.GetObject("ToolStripButton4.Image"), Image)
        ToolStripButton4.ImageTransparentColor = Color.Magenta
        ToolStripButton4.Name = "ToolStripButton4"
        ToolStripButton4.Size = New Size(23, 22)
        ToolStripButton4.Text = "Search"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(6, 25)
        ' 
        ' ToolStripLabel4
        ' 
        ToolStripLabel4.Name = "ToolStripLabel4"
        ToolStripLabel4.Size = New Size(93, 22)
        ToolStripLabel4.Text = "Filter DB Source:"
        ' 
        ' ToolStripComboBox1
        ' 
        ToolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ToolStripComboBox1.Name = "ToolStripComboBox1"
        ToolStripComboBox1.Size = New Size(121, 25)
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(6, 25)
        ' 
        ' ToolStripButton5
        ' 
        ToolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton5.Image = CType(resources.GetObject("ToolStripButton5.Image"), Image)
        ToolStripButton5.ImageTransparentColor = Color.Magenta
        ToolStripButton5.Name = "ToolStripButton5"
        ToolStripButton5.Size = New Size(23, 22)
        ToolStripButton5.Text = "Add"
        ' 
        ' ToolStripSeparator4
        ' 
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New Size(6, 25)
        ' 
        ' ToolStripLabel5
        ' 
        ToolStripLabel5.Name = "ToolStripLabel5"
        ToolStripLabel5.Size = New Size(134, 22)
        ToolStripLabel5.Text = "Search Substrate Model:"
        ' 
        ' ToolStripButton6
        ' 
        ToolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton6.Image = CType(resources.GetObject("ToolStripButton6.Image"), Image)
        ToolStripButton6.ImageTransparentColor = Color.Magenta
        ToolStripButton6.Name = "ToolStripButton6"
        ToolStripButton6.Size = New Size(23, 22)
        ToolStripButton6.Text = "Search Substrate"
        ' 
        ' ToolStripButton7
        ' 
        ToolStripButton7.DisplayStyle = ToolStripItemDisplayStyle.Image
        ToolStripButton7.Image = CType(resources.GetObject("ToolStripButton7.Image"), Image)
        ToolStripButton7.ImageTransparentColor = Color.Magenta
        ToolStripButton7.Name = "ToolStripButton7"
        ToolStripButton7.Size = New Size(23, 22)
        ToolStripButton7.Text = "Metabolic Symbol Table"
        ' 
        ' ContextMenuStrip2
        ' 
        ContextMenuStrip2.Items.AddRange(New ToolStripItem() {CopyEquationStringToolStripMenuItem})
        ContextMenuStrip2.Name = "ContextMenuStrip2"
        ContextMenuStrip2.Size = New Size(187, 48)
        ' 
        ' CopyEquationStringToolStripMenuItem
        ' 
        CopyEquationStringToolStripMenuItem.Name = "CopyEquationStringToolStripMenuItem"
        CopyEquationStringToolStripMenuItem.Size = New Size(186, 22)
        CopyEquationStringToolStripMenuItem.Text = "Copy Equation String"
        ' 
        ' FormMetabolicEditor
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1394, 821)
        Controls.Add(SplitContainer1)
        Controls.Add(ToolStrip1)
        DockAreas = Microsoft.VisualStudio.WinForms.Docking.DockAreas.Float Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockLeft Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockRight Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockTop Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.DockBottom Or Microsoft.VisualStudio.WinForms.Docking.DockAreas.Document
        DoubleBuffered = True
        Name = "FormMetabolicEditor"
        ShowHint = Microsoft.VisualStudio.WinForms.Docking.DockState.Unknown
        TabPageContextMenuStrip = DockContextMenuStrip1
        Text = "Edit Metabolic Network"
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        CType(DataGridView1, ISupportInitialize).EndInit()
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel2.ResumeLayout(False)
        CType(SplitContainer2, ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        CType(DataGridView2, ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        ContextMenuStrip2.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripLabel2 As ToolStripLabel
    Friend WithEvents ToolStripTextBox1 As ToolStripTextBox
    Friend WithEvents ToolStripButton2 As ToolStripButton
    Friend WithEvents ToolStripButton3 As ToolStripButton
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripLabel3 As ToolStripLabel
    Friend WithEvents ToolStripTextBox2 As ToolStripTextBox
    Friend WithEvents ToolStripButton4 As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripButton5 As ToolStripButton
    Friend WithEvents Column11 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column10 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Button3 As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button4 As Button
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripLabel4 As ToolStripLabel
    Friend WithEvents ToolStripComboBox1 As ToolStripComboBox
    Friend WithEvents CopySymbolNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents Column9 As DataGridViewTextBoxColumn
    Friend WithEvents Column12 As DataGridViewTextBoxColumn
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripLabel5 As ToolStripLabel
    Friend WithEvents ToolStripButton6 As ToolStripButton
    Friend WithEvents ToolStripButton7 As ToolStripButton
    Friend WithEvents Label4 As Label
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Button5 As Button
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents CopyEquationStringToolStripMenuItem As ToolStripMenuItem
End Class
