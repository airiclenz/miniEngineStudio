' ////////////////////////////////////////////////////////////////////////////////////////
' //                                                                                    //
' //  Author: Airic Lenz                                                                //
' //                                                                                    //
' //  See http://airiclenz.com for more information                                     //
' //                                                                                    //
' //  This program is free software: you can redistribute it and/or modify              //
' //  it under the terms of the GNU General Public License as published by              //
' //  the Free Software Foundation, either version 3 of the License, or                 //
' //  (at your option) any later version.                                               //
' //                                                                                    //
' //  This program is distributed in the hope that it will be useful,                   // 
' //  but WITHOUT ANY WARRANTY; without even the implied warranty of                    //
' //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                     //
' //  GNU General Public License for more details.                                      //
' //                                                                                    //
' //  You should have received a copy of the GNU General Public License                 // 
' //  along with this program.  If not, see http://www.gnu.org/licenses/.               //
' //                                                                                    //
' ////////////////////////////////////////////////////////////////////////////////////////

Option Strict On
Option Explicit On

' =========================================================================
' =========================================================================
' =========================================================================
Public Class Form_About


    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R I V A T E   F U N C T I O N S                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Private Sub Form_About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LabelVersion.Text = "Version " + Form_Main.VersionString
        LabelVersion.BackColor = Color.Transparent
        LabelVersion.ForeColor = Color.FromArgb(27, 93, 55)
        LabelVersion.Visible = True

        LabelInfo.Text = My.Resources.about_info.ToString
        LabelInfo.BackColor = Color.Transparent
        LabelInfo.ForeColor = Color.FromArgb(22, 74, 44)
        LabelInfo.Visible = True

        LabelWebite.ForeColor = Color.FromArgb(22, 74, 60)
        LabelLicense.ForeColor = Color.FromArgb(22, 74, 60)
        LabelIcons.ForeColor = Color.FromArgb(22, 74, 60)

    End Sub


    ' =========================================================================
    Private Sub Form_About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click
        Close()
    End Sub

    ' =========================================================================
    Private Sub LabelInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelInfo.Click
        Close()
    End Sub

    ' =========================================================================
    Private Sub LabelWebite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelWebite.Click
        Process.Start("http://minie.airiclenz.com")
        Me.Close()
    End Sub

    ' =========================================================================
    Private Sub LabelLicense_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelLicense.Click
        Process.Start("https://www.gnu.org/copyleft/gpl.html")
        Me.Close()
    End Sub

    ' =========================================================================
    Private Sub LabelIcons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelIcons.Click
        Process.Start("http://www.visualpharm.com")
        Me.Close()
    End Sub
End Class