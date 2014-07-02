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

Imports System.IO.Ports
Imports System.Text

' ==================================================================
' ==================================================================
' ==================================================================
Public Class Uplink


    Private WithEvents mPort As SerialPort
    Private mParent As Object

    Private mBuffer As New List(Of Byte)

    Private mTimeout As Long = 400
    Private mConnected As Boolean = False

    Private mDataRequest As Byte = UplinkCommands.REQUEST_NONE
    Private mReceivedValue As Object
    Private mCommandSent As Boolean

    Private mCurveLimit As Short

    Private mAxes As List(Of Axis)


    Public Event ConnectionEstablished(ByVal sender As System.Object)
    Public Event ConnectionLost(ByVal sender As System.Object)

    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    C O N S T R U C T O R                                            //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' ==================================================================
    Public Sub New(ByRef parent As Object, ByRef comPort As SerialPort)

        mParent = parent
        mPort = comPort

        mCurveLimit = 0

        mAxes = New List(Of Axis)

        mCommandSent = False
        mDataRequest = UplinkCommands.REQUEST_NONE
        mReceivedValue = Nothing

    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' ==================================================================
    Public Sub Open()

        Try

            mPort.Open()
            mPort.DiscardOutBuffer()
            mPort.DiscardInBuffer()

        Catch ex As Exception
        End Try

    End Sub

    ' ==================================================================
    Public Sub Close()

        mConnected = False

        Try
            mPort.Close()
        Catch ex As Exception
        End Try

    End Sub


    ' ==================================================================
    Public Function ConnectMiniEngine() As Boolean

        ' try to open the com port if it is closed
        If Not mPort.IsOpen Then
            mPort.Open()
        End If

        ' is the com port open?
        If mPort.IsOpen Then

            ' initiate the handshake with the miniEngine
            SendLine("mEStart")

            Dim startTime As Long = Utils.Millis()

            ' check if the connection was established
            ' (until we run into the timeout)
            While Not mConnected And
                (startTime + mTimeout) > Utils.Millis()

                ' handle the application-events
                Application.DoEvents()
                Application.RaiseIdle(New System.EventArgs)

            End While ' end: wait loop


            ' the result object (can be byte, int16, int32 and float)
            Dim res As Object



            'request the curve limit
            res = SendRequest(UplinkCommands.GetCurveLimit, UplinkCommands.REQUEST_INT16)
            If res IsNot Nothing Then
                mCurveLimit = CShort(res)
            End If


            ' request the axis count from the miniEngine
            res = SendRequest(UplinkCommands.GetMotorCount, UplinkCommands.REQUEST_INT16)
            If res IsNot Nothing Then

                Dim axisCount = CShort(res)
                mAxes.Clear()

                ' loop all axes 
                For i As Byte = 0 To CByte(axisCount - 1)

                    ' selecte the current axis
                    If SendCommand(UplinkCommands.SetActiveMotor, i) Then

                        ' add a new axis
                        mAxes.Add(New Axis())

                        ' set the name of the nw Axis (use the same name as used in the miniEngine)
                        mAxes.Last.Name = "Motor " + CStr(i + 1)
                        mAxes.Last.MotorNumber = i
                        mAxes.Last.Used = False

                        'request the axis type
                        res = SendRequest(UplinkCommands.GetMotorType, UplinkCommands.REQUEST_INT8)
                        If res IsNot Nothing Then
                            mAxes.Last.Type = CByte(res)
                        End If

                        'request the motor position
                        res = SendRequest(UplinkCommands.GetMotorPosition, UplinkCommands.REQUEST_FLOAT)
                        If res IsNot Nothing Then
                            mAxes.Last.MotorPosition = CSng(res)
                        End If

                        'request the motor calibration
                        res = SendRequest(UplinkCommands.GetMotorCalibration, UplinkCommands.REQUEST_FLOAT)
                        If res IsNot Nothing Then
                            mAxes.Last.Calibration = CSng(res)
                        End If

                    End If

                Next

            End If



            ' if we connected successfully 
            If mConnected Then

                SendCommand(UplinkCommands.SetSetupStyleKeyframe)

                RaiseEvent ConnectionEstablished(Me)
                Return True

            End If

        End If ' end: com port is open

        Return False

    End Function



    ' ==================================================================
    Public Sub ClearInputBuffer()
        mBuffer.Clear()
    End Sub


    ' ==================================================================
    Public Sub MarkAllMotorsUnused()

        For Each motor In mAxes
            motor.Used = False
        Next

    End Sub

    ' ==================================================================
    Public Function SendCommand(ByVal commandCode As Byte) As Boolean

        If mConnected Then

            mCommandSent = True
            Dim startTime As Long = Utils.Millis()

            SendLine({commandCode})

            While mCommandSent And
                (startTime + mTimeout) > Utils.Millis()

                ' handle the application-events
                Application.DoEvents()
                Application.RaiseIdle(New System.EventArgs)

            End While

            If Not mCommandSent Then
                Return True
            End If

        End If

        mCommandSent = False
        Return False

    End Function

    ' ==================================================================
    Public Function SendCalibration(ByVal motorIndex As Integer, ByVal value As Single) As Boolean

        If mConnected Then

            ' set the active motor
            If SendCommand(UplinkCommands.SetActiveMotor, CByte(motorIndex)) Then

                If SendCommand(UplinkCommands.SetMotorCalibration, value) Then

                    ' set the local calibration value as well as it was set now
                    mAxes(motorIndex).Calibration = value

                    Return True

                End If

            End If

        End If

        Return False

    End Function


    ' ==================================================================
    Public Function SendCurves(ByVal motorIndex As Integer, ByVal curves As List(Of QuadBezier)) As Boolean

        If mConnected Then

            ' set the active motor
            If SendCommand(UplinkCommands.SetActiveMotor, CByte(motorIndex)) Then

                ' clear the moves from the motor
                If SendCommand(UplinkCommands.ClearMotorCurves) Then

                    For Each curve In curves

                        Dim p1x() = BitConverter.GetBytes(CSng(CSng(curve.Point1.X) / 1000.0))
                        Dim p1y() = BitConverter.GetBytes(CSng(curve.Point1.Y))
                        Dim p2x() = BitConverter.GetBytes(CSng(CSng(curve.Point2.X) / 1000.0))
                        Dim p2y() = BitConverter.GetBytes(CSng(curve.Point2.Y))
                        Dim p3x() = BitConverter.GetBytes(CSng(CSng(curve.Point3.X) / 1000.0))
                        Dim p3y() = BitConverter.GetBytes(CSng(curve.Point3.Y))
                        Dim p4x() = BitConverter.GetBytes(CSng(CSng(curve.Point4.X) / 1000.0))
                        Dim p4y() = BitConverter.GetBytes(CSng(curve.Point4.Y))

                        mCommandSent = True
                        Dim startTime As Long = Utils.Millis()

                        SendLine({UplinkCommands.AddMotorCurve,
                                  p1x(0), p1x(1), p1x(2), p1x(3),
                                  p1y(0), p1y(1), p1y(2), p1y(3),
                                  p2x(0), p2x(1), p2x(2), p2x(3),
                                  p2y(0), p2y(1), p2y(2), p2y(3),
                                  p3x(0), p3x(1), p3x(2), p3x(3),
                                  p3y(0), p3y(1), p3y(2), p3y(3),
                                  p4x(0), p4x(1), p4x(2), p4x(3),
                                  p4y(0), p4y(1), p4y(2), p4y(3)})

                        While mCommandSent And
                            (startTime + mTimeout) > Utils.Millis()

                            ' handle the application-events
                            Application.DoEvents()
                            Application.RaiseIdle(New System.EventArgs)

                        End While

                        If mCommandSent Then
                            mCommandSent = False
                            Return False
                        End If

                    Next

                End If ' end: clear the motor curves

            End If ' end: Set active motor for this index succeeded

        End If ' end: we are connected

        Return True

    End Function



    ' ==================================================================
    Public Function SendCommand(ByVal commandCode As Byte, ByVal value As Byte) As Boolean

        If mConnected Then

            mCommandSent = True
            Dim startTime As Long = Utils.Millis()

            SendLine({commandCode, value})

            While mCommandSent And
                (startTime + mTimeout) > Utils.Millis()

                ' handle the application-events
                Application.DoEvents()
                Application.RaiseIdle(New System.EventArgs)

            End While

            If Not mCommandSent Then
                Return True
            End If

        End If

        mCommandSent = False
        Return False

    End Function


    ' ==================================================================
    Public Function SendCommand(ByVal commandCode As Byte, ByVal value As Single) As Boolean

        If mConnected Then

            mCommandSent = True
            Dim startTime As Long = Utils.Millis()

            ' convert the single value to a byte array
            Dim floatArray = BitConverter.GetBytes(value)
            ' send the data
            SendLine({commandCode, floatArray(0), floatArray(1), floatArray(2), floatArray(3)})

            While mCommandSent And
                (startTime + mTimeout) > Utils.Millis()

                ' handle the application-events
                Application.DoEvents()
                Application.RaiseIdle(New System.EventArgs)

            End While

            If Not mCommandSent Then
                Return True
            End If

        End If

        mCommandSent = False
        Return False

    End Function


    ' ==================================================================
    Public Function SendRequest(ByVal commandCode As Byte, ByVal RequestType As Byte) As Object

        If mConnected Then

            mDataRequest = RequestType
            Dim startTime As Long = Utils.Millis()

            SendLine({commandCode})

            While mDataRequest <> UplinkCommands.REQUEST_NONE And
                (startTime + mTimeout) > Utils.Millis()

                ' handle the application-events
                Application.DoEvents()
                Application.RaiseIdle(New System.EventArgs)

            End While

            If mDataRequest = UplinkCommands.REQUEST_NONE Then
                Return mReceivedValue
            End If

        End If

        mDataRequest = UplinkCommands.REQUEST_NONE
        Return Nothing

    End Function



    ' ==================================================================
    Public Sub MotorMove(ByVal motorNum As Integer, ByVal distance As Single)

        Dim ax As Axis = Axis.GetAxisWithMotorNumber(motorNum, mAxes)

        If ax IsNot Nothing Then

            ax.MotorPosition = ax.MotorPosition + distance
            SendCommand(UplinkCommands.MoveMotorToPosition, ax.MotorPosition)

        End If



    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////

    ' ==================================================================
    Public Property Timeout As Long
        Get
            Return mTimeout
        End Get
        Set(ByVal value As Long)
            mTimeout = value
        End Set
    End Property


    ' ==================================================================
    Public ReadOnly Property Parent As Object
        Get
            Return mParent
        End Get
    End Property


    ' ==================================================================
    Public ReadOnly Property IsOpen As Boolean
        Get
            Return mPort.IsOpen()
        End Get
    End Property


    ' ==================================================================
    Public ReadOnly Property IsConnected As Boolean
        Get
            Return mConnected
        End Get
    End Property


    ' ==================================================================
    Public Property PortName As String
        Get
            Return mPort.PortName
        End Get
        Set(ByVal value As String)

            If value <> "" Then
                Try
                    mConnected = False

                    If mPort.IsOpen Then
                        mPort.Close()
                    End If

                    mPort.PortName = value

                Catch ex As Exception
                End Try
            End If

        End Set
    End Property



    ' ==================================================================
    Public ReadOnly Property Axes As List(Of Axis)
        Get
            Return mAxes
        End Get
    End Property

    ' ==================================================================
    Public ReadOnly Property AxesCount As Integer
        Get
            Return mAxes.Count
        End Get
    End Property

    ' ==================================================================
    Public ReadOnly Property CurveLimit As Integer
        Get
            Return mCurveLimit
        End Get
    End Property


    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    E V E N T S   I N                                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////



    ' ==================================================================
    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles mPort.DataReceived

        While mPort.BytesToRead > 0

            mBuffer.Add(CByte(mPort.ReadByte()))

        End While

        CheckBuffer()

    End Sub




    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R I V A T E   F U N C T I O N S                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' ==================================================================
    Private Sub ReadInputBuffer()

        While mPort.BytesToRead > 0

            mBuffer.Add(CByte(mPort.ReadByte()))

        End While

        CheckBuffer()

    End Sub



    ' ==================================================================
    Private Sub CheckBuffer()

        Dim clearPos As Integer = 0

        ' is there something in the buffer?
        If mBuffer.Count > 0 Then

            ' if the 1st buffer value is a half linebreak,
            ' then clean up that mess
            If mBuffer(0) = 10 Then
                mBuffer.RemoveAt(0)
            End If

            ' loop the whole buffer (but not the last sign as we
            ' need to check it  anyways and so we know that there is one more
            ' available ahich we can access directly)
            For i = 0 To mBuffer.Count - 2

                ' did we find a linebreak?
                If mBuffer(i) = 13 And
                    mBuffer(i + 1) = 10 Then

                    ' clear the buffer up to here
                    clearPos = i + 1

                    ' check what the data contains 
                    Decode(i - 1)

                    ' do some debugging output
                    PrintBuffer(i - 1)

                End If

            Next

            ClearBufferToPos(clearPos)

        End If

    End Sub


    ' ==================================================================
    Private Sub Decode(ByVal msgEnd As Integer)


        ' ///////////////////////////////////////
        If msgEnd = 9 Then

            If mBuffer(0) = 109 And
                mBuffer(1) = 69 And
                mBuffer(2) = 83 And
                mBuffer(3) = 116 And
                mBuffer(4) = 97 And
                mBuffer(5) = 114 And
                mBuffer(6) = 116 And
                mBuffer(7) = 65 And
                mBuffer(8) = 99 And
                mBuffer(9) = 107 Then

                mConnected = True

            End If

        End If


        ' ///////////////////////////////////////
        ' did we request data?
        If mDataRequest <> UplinkCommands.REQUEST_NONE Then

            ' If we just received a byte
            If msgEnd = 0 And mDataRequest = UplinkCommands.REQUEST_INT8 Then
                mReceivedValue = mBuffer(0)
                mDataRequest = UplinkCommands.REQUEST_NONE
            End If

            ' If we just received a 16 bit value
            If msgEnd = 1 And mDataRequest = UplinkCommands.REQUEST_INT16 Then
                mReceivedValue = BitConverter.ToInt16({mBuffer(0), mBuffer(1)}, 0)
                mDataRequest = UplinkCommands.REQUEST_NONE
            End If

            ' If we just received a 32 bit value
            If msgEnd = 3 And mDataRequest = UplinkCommands.REQUEST_INT32 Then
                mReceivedValue = BitConverter.ToInt16({mBuffer(0), mBuffer(1), mBuffer(2), mBuffer(3)}, 0)
                mDataRequest = UplinkCommands.REQUEST_NONE
            End If

            ' If we just received a 32 bit value
            If msgEnd = 3 And mDataRequest = UplinkCommands.REQUEST_FLOAT Then
                mReceivedValue = BitConverter.ToSingle({mBuffer(0), mBuffer(1), mBuffer(2), mBuffer(3)}, 0)
                mDataRequest = UplinkCommands.REQUEST_NONE
            End If

            ' If we just received a 64 bit value
            If msgEnd = 3 And mDataRequest = UplinkCommands.REQUEST_DOUBLE Then
                mReceivedValue = BitConverter.ToSingle({mBuffer(0), mBuffer(1), mBuffer(2), mBuffer(3), mBuffer(4), mBuffer(5), mBuffer(6), mBuffer(7)}, 0)
                mDataRequest = UplinkCommands.REQUEST_NONE
            End If

        End If

        ' ///////////////////////////////////////
        ' did we send a command?
        If mCommandSent Then

            If msgEnd = 2 Then

                ' was an "Ack" sent?
                If mBuffer(0) = 65 And
                    mBuffer(1) = 99 And
                    mBuffer(2) = 107 Then

                    mCommandSent = False

                End If

            End If

        End If


    End Sub


    ' ==================================================================
    Private Sub PrintBuffer(ByVal msgEnd As Integer)

        If mBuffer.Count >= msgEnd Then
            Console.WriteLine(Encoding.ASCII.GetChars(mBuffer.ToArray))
        End If

    End Sub



    ' ==================================================================
    Private Sub ClearBufferToPos(ByVal pos As Integer)

        If pos > 0 Then

            If mBuffer.Count > (pos + 1) Then
                mBuffer.RemoveRange(0, pos)
            Else
                mBuffer.Clear()
            End If

        End If

    End Sub


    ' ==================================================================
    Private Sub Send(ByVal array As Byte())
        Try
            mPort.Write(array, 0, array.Length)
        Catch ex As Exception
            mConnected = False
            RaiseEvent ConnectionLost(Me)
        End Try

    End Sub

    ' ==================================================================
    Private Sub SendLine(ByVal array As Byte())
        Try
            mPort.Write(array, 0, array.Length)
            SendLinebreak()
        Catch ex As Exception
            mConnected = False
            RaiseEvent ConnectionLost(Me)
        End Try

    End Sub


    ' ==================================================================
    Private Sub Send(ByVal value As String)
        Try
            mPort.WriteLine(value)
        Catch ex As Exception
            mConnected = False
            RaiseEvent ConnectionLost(Me)
        End Try

    End Sub

    ' ==================================================================
    Private Sub SendLine(ByVal value As String)
        Try
            mPort.Write(value)
            SendLinebreak()
        Catch ex As Exception
            mConnected = False
            RaiseEvent ConnectionLost(Me)
        End Try

    End Sub


    ' ==================================================================
    Private Sub SendLinebreak()
        Try
            mPort.Write(UplinkCommands.LineBreak, 0, UplinkCommands.LineBreak.Length)
        Catch ex As Exception
            mConnected = False
            RaiseEvent ConnectionLost(Me)
        End Try

    End Sub



End Class
