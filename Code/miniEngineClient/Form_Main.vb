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

Imports System.IO

' =========================================================================
' =========================================================================
' =========================================================================
Public Class Form_Main



    Public Const Version As Byte = 1
    Public Const SubVersion As Byte = 0
    Public Const SubSubVersion As Byte = 0

    Private Const Title As String = "miniEngine Studio"

    Private Const VersionURL As String = "https://raw.github.com/airiclenz/miniEngineStudio/master/version.md"
    Private Const HomeURL As String = "https://github.com/airiclenz/miniEngineStudio"


    Private mAxisColors As Color() = {
        Color.FromArgb(128, 167, 255),
        Color.FromArgb(255, 132, 66),
        Color.FromArgb(0, 255, 110),
        Color.FromArgb(248, 139, 179),
        Color.FromArgb(255, 255, 59)
    }


    Private mDecimalSeparator As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator


    Dim mMouseOverGraph As Boolean = True
    Private mLoadingDone As Boolean = False
    Protected WithEvents mUplink As Uplink

    Private mFilename As String = ""


    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    E V E N T S                                                      //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////




    ' =========================================================================
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Visible = False
        Form_Splashscreen.Show()

        Cursor = Cursors.WaitCursor

        graph.Init()

        Panel1.HorizontalScroll.Visible = True
        Panel1.HorizontalScroll.Enabled = True

        Panel1.VerticalScroll.Visible = False
        Panel1.VerticalScroll.Enabled = False

        mUplink = New Uplink(Me, mComPort)

        LoadSettings()

        mLoadingDone = True

        ' zoom graph to fit screen
        FitToScreen()

        ' check every given parameter for a valid file
        For Each cmd In My.Application.CommandLineArgs

            ' if the path is valid
            If Directory.Exists(Path.GetDirectoryName(cmd)) Then
                'if the file exists
                If File.Exists(cmd) Then

                    ' load the file
                    If (LoadFile(cmd)) Then

                        ' remember the filename
                        mFilename = cmd
                        My.Settings.LastFile = cmd
                        ' enable the save button
                        SaveToolStripMenuItem.Enabled = True

                        ' leave the checking of each parameter as we loaded a file now
                        Exit For

                    End If

                End If
            End If
        Next

        ' if no other file was loaded based on the parameters sent to the application...
        If mFilename = "" And
            My.Settings.OpenLastFile = True Then

            ' load the file
            If My.Settings.LastFile <> "" Then
                If (LoadFile(My.Settings.LastFile)) Then
                    SaveToolStripMenuItem.Enabled = True
                End If
            End If

        End If

        graph.Update()

        Cursor = Cursors.Default

    End Sub

    ' =========================================================================
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        ' store the com port information
        If mUplink.IsOpen Then

            My.Settings.Com_Port = mUplink.PortName
            mUplink.Close()

        End If

        My.Settings.HighContrastMode = btn_highContrast.Checked
        My.Settings.ShowHelper = btn_showHelper.Checked
        My.Settings.SnapToFullUnits = btn_snap.Checked
        
        My.Settings.OpenLastFile = OpenLastFileAutomaticallyToolStripMenuItem.Checked
        My.Settings.Restore_Window_Position = RestoreWindowPositionToolStripMenuItem.Checked
        My.Settings.AutoCheckUpdate = AutocheckForUpdatesToolStripMenuItem.Checked


        My.Settings.Save()


        ' do we have unsaved changes?
        If graph.isDirty Then

            Dim res = MessageBox.Show("There are unsaved changes. Do you want to save these changes?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If res = DialogResult.Yes Then
                SaveFile(mFilename)
            End If

        End If

    End Sub


    ' =========================================================================
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub


    ' =========================================================================
    Private Sub Panel1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        graph.Update()
    End Sub


    ' =========================================================================
    Private Sub TimelineGraph1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mMouseOverGraph = False
    End Sub

    ' =========================================================================
    Private Sub TimelineGraph1_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mMouseOverGraph = True
    End Sub

    ' =========================================================================
    Private Sub btn_zoomIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_zoomIn.Click

        Dim scrollPos As Double = Panel1.HorizontalScroll.Value
        Dim scrollMax As Double = Panel1.HorizontalScroll.Maximum

        graph.ZoomIn()

        Panel1.HorizontalScroll.Value = CInt((scrollPos / scrollMax) * CDbl(Panel1.HorizontalScroll.Maximum))

    End Sub

    ' =========================================================================
    Private Sub btn_zoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_zoomOut.Click

        Dim scrollPos As Double = Panel1.HorizontalScroll.Value
        Dim scrollMax As Double = Panel1.HorizontalScroll.Maximum

        graph.ZoomOut()

        Panel1.HorizontalScroll.Value = CInt((scrollPos / scrollMax) * CDbl(Panel1.HorizontalScroll.Maximum))

    End Sub


    ' =========================================================================
    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        StoreFormSizeAndPosition()
    End Sub

    ' =========================================================================
    Private Sub Form1_LocationChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.LocationChanged
        StoreFormSizeAndPosition()
    End Sub




    ' =========================================================================
    Private Sub btn_addAxis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addAxis.Click

        Dialog_Axis.Axis = New Axis()


        ' set the default color and the type if available
        If mAxisColors.Count > ListView_Axes.Items.Count Then

            Dialog_Axis.Axis.Color = mAxisColors(ListView_Axes.Items.Count)

            If graph.Axes.Count < mUplink.AxesCount Then
                Dialog_Axis.Axis.Type = mUplink.AxisTypes(graph.Axes.Count)
            Else
                Dialog_Axis.Axis.Type = AxisType.Linear
            End If

            Dialog_Axis.UpdateDialog()
        End If


        If Dialog_Axis.ShowDialog = DialogResult.OK Then

            Dim axis As Axis = Dialog_Axis.Axis

            ' add a first cuve so that something exists
            Dim curve1 As New QuadBezier(axis)

            curve1.Point1 = New Point(0, 0, curve1)
            curve1.Point1.Type = PointType.Free

            curve1.Point2 = New Point(30000, 0, curve1)
            curve1.Point2.Type = PointType.Helper

            curve1.Point3 = New Point(30000, 100, curve1)
            curve1.Point3.Type = PointType.Helper

            curve1.Point4 = New Point(60000, 100, curve1)
            curve1.Point4.Type = PointType.Free

            axis.Curves.Add(curve1)

            ' add the new Axis to the graph component
            graph.Axes.Add(axis)

        End If

        graph.Update()
        AxesListUpdate(graph.Axes.Count - 1)



    End Sub


    ' ==================================================================
    Private Sub TimelineGraph1_MousePositionTimeChanged(ByVal newTime As System.Int64)
        ToolStripStatusTimeLabel.Text = graph.GetTimeString(newTime, True)
    End Sub


    ' ==================================================================
    Private Sub COMPortToolStripMenuItem_DropDownItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles COMPortToolStripMenuItem.DropDownItemClicked

        ConnectComPort(e.ClickedItem.Text)

    End Sub


    ' =====================================#============================
    Private Sub SettingsToolStripMenuItem_DropDownOpened(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.DropDownOpened
        UpdateAvailableCOMPorts()
    End Sub


    ' ==================================================================
    Private Sub btn_showHelper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_showHelper.Click
        graph.ShowHelper = btn_showHelper.Checked
    End Sub


    ' ==================================================================
    Private Sub AutocheckForUpdatesToolStripMenuItem_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutocheckForUpdatesToolStripMenuItem.CheckStateChanged
        My.Settings.AutoCheckUpdate = AutocheckForUpdatesToolStripMenuItem.Checked
    End Sub


    ' ==================================================================
    Private Sub btn_ZoomToFit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ZoomToFit.Click
        FitToScreen()
    End Sub


    ' ==================================================================
    Private Sub TimelineGraph1_SelectionChanged(ByVal point As Point) Handles graph.SelectionChanged

        If point IsNot Nothing Then

            ' set the current point coordinated to the text fields
            edit_X.Text = Format(point.X / 1000, "0.00")
            edit_Y.Text = Format(point.Y, "0.0")

            edit_X.Enabled = True
            edit_Y.Enabled = True

        Else

            edit_X.Text = ""
            edit_Y.Text = ""

            edit_X.Enabled = False
            edit_Y.Enabled = False

        End If

        ' check which buttons need to be anabled and which needs to
        ' Be disabled
        CheckButtonStates()

    End Sub

    ' ==================================================================
    Private Sub graph_PointDragged(ByVal point As mEStudio.Point)

        If point IsNot Nothing Then

            ' set the current point coordinated to the text fields
            edit_X.Text = Format(point.X / 1000, "0.00")
            edit_Y.Text = Format(point.Y, "0.00")

        End If

    End Sub


    ' ==================================================================
    Private Sub edit_X_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles edit_X.KeyDown

        If e.KeyCode = Keys.Enter Then
            UpdatePoint()
        End If

    End Sub

    ' ==================================================================
    Private Sub edit_Y_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles edit_Y.KeyDown

        If e.KeyCode = Keys.Enter Then
            UpdatePoint()
        End If

    End Sub

    ' ==================================================================
    Private Sub edit_X_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        UpdatePoint()
    End Sub

    ' ==================================================================
    Private Sub edit_Y_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        UpdatePoint()
    End Sub



    ' ==================================================================
    Private Sub ListView_Axes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView_Axes.SelectedIndexChanged

        ' deselect all axes and all point in any case
        graph.DeselectAllAxes()

        ' is there something selected?
        If ListView_Axes.SelectedIndices.Count = 1 Then

            ' select the axis
            graph.Axes.Item(ListView_Axes.SelectedIndices(0)).Selected = True

            If graph.Axes.Item(ListView_Axes.SelectedIndices(0)).Type = AxisType.Linear Then
                label_unit.Text = "cm"
                ToolStripLabel_unit.Text = "cm"
            Else
                label_unit.Text = "°"
                ToolStripLabel_unit.Text = "°"
            End If

            ' select the miniengine axis
            mUplink.SendCommand(UplinkCommands.SetActiveMotor, CByte(ListView_Axes.SelectedIndices(0)))

        Else

            label_unit.Text = ""
            ToolStripLabel_unit.Text = "  "

        End If

        ' check which buttons need to on and which ones off
        CheckButtonStates()

        ' repaint the graph
        graph.Update()

    End Sub


    ' ==================================================================
    Private Sub ListView_Axes_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView_Axes.MouseDoubleClick
        EditAxis()
    End Sub


    ' ==================================================================
    Private Sub btn_editAxis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_editAxis.Click
        EditAxis()
    End Sub



    ' ==================================================================
    Private Sub btn_visible_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_visible.Click

        If ListView_Axes.SelectedIndices.Count = 1 Then
            graph.Axes(ListView_Axes.SelectedIndices(0)).Visible = btn_visible.Checked

            ' repaint the axes list and select the old axis
            AxesListUpdate(ListView_Axes.SelectedIndices(0))

            ' repaint the graph
            graph.Update()
        End If

    End Sub

    ' ==================================================================
    Private Sub btn_deleteAxis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_deleteAxis.Click

        ' if there is a valid selection
        If ListView_Axes.SelectedIndices.Count = 1 Then

            ' ask once again if this is really what needs to be done
            If MessageBox.Show("Do you really want to delete the Axis """ + ListView_Axes.SelectedItems(0).Text + """?",
                               "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then

                ' remove the axis
                graph.Axes.RemoveAt(ListView_Axes.SelectedIndices(0))
                ' repaint the axes list and select the old axis
                AxesListUpdate()
                ' repaint the graph
                graph.Update()

            End If

        End If

    End Sub


    ' =========================================================================
    Private Sub btn_highContrast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_highContrast.Click
        graph.HighContrast = btn_highContrast.Checked
    End Sub

    ' =========================================================================
    Private Sub btn_snap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_snap.Click
        graph.SnapToUnit = btn_snap.Checked
    End Sub


    ' =========================================================================
    Private Sub SaveFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveFileToolStripMenuItem.Click
        SaveFile()
    End Sub

    ' =========================================================================
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SaveFile(mFilename)
    End Sub

    ' =========================================================================
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If LoadFile() Then
            SaveToolStripMenuItem.Enabled = True
            My.Settings.LastFile = mFilename
        End If
    End Sub

    Private Sub graph_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles graph.Paint
        ' repaint the graph
        graph.Update()
    End Sub

    ' =========================================================================
    Private Sub Form_Main_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        ' zoom the graph
        If mMouseOverGraph Then
            If e.KeyCode = Keys.Add Then btn_zoomIn_Click(sender, Nothing)
            If e.KeyCode = Keys.Subtract Then btn_zoomOut_Click(sender, Nothing)
        End If

        ' save the current file
        If e.Control Then
            If e.KeyCode = Keys.S Then

                SaveFile(mFilename)

            End If
        End If

    End Sub


    ' ==================================================================
    Private Sub btn_addSegment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addSegment.Click

        Dim curveCount As Integer = 0
        Dim counter As Integer = 0

        'check how many curves were assigned allready
        For Each axis In graph.Axes

            curveCount = curveCount + axis.Curves.Count

            counter = counter + 1

            ' stop counting when we reached the max supported axes count
            ' (even though there might be more defined)
            If counter = mUplink.AxesCount Then
                Exit For
            End If

        Next


        ' do we have want to add more than the allowed curves?
        If curveCount >= mUplink.CurveLimit And
            mUplink.IsConnected Then

            ' display a warning message
            MessageBox.Show("You cannot add more than " + mUplink.CurveLimit.ToString + " curve-segments!",
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)

        Else

            ' if there is a selected curve
            If ListView_Axes.SelectedIndices.Count = 1 Then

                Dim axIndex As Integer = ListView_Axes.SelectedIndices(0)

                ' get the type from the last point
                Dim lastType As Byte = PointType.Free

                If graph.Axes(axIndex).Curves.Count > 0 Then
                    lastType = graph.Axes(axIndex).Curves(graph.Axes(axIndex).Curves.Count - 1).Point4.Type
                End If

                ' the index of the currently selected axis
                Dim idx As Integer = ListView_Axes.SelectedIndices(0)

                ' update the current motor position
                Dim pos As Object = mUplink.SendRequest(UplinkCommands.GetMotorPosition, UplinkCommands.REQUEST_FLOAT)

                If pos IsNot Nothing Then
                    graph.Axes(axIndex).MotorPosition = CSng(pos)
                End If

                ' add a new segment with the length og 60 sec to the current motor position
                graph.Axes(axIndex).AddCurve(60000, lastType)
                graph.Update()

            End If

        End If

    End Sub



    ' ==================================================================
    Private Sub btn_locked_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_locked.CheckedChanged

        ' main point?
        For Each p In graph.Selection

            If p.SequenceInCurve = 0 Or
                p.SequenceInCurve = 3 Then

                If btn_locked.Checked Then
                    p.Type = PointType.Locked
                Else
                    p.Type = PointType.Free
                End If

            End If

        Next

        graph.Update()

    End Sub


    ' ==================================================================
    Private Sub Panel1_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel1.ClientSizeChanged
        graph.Height = Panel1.Height - 25
    End Sub

    ' ==================================================================
    Private Sub AboutToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        Form_About.ShowDialog()
    End Sub


    ' ==================================================================
    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click

        CheckVersion(True)

    End Sub

    ' ==================================================================
    Private Sub RegistermepFilesWithPathDesignerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegistermepFilesWithPathDesignerToolStripMenuItem.Click
        CheckRegistry(True)
        MessageBox.Show("Sucessfully registered the filetype.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    ' ==================================================================
    Private Sub btn_home_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_home.Click

        Dim idx As Integer = ListView_Axes.SelectedIndices(0)

        ' move back to home
        If mUplink.SendCommand(UplinkCommands.MoveMotorToPosition, 0) Then
            mUplink.MotorPositions(idx) = 0
            graph.Axes(idx).MotorPosition = 0

            graph.Update()

        End If


    End Sub


    ' ==================================================================
    Private Sub btn_moveLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_moveLeft.Click

        Dim idx As Integer = ListView_Axes.SelectedIndices(0)
        Dim dist As Single


        ' update the current motor position
        Dim res As Object = mUplink.SendRequest(UplinkCommands.GetMotorPosition, UplinkCommands.REQUEST_FLOAT)

        If res IsNot Nothing Then

            mUplink.MotorPositions(idx) = CSng(res)
            graph.Axes(idx).MotorPosition = CSng(res)

            edit_moveDistance.Text = edit_moveDistance.Text.Replace(".", mDecimalSeparator)
            edit_moveDistance.Text = edit_moveDistance.Text.Replace(",", mDecimalSeparator)

            If Single.TryParse(edit_moveDistance.Text, dist) Then

                mUplink.MotorMove(idx, -dist)

                mUplink.MotorPositions(idx) = mUplink.MotorPositions(idx) - dist
                graph.Axes(idx).MotorPosition = graph.Axes(idx).MotorPosition - dist

            End If

            graph.Update()

        End If


    End Sub


    ' ==================================================================
    Private Sub btn_moveRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_moveRight.Click

        Dim idx As Integer = ListView_Axes.SelectedIndices(0)
        Dim dist As Single

        ' update the current motor position
        Dim res As Object = mUplink.SendRequest(UplinkCommands.GetMotorPosition, UplinkCommands.REQUEST_FLOAT)

        If res IsNot Nothing Then

            mUplink.MotorPositions(idx) = CSng(res)
            graph.Axes(idx).MotorPosition = CSng(res)

            edit_moveDistance.Text = edit_moveDistance.Text.Replace(".", mDecimalSeparator)
            edit_moveDistance.Text = edit_moveDistance.Text.Replace(",", mDecimalSeparator)

            If Single.TryParse(edit_moveDistance.Text, dist) Then

                
                mUplink.MotorMove(idx, dist)
                mUplink.MotorPositions(idx) = mUplink.MotorPositions(idx) + dist
                graph.Axes(idx).MotorPosition = graph.Axes(idx).MotorPosition + dist

            End If

            graph.Update()

        End If

    End Sub

    ' ==================================================================
    Private Sub btn_moveToPos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_moveToPos.Click

        Dim idx As Integer = ListView_Axes.SelectedIndices(0)
        Dim dist As Single

        ' update the current motor position
        Dim res As Object = mUplink.SendRequest(UplinkCommands.GetMotorPosition, UplinkCommands.REQUEST_FLOAT)

        If res IsNot Nothing Then

            mUplink.MotorPositions(idx) = CSng(res)
            graph.Axes(idx).MotorPosition = CSng(res)

            edit_moveDistance.Text = edit_moveDistance.Text.Replace(".", mDecimalSeparator)
            edit_moveDistance.Text = edit_moveDistance.Text.Replace(",", mDecimalSeparator)

            If Single.TryParse(edit_moveDistance.Text, dist) Then

                mUplink.MotorMove(idx, dist - mUplink.MotorPositions(idx))

                mUplink.MotorPositions(idx) = dist - mUplink.MotorPositions(idx)
                graph.Axes(idx).MotorPosition = dist - mUplink.MotorPositions(idx)

            End If

            graph.Update()

        End If

    End Sub


    ' ==================================================================
    Private Sub btn_toStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_toStart.Click

        Dim counter As Integer = 0

        For Each axis In graph.Axes

            ' Are there curves defined
            If axis.Curves.Count > 0 Then

                ' where to go to to finally?
                Dim pos = axis.Curves(0).Point1.Y

                If mUplink.SendCommand(UplinkCommands.MoveMotorToPosition, CSng(pos)) Then

                    mUplink.MotorPositions(counter) = CSng(pos)
                    graph.Axes(counter).MotorPosition = CSng(pos)

                End If

            End If


            ' stop here if we reached the last real axes
            counter = counter + 1

            If counter = mUplink.AxesCount Then
                Exit For
            End If

        Next

    End Sub


    ' ==================================================================
    Private Sub btn_play_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_play.Click

        ' stop any program if running
        mUplink.SendCommand(UplinkCommands.EngineStop)

        ' set the miniEngine mode to the keyframe based setup
        mUplink.SendCommand(UplinkCommands.SetSetupStyleKeyframe)

        SendSetupData()
        mUplink.SendCommand(UplinkCommands.EngineStart)
    End Sub


    ' ==================================================================
    Private Sub btn_stop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_stop.Click
        mUplink.SendCommand(UplinkCommands.EngineStop)
    End Sub


    ' ==================================================================
    Private Sub miniEngineConnectionLost(ByVal sender As System.Object) Handles mUplink.ConnectionLost

        CheckButtonStates()

    End Sub



    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P U B L I C   F U N C T I O N S                                  //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////



    ' ==================================================================
    Public Sub CheckVersion(Optional ByVal showAdditionalMessages As Boolean = False)

        Try

            Dim newV As Boolean = IsNewVersionAvailable(showAdditionalMessages)

            If newV Then

                Dim res As DialogResult = MessageBox.Show("There is a newer version of this software available!" + vbNewLine +
                                                          "Do you want to open the browser to download it?",
                                                          "Information",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Information)

                If res = DialogResult.Yes Then
                    ' Open the website
                    Process.Start(HomeURL)

                End If

            ElseIf Not newV And showAdditionalMessages Then

                MessageBox.Show("Your version is up-to-date.",
                                "Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)

            End If

        Catch ex As Exception
            ' nothing to do here
        End Try

    End Sub


    ' ==================================================================
    Public Sub CheckRegistry(Optional ByVal forceAssociate As Boolean = False)


        Dim regkey As Microsoft.Win32.RegistryKey

        ' open / create the main application keys
        regkey = OpenOrCreateHKCURegistryKey("Software\Classes\pathDesigner.file.mep")

        ' check the associate value
        Dim associateVal = GetOrCreateRegistryValue(regkey, "Associate").ToUpper

        ' did we never ever before start the app?
        If associateVal = "" Then

            ' ask the user if he/she want to associate the file type
            Dim msgres As DialogResult

            msgres = MessageBox.Show("Do you want to associate the filetype .mep (miniEngine path file) with this tool?",
                                     "Question",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question)

            If msgres = Windows.Forms.DialogResult.Yes Then
                regkey.SetValue("Associate", "YES")
                associateVal = "YES"
            Else
                regkey.SetValue("Associate", "NO")
                associateVal = "NO"
            End If

        End If


        If forceAssociate Then
            regkey.SetValue("Associate", "YES")
            associateVal = "YES"
        End If


        If associateVal = "YES" Then

            ' update the defaultIcon
            regkey = OpenOrCreateHKCURegistryKey("Software\Classes\pathDesigner.file.mep\DefaultIcon")
            regkey.SetValue("", Application.ExecutablePath + ",1")

            ' the shell command
            regkey = OpenOrCreateHKCURegistryKey("Software\Classes\pathDesigner.file.mep\shell\open\command")
            regkey.SetValue("", Application.ExecutablePath + " %1")

            ' the file type association
            regkey = OpenOrCreateHKCURegistryKey("Software\Classes\.mep")
            regkey.SetValue("", "pathDesigner.file.mep")

            ' refresh the icon cache
            Utils.SHChangeNotify(&H8000000, &H0, 0, 0)

        End If

    End Sub




    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R O P E R T I E S                                              //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////




    ' =========================================================================
    Public ReadOnly Property VersionString As String
        Get
            Return Version.ToString + "." +
                SubVersion.ToString + "." +
                SubSubVersion.ToString
        End Get
    End Property





    ' /////////////////////////////////////////////////////////////////////////
    ' //                                                                     //
    ' //    P R I V A T E   F U N C T I O N S                                //
    ' //                                                                     //
    ' /////////////////////////////////////////////////////////////////////////


    ' =========================================================================
    ''' <summary>
    ''' Sends the curves to the connected miniEngine
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SendSetupData() As Boolean

        Me.Cursor = Cursors.WaitCursor

        ' loop all existing axes of the miniEngine
        For i As Integer = 0 To mUplink.AxesCount - 1

            ' if we have an axis defined for this miniEngine axis
            If graph.Axes.Count > i Then

                ' send the curves data
                If Not mUplink.SendCurves(i, graph.Axes(i).Curves) Then

                    Me.Cursor = Cursors.Default
                    MessageBox.Show("Data not sent!")
                    Return False

                End If

            Else

                ' send some empty curve data to clear this axis
                If Not mUplink.SendCurves(i, New List(Of QuadBezier)) Then

                    Me.Cursor = Cursors.Default
                    MessageBox.Show("Data not sent!")
                    Return False

                End If

            End If

        Next


        ' now initiate the curve-check to make sure all
        ' depending values are correct (e.g. total run duration)
        mUplink.SendCommand(UplinkCommands.CheckCurves)

        Me.Cursor = Cursors.Default

        Return True

    End Function


    ' =========================================================================
    Private Sub FitToScreen()

        ' zoom graph to fit screen
        graph.Width = Panel1.Width - 10
        graph.Height = Panel1.Height - 25

    End Sub


    ' ==================================================================
    Private Sub EditAxis()

        ' is there a valid selection?
        If ListView_Axes.SelectedIndices.Count = 1 Then

            ' remeber the old selection
            Dim oldSelection As Integer = ListView_Axes.SelectedIndices(0)

            ' send the axis to the dialog for having all the data in there
            Dialog_Axis.Axis = graph.Axes.Item(ListView_Axes.SelectedIndices(0))

            ' if the dialog was enden with an ok
            If Dialog_Axis.ShowDialog = DialogResult.OK Then

                Dim axis As Axis = Dialog_Axis.Axis

                graph.Axes.Item(ListView_Axes.SelectedIndices(0)).Name = axis.Name
                graph.Axes.Item(ListView_Axes.SelectedIndices(0)).Color = axis.Color
                graph.Axes.Item(ListView_Axes.SelectedIndices(0)).Type = axis.Type

                ' repaint the list of axes
                AxesListUpdate(oldSelection)

                'repaint the graph
                graph.Update()

            End If

        End If

    End Sub



    ' =========================================================================
    Private Function LoadFile(Optional ByVal filename As String = "") As Boolean

        CreateNew()

        Dim axis As New Axis
        Dim curve As New QuadBezier(Nothing)
        Dim point As New Point

        ' if no parameter was given...
        If filename = "" Then

            ' ask the user which file to open
            If OpenFileDialog1.ShowDialog = DialogResult.OK Then
                filename = OpenFileDialog1.FileName
                mFilename = filename
            End If

        End If

        ' if we received some valid data
        If filename <> "" Then

            Try

                ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
                Dim XMLobj As Xml.XmlTextReader = New Xml.XmlTextReader(filename)


                With XMLobj

                    'Disable whitespace so that you don't have to read over whitespaces
                    .WhitespaceHandling = Xml.WhitespaceHandling.None
                    'read the xml declaration and advance to family tag
                    .Read()
                    'read the family tag
                    .Read()


                    ' read all the data
                    Do While (.Read())

                        Select Case .NodeType

                            ' check the type of the node
                            Case Xml.XmlNodeType.Element

                                If .Name = "axis" Then
                                    axis = New Axis

                                    If .HasAttributes Then
                                        While .MoveToNextAttribute()

                                            If .Name = "name" Then axis.Name = .Value
                                            If .Name = "color" Then axis.Color = Color.FromArgb(CInt(.Value))
                                            If .Name = "visible" Then axis.Visible = CBool(.Value)
                                            If .Name = "type" Then
                                                If .Value = "linear" Then axis.Type = AxisType.Linear
                                                If .Value = "radial" Then axis.Type = AxisType.Radial
                                            End If


                                        End While
                                    End If
                                End If


                                If .Name = "segment" Then
                                    curve = New QuadBezier(axis)
                                    curve.Points.Clear()
                                End If

                                If .Name = "point" Then
                                    point = New Point

                                    If .HasAttributes Then
                                        While .MoveToNextAttribute()

                                            If .Name = "x" Then point.X = CLng(.Value)
                                            If .Name = "y" Then point.Y = CDbl(.Value.Replace(".", mDecimalSeparator))
                                            If .Name = "type" Then
                                                If .Value = "free" Then point.Type = PointType.Free
                                                If .Value = "locked" Then point.Type = PointType.Locked
                                                If .Value = "helper" Then point.Type = PointType.Helper
                                            End If

                                        End While

                                        curve.Points.Add(point)
                                    End If
                                End If


                            Case Xml.XmlNodeType.EndElement

                                If .Name = "segment" And
                                    curve.Points.Count = 4 Then
                                    axis.Curves.Add(curve)
                                End If

                                If .Name = "axis" Then
                                    graph.Axes.Add(axis)
                                End If

                        End Select

                    Loop

                    .Close()

                    mFilename = filename



                End With

            Catch ex As Exception
                MessageBox.Show("The file could not be loaded as the file format is not correct", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End Try

        End If

        ' beacuse we loaded the file, there are no unsaved changes
        graph.ClearDirty()

        Me.AxesListUpdate()

        Return True


    End Function



    ' =========================================================================
    Private Sub SaveFile(Optional ByVal filename As String = "")

        ' if no parameter was given...
        If filename = "" Then

            ' ask the user which file to open
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                filename = SaveFileDialog1.FileName
                mFilename = filename
                My.Settings.LastFile = filename
                SaveToolStripMenuItem.Enabled = True
            End If

        End If


        If filename <> "" Then

            Dim enc As New System.Text.UnicodeEncoding

            ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
            Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(filename, enc)

            With XMLobj

                ' Formatierung: 4er-Einzüge verwenden 
                .Formatting = Xml.Formatting.Indented
                .Indentation = 4

                .WriteStartDocument()
                .WriteStartElement("axes")

                For Each axis In graph.Axes

                    .WriteStartElement("axis")
                    .WriteAttributeString("name", axis.Name)
                    .WriteAttributeString("color", axis.Color.ToArgb().ToString)
                    .WriteAttributeString("visible", CStr(axis.Visible))

                    Select Case axis.Type
                        Case AxisType.Linear
                            .WriteAttributeString("type", "linear")
                        Case AxisType.Radial
                            .WriteAttributeString("type", "radial")
                    End Select

                    For Each curve In axis.Curves

                        .WriteStartElement("segment")

                        For Each point In curve.Points

                            .WriteStartElement("point")
                            .WriteAttributeString("x", point.X.ToString.Replace(",", "."))
                            .WriteAttributeString("y", point.Y.ToString.Replace(",", "."))

                            Select Case point.Type
                                Case PointType.Free
                                    .WriteAttributeString("type", "free")
                                Case PointType.Locked
                                    .WriteAttributeString("type", "locked")
                                Case PointType.Helper
                                    .WriteAttributeString("type", "helper")
                            End Select


                            .WriteEndElement()  ' Point

                        Next

                        .WriteEndElement()  ' Segment

                    Next

                    .WriteEndElement()  ' Axis

                Next

                .WriteEndElement()  ' Axes

                .Close() ' Document 

                ToolStripStatusLabel1.Text = "Saved the file successfully."

                ' mark all data as saved
                graph.ClearDirty()

            End With

        End If

    End Sub



    ' =========================================================================
    Private Sub ConnectComPort(ByVal portName As String)

        If mUplink.IsOpen Then
            mUplink.Close()
        End If

        mUplink.PortName = portName
        mUplink.Open()

        ' wait for a short amaount of time
        Me.Cursor = Cursors.WaitCursor
        Utils.Delay(400)
        Me.Cursor = Cursors.Default

        If mUplink.IsOpen Then

            ' try to do the handshake with the miniengine
            If mUplink.ConnectMiniEngine() Then
                ' update the menu point with the connected com-port
                COMPortToolStripMenuItem.Text = "Com Port - " + mUplink.PortName
                COMPortToolStripMenuItem.Image = My.Resources.link_26

                Me.Text = Title + " - Connected"

            Else

                ' set the menu point to "Broken connection".
                ' this will be updated if the connection suceeds
                COMPortToolStripMenuItem.Text = "Com Port - " + portName + " ( ! )"
                COMPortToolStripMenuItem.Image = My.Resources.broken_link_26

                Me.Text = Title

            End If


            CheckButtonStates()

        End If

    End Sub


    ' =========================================================================
    Private Sub AxesListUpdate(Optional ByVal selectionIndex As Integer = -1)

        ' re create all the images for the list icons
        ImageList_axes.Images.Clear()
        ListView_Axes.Items.Clear()

        ' create new list icons
        For Each a As Axis In graph.Axes
            Dim icon As Image = New Bitmap(32, 16)
            Dim g As Graphics = Graphics.FromImage(icon)
            g.Clear(a.Color)

            ' add the invisible icon if the axis is not visible
            If Not a.Visible Then
                g.DrawImage(My.Resources.invisible_26, 8, 0, 16, 16)
            End If

            ImageList_axes.Images.Add(icon)
        Next

        Dim counter As Integer = 0

        For Each axis As Axis In graph.Axes

            Dim lvi As New ListViewItem

            lvi.Text = "(" + (counter + 1).ToString + ") - " + axis.Name
            lvi.StateImageIndex = counter
            ListView_Axes.Items.Add(lvi)

            counter = counter + 1

        Next


        If selectionIndex <> -1 Then
            ListView_Axes.SelectedIndices.Add(selectionIndex)
        Else
            graph.DeselectAllAxes()
        End If

        graph.Update()

    End Sub


    ' ==================================================================
    Private Sub LoadSettings()

        RestoreWindowPositionToolStripMenuItem.Checked = My.Settings.Restore_Window_Position

        If My.Settings.Restore_Window_Position Then

            If Not (My.Settings.Form_Main_Left = 0 And
                My.Settings.Form_Main_Top = 0 And
                My.Settings.Form_Main_Width = 0 And
                My.Settings.Form_Main_Height = 0) Then

                Me.StartPosition = FormStartPosition.Manual

                Me.Size = New Size(My.Settings.Form_Main_Width, My.Settings.Form_Main_Height)
                Me.Location = New System.Drawing.Point(My.Settings.Form_Main_Left, My.Settings.Form_Main_Top)

                Me.WindowState = CType(My.Settings.Form_Main_State, FormWindowState)

            End If

        End If

        ' try to connect to the last used com port
        ConnectComPort(My.Settings.Com_Port)

        graph.AutoUpdate = False

        graph.HighContrast = My.Settings.HighContrastMode
        btn_highContrast.Checked = My.Settings.HighContrastMode

        graph.SnapToUnit = My.Settings.SnapToFullUnits
        btn_snap.Checked = My.Settings.SnapToFullUnits

        graph.ShowHelper = My.Settings.ShowHelper
        btn_showHelper.Checked = My.Settings.ShowHelper

        OpenLastFileAutomaticallyToolStripMenuItem.Checked = My.Settings.OpenLastFile
        AutocheckForUpdatesToolStripMenuItem.Checked = My.Settings.AutoCheckUpdate

        graph.AutoUpdate = True
        graph.Update()

    End Sub


    ' ==================================================================
    Private Sub StoreFormSizeAndPosition()

        If mLoadingDone And
            Me.WindowState <> FormWindowState.Minimized Then

            My.Settings.Form_Main_Top = Me.Top
            My.Settings.Form_Main_Left = Me.Left
            My.Settings.Form_Main_Width = Me.Width
            My.Settings.Form_Main_Height = Me.Height

            My.Settings.Form_Main_State = Me.WindowState

        End If

    End Sub




    ' ==================================================================
    Private Sub UpdateAvailableCOMPorts()

        ' remove all sub menu entries
        COMPortToolStripMenuItem.DropDownItems.Clear()
        Dim counter As Integer = 0
        Dim menuItem As ToolStripItem

        For Each item As String In IO.Ports.SerialPort.GetPortNames

            menuItem = COMPortToolStripMenuItem.DropDownItems.Add(item)
            menuItem.Tag = counter

            counter = counter + 1

        Next

    End Sub


    ' ==================================================================
    Private Sub UpdatePoint()

        Try

            Dim p As New Point(CLng(edit_X.Text) * 1000, CDbl(edit_Y.Text))
            graph.SetSelectionCoordinates(p)

        Catch ex As Exception
        End Try

    End Sub


    ' ==================================================================
    Private Sub btn_linear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_linear.Click

        graph.MakeSelectedSegmentLinear()

    End Sub



    ' ==================================================================
    Private Sub CheckButtonStates()

        ' /////////////////////////////
        ' helper point?
        If graph.Selection.Count = 1 Then

            If graph.Selection(0).SequenceInCurve = 1 Or
                graph.Selection(0).SequenceInCurve = 2 Then

                btn_linear.Enabled = True
            Else
                btn_linear.Enabled = False
            End If
        Else
            btn_linear.Enabled = False
        End If


        '/////////////////////////////
        ' main point(s)?
        Dim cnt As Integer = 0
        Dim conditionOK As Boolean = (graph.Selection.Count > 0)

        For Each p In graph.Selection

            ' are ther helper points?
            ' if yes = error
            If p.SequenceInCurve = 1 Or
                p.SequenceInCurve = 2 Then
                conditionOK = False
                Exit For
            End If

            ' is the next point in the selection 
            ' a point that follows the current one?
            ' if not = error
            If cnt < (graph.Selection.Count - 1) Then
                If CInt(p.Tag) <> ((CInt(graph.Selection(cnt + 1).Tag)) - 1) Then
                    conditionOK = False
                    Exit For
                End If
            End If

            cnt = cnt + 1
        Next

        ' if no error was found
        If conditionOK Then
            btn_locked.Enabled = True
            btn_locked.Checked = (graph.Selection(0).Type = PointType.Locked)
        Else
            btn_locked.Enabled = False
            btn_locked.Checked = False
        End If


        '/////////////////////////////
        ' Axis selection
        If ListView_Axes.SelectedIndices.Count = 1 Then

            ' the axis that is selected
            Dim axis As Axis = graph.Axes(ListView_Axes.SelectedIndices(0))

            btn_visible.Enabled = True
            btn_visible.Checked = axis.Visible

            btn_editAxis.Enabled = True

            btn_deleteAxis.Enabled = True
            btn_addSegment.Enabled = True
            btn_DeleteSegment.Enabled = True

            ' if we are connected to the miniEngine
            If mUplink.IsConnected Then

                edit_moveDistance.Enabled = True
                btn_moveLeft.Enabled = True
                btn_moveRight.Enabled = True
                btn_home.Enabled = True
                btn_moveToPos.Enabled = True

            End If

        Else
            btn_visible.Checked = False
            btn_editAxis.Enabled = False
            btn_visible.Enabled = False
            btn_deleteAxis.Enabled = False
            btn_addSegment.Enabled = False
            btn_DeleteSegment.Enabled = False


            edit_moveDistance.Enabled = False
            btn_moveLeft.Enabled = False
            btn_moveRight.Enabled = False
            btn_home.Enabled = False
            btn_moveToPos.Enabled = False

        End If


        '/////////////////////////////
        ' miniEngine connected
        If mUplink.IsConnected Then

            btn_toStart.Enabled = True
            btn_play.Enabled = True
            btn_stop.Enabled = True
            btn_SendData.Enabled = True

        Else

            btn_toStart.Enabled = False
            btn_play.Enabled = False
            btn_stop.Enabled = False
            btn_SendData.Enabled = False

        End If



    End Sub

    ' ==================================================================
    Private Sub btn_DeleteSegment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DeleteSegment.Click

        graph.DeleteLastSegment()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mUplink.ConnectMiniEngine()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mUplink.ClearInputBuffer()
    End Sub



    ' ==================================================================
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click

        CreateNew()

    End Sub


    ' ==================================================================
    Private Sub CreateNew()

        Dim dRes = DialogResult.No

        If mFilename <> "" Then
            dRes = MessageBox.Show("The current file is not yet saved. Do you want to save it now?",
                          "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        End If

        Select Case dRes

            Case DialogResult.Yes

                ' save the file
                SaveFile(mFilename)

            Case DialogResult.Cancel

                ' leave this function
                Return

        End Select

        ' create a new set
        graph.Axes.Clear()
        ListView_Axes.Clear()
        graph.Update()


    End Sub




    ' ==================================================================
    Private Function IsNewVersionAvailable(ByVal showWarnings As Boolean) As Boolean

        Dim newestVersion As String = Utils.ReadFileFromURL(VersionURL, showWarnings).Trim

        ' divide the string into seperate subStrings
        Dim separator As String() = {"."}
        Dim segments As String() = newestVersion.Split(separator, StringSplitOptions.RemoveEmptyEntries)

        ' did we receive 3 version segments (main version, sub version, sub sub version)
        If segments.Count = 3 Then

            ' initiate som neede variables
            Dim newestVer As Integer = 0
            Dim newestSubVer As Integer = 0
            Dim newestSubsubVer As Integer = 0

            ' are we able to convert all 3 values to integers?
            If Integer.TryParse(segments(0), newestVer) And
               Integer.TryParse(segments(1), newestSubVer) And
                Integer.TryParse(segments(2), newestSubsubVer) Then

                ' make the numbers biiig so that an overflow is unlikely
                ' (this is the lazy approach to avoid complex AND/OR logic)
                Dim versionNum As Long = (Version * 100000000) + (SubVersion * 10000) + (SubSubVersion)
                Dim newestVerNum As Long = (newestVer * 100000000) + (newestSubVer * 10000) + (newestSubsubVer)

                ' do we have a new version?
                If newestVerNum > versionNum Then
                    Return True
                End If

            End If

        Else

            Throw New Exception("The received data is no valid version information. Expected format: Int16.Int16.Int16")

        End If

        Return False

    End Function




    ' ==================================================================
    Private Function OpenOrCreateHKCURegistryKey(ByVal key As String) As Microsoft.Win32.RegistryKey

        Dim result As Microsoft.Win32.RegistryKey

        result = My.Computer.Registry.CurrentUser.OpenSubKey(key, True)

        If result Is Nothing Then
            result = My.Computer.Registry.CurrentUser.CreateSubKey(key)
        End If

        Return result

    End Function

    ' ==================================================================
    Private Function GetOrCreateRegistryValue(ByVal key As Microsoft.Win32.RegistryKey, ByVal name As String) As String

        Dim result As Object = key.GetValue(name)

        If result Is Nothing Then
            key.SetValue(name, "")
            result = ""
        End If

        Return result.ToString

    End Function


    ' ==================================================================
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendData.Click
        If (SendSetupData()) Then
            MessageBox.Show("Uploaded the data successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Error during the data upload!" + vbNewLine + "Please try again.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

   
   
   
End Class
