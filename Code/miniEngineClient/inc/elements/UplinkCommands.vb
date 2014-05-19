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

' ==================================================================
' ==================================================================
' ==================================================================
Public Class UplinkCommands

    ' /////////////////////////////////////////////////////
    ' Request types
    Public Shared ReadOnly REQUEST_NONE As Byte = 0
    Public Shared ReadOnly REQUEST_INT8 As Byte = 1
    Public Shared ReadOnly REQUEST_INT16 As Byte = 2
    Public Shared ReadOnly REQUEST_INT32 As Byte = 3
    Public Shared ReadOnly REQUEST_FLOAT As Byte = 4
    Public Shared ReadOnly REQUEST_DOUBLE As Byte = 5



    ' /////////////////////////////////////////////////////
    ' Linebreak
    Public Shared ReadOnly LineBreak As Byte() = {13, 10}



    ' /////////////////////////////////////////////////////
    ' Command codes
    Public Shared ReadOnly EngineStart As Byte = 1
    Public Shared ReadOnly EngineStop As Byte = 2
    Public Shared ReadOnly Repaint As Byte = 3

    Public Shared ReadOnly SetSetupStyleKeyframe As Byte = 5
    Public Shared ReadOnly GetCurveLimit As Byte = 6
    Public Shared ReadOnly GetMotorCount As Byte = 7

    Public Shared ReadOnly SetActiveMotor As Byte = 10
    Public Shared ReadOnly GetMotorType As Byte = 11
    Public Shared ReadOnly SetMotorDirection As Byte = 12
    Public Shared ReadOnly GetMotorPosition As Byte = 13
    Public Shared ReadOnly MoveMotorToPosition As Byte = 14
    Public Shared ReadOnly ClearMotorCurves As Byte = 15
    Public Shared ReadOnly AddMotorCurve As Byte = 16
    Public Shared ReadOnly CheckCurves As Byte = 17




End Class
