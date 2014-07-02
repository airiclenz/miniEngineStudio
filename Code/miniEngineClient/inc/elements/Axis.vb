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
<Serializable()>
Public Class Axis

    Private mMotorNumber As Integer
    Private mUsed As Boolean

    Private mName As String
    Private mType As Byte
    Private mCurves As List(Of QuadBezier)
    Private mColor As Color
    Private mVisible As Boolean
    Private mSelected As Boolean

    Private mEndTime As Long

    Private mMaxX As Long
    Private mMinX As Long
    Private mMaxY As Double
    Private mMinY As Double
    

    Private mScreenFactorX As Double
    Private mScreenFactorY As Double

    Private mMotorPosition As Single
    Private mCalibration As Single

    Private mDirty As Boolean



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    C O N S T R U C T O R                                            //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    ''' <summary>
    ''' The constructor function with no parameters
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        mName = "New Axis"
        mMotorNumber = -1
        mUsed = False

        mCurves = New List(Of QuadBezier)
        mColor = Color.Orange
        mType = AxisType.Linear
        mCalibration = 500
        mVisible = True
        mSelected = False

        mMotorPosition = 0

        mDirty = False

        mScreenFactorY = 1

    End Sub



    ' =========================================================================
    ''' <summary>
    ''' The constructor function with some basic parameters
    ''' </summary>
    ''' <param name="color"></param>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal color As Color, ByVal type As Byte)

        mName = "New Axis"
        mMotorNumber = -1
        mUsed = False

        mCurves = New List(Of QuadBezier)
        mColor = color
        mType = type
        mCalibration = 500
        mVisible = True
        mSelected = False

        mMotorPosition = 0

        mDirty = False

        mScreenFactorY = 1

    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub ClearDirty()

        mDirty = False

        For Each c In mCurves
            c.ClearDirty()
        Next

    End Sub


    ' =========================================================================
    ''' <summary>
    ''' This function returns if there are any changes made since the last
    ''' CleanDirty call
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsDirty() As Boolean

        Dim cDirty As Boolean = False

        ' check if one of the points is dirty
        For Each c In mCurves
            If c.IsDirty Then
                cDirty = True
                Exit For
            End If
        Next

        ' return true if one of the points or the curve itself is dirty
        Return mDirty Or cDirty

    End Function


    ' =========================================================================
    ''' <summary>
    ''' Updates the min- and max values of the contained curve-segements point's
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()

        ' if we have curves
        If mCurves.Count > 0 Then

            mEndTime = mCurves.Item(mCurves.Count - 1).Point4.X

            mMinX = Long.MaxValue
            mMaxX = Long.MinValue
            mMinY = Double.MaxValue
            mMaxY = Double.MinValue

            ' loop all curves
            For Each curve As QuadBezier In mCurves

                If curve.Point1.X > mMaxX Then mMaxX = curve.Point1.X
                If curve.Point2.X > mMaxX Then mMaxX = curve.Point2.X
                If curve.Point3.X > mMaxX Then mMaxX = curve.Point3.X
                If curve.Point4.X > mMaxX Then mMaxX = curve.Point4.X

                If curve.Point1.X < mMinX Then mMinX = curve.Point1.X
                If curve.Point2.X < mMinX Then mMinX = curve.Point2.X
                If curve.Point3.X < mMinX Then mMinX = curve.Point3.X
                If curve.Point4.X < mMinX Then mMinX = curve.Point4.X

                If curve.Point1.Y > mMaxY Then mMaxY = curve.Point1.Y
                If curve.Point2.Y > mMaxY Then mMaxY = curve.Point2.Y
                If curve.Point3.Y > mMaxY Then mMaxY = curve.Point3.Y
                If curve.Point4.Y > mMaxY Then mMaxY = curve.Point4.Y

                If curve.Point1.Y < mMinY Then mMinY = curve.Point1.Y
                If curve.Point2.Y < mMinY Then mMinY = curve.Point2.Y
                If curve.Point3.Y < mMinY Then mMinY = curve.Point3.Y
                If curve.Point4.Y < mMinY Then mMinY = curve.Point4.Y

            Next

        Else
            mEndTime = 0

            mMaxX = 0
            mMinX = 0
            mMaxY = 0
            mMinY = 0

        End If

        ' check the motor position as additional value
        If mMotorPosition > mMaxY Then mMaxY = mMotorPosition
        If mMotorPosition < mMinY Then mMinY = mMotorPosition


    End Sub



    ' =========================================================================
    ''' <summary>
    ''' Adds an empty curve to the end of the axis
    ''' </summary>
    ''' <param name="duration"></param>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Public Sub AddCurve(ByVal duration As Long, ByVal type As Byte)

        Dim curve As New QuadBezier(Me)

        ' if we have already a curve
        If mCurves.Count > 0 Then

            ' get the last curve of the current array
            Dim lastCurve As QuadBezier = mCurves.Item(mCurves.Count - 1)

            ' define the new curve so that it starts coupled to the last one
            curve.Point1 = New Point(lastCurve.Point4.X,
                                     lastCurve.Point4.Y,
                                     curve)

            curve.Point2 = New Point(CLng(lastCurve.Point4.X + (duration * (1 / 3))),
                                     lastCurve.Point4.Y,
                                     curve)

            curve.Point3 = New Point(CLng(lastCurve.Point4.X + (duration * (2 / 3))),
                                     mMotorPosition,
                                     curve)

            curve.Point4 = New Point(lastCurve.Point4.X + duration,
                                     mMotorPosition,
                                     curve)


        Else ' this is the first curve to be added to this axis:

            curve.Point2.X = CLng(duration * (1 / 3))
            curve.Point3.X = CLng(duration * (2 / 3))
            curve.Point4.X = duration

        End If

        ' set the curve-point-types
        curve.Point1.Type = type
        curve.Point2.Type = PointType.Helper
        curve.Point3.Type = PointType.Helper
        curve.Point4.Type = PointType.Free

        ' finally add the new curve
        mCurves.Add(curve)

        mDirty = True

    End Sub



    ' =========================================================================
    ''' <summary>
    ''' Sets the screen Facor for the Y axis (the factor between screen and reality)
    ''' </summary>
    ''' <param name="paintHeight"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetScreenFactorY(ByVal paintHeight As Integer) As Double

        Dim dimensionY As Double = CDbl(mMaxY - mMinY)
        If dimensionY = 0 Then
            dimensionY = 1
        End If

        mScreenFactorY = CDbl(paintHeight) / dimensionY

        Return mScreenFactorY

    End Function

    ' =========================================================================
    ''' <summary>
    ''' Sets the screen Facor for the X axis (the factor between screen and reality)
    ''' </summary>
    ''' <param name="paintHeight"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetScreenFactorX(ByVal paintHeight As Integer) As Double

        Dim dimensionX As Double = CDbl(mMaxX - mMinX)
        If dimensionX = 0 Then
            dimensionX = 1
        End If

        mScreenFactorX = CDbl(paintHeight) / dimensionX

        Return mScreenFactorX

    End Function



    ' ==================================================================
    ''' <summary>
    ''' Returns the axis from the provided list that has the given motorNumber
    ''' </summary>
    ''' <param name="motorNumber"></param>
    ''' <param name="axes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAxisWithMotorNumber(ByVal motorNumber As Integer, ByVal axes As List(Of Axis)) As Axis

        For Each axis In axes
            If axis.MotorNumber = motorNumber Then
                Return axis
            End If
        Next

        Return Nothing

    End Function



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////



    ' =========================================================================
    ''' <summary>
    ''' Get or set if this axis is in use or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Used As Boolean
        Get
            Return mUsed
        End Get
        Set(ByVal value As Boolean)
            mUsed = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Get or sets the "used" flag of the axis
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MotorNumber As Integer
        Get
            Return mMotorNumber
        End Get
        Set(ByVal value As Integer)
            mMotorNumber = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns or sets the axis's name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Name As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns or sets the axis's Color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Color As Color
        Get
            Return mColor
        End Get
        Set(ByVal value As Color)
            mColor = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns or sets the axis's curve-array
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Curves As List(Of QuadBezier)
        Get
            Return mCurves
        End Get
        Set(ByVal value As List(Of QuadBezier))
            mCurves = value
            mDirty = True
        End Set
    End Property

    ' =========================================================================
    ''' <summary>
    ''' Returns or sets the axis's Type (use AxisType)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Type As Byte
        Get
            Return mType
        End Get
        Set(ByVal value As Byte)
            mType = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Retruns or sets the axis's Calibration value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Calibration As Single
        Get
            Return mCalibration
        End Get
        Set(ByVal value As Single)
            mCalibration = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns or sets the axis's visibility
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Visible As Boolean
        Get
            Return mVisible
        End Get
        Set(ByVal value As Boolean)
            mVisible = value
            mDirty = True
        End Set
    End Property

    ' =========================================================================
    ''' <summary>
    ''' Returns or sets the axis's selection-state
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Selected As Boolean
        Get
            Return mSelected
        End Get
        Set(ByVal value As Boolean)
            mSelected = value
        End Set
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns the axis's total duration (X-Max)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Duration As Long
        Get
            Return mEndTime
        End Get
    End Property

    ' =========================================================================
    ''' <summary>
    ''' Returns the minimal Y value of all curves in this axis
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MinimumY As Double
        Get
            Return mMinY
        End Get
    End Property

    ' =========================================================================
    ''' <summary>
    ''' Returns the maximal Y value of all curves in this axis
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MaximumY As Double
        Get
            Return mMaxY
        End Get
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns the maximal X value of all curves in this axis
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MaximumX As Double
        Get
            Return mMaxX
        End Get
    End Property

    ' =========================================================================
    ''' <summary>
    ''' Returns the minimal X value of all curves in this axis
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MinimumX As Double
        Get
            Return mMinX
        End Get
    End Property


    ' =========================================================================
    ''' <summary>
    ''' Returns x-screen factor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ScreenFactorX As Double
        Get
            Return mScreenFactorX
        End Get
    End Property

    ' =========================================================================
    ''' <summary>
    ''' Returns y-screen factor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ScreenFactorY As Double
        Get
            Return mScreenFactorY
        End Get
    End Property


    ' =========================================================================
    Public Property MotorPosition() As Single
        Get
            Return mMotorPosition
        End Get
        Set(ByVal value As Single)
            mMotorPosition = value
        End Set
    End Property







End Class
