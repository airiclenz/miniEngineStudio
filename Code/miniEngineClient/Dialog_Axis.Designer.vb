<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dialog_Axis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dialog_Axis))
        Me.edit_Name = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.combo_Type = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel_color = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.btn_OK = New System.Windows.Forms.Button()
        Me.btn_Cancel = New System.Windows.Forms.Button()
        Me.edit_Calibration = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label_cal_unit = New System.Windows.Forms.Label()
        Me.combo_Motor = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'edit_Name
        '
        Me.edit_Name.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edit_Name.Location = New System.Drawing.Point(160, 46)
        Me.edit_Name.Name = "edit_Name"
        Me.edit_Name.Size = New System.Drawing.Size(388, 20)
        Me.edit_Name.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Axis Name:"
        '
        'combo_Type
        '
        Me.combo_Type.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.combo_Type.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.combo_Type.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.combo_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.combo_Type.FormattingEnabled = True
        Me.combo_Type.Items.AddRange(New Object() {"Linear", "Radial"})
        Me.combo_Type.Location = New System.Drawing.Point(160, 72)
        Me.combo_Type.Name = "combo_Type"
        Me.combo_Type.Size = New System.Drawing.Size(388, 21)
        Me.combo_Type.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Type:"
        '
        'Panel_color
        '
        Me.Panel_color.BackColor = System.Drawing.Color.Orange
        Me.Panel_color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel_color.Location = New System.Drawing.Point(160, 125)
        Me.Panel_color.Name = "Panel_color"
        Me.Panel_color.Size = New System.Drawing.Size(50, 20)
        Me.Panel_color.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Color:"
        '
        'btn_OK
        '
        Me.btn_OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_OK.Location = New System.Drawing.Point(392, 170)
        Me.btn_OK.Name = "btn_OK"
        Me.btn_OK.Size = New System.Drawing.Size(75, 23)
        Me.btn_OK.TabIndex = 6
        Me.btn_OK.Text = "OK"
        Me.btn_OK.UseVisualStyleBackColor = True
        '
        'btn_Cancel
        '
        Me.btn_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_Cancel.Location = New System.Drawing.Point(473, 170)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.btn_Cancel.TabIndex = 7
        Me.btn_Cancel.Text = "Cancel"
        Me.btn_Cancel.UseVisualStyleBackColor = True
        '
        'edit_Calibration
        '
        Me.edit_Calibration.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.edit_Calibration.Location = New System.Drawing.Point(160, 99)
        Me.edit_Calibration.Name = "edit_Calibration"
        Me.edit_Calibration.Size = New System.Drawing.Size(323, 20)
        Me.edit_Calibration.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 102)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Calibration:"
        '
        'Label_cal_unit
        '
        Me.Label_cal_unit.AutoSize = True
        Me.Label_cal_unit.Location = New System.Drawing.Point(489, 102)
        Me.Label_cal_unit.Name = "Label_cal_unit"
        Me.Label_cal_unit.Size = New System.Drawing.Size(57, 13)
        Me.Label_cal_unit.TabIndex = 10
        Me.Label_cal_unit.Text = "steps / cm"
        Me.Label_cal_unit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'combo_Motor
        '
        Me.combo_Motor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.combo_Motor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.combo_Motor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.combo_Motor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.combo_Motor.FormattingEnabled = True
        Me.combo_Motor.Items.AddRange(New Object() {"Linear", "Radial"})
        Me.combo_Motor.Location = New System.Drawing.Point(158, 19)
        Me.combo_Motor.Name = "combo_Motor"
        Me.combo_Motor.Size = New System.Drawing.Size(388, 21)
        Me.combo_Motor.Sorted = True
        Me.combo_Motor.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "miniEngine Motor:"
        '
        'Dialog_Axis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(560, 205)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.combo_Motor)
        Me.Controls.Add(Me.Label_cal_unit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.edit_Calibration)
        Me.Controls.Add(Me.btn_Cancel)
        Me.Controls.Add(Me.btn_OK)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel_color)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.combo_Type)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.edit_Name)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Dialog_Axis"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Axis"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents edit_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents combo_Type As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel_color As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents btn_OK As System.Windows.Forms.Button
    Friend WithEvents btn_Cancel As System.Windows.Forms.Button
    Friend WithEvents edit_Calibration As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label_cal_unit As System.Windows.Forms.Label
    Friend WithEvents combo_Motor As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
