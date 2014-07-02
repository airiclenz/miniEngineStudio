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


Imports System
Imports System.Globalization


' ==================================================================
' ==================================================================
' ==================================================================
Public Class Utils



    ' =========================================================================
    Public Shared Function GetDecimalSeperator() As String

        ' get the local comma seperator
        Dim ci As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        Return ci.NumberFormat.CurrencyDecimalSeparator

    End Function


    ' =========================================================================
    Public Declare Function SHChangeNotify Lib "Shell32.dll" (ByVal wEventID As Int32,
                                                               ByVal uFlags As Int32,
                                                               ByVal dwItem1 As Int32,
                                                               ByVal dwItem2 As Int32) As Int32


    ' ==================================================================
    Public Shared Function ReadFileFromURL(ByVal url As String, ByVal showWarnings As Boolean) As String

        Dim weblient As New System.Net.WebClient()
        Dim content As String = String.Empty

        Try
            content = weblient.DownloadString(url)
        Catch ex As Exception

            If showWarnings Then
                MessageBox.Show("I was not able to connect to the webaddress!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        End Try

        Return content

    End Function



    ' ==================================================================
    Public Shared Sub Delay(ByVal milliseconds As Long)
        For i = 1 To (milliseconds / 100)
            Threading.Thread.Sleep(100)
            Application.DoEvents()
            Application.RaiseIdle(New System.EventArgs)
        Next
    End Sub

    ' ==================================================================
    Public Shared Function Millis() As Long
        Return CLng(Date.Now.Ticks / TimeSpan.TicksPerMillisecond)
    End Function


    ' =========================================================================
    Public Shared Function RoundToMultiplier(ByVal number As Double, ByVal multiplier As Integer) As Long
        Return CLng(Math.Round(number / multiplier) * multiplier)
    End Function


    ' =========================================================================
    Public Shared Function GetVisibleRectangle(ByVal host As ScrollableControl, ByVal child As Control) As Rectangle

        If host IsNot Nothing And
            child IsNot Nothing Then
            Return New Rectangle(-child.Bounds.X, -child.Bounds.Y, host.ClientRectangle.Width, host.ClientRectangle.Height)
        Else
            Return New Rectangle()
        End If

    End Function


End Class


