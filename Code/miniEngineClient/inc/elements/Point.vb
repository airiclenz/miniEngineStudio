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
Public Class Point


    Private _x As Long
    Private _y As Double

    Private mPointScreen As System.Drawing.Point
    Private mParentCurve As QuadBezier

    Private mSeq As Byte = 0
    Private mTag As Object = Nothing
    Private mSelected As Boolean = False
    Private mDragging As Boolean = False
    Private mDraggingYFac As Double = 0
    Private mType As Byte = PointType.Free


    Private mDirty As Boolean



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    C O N S T R U C T O R                                            //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub New()

        mParentCurve = Nothing

        _x = 0
        _y = 0

        mPointScreen = New System.Drawing.Point
        mDirty = False
    End Sub


    ' =========================================================================
    Public Sub New(ByVal point As Point)

        mParentCurve = Nothing

        _x = point.X
        _y = point.Y

        mPointScreen = New System.Drawing.Point
        mDirty = False
    End Sub


    ' =========================================================================
    Public Sub New(ByVal type As Byte)

        mType = type

        mParentCurve = Nothing

        _x = 0
        _y = 0

        mPointScreen = New System.Drawing.Point
        mDirty = False
    End Sub


    ' =========================================================================
    Public Sub New(ByRef parent As QuadBezier)

        mParentCurve = parent

        _x = 0
        _y = 0

        mPointScreen = New System.Drawing.Point
        mDirty = False
    End Sub


    ' =========================================================================
    Public Sub New(ByVal X As Long, ByVal Y As Double)

        mParentCurve = Nothing

        _x = X
        _y = Y

        mPointScreen = New System.Drawing.Point
        mDirty = False
    End Sub


    ' =========================================================================
    Public Sub New(ByVal X As Long, ByVal Y As Double, ByRef parent As QuadBezier)

        mParentCurve = parent

        _x = X
        _y = Y

        mPointScreen = New System.Drawing.Point
        mDirty = False
    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub ClearDirty()
        mDirty = False
    End Sub


    ' =========================================================================
    ''' <summary>
    ''' This function returns if there are any changes made since the last
    ''' CleanDirty call
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsDirty() As Boolean
        Return mDirty
    End Function






    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Property X As Long
        Get
            Return _x
        End Get
        Set(ByVal value As Long)
            _x = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    Public Property Y As Double
        Get
            Return _y
        End Get
        Set(ByVal value As Double)
            _y = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    Public Property PointScreen As System.Drawing.Point
        Get
            Return mPointScreen
        End Get
        Set(ByVal value As System.Drawing.Point)
            mPointScreen = value
        End Set
    End Property



    ' =========================================================================
    Public Property Tag As Object
        Get
            Return mTag
        End Get
        Set(ByVal value As Object)
            mTag = value
        End Set
    End Property


    ' =========================================================================
    Public Property Selected As Boolean
        Get
            Return mSelected
        End Get
        Set(ByVal value As Boolean)
            mSelected = value
        End Set
    End Property


    ' =========================================================================
    Public Property Dragging As Boolean
        Get
            Return mDragging
        End Get
        Set(ByVal value As Boolean)
            mDragging = value
        End Set
    End Property


    ' =========================================================================
    Public Property YFac As Double
        Get
            Return mDraggingYFac
        End Get
        Set(ByVal value As Double)
            mDraggingYFac = value
        End Set
    End Property


    ' =========================================================================
    Public Property SequenceInCurve As Byte
        Get
            Return mSeq
        End Get
        Set(ByVal value As Byte)
            mSeq = value
        End Set
    End Property


    ' =========================================================================
    Public Property ParentCurve As QuadBezier
        Get
            Return mParentCurve
        End Get
        Set(ByVal value As QuadBezier)
            mParentCurve = value
        End Set
    End Property


    ' =========================================================================
    Public Property Type As Byte
        Get
            Return mType
        End Get
        Set(ByVal value As Byte)
            mType = value
            'mDirty = True
        End Set
    End Property

End Class
