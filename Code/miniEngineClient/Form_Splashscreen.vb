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
Public Class Form_Splashscreen


    Private mFadeIn As Boolean = True
    Private Const mDisplayDuration = 4000   ' ms
    Private Const mClickDuration = 1700     ' ms

    Private mStartTime As Long

    Private WithEvents mTimer_Main As Timer
    Private WithEvents mTimer_Fade As Timer




    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    E V E N T S   I N                                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Private Sub Form_Splashscreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Opacity = 0

        LabelVersion.Text = "Version " + Form_Main.VersionString

        LabelVersion.BackColor = Color.Transparent
        LabelVersion.ForeColor = Color.FromArgb(27, 93, 55)

        LabelVersion.Visible = True

        mStartTime = Utils.Millis()

        mTimer_Main = New Timer()
        mTimer_Main.Interval = mDisplayDuration
        mTimer_Main.Start()

        mTimer_Fade = New Timer()
        mTimer_Fade.Interval = 20   ' 20 ms
        mTimer_Fade.Start()

    End Sub

    ' =========================================================================
    Private Sub Form_Splashscreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click

        ' if the splashscreen was shown for at theast the needed time
        If (mStartTime + mClickDuration) < Utils.Millis Then

            mTimer_Main.Stop()
            EndSplashscreen()

        End If

    End Sub


    ' =========================================================================
    Private Sub TimerMainTick() Handles mTimer_Main.Tick

        mTimer_Main.Stop()
        EndSplashscreen()

    End Sub


    ' =========================================================================
    Private Sub TimerFadeTick() Handles mTimer_Fade.Tick

        If mFadeIn Then

            If Opacity < 1 Then
                Opacity = Opacity + 0.2
            Else
                mTimer_Fade.Stop()
                mFadeIn = False
            End If

        Else

            If Opacity > 0 Then
                Opacity = Opacity - 0.05
            Else
                mTimer_Fade.Stop()
                Close()

                Form_Main.CheckVersion()
                Form_Main.CheckRegistry()

            End If

        End If

        Application.DoEvents()
        Application.RaiseIdle(New System.EventArgs)

    End Sub





    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R I V A T E   F U N C T I O N S                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Private Sub EndSplashscreen()

        Form_Main.Opacity = 1
        Form_Main.Enabled = True

        mTimer_Fade = New Timer()
        mTimer_Fade.Interval = 20   ' 20 ms
        mTimer_Fade.Start()

    End Sub



   

End Class