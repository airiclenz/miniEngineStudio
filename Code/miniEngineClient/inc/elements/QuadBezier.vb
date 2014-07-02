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
Public Class QuadBezier


    Private mParentAxis As Axis
    Private mPoints As New List(Of Point)

    Private mDirty As Boolean

    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    C O N S T R U C T O R                                            //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub New(ByRef parentAxis As Axis)
        'Public Sub New()

        mParentAxis = parentAxis

        ' add the 4 needed ppoints to our point array
        mPoints.Add(New Point)
        mPoints.Add(New Point)
        mPoints.Add(New Point)
        mPoints.Add(New Point)

        mDirty = False

        'mParentAxis = Nothing

    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub ClearDirty()

        mDirty = False

        For Each p In mPoints
            p.ClearDirty()
        Next

    End Sub


    ' =========================================================================
    Public Function IsDirty() As Boolean

        Dim pDirty As Boolean = False

        ' check if one of the points is dirty
        For Each p In mPoints
            If p.IsDirty Then
                pDirty = True
                Exit For
            End If
        Next

        ' return true if one of the points or the curve itself is dirty
        Return mDirty Or pDirty

    End Function



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public ReadOnly Property ParentAxis As Axis
        Get
            Return mParentAxis
        End Get
    End Property




    ' =========================================================================
    Public Property Point1 As Point
        Get
            Return mPoints(0)
        End Get
        Set(ByVal value As Point)
            mPoints(0) = value
            mDirty = True
        End Set
    End Property

    ' =========================================================================
    Public Property Point2 As Point
        Get
            Return mPoints(1)
        End Get
        Set(ByVal value As Point)
            mPoints(1) = value
            mDirty = True
        End Set
    End Property

    ' =========================================================================
    Public Property Point3 As Point
        Get
            Return mPoints(2)
        End Get
        Set(ByVal value As Point)
            mPoints(2) = value
            mDirty = True
        End Set
    End Property

    ' =========================================================================
    Public Property Point4 As Point
        Get
            Return mPoints(3)
        End Get
        Set(ByVal value As Point)
            mPoints(3) = value
            mDirty = True
        End Set
    End Property


    ' =========================================================================
    Public ReadOnly Property Points As List(Of Point)
        Get
            Return mPoints
        End Get
    End Property


End Class
