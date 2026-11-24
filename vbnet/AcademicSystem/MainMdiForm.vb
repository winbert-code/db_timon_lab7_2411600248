Imports System.Linq

Namespace AcademicSystem

    ' MDI parent that provides role-aware navigation for all child windows.
    Public Class MainMdiForm
        Inherits Form

        Private _dashboard As Form
        Private _splashPreview As SplashForm

        Public Sub New()
            IsMdiContainer = True
            Text = "DB_Timon - Academic Management System"
            ClientSize = New Size(1240, 650)
            StartPosition = FormStartPosition.CenterScreen
            BackColor = Color.FromArgb(240, 244, 248)
            WindowState = FormWindowState.Maximized

            BuildMenu()
            BuildToolbar()
            SessionUi.AddStatusBar(Me)

            Dim strip As StatusStrip = TryCast(Controls("sessionStatus"), StatusStrip)
            If strip IsNot Nothing Then
                strip.Items.Add(New ToolStripStatusLabel With {
                    .Name = "lab7Status",
                    .Text = "Ready - MDI workspace",
                    .Alignment = ToolStripItemAlignment.Right,
                    .ForeColor = Color.FromArgb(31, 78, 121)
                })
            End If

            AdvancedUi.ApplyTooltips(Me)
            OpenDashboard()
        End Sub

        Private Sub SetStatus(text As String)
            Dim strip As StatusStrip = TryCast(Controls("sessionStatus"), StatusStrip)
            If strip Is Nothing Then Return
            Dim label As ToolStripItem = strip.Items("lab7Status")
            If label IsNot Nothing Then label.Text = text
        End Sub

        Private Sub BuildMenu()
            Dim menu As New MenuStrip()

            Dim fileMenu As New ToolStripMenuItem("&File")
            AddMenuItem(fileMenu, "Dashboard", Sub(sender, e) OpenDashboard())
            AddMenuItem(fileMenu, "Notifications", Sub(sender, e) OpenNotifications())
            AddMenuItem(fileMenu, "Settings", Sub(sender, e) OpenSettings())
            fileMenu.DropDownItems.Add(New ToolStripSeparator())
            AddMenuItem(fileMenu, "Exit", Sub(sender, e) Close())

            Dim viewMenu As New ToolStripMenuItem("&View")
            AddMenuItem(viewMenu, "Cascade windows", Sub(sender, e) LayoutMdi(MdiLayout.Cascade))
            AddMenuItem(viewMenu, "Tile horizontally", Sub(sender, e) LayoutMdi(MdiLayout.TileHorizontal))
            AddMenuItem(viewMenu, "Tile vertically", Sub(sender, e) LayoutMdi(MdiLayout.TileVertical))
            AddMenuItem(viewMenu, "Close all children", Sub(sender, e) CloseAllChildren())

            Dim toolsMenu As New ToolStripMenuItem("&Tools")
            AddMenuItem(toolsMenu, "Advanced Search", Sub(sender, e) OpenSearch())
            AddMenuItem(toolsMenu, "Visualizations", Sub(sender, e) OpenVisualization())
            AddMenuItem(toolsMenu, "Bulk Grade Entry", Sub(sender, e) OpenBulkGrades())
            AddMenuItem(toolsMenu, "Reports", Sub(sender, e) OpenReports())
            If Session.RoleId = 1 Then
                toolsMenu.DropDownItems.Add(New ToolStripSeparator())
                AddMenuItem(toolsMenu, "Audit Trail", Sub(sender, e) OpenAudit())
                AddMenuItem(toolsMenu, "Backup Utility", Sub(sender, e) OpenBackup())
            End If

            Dim helpMenu As New ToolStripMenuItem("&Help")
            AddMenuItem(helpMenu, "Code Samples", Sub(sender, e) OpenChild(Of CodeSamplesForm)("Lab 7 Code Samples"))
            AddMenuItem(helpMenu, "Show Splash", Sub(sender, e) ShowSplashPreview())
            AddMenuItem(helpMenu, "Simulate Error", Sub(sender, e) SimulateError())
            AddMenuItem(helpMenu, "Application log", Sub(sender, e) OpenLog())

            menu.Items.AddRange(New ToolStripItem() {fileMenu, viewMenu, toolsMenu, helpMenu})
            MainMenuStrip = menu
            Controls.Add(menu)
        End Sub

        Private Sub BuildToolbar()
            Dim toolbar As New ToolStrip() With {.Dock = DockStyle.Top, .GripStyle = ToolStripGripStyle.Hidden,
                                                .BackColor = Color.FromArgb(31, 78, 121), .ForeColor = Color.White}
            toolbar.Items.Add(New ToolStripButton("Dashboard", Nothing, Sub(sender, e) OpenDashboard()))
            toolbar.Items.Add(New ToolStripButton("Charts", Nothing, Sub(sender, e) OpenVisualization()))
            toolbar.Items.Add(New ToolStripButton("Search", Nothing, Sub(sender, e) OpenSearch()))
            toolbar.Items.Add(New ToolStripButton("Notifications", Nothing, Sub(sender, e) OpenNotifications()))
            toolbar.Items.Add(New ToolStripButton("Settings", Nothing, Sub(sender, e) OpenSettings()))
            toolbar.Items.Add(New ToolStripSeparator())
            toolbar.Items.Add(New ToolStripButton("Tile", Nothing, Sub(sender, e) LayoutMdi(MdiLayout.TileVertical)))
            toolbar.Items.Add(New ToolStripButton("Cascade", Nothing, Sub(sender, e) LayoutMdi(MdiLayout.Cascade)))
            Controls.Add(toolbar)
        End Sub

        Private Sub AddMenuItem(parent As ToolStripMenuItem, text As String, handler As EventHandler)
            Dim item As New ToolStripMenuItem(text)
            AddHandler item.Click, handler
            parent.DropDownItems.Add(item)
        End Sub

        Private Sub CloseAllChildren()
            For Each child As Form In MdiChildren.ToList()
                child.Close()
            Next
        End Sub

        Private Sub OpenChild(Of T As {Form, New})(Optional title As String = Nothing)
            Try
                For Each existing As Form In MdiChildren
                    If TypeOf existing Is T Then
                        existing.BringToFront()
                        existing.Focus()
                        SetStatus(If(title, existing.Text) & " is already open")
                        Return
                    End If
                Next

                Dim child As New T()
                child.MdiParent = Me
                child.WindowState = FormWindowState.Normal
                If Not String.IsNullOrWhiteSpace(title) Then child.Text = title
                AdvancedUi.CentreChild(child, Me)
                child.Show()
                child.BringToFront()
                SetStatus("Opened: " & child.Text)
            Catch ex As Exception
                GlobalExceptionHandler.Handle("MDI child", ex)
            End Try
        End Sub

        Private Sub OpenDashboard()
            Select Case Session.RoleId
                Case 1
                    OpenChild(Of AdminForm)("Admin Management")
                Case 2
                    OpenChild(Of TeacherDashboard)("Teacher Dashboard")
                Case 3
                    OpenChild(Of StudentDashboard)("Student Dashboard")
                Case Else
                    SetStatus("No dashboard for this role.")
            End Select
        End Sub

        Private Sub OpenNotifications()
            OpenChild(Of NotificationCenterForm)("Notification Center")
        End Sub

        Private Sub OpenSearch()
            OpenChild(Of SearchAdvancedForm)("Advanced Search")
        End Sub

        Private Sub OpenSettings()
            OpenChild(Of SettingsForm)("Settings")
        End Sub

        Private Sub OpenVisualization()
            OpenChild(Of VisualizationForm)("Academic Visualizations")
        End Sub

        Private Sub OpenBulkGrades()
            If Session.RoleId = 3 Then
                SetStatus("Bulk grade entry is available to admin and teacher roles.")
                Return
            End If
            OpenChild(Of BulkGradeEntryForm)("Bulk Grade Entry")
        End Sub

        Private Sub OpenReports()
            If Session.RoleId = 3 Then
                SetStatus("Reports are available to admin and teacher roles.")
                Return
            End If
            OpenChild(Of ReportsForm)("Reports and Exports")
        End Sub

        Private Sub OpenAudit()
            OpenChild(Of AuditLogForm)("Audit Trail Viewer")
        End Sub

        Private Sub OpenBackup()
            OpenChild(Of BackupForm)("Backup Utility")
        End Sub

        Private Sub ShowSplashPreview()
            If _splashPreview IsNot Nothing AndAlso Not _splashPreview.IsDisposed Then
                _splashPreview.BringToFront()
                Return
            End If
            _splashPreview = New SplashForm(True)
            AddHandler _splashPreview.FormClosed, Sub(sender, e) _splashPreview = Nothing
            _splashPreview.Show(Me)
        End Sub

        Private Sub SimulateError()
            Try
                Throw New InvalidOperationException(
                    "Demo: a report calculation received an invalid grade value.")
            Catch ex As Exception
                GlobalExceptionHandler.Handle("demo error", ex)
            End Try
        End Sub

        Private Sub OpenLog()
            Try
                If Not IO.File.Exists(AppLogger.LogFilePath) Then AppLogger.Initialize()
                Dim viewer As New LogViewerForm()
                viewer.MdiParent = Me
                AdvancedUi.CentreChild(viewer, Me)
                viewer.Show()
                SetStatus("Opened: application log")
            Catch ex As Exception
                GlobalExceptionHandler.Handle("log viewer", ex)
            End Try
        End Sub

    End Class

End Namespace
