<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_About
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
        Me.LabelVersion = New System.Windows.Forms.Label()
        Me.LabelInfo = New System.Windows.Forms.Label()
        Me.LabelWebite = New System.Windows.Forms.Label()
        Me.LabelLicense = New System.Windows.Forms.Label()
        Me.LabelIcons = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LabelVersion
        '
        Me.LabelVersion.AutoSize = True
        Me.LabelVersion.BackColor = System.Drawing.Color.Transparent
        Me.LabelVersion.Location = New System.Drawing.Point(12, 256)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(66, 13)
        Me.LabelVersion.TabIndex = 0
        Me.LabelVersion.Text = "Version x.x.x"
        Me.LabelVersion.Visible = False
        '
        'LabelInfo
        '
        Me.LabelInfo.BackColor = System.Drawing.Color.Transparent
        Me.LabelInfo.Location = New System.Drawing.Point(15, 128)
        Me.LabelInfo.Name = "LabelInfo"
        Me.LabelInfo.Size = New System.Drawing.Size(557, 87)
        Me.LabelInfo.TabIndex = 1
        Me.LabelInfo.Text = "Info"
        Me.LabelInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.LabelInfo.Visible = False
        '
        'LabelWebite
        '
        Me.LabelWebite.BackColor = System.Drawing.Color.Transparent
        Me.LabelWebite.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelWebite.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelWebite.Location = New System.Drawing.Point(12, 217)
        Me.LabelWebite.Name = "LabelWebite"
        Me.LabelWebite.Size = New System.Drawing.Size(560, 16)
        Me.LabelWebite.TabIndex = 2
        Me.LabelWebite.Text = "www.airiclenz.com"
        Me.LabelWebite.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LabelLicense
        '
        Me.LabelLicense.BackColor = System.Drawing.Color.Transparent
        Me.LabelLicense.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelLicense.Location = New System.Drawing.Point(110, 256)
        Me.LabelLicense.Name = "LabelLicense"
        Me.LabelLicense.Size = New System.Drawing.Size(364, 13)
        Me.LabelLicense.TabIndex = 3
        Me.LabelLicense.Text = "Licensed under the GPL v3 License"
        Me.LabelLicense.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LabelIcons
        '
        Me.LabelIcons.BackColor = System.Drawing.Color.Transparent
        Me.LabelIcons.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelIcons.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelIcons.Location = New System.Drawing.Point(12, 235)
        Me.LabelIcons.Name = "LabelIcons"
        Me.LabelIcons.Size = New System.Drawing.Size(560, 16)
        Me.LabelIcons.TabIndex = 4
        Me.LabelIcons.Text = "Icons by www.visualpharm.com"
        Me.LabelIcons.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Form_About
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.mEStudio.My.Resources.Resources.about
        Me.ClientSize = New System.Drawing.Size(584, 281)
        Me.Controls.Add(Me.LabelIcons)
        Me.Controls.Add(Me.LabelLicense)
        Me.Controls.Add(Me.LabelWebite)
        Me.Controls.Add(Me.LabelInfo)
        Me.Controls.Add(Me.LabelVersion)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form_About"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form_About"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents LabelInfo As System.Windows.Forms.Label
    Friend WithEvents LabelWebite As System.Windows.Forms.Label
    Friend WithEvents LabelLicense As System.Windows.Forms.Label
    Friend WithEvents LabelIcons As System.Windows.Forms.Label
End Class
