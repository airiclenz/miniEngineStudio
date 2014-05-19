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

Imports System.Windows

' =========================================================================
' =========================================================================
' =========================================================================
Public Class TimelineGraph
    Inherits Panel


    Private Const MIN_DURATION As Long = 15000
    Private Const MIN_KEYFRAME_DIST As Long = 1000
    Private Const HELPER_RANGE As Double = 0.95


    ' painting borders for the actual graph
    Private mAutoUpdate As Boolean = True

    Private mLeftBorder As Integer = 60
    Private mRightBorder As Integer = 40
    Private mTopBorder As Integer = 30
    Private mBottomBorder As Integer = 30
    Private mPointSize As Integer = 8
    Private mHighContrast As Boolean = False
    Private mSnap As Boolean = False



    ' the factor from real coordinates to screen coordinates for the x axis
    Private mXStep As Double = 1
    ' the timecode for the current mouse position
    Private mMousePositionTime As Long



    ' the factor to increase or decrease the zoom when zooming
    Private mZoomFactor As Double = 0.1
    ' the fps value used for the timecode
    Private mFPS As Integer = 25
    ' the snapping distance for the mouse
    Private mMouseDetectDistance As Integer = 5

    Private mCurveLineWidth As Single = 1.5
    Private mRulerColor As Color = Color.Gray

    Private mAxes As New List(Of Axis)
    Private mDuration As Long = MIN_DURATION

    Private mDraggingOrigin As New System.Drawing.Point
    Private mIsDraggingPoint As Boolean = False
    Private mSelectionBox As New System.Drawing.Rectangle
    Private mIsSelecting As Boolean = False
    Private mSelection As New List(Of Point)

    Private mPointUnderMouse As Point
    Private mLockedMainPoint1 As Point
    Private mLockedMainPoint2 As Point
    Private mLockedHelper1 As Point
    Private mLockedHelper2 As Point

    'Public Shadows Event KeyPress As KeyPressEventHandler
    Public Event SelectionChanged(ByVal point As Point)
    Public Event PointDragged(ByVal point As Point)
    Public Event ZoomChanged()
    Public Event MousePositionTimeChanged(ByVal newTime As Long)


    Private initiated As Boolean = False
    Private mBackBuffer As Bitmap
    Private mShowHelper As Boolean = True




    ' =========================================================================
    Public Sub New()

        MyBase.New()

        mAxes = New List(Of Axis)
        mSelection = New List(Of Point)

        Init()

    End Sub




    ' =========================================================================
    Private Sub InitializeComponent()

        Me.SuspendLayout()
        '
        'TimelineGraph
        '
        Me.ResumeLayout(False)

    End Sub


    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub Init()

        initiated = True
        Update()

    End Sub

    ' =========================================================================
    Public Overloads Sub Update()

        MyBase.Update()

        UpdateAxes()
        DetermineDuration()

        ' update the interal variable used to calculate the time of any X-position on the graph
        mXStep = CDbl(Width - mLeftBorder - mRightBorder) / CDbl(mDuration)

        PrepareData()

        PaintGraph()

    End Sub


    ' =========================================================================
    Public Function GetTimeString(ByVal milliseconds As Long, ByVal showFrames As Boolean) As String

        Dim hours As Long = CLng(milliseconds / 3600000)
        Dim minutes As Long = CLng(Math.Floor((milliseconds - (hours * 3600000)) / 60000))
        Dim seconds As Long = CLng(Math.Floor((milliseconds - (hours * 3600000) - (minutes * 60000)) / 1000))
        Dim frames As Long = CLng(Math.Floor((milliseconds - (hours * 3600000) - (minutes * 60000) - (seconds * 1000)) / (1000 / mFPS)))

        Dim hPart As String

        If hours = 0 Then
            hPart = ""
        Else
            hPart = hours.ToString + ":"
        End If


        If showFrames Then
            Return hPart + minutes.ToString("D2") + ":" + seconds.ToString("D2") + "s " + frames.ToString + "f"
        Else
            Return hPart + minutes.ToString("D2") + ":" + seconds.ToString("D2") + "s"
        End If


    End Function


    ' =========================================================================
    Public Sub SetSelectionCoordinates(ByVal position As Point)

        Dim deltaX As Long
        Dim deltaY As Double

        For Each p As Point In mSelection

            deltaX = position.X - p.X
            deltaY = position.Y - p.Y

            p.X = position.X
            p.Y = position.Y

            CheckLockPoints(p, deltaX, deltaY)
        Next

        CheckPointLimits()
        Update()

    End Sub


    ' =========================================================================
    Public Sub DeselectAllAxes()

        For Each axis In mAxes
            axis.Selected = False

            For Each curve In axis.Curves

                For Each point In curve.Points

                    point.Selected = False

                Next

            Next

            mSelection.Clear()

            RaiseEvent SelectionChanged(Nothing)

        Next

    End Sub


    ' =========================================================================
    ''' <summary>
    ''' Makes the segment of the first selected point completely flat
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub MakeSelectedSegmentLinear()

        ' is there a valid selection?
        If mSelection.Count > 0 Then

            Dim curve As QuadBezier = mSelection(0).ParentCurve
            Dim p2 = New Point(curve.Point2)
            Dim p3 = New Point(curve.Point3)

            curve.Point2.X = CLng(curve.Point1.X + ((curve.Point4.X - curve.Point1.X) * 0.4))
            curve.Point2.Y = curve.Point1.Y + ((curve.Point4.Y - curve.Point1.Y) * 0.4)

            curve.Point3.X = CLng(curve.Point1.X + ((curve.Point4.X - curve.Point1.X) * 0.6))
            curve.Point3.Y = curve.Point1.Y + ((curve.Point4.Y - curve.Point1.Y) * 0.6)

            ' if the enpoints are locked, then adjust the corresponding helpers of the
            ' close by curves
            CheckLockPoints(curve.Point2, p2.X - curve.Point2.X, p2.Y - curve.Point2.Y)
            CheckLockPoints(curve.Point3, p3.X - curve.Point3.X, p3.Y - curve.Point3.Y)

            CheckPointLimits()
            Update()

        End If

    End Sub

    ' =========================================================================
    ''' <summary>
    ''' Removes the last segment of all selected Axes but not if there is only
    ''' one segement left
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteLastSegment()

        Dim axis As Axis = GetSelectedAxis()

        If axis IsNot Nothing Then

            If axis.Curves.Count > 1 Then

                ' remove the last curve
                axis.Curves.RemoveAt(axis.Curves.Count - 1)

            End If

        End If

        Update()

    End Sub


    ' ========================================================================
    ''' <summary>
    ''' Returns the currently selected Axis. Returns Nothing if no axis is 
    ''' selected.
    ''' </summary>
    ''' <returns>Axis</returns>
    ''' <remarks></remarks>
    Public Function GetSelectedAxis() As Axis

        For Each axis In mAxes

            If axis.Selected Then

                Return axis

            End If

        Next

        Return Nothing

    End Function


    ' ========================================================================
    ''' <summary>
    ''' Returns the currently selected Axis. Returns Nothing if no axis is 
    ''' selected.
    ''' </summary>
    ''' <returns>Axis</returns>
    ''' <remarks></remarks>
    Public Function GetSelectionIndex() As Integer

        Dim counter As Integer = 0

        For Each axis In mAxes

            If axis.Selected Then

                Return counter

            End If

            counter = counter + 1

        Next

        Return Nothing

    End Function


    ' ========================================================================
    ''' <summary>
    ''' This function returns if there are any changes made since the last
    ''' CleanDirty call
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isDirty() As Boolean

        For Each a In mAxes
            If a.IsDirty Then
                Return True
            End If
        Next

        Return False

    End Function



    ' ========================================================================
    Public Sub ClearDirty()
        For Each a In mAxes
            a.ClearDirty()
        Next
    End Sub




    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    E V E N T S   O U T                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Public Sub ZoomIn()

        Update()

        If (Width * (1 + mZoomFactor)) < 10000 Then
            Width = CInt(Width * (1 + mZoomFactor))
            RaiseEvent ZoomChanged()
        End If

    End Sub

    ' =========================================================================
    Public Sub ZoomOut()

        Update()

        Width = CInt(Width * (1 - mZoomFactor))
        RaiseEvent ZoomChanged()

    End Sub


    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    E V E N T S   I N                                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Private Sub TimelineGraph_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged

        Update()

    End Sub



    ' =========================================================================
    Private Sub TimelineGraph_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        ' //////////////////////////////////////////////////////////////
        If mIsDraggingPoint Then    ' are we draggind a point rignt now?

            Dim screenp, point As New Point

            Dim deltaXScreen As Integer = e.X - mDraggingOrigin.X
            Dim deltaYScreen As Integer = e.Y - mDraggingOrigin.Y

            Dim deltaX As Long = 0
            Dim deltaY As Double = 0

            ' re-gather the points with the most current values
            mSelection = GetDraggingPoints()

            ' if there is anty point selected
            If mSelection.Count > 0 Then

                ' copy the most recent yFac for this axis
                mPointUnderMouse.YFac = mSelection(0).YFac

                ' translate the mouse position to real coordinates
                ' screenp = mous position on screen
                ' point = real coordinates
                screenp = New Point(mPointUnderMouse.PointScreen.X + deltaXScreen, mPointUnderMouse.PointScreen.Y + deltaYScreen)
                point = ConvertToReal(screenp, mPointUnderMouse.YFac)

                ' shift the point so that is received the shift it has before
                point.Y = point.Y + mPointUnderMouse.ParentCurve.ParentAxis.MinimumY

                ' what is the delta moved in real (non-screen) coordinates?
                deltaX = point.X - mPointUnderMouse.X
                deltaY = point.Y - mPointUnderMouse.Y

                ' loop all the points in the selection
                For Each selPoint In mSelection

                    ' do we take care of the point under the mouse right now?
                    If (CInt(mPointUnderMouse.Tag) = CInt(selPoint.Tag)) Then

                        selPoint.X = point.X
                        selPoint.Y = point.Y

                        ' update the global selection Point object
                        mPointUnderMouse = selPoint

                    Else
                        ' if not the point under the mouse, 
                        ' drag all the others as well with the same shift-values

                        selPoint.X = selPoint.X + deltaX
                        selPoint.Y = selPoint.Y + deltaY

                    End If

                Next

                ' check if we are moving the helper-point of a locked point
                ' or the locked point itself and adjust its other helper-point or
                ' both helper points accordingly()
                CheckLockPoints(mPointUnderMouse, deltaX, deltaY)

            End If


            ' remember where we are right now for the next time the mouse is moved
            mDraggingOrigin = New System.Drawing.Point(CInt(screenp.X), CInt(screenp.Y))

            ' make sure all points are in a valid position
            CheckPointLimits()
            PaintGraph()

            ' Raise the event that a point was dragged
            If mSelection.Count > 0 Then
                RaiseEvent PointDragged(mSelection(0))
            End If


            ' //////////////////////////////////////////////////////////////
        ElseIf mIsSelecting Then ' we are selecting right now with a selection-box

            ' define the new selectionbox
            mSelectionBox.Size = New System.Drawing.Size(e.Location.X - mSelectionBox.X,
                                                         e.Location.Y - mSelectionBox.Y)

            ' paint the graph
            PaintGraph()


            ' //////////////////////////////////////////////////////////////
        Else ' we are doing nothing but simply moving the mouse arround

            ' get the point over which we are right now
            Dim list As List(Of Point) = GetPointsAtScreenPos(e)

            ' is the mouse currently over a point?
            If list.Count > 0 Then

                ' update the mouse cursor so that it replects
                ' the actions we can make
                If Me.Cursor <> Cursors.Hand Then
                    Me.Cursor = Cursors.Hand
                End If
            Else
                If Me.Cursor <> Cursors.Cross Then
                    Me.Cursor = Cursors.Cross
                End If
            End If

        End If


        ' //////////////////////////////////////////////////////////////
        ' calculate the time of the x position of the mouse
        If e.Location.X >= mLeftBorder And
            e.Location.X <= (Width - mRightBorder) And
            e.Location.Y >= mTopBorder And
            e.Location.Y <= (Height - mBottomBorder) Then

            mMousePositionTime = CLng((e.Location.X - mLeftBorder) / mXStep)
            RaiseEvent MousePositionTimeChanged(mMousePositionTime)

        End If


    End Sub



    ' =========================================================================
    Private Sub TimelineGraph_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

        UpdateAxes()
        PrepareData()

        ' get all points under the mouse
        Dim pList As List(Of Point) = GetPointsAtScreenPos(e)


        ' if we are over a point...
        If pList.Count > 0 Then

            ' sort the point array
            pList = SortSelection(pList)

            ' if the point we clicked is not in the
            ' current selection then set the selection to this point
            If Not ContainsPoint(mSelection, pList(0)) Then

                mSelection.Clear()

                ' ensure that always only one helper point is in
                ' the selection
                For Each point In pList

                    If point.Type = PointType.Helper Then
                        mSelection.Add(point)
                        Exit For
                    Else
                        mSelection.Add(point)
                    End If

                Next

            End If

            ' which is the point we clicked?
            mPointUnderMouse = pList(0)

            ' stop dragging for all points
            UndragAllPoints()

            ' deselect all points
            DeselectAllPoints()

            ' start dragmode
            mIsDraggingPoint = True

            ' if no point was selected
            If mSelection.Count = 0 Then
                mSelection = pList
            End If

            ' enable dragging for all point in the selection
            For Each p In mSelection
                p.Dragging = True
                p.Selected = True
            Next

            ' remember where we started dragging
            mDraggingOrigin = e.Location

        Else

            mIsSelecting = True
            mSelectionBox = New Rectangle(e.Location, New System.Drawing.Size(0, 0))

        End If


    End Sub



    ' =========================================================================
    Private Sub TimelineGraph_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp

        If mIsDraggingPoint Then

            ' get the point over which we are right now
            mSelection = GetPointsAtScreenPos(e)

            ' stop dradmode
            mIsDraggingPoint = False

            ' stop dragging for all points
            UndragAllPoints()

        ElseIf mIsSelecting Then

            mIsSelecting = False

            mSelection.Clear()
            DeselectAllPoints()

            Dim rect As Rectangle = SortRectangle(mSelectionBox)

            ' check which points need to be added to the selection
            For Each axis In mAxes
                If axis.Selected Then
                    For Each curve In axis.Curves
                        For Each point In curve.Points

                            If point.PointScreen.X >= rect.X And
                                point.PointScreen.Y >= rect.Y And
                                point.PointScreen.X <= rect.X + rect.Width And
                                point.PointScreen.Y <= rect.Y + rect.Height Then

                                mSelection.Add(point)
                                point.Selected = True

                            End If

                        Next
                    Next
                End If
            Next

        End If

        ' check whick kind of "selection changed" event we need to fire
        If mSelection.Count > 0 Then

            mSelection = SortSelection(mSelection)

            If mSelection.Count = 1 Then

                RaiseEvent SelectionChanged(mSelection(0))

            ElseIf mSelection.Count = 2 Then

                If mSelection(0).SequenceInCurve = 3 And
                    mSelection(1).SequenceInCurve = 0 Then
                    RaiseEvent SelectionChanged(mSelection(0))
                End If

            Else
                RaiseEvent SelectionChanged(Nothing)
            End If

        Else

            RaiseEvent SelectionChanged(Nothing)

        End If

        mPointUnderMouse = Nothing
        mLockedHelper1 = Nothing
        mLockedHelper2 = Nothing
        mLockedMainPoint1 = Nothing
        mLockedMainPoint2 = Nothing

        ' paint the graph
        Update()


    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////



    ' =========================================================================
    Public ReadOnly Property Selection As List(Of Point)
        Get
            Return GetSelectedPoints()
        End Get
    End Property


    ' =========================================================================
    Public Property AutoUpdate() As Boolean
        Get
            Return mAutoUpdate
        End Get
        Set(ByVal value As Boolean)
            mAutoUpdate = value
        End Set
    End Property



    ' =========================================================================
    ''' <summary>
    ''' Percent of increase and deacesae of the element per zoom step.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Percent of increase and deacesae of the element per zoom step.</remarks>
    Public Property ZoomFactor As Integer
        Get
            Return CInt(mZoomFactor * 100)
        End Get
        Set(ByVal value As Integer)
            If value > 0 And value < 100 Then
                mZoomFactor = value / 100.0
            End If

            If mAutoUpdate Then Update()
        End Set
    End Property


    ' =========================================================================
    Public Property FPS As Integer
        Get
            Return mFPS
        End Get
        Set(ByVal value As Integer)
            If value > 1 And value < 500 Then
                mFPS = value
            End If

        End Set
    End Property

    ' =========================================================================
    <System.ComponentModel.Browsable(False)>
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property Axes As List(Of Axis)
        Get
            Return mAxes
        End Get
        'Set(ByVal value As List(Of Axis))
        '    mAxes = value
        'Update()
        'End Set
    End Property


    ' =========================================================================
    Public Property CurveLineWidth As Single
        Get
            Return mCurveLineWidth
        End Get
        Set(ByVal value As Single)
            If value > 0 Then mCurveLineWidth = value
            If mAutoUpdate Then PaintGraph()
        End Set
    End Property


    ' =========================================================================
    Public Property RulerColor As Color
        Get
            Return mRulerColor
        End Get
        Set(ByVal value As Color)
            mRulerColor = value
            If mAutoUpdate Then PaintGraph()
        End Set
    End Property



    ' =========================================================================
    Public Shadows Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            MyBase.BackColor = value
            If mAutoUpdate Then PaintGraph()
        End Set
    End Property

    ' =========================================================================
    Public Shadows Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If mAutoUpdate Then PaintGraph()
        End Set
    End Property

    ' =========================================================================
    Public Property ShowHelper As Boolean
        Get
            Return mShowHelper
        End Get
        Set(ByVal value As Boolean)
            mShowHelper = value
            If mAutoUpdate Then PaintGraph()
        End Set
    End Property

    ' =========================================================================
    Public Property HighContrast As Boolean
        Get
            Return mHighContrast
        End Get
        Set(ByVal value As Boolean)
            mHighContrast = value
            If mAutoUpdate Then PaintGraph()
        End Set
    End Property


    ' =========================================================================
    Public Property SnapToUnit As Boolean
        Get
            Return mSnap
        End Get
        Set(ByVal value As Boolean)
            mSnap = value
            CheckPointLimits()
            If mAutoUpdate Then Update()
        End Set
    End Property




    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R I V A T E   F U N C T I O N S                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    Private Sub PaintGraph()

        mBackBuffer = New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(mBackBuffer)

        g.SetClip(Utils.GetVisibleRectangle(CType(Me.Parent, ScrollableControl), Me))
        g.Clear(Me.BackColor)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias


        PaintSelection(g)
        If initiated Then
            PaintAxes(g)
        End If

        PaintMotorPosition(g)
        PaintRulers(g)

        ' paint the backBuffer pixture
        Me.CreateGraphics.DrawImage(mBackBuffer, 0, 0)
        g.Dispose()

    End Sub

    ' =========================================================================
    Private Sub PaintAxes(ByRef g As Graphics)


        ' sort the list so that the selected axis is painted last 
        ' (on top of all the others)
        Dim axesSorted As New List(Of Axis)
        Dim selectedAxisAvailable As Boolean = False

        ' get all that are not selected an visible
        For Each axis In mAxes
            If Not axis.Selected And axis.Visible Then
                axesSorted.Add(axis)
            End If
        Next


        ' get the selected axis
        For Each axis In mAxes
            If axis.Selected Then
                axesSorted.Add(axis)
                selectedAxisAvailable = True
            End If
        Next


        ' loop all the Axes we have
        For Each axis As Axis In axesSorted


            ' decide on the color for this axis
            ' based on high contrast mode and selection status
            Dim col As Color
            If selectedAxisAvailable And
                Not axis.Selected And
                mHighContrast Then
                col = Color.FromArgb(70, 70, 70)
            Else
                col = axis.Color
            End If

            Dim halfPointSize As Integer = CInt(mPointSize / 2)

            ' define all the painting tools we need
            Dim bez_pen As Pen
            If axis.Visible Then
                bez_pen = New Pen(col, mCurveLineWidth)
            Else
                bez_pen = New Pen(Color.FromArgb(128, col), mCurveLineWidth)
                bez_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            End If


            Dim bez_penTrans As New Pen(Color.FromArgb(128, col), mCurveLineWidth)
            Dim sel_brush As New SolidBrush(Color.FromArgb(50, col))
            Dim pnt_brush As New SolidBrush(col)
            Dim bkg_brush As New SolidBrush(BackColor)
            Dim penDotted As New Pen(Color.FromArgb(160, col), 1)
            penDotted.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash

            axis.SetScreenFactorY(Height - mTopBorder - mBottomBorder)
            Dim ccount As Integer = 0


            ' Loop all curves of this axis
            For Each curve As QuadBezier In axis.Curves

                ccount = ccount + 1

                Dim p1 As Point = ConvertToScreen(New Point(curve.Point1.X, curve.Point1.Y - axis.MinimumY), axis.ScreenFactorY)
                Dim p2 As Point = ConvertToScreen(New Point(curve.Point2.X, curve.Point2.Y - axis.MinimumY), axis.ScreenFactorY)
                Dim p3 As Point = ConvertToScreen(New Point(curve.Point3.X, curve.Point3.Y - axis.MinimumY), axis.ScreenFactorY)
                Dim p4 As Point = ConvertToScreen(New Point(curve.Point4.X, curve.Point4.Y - axis.MinimumY), axis.ScreenFactorY)


                ' update the current screen coordinates
                curve.Point1.PointScreen = New System.Drawing.Point(CInt(p1.X), CInt(p1.Y))
                curve.Point2.PointScreen = New System.Drawing.Point(CInt(p2.X), CInt(p2.Y))
                curve.Point3.PointScreen = New System.Drawing.Point(CInt(p3.X), CInt(p3.Y))
                curve.Point4.PointScreen = New System.Drawing.Point(CInt(p4.X), CInt(p4.Y))

                'paint the current curve
                g.DrawBezier(bez_pen, curve.Point1.PointScreen,
                                      curve.Point2.PointScreen,
                                      curve.Point3.PointScreen,
                                      curve.Point4.PointScreen)


                ' are we supposed to paint the helper lines as well as the points?
                If mShowHelper And axis.Selected And axis.Visible Then

                    ' draw the helper lines
                    g.DrawLine(penDotted, curve.Point1.PointScreen, curve.Point2.PointScreen)
                    g.DrawLine(penDotted, curve.Point3.PointScreen, curve.Point4.PointScreen)



                    ' paint the curves points 1 - 3; No 4 is the starting point
                    ' of the following curve so we don't need to point it
                    Select Case curve.Point1.Type
                        Case PointType.Free
                            g.FillRectangle(pnt_brush, CInt(p1.X - halfPointSize), CInt(p1.Y - halfPointSize), mPointSize, mPointSize)
                            g.FillRectangle(bkg_brush, CInt(p1.X - halfPointSize + 2), CInt(p1.Y - halfPointSize + 2), mPointSize - 4, mPointSize - 4)
                        Case PointType.Locked
                            g.FillEllipse(pnt_brush, CInt(p1.X - halfPointSize), CInt(p1.Y - halfPointSize), mPointSize, mPointSize)
                    End Select
                    g.DrawEllipse(bez_penTrans, CInt(p2.X - halfPointSize + 1), CInt(p2.Y - halfPointSize + 1), mPointSize - 2, mPointSize - 2)
                    g.DrawEllipse(bez_penTrans, CInt(p3.X - halfPointSize + 1), CInt(p3.Y - halfPointSize + 1), mPointSize - 2, mPointSize - 2)

                    ' If this is the last curve, paint point 4 as well
                    If ccount = axis.Curves.Count Then
                        Select Case curve.Point4.Type
                            Case PointType.Free
                                g.FillRectangle(pnt_brush, CInt(p4.X - halfPointSize), CInt(p4.Y - halfPointSize), mPointSize, mPointSize)
                                g.FillRectangle(bkg_brush, CInt(p4.X - halfPointSize + 2), CInt(p4.Y - halfPointSize + 2), mPointSize - 4, mPointSize - 4)
                            Case PointType.Locked
                                g.FillEllipse(pnt_brush, CInt(p4.X - halfPointSize), CInt(p4.Y - halfPointSize), mPointSize, mPointSize)
                        End Select
                    End If


                    ' paint the selection if needed
                    If curve.Point1.Selected Then
                        g.FillEllipse(sel_brush, CInt(p1.X - 7), CInt(p1.Y - 7), 14, 14)
                        g.FillEllipse(sel_brush, CInt(p1.X - 10), CInt(p1.Y - 10), 20, 20)
                    End If

                    If curve.Point2.Selected Then
                        g.FillEllipse(sel_brush, CInt(p2.X - 7), CInt(p2.Y - 7), 14, 14)
                        g.FillEllipse(sel_brush, CInt(p2.X - 10), CInt(p2.Y - 10), 20, 20)
                    End If

                    If curve.Point3.Selected Then
                        g.FillEllipse(sel_brush, CInt(p3.X - 7), CInt(p3.Y - 7), 14, 14)
                        g.FillEllipse(sel_brush, CInt(p3.X - 10), CInt(p3.Y - 10), 20, 20)
                    End If

                    If ccount = axis.Curves.Count And
                        curve.Point4.Selected Then
                        g.FillEllipse(sel_brush, CInt(p4.X - 7), CInt(p4.Y - 7), 14, 14)
                        g.FillEllipse(sel_brush, CInt(p4.X - 10), CInt(p4.Y - 10), 20, 20)
                    End If


                End If
            Next

        Next ' end: loop all Axes

    End Sub


    ' =========================================================================
    Private Sub PaintMotorPosition(ByVal g As Graphics)

        Dim axis As Axis = GetSelectedAxis()


        
        ' if there is an axis selcted
        If axis IsNot Nothing Then

            Dim pen As New Pen(Color.FromArgb(128, axis.Color), 1.5)
            Dim penDottedH As New Pen(Color.FromArgb(128, axis.Color), 1.5)
            penDottedH.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot
            Dim brushText As New System.Drawing.SolidBrush(axis.Color)
            Dim brushFill As New System.Drawing.SolidBrush(Color.FromArgb(50, axis.Color))


            Dim xLenghth As Integer = Width - mLeftBorder - mRightBorder
            Dim yHeight As Integer = Height - mTopBorder - mBottomBorder


            Dim mp As Point = ConvertToScreen(New Point(0, axis.MotorPosition - axis.MinimumY), axis.ScreenFactorY)

            ' paint the vertical line
            g.DrawLine(penDottedH, mLeftBorder, CInt(mp.Y), Width - mRightBorder, CInt(mp.Y))

            ' paint the "M" marker
            Dim textSize As System.Drawing.Size = TextRenderer.MeasureText("M", Font)
            Dim p As New PointF(Width - mRightBorder + 9,
                                CSng(mp.Y - (textSize.Height / 2) - 1))

            g.FillEllipse(brushFill, Width - mRightBorder + 6, p.Y - 2, textSize.Width + 2, textSize.Width + 1)
            g.DrawEllipse(pen, Width - mRightBorder + 6, p.Y - 2, textSize.Width + 2, textSize.Width + 1)
            g.DrawString("M", Font, brushText, p)

        End If






    End Sub


    ' =========================================================================
    Private Sub PaintRulers(ByRef g As Graphics)

        PaintRulerVertical(g)
        PaintRulerHorizontal(g)

    End Sub


    ' =========================================================================
    Private Sub PaintRulerVertical(ByVal g As Graphics)

        Dim pen As New Pen(Color.FromArgb(100, mRulerColor), 1)
        Dim penDottedH As New Pen(Color.FromArgb(50, mRulerColor), 1)
        Dim penDottedL As New Pen(Color.FromArgb(15, mRulerColor), 1)
        penDottedH.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
        penDottedL.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Dim brush As New System.Drawing.SolidBrush(Color.FromArgb(100, mRulerColor))

        Dim xLenghth As Integer = Width - mLeftBorder - mRightBorder
        Dim yHeight As Integer = Height - mTopBorder - mBottomBorder


        ' paint the vertical line
        g.DrawLine(pen, mLeftBorder, mTopBorder, mLeftBorder, Height - mBottomBorder)

        Dim axis As Axis = GetSelectedAxis()

        ' if there is an axis selcted
        If axis IsNot Nothing Then

            Dim unitDivider As Integer
            Dim divider As Integer
            Dim majorDivider As Integer
            Dim unit As String = String.Empty
            Dim len As Integer = 2
            Dim ypos As Integer
            Dim value As Integer


            ' What are the conversion factors to paint it correctly on the screen?
            Dim yDimension As Double = axis.MaximumY - axis.MinimumY
            Dim yStep As Double = yHeight / yDimension


            ' as we are using floating point numbers - what is the offset to the 
            ' first full number (in pixels)? 
            Dim yOffset As Integer = CInt(Math.Round(yStep * (Math.Ceiling(axis.MinimumY) - axis.MinimumY)))



            '///////////////////////////////////////
            ' paint the segments if fitting <= 10 cm or °
            If yDimension <= 10 Then

                Select Case axis.Type
                    Case AxisType.Linear
                        unit = "cm"
                    Case AxisType.Radial
                        unit = "°"
                End Select


                ' loop all the full values
                For i As Integer = 0 To CInt(Math.Floor(axis.MaximumY - axis.MinimumY))

                    ' the actual value including the offset
                    value = CInt(i + axis.MinimumY)

                    ' the current position (y) on the screen
                    ypos = Height - mBottomBorder - CInt(i * yStep)

                    ' how long do the lines need to be and what
                    ' is the current text?
                    If value Mod 1 = 0 Then
                        len = 3

                        Dim markerStr As String = value.ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 8, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedL,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)

                    Else
                        len = 3

                    End If

                    ' Drawing the segmentation-lines
                    g.DrawLine(pen, mLeftBorder - len,
                                    ypos,
                                    mLeftBorder,
                                    ypos)

                Next


                '///////////////////////////////////////
                ' paint the segments if fitting 10 cm up to 100 or more cm
            ElseIf (yHeight >= (3 * yDimension)) Then

                ' find a reasonable data divider for our current
                ' axis as well as the needed unit
                Select Case axis.Type
                    Case AxisType.Linear
                        unit = "cm"
                        unitDivider = 1
                        divider = 10
                        majorDivider = 50
                    Case AxisType.Radial
                        unit = "°"
                        unitDivider = 1
                        divider = 15
                        majorDivider = 90
                End Select


                ' loop all the full values
                For i As Integer = 0 To CInt(Math.Floor(axis.MaximumY - axis.MinimumY))

                    ' the actual value including the offset
                    value = CInt(i + axis.MinimumY)

                    ' the current position (y) on the screen
                    ypos = Height - mBottomBorder - CInt(i * yStep)

                    ' how long do the lines need to be and what
                    ' is the current text?
                    If value Mod majorDivider = 0 Then
                        len = 4

                        Dim markerStr As String = (value / unitDivider).ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 10, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedH,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)

                    ElseIf value Mod divider = 0 Then
                        len = 3

                        Dim markerStr As String = (value / unitDivider).ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 8, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedL,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)


                    Else
                        len = 2
                    End If

                    ' Drawing the segmentation-lines
                    g.DrawLine(pen, mLeftBorder - len,
                                    ypos,
                                    mLeftBorder,
                                    ypos)

                Next


                '///////////////////////////////////////
                ' paint the segments if fitting 10 cm up to 100 or more cm
            ElseIf (yHeight >= (0.3 * yDimension)) Then

                ' find a reasonable data divider for our current
                ' axis as well as the needed unit
                Select Case axis.Type
                    Case AxisType.Linear
                        unit = "m"
                        unitDivider = 100
                        divider = 50
                        majorDivider = 100
                    Case AxisType.Radial
                        unit = "°"
                        unitDivider = 1
                        divider = 90
                        majorDivider = 360
                End Select


                ' loop all the full values
                For i As Integer = 0 To CInt(Math.Floor(axis.MaximumY - axis.MinimumY))

                    ' the actual value including the offset
                    value = CInt(i + axis.MinimumY)

                    ' the current position (y) on the screen
                    ypos = Height - mBottomBorder - CInt(i * yStep)

                    ' how long do the lines need to be and what
                    ' is the current text?
                    If value Mod majorDivider = 0 Then
                        len = 4

                        Dim markerStr As String = (value / unitDivider).ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 10, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedH,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)

                    ElseIf value Mod divider = 0 Then
                        len = 3

                        Dim markerStr As String = (value / unitDivider).ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 8, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedL,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)


                    Else
                        len = 0
                    End If

                    ' paint a line here?
                    If len > 0 Then
                        ' Drawing the segmentation-lines
                        g.DrawLine(pen, mLeftBorder - len,
                                        ypos,
                                        mLeftBorder,
                                        ypos)

                    End If

                Next

                '///////////////////////////////////////
                ' paint the segments if fitting 10 cm up to 100 or more cm
            ElseIf (yHeight >= (0.03 * yDimension)) Then

                ' find a reasonable data divider for our current
                ' axis as well as the needed unit
                Select Case axis.Type
                    Case AxisType.Linear
                        unit = "m"
                        unitDivider = 100
                        divider = 500
                        majorDivider = 1000
                    Case AxisType.Radial
                        unit = "rev"
                        unitDivider = 360
                        divider = 360
                        majorDivider = 1800
                End Select


                ' loop all the full values
                For i As Integer = 0 To CInt(Math.Floor(axis.MaximumY - axis.MinimumY))

                    ' the actual value including the offset
                    value = CInt(i + axis.MinimumY)

                    ' the current position (y) on the screen
                    ypos = Height - mBottomBorder - CInt(i * yStep)

                    ' how long do the lines need to be and what
                    ' is the current text?
                    If value Mod majorDivider = 0 Then
                        len = 4

                        Dim markerStr As String = (value / unitDivider).ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 10, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedH,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)

                    ElseIf value Mod divider = 0 Then
                        len = 3

                        Dim markerStr As String = (value / unitDivider).ToString + unit
                        Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                        Dim p As New PointF(mLeftBorder - textSize.Width - 8, CSng(ypos - (textSize.Height / 2)) - 1)
                        g.DrawString(markerStr, Font, brush, p)

                        g.DrawLine(penDottedL,
                                   mLeftBorder,
                                   ypos,
                                   mLeftBorder + xLenghth,
                                   ypos)


                    Else
                        len = 0
                    End If

                    ' paint a line here?
                    If len > 0 Then
                        ' Drawing the segmentation-lines
                        g.DrawLine(pen, mLeftBorder - len,
                                        ypos,
                                        mLeftBorder,
                                        ypos)

                    End If

                Next


            End If

        End If

    End Sub




    ' =========================================================================
    Private Sub PaintRulerHorizontal(ByVal g As Graphics)

        Dim pen As New Pen(Color.FromArgb(100, mRulerColor), 1)
        Dim penDottedH As New Pen(Color.FromArgb(50, mRulerColor), 1)
        Dim penDottedL As New Pen(Color.FromArgb(15, mRulerColor), 1)
        penDottedH.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
        penDottedL.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        Dim brush As New System.Drawing.SolidBrush(Color.FromArgb(100, mRulerColor))

        Dim xLenghth As Integer = Width - mLeftBorder - mRightBorder
        Dim yHeight As Integer = Height - mTopBorder - mBottomBorder

        Dim durationSeconds As Long = CLng(mDuration / 1000)
        Dim durationMinutes As Long = CLng(durationSeconds / 60)
        Dim durationHours As Long = CLng(durationMinutes / 60)

        Dim seconds As Long = CLng(mDuration / 1000)
        Dim minutes As Long = CLng((mDuration - (seconds * 1000)) / 60)
        Dim hours As Long = CLng((mDuration - (seconds * 1000) - (minutes * 60000)) / 60)


        ' paint the horizontal line
        g.DrawLine(pen, mLeftBorder, Height - mBottomBorder, Width - mRightBorder, Height - mBottomBorder)



        '///////////////////////////////////////
        ' paint the second segments if fitting
        If (xLenghth >= (3 * durationSeconds)) Then

            Dim secondStep As Double = CDbl(xLenghth) / CDbl(durationSeconds)
            Dim len As Integer = 2

            For i = 0 To durationSeconds


                If i Mod 30 = 0 Then

                    len = 4

                    Dim markerStr As String = GetTimeString(i * 1000, False)
                    Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                    Dim p As New PointF(CSng(mLeftBorder + CInt(i * secondStep) - (textSize.Width / 2) + 2), Height - mBottomBorder + 6)
                    g.DrawString(markerStr, Font, brush, p)

                    g.DrawLine(penDottedH,
                               mLeftBorder + CInt(i * secondStep),
                               mTopBorder,
                               mLeftBorder + CInt(i * secondStep),
                               Height - mBottomBorder)

                ElseIf i Mod 15 = 0 Then

                    len = 3

                    Dim markerStr As String = GetTimeString(i * 1000, False)
                    Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                    Dim p As New PointF(CSng(mLeftBorder + CInt(i * secondStep) - (textSize.Width / 2) + 2), Height - mBottomBorder + 6)
                    g.DrawString(markerStr, Font, brush, p)

                    g.DrawLine(penDottedL,
                               mLeftBorder + CInt(i * secondStep),
                               mTopBorder,
                               mLeftBorder + CInt(i * secondStep),
                               Height - mBottomBorder)

                Else
                    len = 2
                End If



                g.DrawLine(pen, mLeftBorder + CInt(i * secondStep),
                                Height - mBottomBorder,
                                mLeftBorder + CInt(i * secondStep),
                                Height - mBottomBorder + len)
            Next


            '///////////////////////////////////////
            ' paint the 10 second segments if fitting
        ElseIf (xLenghth >= (10 * (durationSeconds / 10))) Then

            Dim tenStep As Double = CDbl(xLenghth) / CDbl(durationSeconds / 10)
            Dim len As Integer = 2

            For i = 0 To (durationSeconds / 10)

                If i Mod 6 = 0 Then
                    len = 5

                    Dim markerStr As String = GetTimeString(CLng(i * 1000 * 10), False)
                    Dim textSize As System.Drawing.Size = TextRenderer.MeasureText(markerStr, Font)
                    Dim p As New PointF(CSng(mLeftBorder + CInt(i * tenStep) - (textSize.Width / 2) + 2), Height - mBottomBorder + 6)
                    g.DrawString(markerStr, Font, brush, p)

                    g.DrawLine(penDottedH,
                               mLeftBorder + CInt(i * tenStep),
                               mTopBorder,
                               mLeftBorder + CInt(i * tenStep),
                               Height - mBottomBorder)

                ElseIf i Mod 3 = 0 Then

                    len = 2

                    g.DrawLine(penDottedL,
                               mLeftBorder + CInt(i * tenStep),
                               mTopBorder,
                               mLeftBorder + CInt(i * tenStep),
                               Height - mBottomBorder)

                Else
                    len = 2
                End If

                g.DrawLine(pen, mLeftBorder + CInt(i * tenStep),
                                Height - mBottomBorder,
                                mLeftBorder + CInt(i * tenStep),
                                Height - mBottomBorder + len)
            Next


        End If

    End Sub



    ' =========================================================================
    Private Sub PaintSelection(ByRef g As Graphics)

        If mIsSelecting Then

            Dim sel_brush As New SolidBrush(Color.FromArgb(20, Color.PaleTurquoise))
            Dim sel_pen As New Pen(Color.FromArgb(128, Color.PaleTurquoise), 1)
            sel_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash

            Dim rect As Rectangle = SortRectangle(mSelectionBox)

            g.DrawRectangle(sel_pen, rect)
            g.FillRectangle(sel_brush, rect)

        End If

    End Sub


    ' =========================================================================
    Private Function SortRectangle(ByVal rect As Rectangle) As Rectangle

        Dim result As New Rectangle

        If rect.Width < 0 Then
            result.X = rect.X + rect.Width
            result.Width = -rect.Width
        Else
            result.X = rect.X
            result.Width = rect.Width
        End If

        If rect.Height < 0 Then
            result.Y = rect.Y + rect.Height
            result.Height = -rect.Height
        Else
            result.Y = rect.Y
            result.Height = rect.Height
        End If

        Return result

    End Function


    ' =========================================================================
    Private Sub PrepareData()

        Dim pointIDCounter As Integer = 0

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' Loop all curves of this axis
            For Each curve As QuadBezier In axis.Curves

                ' assign some unique IDs to the points
                curve.Point1.Tag = pointIDCounter
                curve.Point1.SequenceInCurve = 0
                curve.Point1.ParentCurve = curve
                pointIDCounter = pointIDCounter + 1

                curve.Point2.Tag = pointIDCounter
                curve.Point2.SequenceInCurve = 1
                curve.Point2.ParentCurve = curve
                curve.Point2.Type = PointType.Helper
                pointIDCounter = pointIDCounter + 1

                curve.Point3.Tag = pointIDCounter
                curve.Point3.SequenceInCurve = 2
                curve.Point3.ParentCurve = curve
                curve.Point3.Type = PointType.Helper
                pointIDCounter = pointIDCounter + 1

                curve.Point4.Tag = pointIDCounter
                curve.Point4.SequenceInCurve = 3
                curve.Point4.ParentCurve = curve
                pointIDCounter = pointIDCounter + 1

            Next

        Next ' end: loop all Axes

    End Sub



    ' =========================================================================
    Private Sub UpdateAxes()

        For Each axis As Axis In mAxes

            axis.Update()
            axis.SetScreenFactorX(Me.Width - mLeftBorder - mRightBorder)
            axis.SetScreenFactorY(Me.Height - mTopBorder - mBottomBorder)

        Next

    End Sub



    ' =========================================================================
    Private Sub DetermineDuration()

        mDuration = 0

        For Each axis As Axis In mAxes

            If axis.Duration > mDuration Then
                mDuration = axis.Duration
            End If

        Next

        If mDuration < MIN_DURATION Then
            mDuration = MIN_DURATION
        End If

    End Sub


    ' =========================================================================
    Private Sub CheckLockPoints(ByVal movePoint As Point, ByVal deltaX As Long, ByVal deltaY As Double)

        ' get all the points involved
        GetLockPoints(movePoint)

        ' if there is something to do
        If mLockedMainPoint1 IsNot Nothing Then

            ' we are moving the main point
            If movePoint.Equals(mLockedMainPoint1) Then

                If mLockedMainPoint2 IsNot Nothing Then
                    mLockedMainPoint2.X = movePoint.X
                    mLockedMainPoint2.Y = movePoint.Y
                End If

                If mLockedHelper1 IsNot Nothing Then
                    mLockedHelper1.X = mLockedHelper1.X + deltaX
                    mLockedHelper1.Y = mLockedHelper1.Y + deltaY
                End If

                If mLockedHelper2 IsNot Nothing Then
                    mLockedHelper2.X = mLockedHelper2.X + deltaX
                    mLockedHelper2.Y = mLockedHelper2.Y + deltaY
                End If

            End If

            ' we are moving the left helper point
            If movePoint.Equals(mLockedHelper1) And
                mLockedHelper2 IsNot Nothing Then

                Dim vecDir As New Vector(mLockedMainPoint1.X - mLockedHelper1.X,
                                         mLockedMainPoint1.Y - mLockedHelper1.Y)

                Dim vecDist As New Vector((mLockedHelper2.X - mLockedMainPoint2.X),
                                          (mLockedHelper2.Y - mLockedMainPoint2.Y))

                ' TEMP
                vecDist = vecDir


                vecDir.Normalize()
                vecDir = Vector.Multiply(vecDist.Length, vecDir)

                mLockedHelper2.X = CLng(mLockedMainPoint2.X + vecDir.X)
                mLockedHelper2.Y = mLockedMainPoint2.Y + vecDir.Y

            End If



            ' we are moving the right helper point
            If movePoint.Equals(mLockedHelper2) And
                mLockedHelper1 IsNot Nothing Then

                Dim vecDir As New Vector(mLockedMainPoint2.X - mLockedHelper2.X,
                                         mLockedMainPoint2.Y - mLockedHelper2.Y)

                Dim vecDist As New Vector((mLockedHelper1.X - mLockedMainPoint1.X),
                                          (mLockedHelper1.Y - mLockedMainPoint1.Y))

                ' TEMP
                vecDist = vecDir


                vecDir.Normalize()
                vecDir = Vector.Multiply(vecDist.Length, vecDir)

                mLockedHelper1.X = CLng(mLockedMainPoint1.X + vecDir.X)
                mLockedHelper1.Y = mLockedMainPoint1.Y + vecDir.Y

            End If


        ElseIf mLockedMainPoint2 IsNot Nothing Then

            ' we are moving the main point (first point in the axix)
            ' so just adjust the y of the one helper point connected to this main point
            mLockedHelper2.Y = mLockedHelper2.Y + deltaY

        End If

    End Sub



    ' =========================================================================
    ''' <summary>
    ''' Retreives the points involved in a dragging action if the center-point
    ''' is locked and thus the helper points need to be moved in a locked way
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetLockPoints(ByVal movePoint As Point)

        ' if we did not yet do this...
        If mLockedMainPoint1 Is Nothing And
            mLockedMainPoint2 Is Nothing Then

            ' is ther is a point under the mouse
            If movePoint IsNot Nothing Then

                ' if the dragged point is the left helper and 
                ' its main point locked
                If movePoint.SequenceInCurve = 2 And
                    movePoint.ParentCurve.Point4.Type = PointType.Locked Then

                    mLockedHelper1 = movePoint
                    mLockedMainPoint1 = movePoint.ParentCurve.Point4
                    mLockedMainPoint2 = GetPointWithTag(CInt(movePoint.Tag) + 2)
                    If mLockedMainPoint2 IsNot Nothing Then

                        mLockedHelper2 = mLockedMainPoint2.ParentCurve.Point2

                        ' if the first helper is not on the same axis then remove it
                        If Not mLockedHelper1.ParentCurve.ParentAxis.Equals(mLockedHelper2.ParentCurve.ParentAxis) Then
                            mLockedHelper1 = Nothing
                        End If

                    End If

                End If

                ' if the dragged point is the right helper  and 
                ' its main point locked
                If movePoint.SequenceInCurve = 1 And
                    movePoint.ParentCurve.Point1.Type = PointType.Locked Then

                    mLockedHelper1 = GetPointWithTag(CInt(movePoint.Tag) - 3)
                    If mLockedHelper1 IsNot Nothing Then mLockedMainPoint1 = mLockedHelper1.ParentCurve.Point4
                    mLockedMainPoint2 = movePoint.ParentCurve.Point1
                    mLockedHelper2 = movePoint

                    ' if the second helper is not on the same axis then remove it
                    If mLockedHelper1 IsNot Nothing Then
                        If Not mLockedHelper1.ParentCurve.ParentAxis.Equals(mLockedHelper2.ParentCurve.ParentAxis) Then
                            mLockedHelper2 = Nothing
                        End If
                    End If

                End If

                ' if the dragged point is the main point and 
                ' it is locked
                If (movePoint.SequenceInCurve = 0 Or movePoint.SequenceInCurve = 3) And
                    movePoint.ParentCurve.Point4.Type = PointType.Locked Then

                    If movePoint.SequenceInCurve = 0 Then
                        mLockedHelper1 = Nothing
                        mLockedMainPoint1 = Nothing
                        mLockedMainPoint2 = movePoint

                    Else
                        mLockedHelper1 = movePoint.ParentCurve.Point3
                        mLockedMainPoint1 = movePoint
                        mLockedMainPoint2 = GetPointWithTag(CInt(movePoint.Tag) + 1)

                    End If


                    If mLockedMainPoint2 IsNot Nothing Then
                        mLockedHelper2 = mLockedMainPoint2.ParentCurve.Point2

                        If mLockedHelper1 IsNot Nothing Then
                            ' if the second helper is not on the same axis then remove it
                            If Not mLockedHelper1.ParentCurve.ParentAxis.Equals(mLockedHelper2.ParentCurve.ParentAxis) Then
                                mLockedHelper2 = Nothing
                            End If

                        End If
                        
                    End If


                End If

            End If

        End If

    End Sub


    ' =========================================================================
    Private Function GetPointWithTag(ByVal tag As Integer) As Point

        For Each axis In mAxes

            For Each curve In axis.Curves

                For Each point In curve.Points

                    If CInt(point.Tag) = tag Then

                        Return point

                    End If

                Next

            Next

        Next

        Return Nothing

    End Function




    ' =========================================================================
    Private Sub CheckPointLimits()

        Dim xDuration As Double
        Dim cCnt As Integer


        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            cCnt = 0

            ' loop all curves of the axis
            For Each curve As QuadBezier In axis.Curves

                ' point 1 must start at time x=0
                If cCnt = 0 And curve.Point1.X <> 0 Then
                    curve.Point1.X = 0
                End If


                ' point 4.x must be greater than point 1.x
                If curve.Point4.X <= curve.Point1.X Then
                    curve.Point4.X = curve.Point1.X + 10
                End If


                ' point 4 must lay before point 4.x of the next curve:
                ' is there a next curve?
                If cCnt < (axis.Curves.Count - 1) Then

                    If curve.Point4.X > (axis.Curves(cCnt + 1).Point4.X - MIN_KEYFRAME_DIST) Then

                        ' dragging to the right side?
                        If mSelection.Contains(curve.Point4) Then

                            curve.Point4.X = axis.Curves(cCnt + 1).Point4.X - MIN_KEYFRAME_DIST
                            axis.Curves(cCnt + 1).Point1.X = curve.Point4.X

                        Else ' dragging to the left side

                            axis.Curves(cCnt + 1).Point4.X = curve.Point4.X + MIN_KEYFRAME_DIST

                            ' if there is another curve, fix point 1 of this one too
                            If cCnt < (axis.Curves.Count - 2) Then
                                axis.Curves(cCnt + 2).Point1.X = curve.Point4.X + MIN_KEYFRAME_DIST
                            End If

                        End If

                    End If

                End If



                ' get the total duration of the curve
                xDuration = curve.Point4.X - curve.Point1.X

                ' Helper points 2 must be after point 1
                If curve.Point2.X <= CLng(curve.Point1.X + (xDuration * (1 - HELPER_RANGE))) Then
                    curve.Point2.X = CLng(curve.Point1.X + (xDuration * (1 - HELPER_RANGE)))
                End If

                ' Helper points 2 must be before point 4
                If curve.Point2.X >= CLng(curve.Point1.X + (xDuration * HELPER_RANGE)) Then
                    curve.Point2.X = CLng(curve.Point1.X + (xDuration * HELPER_RANGE))
                End If

                ' Helper points 3 must be before point 4
                If curve.Point3.X >= CLng(curve.Point4.X - (xDuration * (1 - HELPER_RANGE))) Then
                    curve.Point3.X = CLng(curve.Point4.X - (xDuration * (1 - HELPER_RANGE)))
                End If

                ' Helper points 3 must be after point 1
                If curve.Point3.X <= CLng(curve.Point4.X - (xDuration * HELPER_RANGE)) Then
                    curve.Point3.X = CLng(curve.Point4.X - (xDuration * HELPER_RANGE))
                End If


                cCnt = cCnt + 1

            Next ' end: loop all curves

        Next ' end: loop all axes


        Console.WriteLine()

    End Sub


    ' =========================================================================
    Private Function GetPointsAtScreenPos(ByRef e As System.Windows.Forms.MouseEventArgs) As List(Of Point)

        ' the result list
        Dim res As New List(Of Point)

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' only check this axis if it is displayed
            If axis.Selected And axis.Visible Then

                ' loop all curves of the axis
                For c As Integer = 0 To axis.Curves.Count - 1

                    ' loop all points of the curve
                    For p As Integer = 0 To axis.Curves(c).Points.Count - 1

                        If e.Location.X >= axis.Curves(c).Points(p).PointScreen.X - mMouseDetectDistance And
                           e.Location.X <= axis.Curves(c).Points(p).PointScreen.X + mMouseDetectDistance And
                           e.Location.Y >= axis.Curves(c).Points(p).PointScreen.Y - mMouseDetectDistance And
                           e.Location.Y <= axis.Curves(c).Points(p).PointScreen.Y + mMouseDetectDistance Then

                            res.Add(axis.Curves(c).Points(p))

                        End If ' end: mouse over point

                    Next ' end: loop points

                Next ' end: loop curves

            End If ' end: is visible

        Next ' end: loop axes


        Return SortSelection(res)

    End Function



    ' =========================================================================
    Private Function SortSelection(ByVal list As List(Of Point)) As List(Of Point)

        Dim res As New List(Of Point)

        If list.Count > 0 Then

            ' sort the result  so that helper points come first
            ' this is needed for good access to the helper points when
            ' a end point and a helper point are at the same position
            For Each p In list
                If p.SequenceInCurve = 1 Or
                    p.SequenceInCurve = 2 Then
                    res.Add(p)
                End If
            Next

            ' ..now add all end points of the curves
            For Each p In list
                If p.SequenceInCurve = 3 Then
                    res.Add(p)
                End If
            Next

            ' ...and finally all start-points
            For Each p In list
                If p.SequenceInCurve = 0 Then
                    res.Add(p)
                End If
            Next

        End If

        Return res

    End Function



    ' =========================================================================
    Private Function GetSelectedPoints() As List(Of Point)

        ' the result list
        Dim res As New List(Of Point)

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' only check this axis if it is displayed
            If axis.Selected And axis.Visible Then

                ' loop all curves of the axis
                For Each c As QuadBezier In axis.Curves

                    ' loop all points of the curve
                    For Each p As Point In c.Points

                        If p.Selected Then
                            res.Add(p)
                        End If

                    Next ' end: loop points

                Next ' end: loop curves

            End If ' end: is visible

        Next ' end: loop axes

        Return SortSelection(res)

    End Function



    ' =========================================================================
    Private Function GetDraggingPoints() As List(Of Point)

        ' the result list
        Dim res As New List(Of Point)

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' only check this axis if it is displayed
            If axis.Visible Then

                'axis.Update()

                ' loop all curves of the axis
                For Each c As QuadBezier In axis.Curves

                    ' loop all points of the curve
                    For Each p As Point In c.Points

                        If p.Dragging Then
                            p.YFac = axis.ScreenFactorY
                            res.Add(p)
                        End If

                    Next ' end: loop points

                Next ' end: loop curves

            End If ' end: is visible

        Next ' end: loop axes

        Return res


    End Function



    ' =========================================================================
    Private Sub DeselectAllPoints()

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' loop all curves of the axis
            For Each curve As QuadBezier In axis.Curves

                ' loop all points of the curve
                For Each p As Point In curve.Points

                    p.Selected = False

                Next ' end: loop points

            Next ' end: loop curves

        Next ' end: loop axes

    End Sub


    ' =========================================================================
    Private Sub UndragAllPoints()

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' loop all curves of the axis
            For Each curve As QuadBezier In axis.Curves

                ' loop all points of the curve
                For Each p As Point In curve.Points

                    p.Dragging = False

                Next ' end: loop points

            Next ' end: loop curves

        Next ' end: loop axes

    End Sub




    ' =========================================================================
    Private Sub SetPointWithTag(ByVal point As Point)

        ' loop all the Axes we have
        For Each axis As Axis In mAxes

            ' loop all curves of the axis
            For Each curve As QuadBezier In axis.Curves

                ' loop all points of the curve
                For Each p As Point In curve.Points

                    If p.Tag.Equals(point.Tag) Then
                        p = point
                    End If

                Next ' end: loop points

            Next ' end: loop curves

        Next ' end: loop axes



    End Sub





    ' =========================================================================
    Private Function ConvertToScreen(ByVal point As Point, ByVal yFactor As Double) As Point

        Return New Point(CInt(mLeftBorder + (point.X * mXStep)), CInt(Height - mBottomBorder - (point.Y * yFactor)))

    End Function

    ' =========================================================================
    Private Function ConvertToReal(ByVal point As Point, ByVal yFactor As Double) As Point

        If SnapToUnit Then
            Return New Point(Utils.RoundToMultiplier(((point.X - mLeftBorder) / mXStep), 1000),
                             CInt((Height - mBottomBorder - point.Y) / yFactor))
        Else
            Return New Point(CLng((point.X - mLeftBorder) / mXStep),
                             (Height - mBottomBorder - point.Y) / yFactor)
        End If

    End Function


    ' =========================================================================
    Private Function ContainsPoint(ByVal list As List(Of Point), ByVal point As Point) As Boolean

        Dim pTag As Integer = CInt(point.Tag)

        For Each p In list

            If CInt(p.Tag) = pTag Then
                Return True
            End If

        Next

        Return False

    End Function


End Class
