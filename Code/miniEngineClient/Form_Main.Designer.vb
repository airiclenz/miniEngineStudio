<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Main
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Main))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.COMPortToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenLastFileAutomaticallyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreWindowPositionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutocheckForUpdatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.RegistermepFilesWithPathDesignerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckForUpdatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusTimeLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.mComPort = New System.IO.Ports.SerialPort(Me.components)
        Me.ImageList_axes = New System.Windows.Forms.ImageList(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.btn_addAxis = New System.Windows.Forms.ToolStripButton()
        Me.btn_editAxis = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_deleteAxis = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_visible = New System.Windows.Forms.ToolStripButton()
        Me.ListView_Axes = New System.Windows.Forms.ListView()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btn_addSegment = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_DeleteSegment = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_zoomIn = New System.Windows.Forms.ToolStripButton()
        Me.btn_zoomOut = New System.Windows.Forms.ToolStripButton()
        Me.btn_ZoomToFit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_linear = New System.Windows.Forms.ToolStripButton()
        Me.btn_locked = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_showHelper = New System.Windows.Forms.ToolStripButton()
        Me.btn_highContrast = New System.Windows.Forms.ToolStripButton()
        Me.btn_snap = New System.Windows.Forms.ToolStripButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.graph = New mEStudio.TimelineGraph()
        Me.btn_sendDataToME = New System.Windows.Forms.ToolStrip()
        Me.label_spacer = New System.Windows.Forms.ToolStripLabel()
        Me.btn_play = New System.Windows.Forms.ToolStripButton()
        Me.btn_stop = New System.Windows.Forms.ToolStripButton()
        Me.btn_toStart = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_SendData = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.btn_moveToPos = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabel_unit = New System.Windows.Forms.ToolStripLabel()
        Me.edit_moveDistance = New System.Windows.Forms.ToolStripTextBox()
        Me.btn_moveRight = New System.Windows.Forms.ToolStripButton()
        Me.btn_moveLeft = New System.Windows.Forms.ToolStripButton()
        Me.btn_home = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.label_unit = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.edit_Y = New System.Windows.Forms.TextBox()
        Me.edit_X = New System.Windows.Forms.TextBox()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.btn_sendDataToME.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.SettingsToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1054, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ToolStripMenuItem3, Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem, Me.SaveFileToolStripMenuItem, Me.ToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(120, 6)
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.OpenToolStripMenuItem.Text = "Open..."
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Enabled = False
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'SaveFileToolStripMenuItem
        '
        Me.SaveFileToolStripMenuItem.Name = "SaveFileToolStripMenuItem"
        Me.SaveFileToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.SaveFileToolStripMenuItem.Text = "Save As..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(120, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = Global.mEStudio.My.Resources.Resources.logout_26
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.ExitToolStripMenuItem.Text = "Exit..."
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.CheckOnClick = True
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.COMPortToolStripMenuItem, Me.ToolStripMenuItem2, Me.OpenLastFileAutomaticallyToolStripMenuItem, Me.RestoreWindowPositionToolStripMenuItem, Me.AutocheckForUpdatesToolStripMenuItem, Me.ToolStripMenuItem5, Me.RegistermepFilesWithPathDesignerToolStripMenuItem})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'COMPortToolStripMenuItem
        '
        Me.COMPortToolStripMenuItem.Image = Global.mEStudio.My.Resources.Resources.broken_link_26
        Me.COMPortToolStripMenuItem.Name = "COMPortToolStripMenuItem"
        Me.COMPortToolStripMenuItem.Size = New System.Drawing.Size(301, 22)
        Me.COMPortToolStripMenuItem.Text = "COM Port - None"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(298, 6)
        '
        'OpenLastFileAutomaticallyToolStripMenuItem
        '
        Me.OpenLastFileAutomaticallyToolStripMenuItem.Checked = True
        Me.OpenLastFileAutomaticallyToolStripMenuItem.CheckOnClick = True
        Me.OpenLastFileAutomaticallyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.OpenLastFileAutomaticallyToolStripMenuItem.Name = "OpenLastFileAutomaticallyToolStripMenuItem"
        Me.OpenLastFileAutomaticallyToolStripMenuItem.Size = New System.Drawing.Size(301, 22)
        Me.OpenLastFileAutomaticallyToolStripMenuItem.Text = "Open last file automatically"
        '
        'RestoreWindowPositionToolStripMenuItem
        '
        Me.RestoreWindowPositionToolStripMenuItem.Checked = True
        Me.RestoreWindowPositionToolStripMenuItem.CheckOnClick = True
        Me.RestoreWindowPositionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RestoreWindowPositionToolStripMenuItem.Name = "RestoreWindowPositionToolStripMenuItem"
        Me.RestoreWindowPositionToolStripMenuItem.Size = New System.Drawing.Size(301, 22)
        Me.RestoreWindowPositionToolStripMenuItem.Text = "Restore Window Position"
        '
        'AutocheckForUpdatesToolStripMenuItem
        '
        Me.AutocheckForUpdatesToolStripMenuItem.Checked = True
        Me.AutocheckForUpdatesToolStripMenuItem.CheckOnClick = True
        Me.AutocheckForUpdatesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutocheckForUpdatesToolStripMenuItem.Name = "AutocheckForUpdatesToolStripMenuItem"
        Me.AutocheckForUpdatesToolStripMenuItem.Size = New System.Drawing.Size(301, 22)
        Me.AutocheckForUpdatesToolStripMenuItem.Text = "Auto-check for Updates"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(298, 6)
        '
        'RegistermepFilesWithPathDesignerToolStripMenuItem
        '
        Me.RegistermepFilesWithPathDesignerToolStripMenuItem.CheckOnClick = True
        Me.RegistermepFilesWithPathDesignerToolStripMenuItem.Image = Global.mEStudio.My.Resources.Resources.icon_mec_26
        Me.RegistermepFilesWithPathDesignerToolStripMenuItem.Name = "RegistermepFilesWithPathDesignerToolStripMenuItem"
        Me.RegistermepFilesWithPathDesignerToolStripMenuItem.Size = New System.Drawing.Size(301, 22)
        Me.RegistermepFilesWithPathDesignerToolStripMenuItem.Text = "Register *.mep files with miniEngine Studio"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckForUpdatesToolStripMenuItem, Me.ToolStripMenuItem4, Me.AboutToolStripMenuItem1})
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(82, 20)
        Me.AboutToolStripMenuItem.Text = "Information"
        '
        'CheckForUpdatesToolStripMenuItem
        '
        Me.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem"
        Me.CheckForUpdatesToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.CheckForUpdatesToolStripMenuItem.Text = "Check for Updates"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(168, 6)
        '
        'AboutToolStripMenuItem1
        '
        Me.AboutToolStripMenuItem1.Name = "AboutToolStripMenuItem1"
        Me.AboutToolStripMenuItem1.Size = New System.Drawing.Size(171, 22)
        Me.AboutToolStripMenuItem1.Text = "About..."
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusTimeLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 544)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1054, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(300, 17)
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusTimeLabel
        '
        Me.ToolStripStatusTimeLabel.Name = "ToolStripStatusTimeLabel"
        Me.ToolStripStatusTimeLabel.Size = New System.Drawing.Size(0, 17)
        '
        'mComPort
        '
        Me.mComPort.BaudRate = 19200
        '
        'ImageList_axes
        '
        Me.ImageList_axes.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit
        Me.ImageList_axes.ImageSize = New System.Drawing.Size(32, 16)
        Me.ImageList_axes.TransparentColor = System.Drawing.Color.Transparent
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "mec"
        Me.OpenFileDialog1.Filter = """miniEngine Path file|*.mep|All files|*.*"""
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "miniEngineSetup"
        Me.SaveFileDialog1.Filter = """miniEngine Path file|*.mep|All files|*.*"""
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 27)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.SplitContainer1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btn_sendDataToME)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer2.Size = New System.Drawing.Size(1054, 514)
        Me.SplitContainer2.SplitterDistance = 393
        Me.SplitContainer2.TabIndex = 7
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListView_Axes)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.SplitContainer1.Panel2.Controls.Add(Me.ToolStrip1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1054, 360)
        Me.SplitContainer1.SplitterDistance = 220
        Me.SplitContainer1.TabIndex = 6
        '
        'ToolStrip2
        '
        Me.ToolStrip2.AutoSize = False
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btn_addAxis, Me.btn_editAxis, Me.ToolStripSeparator6, Me.btn_deleteAxis, Me.ToolStripSeparator5, Me.btn_visible})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(220, 25)
        Me.ToolStrip2.TabIndex = 3
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'btn_addAxis
        '
        Me.btn_addAxis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_addAxis.Image = Global.mEStudio.My.Resources.Resources.add_stepper_motor_26
        Me.btn_addAxis.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_addAxis.Name = "btn_addAxis"
        Me.btn_addAxis.Size = New System.Drawing.Size(23, 22)
        Me.btn_addAxis.Text = "Add an axis"
        '
        'btn_editAxis
        '
        Me.btn_editAxis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_editAxis.Enabled = False
        Me.btn_editAxis.Image = Global.mEStudio.My.Resources.Resources.edit_26
        Me.btn_editAxis.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_editAxis.Name = "btn_editAxis"
        Me.btn_editAxis.Size = New System.Drawing.Size(23, 22)
        Me.btn_editAxis.Text = "ToolStripButton1"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.AutoSize = False
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(25, 25)
        '
        'btn_deleteAxis
        '
        Me.btn_deleteAxis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_deleteAxis.Enabled = False
        Me.btn_deleteAxis.Image = Global.mEStudio.My.Resources.Resources.delete_26
        Me.btn_deleteAxis.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_deleteAxis.Name = "btn_deleteAxis"
        Me.btn_deleteAxis.Size = New System.Drawing.Size(23, 22)
        Me.btn_deleteAxis.Text = "Delete the selected axis"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.AutoSize = False
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(25, 25)
        '
        'btn_visible
        '
        Me.btn_visible.CheckOnClick = True
        Me.btn_visible.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_visible.Enabled = False
        Me.btn_visible.Image = Global.mEStudio.My.Resources.Resources.visible_26
        Me.btn_visible.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_visible.Name = "btn_visible"
        Me.btn_visible.Size = New System.Drawing.Size(23, 22)
        Me.btn_visible.Text = "Visibility of the axis"
        '
        'ListView_Axes
        '
        Me.ListView_Axes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_Axes.FullRowSelect = True
        Me.ListView_Axes.HideSelection = False
        Me.ListView_Axes.Location = New System.Drawing.Point(3, 28)
        Me.ListView_Axes.MultiSelect = False
        Me.ListView_Axes.Name = "ListView_Axes"
        Me.ListView_Axes.Size = New System.Drawing.Size(214, 299)
        Me.ListView_Axes.StateImageList = Me.ImageList_axes
        Me.ListView_Axes.TabIndex = 1
        Me.ListView_Axes.UseCompatibleStateImageBehavior = False
        Me.ListView_Axes.View = System.Windows.Forms.View.List
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btn_addSegment, Me.ToolStripSeparator7, Me.btn_DeleteSegment, Me.ToolStripSeparator3, Me.btn_zoomIn, Me.btn_zoomOut, Me.btn_ZoomToFit, Me.ToolStripSeparator1, Me.btn_linear, Me.btn_locked, Me.ToolStripSeparator2, Me.btn_showHelper, Me.btn_highContrast, Me.btn_snap})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(830, 25)
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btn_addSegment
        '
        Me.btn_addSegment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_addSegment.Enabled = False
        Me.btn_addSegment.Image = Global.mEStudio.My.Resources.Resources.new_curve_26
        Me.btn_addSegment.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_addSegment.Name = "btn_addSegment"
        Me.btn_addSegment.Size = New System.Drawing.Size(23, 22)
        Me.btn_addSegment.Text = "Add a new segment"
        Me.btn_addSegment.ToolTipText = "Add a Segment"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.AutoSize = False
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(25, 25)
        '
        'btn_DeleteSegment
        '
        Me.btn_DeleteSegment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_DeleteSegment.Enabled = False
        Me.btn_DeleteSegment.Image = Global.mEStudio.My.Resources.Resources.delete_26
        Me.btn_DeleteSegment.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_DeleteSegment.Name = "btn_DeleteSegment"
        Me.btn_DeleteSegment.Size = New System.Drawing.Size(23, 22)
        Me.btn_DeleteSegment.Text = "Delete the last segment"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.AutoSize = False
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(25, 25)
        '
        'btn_zoomIn
        '
        Me.btn_zoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_zoomIn.Image = Global.mEStudio.My.Resources.Resources.zoom_in_26
        Me.btn_zoomIn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_zoomIn.Name = "btn_zoomIn"
        Me.btn_zoomIn.Size = New System.Drawing.Size(23, 22)
        Me.btn_zoomIn.Text = "Zoom In [+]"
        '
        'btn_zoomOut
        '
        Me.btn_zoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_zoomOut.Image = Global.mEStudio.My.Resources.Resources.zoom_out_26
        Me.btn_zoomOut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_zoomOut.Name = "btn_zoomOut"
        Me.btn_zoomOut.Size = New System.Drawing.Size(23, 22)
        Me.btn_zoomOut.Text = "Zoom Out [-]"
        '
        'btn_ZoomToFit
        '
        Me.btn_ZoomToFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_ZoomToFit.Image = Global.mEStudio.My.Resources.Resources.zoom_to_fit
        Me.btn_ZoomToFit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_ZoomToFit.Name = "btn_ZoomToFit"
        Me.btn_ZoomToFit.Size = New System.Drawing.Size(23, 22)
        Me.btn_ZoomToFit.Text = "Zoom to fit screen"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.AutoSize = False
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(25, 25)
        '
        'btn_linear
        '
        Me.btn_linear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_linear.Enabled = False
        Me.btn_linear.Image = Global.mEStudio.My.Resources.Resources.make_linear_26
        Me.btn_linear.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_linear.Name = "btn_linear"
        Me.btn_linear.Size = New System.Drawing.Size(23, 22)
        Me.btn_linear.Text = "Make Segment Linear"
        '
        'btn_locked
        '
        Me.btn_locked.CheckOnClick = True
        Me.btn_locked.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_locked.Image = Global.mEStudio.My.Resources.Resources.locked_curve_26
        Me.btn_locked.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_locked.Name = "btn_locked"
        Me.btn_locked.Size = New System.Drawing.Size(23, 22)
        Me.btn_locked.Text = "Set the point's helpers to be locked against each other (symetrical)"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.AutoSize = False
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(25, 25)
        '
        'btn_showHelper
        '
        Me.btn_showHelper.Checked = True
        Me.btn_showHelper.CheckOnClick = True
        Me.btn_showHelper.CheckState = System.Windows.Forms.CheckState.Checked
        Me.btn_showHelper.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_showHelper.Image = Global.mEStudio.My.Resources.Resources.clear_curve_26
        Me.btn_showHelper.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_showHelper.Name = "btn_showHelper"
        Me.btn_showHelper.Size = New System.Drawing.Size(23, 22)
        Me.btn_showHelper.Text = "Show Helpers and Points"
        Me.btn_showHelper.ToolTipText = "Show Helper and Points"
        '
        'btn_highContrast
        '
        Me.btn_highContrast.Checked = True
        Me.btn_highContrast.CheckOnClick = True
        Me.btn_highContrast.CheckState = System.Windows.Forms.CheckState.Checked
        Me.btn_highContrast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_highContrast.Image = Global.mEStudio.My.Resources.Resources.contrast_26
        Me.btn_highContrast.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_highContrast.Name = "btn_highContrast"
        Me.btn_highContrast.Size = New System.Drawing.Size(23, 22)
        Me.btn_highContrast.Text = "High Contrast Mode"
        '
        'btn_snap
        '
        Me.btn_snap.CheckOnClick = True
        Me.btn_snap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_snap.Image = Global.mEStudio.My.Resources.Resources.snap_26
        Me.btn_snap.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_snap.Name = "btn_snap"
        Me.btn_snap.Size = New System.Drawing.Size(23, 22)
        Me.btn_snap.Text = "Snap to full units"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.graph)
        Me.Panel1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Panel1.Location = New System.Drawing.Point(3, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(824, 299)
        Me.Panel1.TabIndex = 4
        '
        'graph
        '
        Me.graph.AutoUpdate = True
        Me.graph.CurveLineWidth = 2.0!
        Me.graph.FPS = 25
        Me.graph.HighContrast = False
        Me.graph.Location = New System.Drawing.Point(3, 3)
        Me.graph.Name = "graph"
        Me.graph.RulerColor = System.Drawing.Color.Silver
        Me.graph.ShowHelper = True
        Me.graph.Size = New System.Drawing.Size(595, 247)
        Me.graph.SnapToUnit = False
        Me.graph.TabIndex = 0
        Me.graph.ZoomFactor = 10
        '
        'btn_sendDataToME
        '
        Me.btn_sendDataToME.BackColor = System.Drawing.Color.Transparent
        Me.btn_sendDataToME.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btn_sendDataToME.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.btn_sendDataToME.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.label_spacer, Me.btn_play, Me.btn_stop, Me.btn_toStart, Me.ToolStripSeparator4, Me.btn_SendData, Me.ToolStripSeparator9, Me.btn_moveToPos, Me.ToolStripLabel1, Me.ToolStripLabel_unit, Me.edit_moveDistance, Me.btn_moveRight, Me.btn_moveLeft, Me.btn_home, Me.ToolStripSeparator8})
        Me.btn_sendDataToME.Location = New System.Drawing.Point(0, 360)
        Me.btn_sendDataToME.Name = "btn_sendDataToME"
        Me.btn_sendDataToME.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btn_sendDataToME.Size = New System.Drawing.Size(1054, 33)
        Me.btn_sendDataToME.TabIndex = 5
        Me.btn_sendDataToME.Text = "ToolStrip3"
        '
        'label_spacer
        '
        Me.label_spacer.AutoSize = False
        Me.label_spacer.Name = "label_spacer"
        Me.label_spacer.Size = New System.Drawing.Size(20, 30)
        '
        'btn_play
        '
        Me.btn_play.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_play.Enabled = False
        Me.btn_play.Image = Global.mEStudio.My.Resources.Resources.play_26
        Me.btn_play.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_play.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_play.Name = "btn_play"
        Me.btn_play.Size = New System.Drawing.Size(30, 30)
        Me.btn_play.Text = "Play"
        '
        'btn_stop
        '
        Me.btn_stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_stop.Enabled = False
        Me.btn_stop.Image = Global.mEStudio.My.Resources.Resources.stop_play_26
        Me.btn_stop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_stop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_stop.Name = "btn_stop"
        Me.btn_stop.Size = New System.Drawing.Size(30, 30)
        Me.btn_stop.Text = "Stop"
        '
        'btn_toStart
        '
        Me.btn_toStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_toStart.Enabled = False
        Me.btn_toStart.Image = Global.mEStudio.My.Resources.Resources.skip_to_start_26
        Me.btn_toStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_toStart.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_toStart.Name = "btn_toStart"
        Me.btn_toStart.Size = New System.Drawing.Size(30, 30)
        Me.btn_toStart.Text = "Move all motors to start key-frame"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.AutoSize = False
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(33, 33)
        '
        'btn_SendData
        '
        Me.btn_SendData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_SendData.Enabled = False
        Me.btn_SendData.Image = CType(resources.GetObject("btn_SendData.Image"), System.Drawing.Image)
        Me.btn_SendData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_SendData.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_SendData.Name = "btn_SendData"
        Me.btn_SendData.Size = New System.Drawing.Size(30, 30)
        Me.btn_SendData.Text = "Send data to the miniEngine"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.AutoSize = False
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(33, 33)
        '
        'btn_moveToPos
        '
        Me.btn_moveToPos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_moveToPos.Enabled = False
        Me.btn_moveToPos.Image = Global.mEStudio.My.Resources.Resources.move_to_pos
        Me.btn_moveToPos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_moveToPos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_moveToPos.Name = "btn_moveToPos"
        Me.btn_moveToPos.Size = New System.Drawing.Size(30, 30)
        Me.btn_moveToPos.Text = "ToolStripButton1"
        Me.btn_moveToPos.ToolTipText = "Move motor to position"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.AutoSize = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(15, 30)
        '
        'ToolStripLabel_unit
        '
        Me.ToolStripLabel_unit.Name = "ToolStripLabel_unit"
        Me.ToolStripLabel_unit.Size = New System.Drawing.Size(13, 30)
        Me.ToolStripLabel_unit.Text = "  "
        '
        'edit_moveDistance
        '
        Me.edit_moveDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edit_moveDistance.Enabled = False
        Me.edit_moveDistance.Name = "edit_moveDistance"
        Me.edit_moveDistance.Size = New System.Drawing.Size(50, 33)
        Me.edit_moveDistance.Text = "1"
        Me.edit_moveDistance.ToolTipText = "Distance to move manually / Position to be moved to"
        '
        'btn_moveRight
        '
        Me.btn_moveRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_moveRight.Enabled = False
        Me.btn_moveRight.Image = Global.mEStudio.My.Resources.Resources.fast_forward_26
        Me.btn_moveRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_moveRight.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_moveRight.Name = "btn_moveRight"
        Me.btn_moveRight.Size = New System.Drawing.Size(30, 30)
        Me.btn_moveRight.Text = "Move clockwise"
        '
        'btn_moveLeft
        '
        Me.btn_moveLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_moveLeft.Enabled = False
        Me.btn_moveLeft.Image = Global.mEStudio.My.Resources.Resources.rewind_26
        Me.btn_moveLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_moveLeft.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_moveLeft.Name = "btn_moveLeft"
        Me.btn_moveLeft.Size = New System.Drawing.Size(30, 30)
        Me.btn_moveLeft.Text = "Move counter-clockwise"
        '
        'btn_home
        '
        Me.btn_home.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btn_home.Enabled = False
        Me.btn_home.Image = Global.mEStudio.My.Resources.Resources.home_26
        Me.btn_home.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btn_home.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btn_home.Name = "btn_home"
        Me.btn_home.Size = New System.Drawing.Size(30, 30)
        Me.btn_home.Text = "Move motor to home position"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.AutoSize = False
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(33, 33)
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.label_unit)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.edit_Y)
        Me.GroupBox1.Controls.Add(Me.edit_X)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1048, 111)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Point Settings"
        '
        'label_unit
        '
        Me.label_unit.AutoSize = True
        Me.label_unit.Location = New System.Drawing.Point(232, 53)
        Me.label_unit.Name = "label_unit"
        Me.label_unit.Size = New System.Drawing.Size(0, 13)
        Me.label_unit.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(232, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(24, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "sec"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Axis position (Y):"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Time position (X):"
        '
        'edit_Y
        '
        Me.edit_Y.Enabled = False
        Me.edit_Y.Location = New System.Drawing.Point(126, 50)
        Me.edit_Y.Name = "edit_Y"
        Me.edit_Y.Size = New System.Drawing.Size(100, 20)
        Me.edit_Y.TabIndex = 1
        '
        'edit_X
        '
        Me.edit_X.Enabled = False
        Me.edit_X.Location = New System.Drawing.Point(126, 24)
        Me.edit_X.Name = "edit_X"
        Me.edit_X.Size = New System.Drawing.Size(100, 20)
        Me.edit_X.TabIndex = 0
        '
        'Form_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1054, 566)
        Me.Controls.Add(Me.SplitContainer2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Enabled = False
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "Form_Main"
        Me.Opacity = 0.0R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "miniEngine Studio"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.btn_sendDataToME.ResumeLayout(False)
        Me.btn_sendDataToME.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents mComPort As System.IO.Ports.SerialPort
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreWindowPositionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusTimeLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents COMPortToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ImageList_axes As System.Windows.Forms.ImageList
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenLastFileAutomaticallyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents label_unit As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents edit_Y As System.Windows.Forms.TextBox
    Friend WithEvents edit_X As System.Windows.Forms.TextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents btn_addAxis As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_deleteAxis As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_visible As System.Windows.Forms.ToolStripButton
    Friend WithEvents ListView_Axes As System.Windows.Forms.ListView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btn_addSegment As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_DeleteSegment As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_linear As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_zoomIn As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_zoomOut As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_ZoomToFit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_showHelper As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_highContrast As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_snap As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents graph As mEStudio.TimelineGraph
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_locked As System.Windows.Forms.ToolStripButton
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btn_editAxis As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AutocheckForUpdatesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckForUpdatesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RegistermepFilesWithPathDesignerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btn_sendDataToME As System.Windows.Forms.ToolStrip
    Friend WithEvents label_spacer As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btn_play As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_stop As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_toStart As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_SendData As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel_unit As System.Windows.Forms.ToolStripLabel
    Friend WithEvents edit_moveDistance As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents btn_moveRight As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_moveLeft As System.Windows.Forms.ToolStripButton
    Friend WithEvents btn_home As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btn_moveToPos As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel

End Class
