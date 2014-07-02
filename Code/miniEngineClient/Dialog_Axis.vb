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


    ' =========================================================================
    Private Sub edit_Calibration_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles edit_Calibration.KeyPress


        Dim seperator As String = Utils.GetDecimalSeperator()


        If (Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or
            (Asc(e.KeyChar) = 8) Or
            (e.KeyChar = seperator) Then

            ' nothing
        Else
            ' set this event to handled so that
            ' it is ignored from here on
            e.Handled = True
        End If

    End Sub



    ' =========================================================================
    Private Sub combo_Type_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles combo_Type.SelectedIndexChanged
        If combo_Type.SelectedItem.ToString = "Linear" Then
            Label_cal_unit.Text = "steps / cm"
        Else
            Label_cal_unit.Text = "steps / °"
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

        ' Update the list of available Axes
        Dim freeMotors = Form_Main.GetUplinkUnusedAxes()

        combo_Motor.Items.Clear()
        For Each motor As Axis In freeMotors
            combo_Motor.Items.Add(motor.Name)
        Next

        combo_Motor.Enabled = Form_Main.IsUplinkConnected


        ' if there is already a motor selected on the current axis,
        ' add this to the dropdown and select it
        If mAxis.MotorNumber <> -1 Then
            combo_Motor.Items.Add("Motor " + CStr(mAxis.MotorNumber + 1))
            combo_Motor.SelectedItem = "Motor " + CStr(mAxis.MotorNumber + 1)
        Else
            ' there is no axis selected
            ' select the first availabel one
            If combo_Motor.Items.Count >= 1 Then
                combo_Motor.SelectedIndex = 0
            End If

        End If




        edit_Name.Text = mAxis.Name

        If mAxis.Type = AxisType.Linear Then
            combo_Type.SelectedItem = "Linear"
        Else
            combo_Type.SelectedItem = "Radial"
        End If

        edit_Calibration.Text = mAxis.Calibration.ToString

        Panel_color.BackColor = mAxis.Color

    End Sub





    ' =========================================================================
    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click

        If combo_Motor.SelectedItem Is Nothing And
            Form_Main.IsUplinkConnected Then

            MessageBox.Show("You need to select a motor for this axis!",
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        mAxis.Name = edit_Name.Text

        If combo_Type.SelectedItem.ToString = "Linear" Then
            mAxis.Type = AxisType.Linear
        Else
            mAxis.Type = AxisType.Radial
        End If

        mAxis.Calibration = Single.Parse(edit_Calibration.Text)

        mAxis.Color = Panel_color.BackColor

        DialogResult = DialogResult.OK

    End Sub

    ' =========================================================================
    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click

        DialogResult = DialogResult.Cancel

    End Sub



    ' =========================================================================
    Private Sub combo_Motor_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles combo_Motor.SelectedValueChanged

        ' Now make all motors that are currently not selected by this axis available again...
        Dim mEmotors As List(Of Axis) = Form_Main.GetUplinkAxes

        ' loop all items of the dropdown
        For Each item As String In combo_Motor.Items

            ' get the motor number for the current item
            Dim num As Byte = Byte.Parse(item.Substring(5))

            ' is the current item also the selected one?
            If item = combo_Motor.SelectedItem.ToString Then

                mAxis.MotorNumber = num - 1

                For Each motor As Axis In mEmotors
                    If motor.Name = item Then
                        ' make the motor unavailable
                        motor.Used = True
                    End If
                Next

                ' update the calibration value to the one from the selected motor



                mAxis.Calibration = Form_Main.GetUplinkAxisWithMotornumber(mAxis.MotorNumber).Calibration
                edit_Calibration.Text = mAxis.Calibration.ToString


                ' update the motor type to the value from the new motor
                mAxis.Type = Form_Main.GetUplinkAxisWithMotornumber(mAxis.MotorNumber).Type
                If mAxis.Type = AxisType.Linear Then
                    combo_Type.SelectedItem = "Linear"
                Else
                    combo_Type.SelectedItem = "Radial"
                End If


            Else
                ' if this item is not selected make
                ' it avialable again
                For Each motor As Axis In mEmotors
                    If motor.Name = item Then
                        ' make the motor availale again
                        motor.Used = False
                    End If
                Next

            End If

        Next


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

End Class