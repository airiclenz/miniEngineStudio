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
Public Class Dialog_Axis

    Private mAxis As Axis


    ' =========================================================================
    Private Sub Panel1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel_color.MouseClick

        If ColorDialog1.ShowDialog = DialogResult.OK Then

            Panel_color.BackColor = ColorDialog1.Color
            mAxis.Color = ColorDialog1.Color

        End If


    End Sub




    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////



    ' =========================================================================
    ''' <summary>
    ''' This function updates the user interface so that it shows the
    ''' actual values of the axis property.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateDialog()

        edit_Name.Text = mAxis.Name

        If mAxis.Type = AxisType.Linear Then
            combo_Type.SelectedItem = "Linear"
        Else
            combo_Type.SelectedItem = "Radial"
        End If


        Panel_color.BackColor = mAxis.Color

    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////



    ' =========================================================================
    Public Property Axis As Axis
        Get
            Return mAxis
        End Get
        Set(ByVal value As Axis)
            mAxis = value
            UpdateDialog()
        End Set
    End Property

    ' =========================================================================
    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click

        mAxis.Name = edit_Name.Text

        If combo_Type.SelectedItem.ToString = "Linear" Then
            mAxis.Type = AxisType.Linear
        Else
            mAxis.Type = AxisType.Radial
        End If

        mAxis.Color = Panel_color.BackColor

        DialogResult = DialogResult.OK

    End Sub

    ' =========================================================================
    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click

        DialogResult = DialogResult.Cancel

    End Sub
End Class