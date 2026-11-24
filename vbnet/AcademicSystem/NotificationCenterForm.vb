Imports System.Data

Namespace AcademicSystem

    ' Shows pending tasks and system notifications gathered from DB_Timon.
    Public Class NotificationCenterForm
        Inherits Form

        Private _grid As DataGridView
        Private _status As Label
        Private _refresher As RealTimeRefresher

        Public Sub New()
            Text = "Notification Center"
            ClientSize = New Size(1100, 600)
            StartPosition = FormStartPosition.CenterScreen
            BackColor = Color.FromArgb(248, 250, 252)

            Dim header As New Label() With {
                .Name = "notificationHeader",
                .Text = "Notification Center - pending tasks and system activity",
                .Dock = DockStyle.Top, .Height = 56,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121), .Padding = New Padding(18, 14, 0, 0)
            }

            _grid = New DataGridView() With {.Dock = DockStyle.Fill}
            AdvancedUi.ApplyGridFeatures(_grid)

            Dim bottom As New Panel() With {.Dock = DockStyle.Bottom, .Height = 62, .Padding = New Padding(16, 12, 16, 8)}
            _status = New Label() With {
                .Text = "Loading notifications...", .Dock = DockStyle.Fill,
                .Font = New Font("Segoe UI", 10), .ForeColor = Color.SeaGreen
            }
            Dim btnRefresh As New Button() With {
                .Text = "Refresh now", .Dock = DockStyle.Right, .Width = 140,
                .Height = 34, .Padding = New Padding(0, 4, 0, 0)
            }
            AddHandler btnRefresh.Click, Sub(sender, e) Apply(Fetch())
            bottom.Controls.AddRange(New Control() {_status, btnRefresh})

            Dim btnClose As New Button() With {
                .Text = "Close", .Dock = DockStyle.Bottom, .Height = 34,
                .DialogResult = DialogResult.OK
            }
            AcceptButton = btnClose

            Controls.AddRange(New Control() {_grid, header, bottom, btnClose})
            SessionUi.AddStatusBar(Me)
            AdvancedUi.ApplyTooltips(Me)

            Apply(Fetch())
            _refresher = New RealTimeRefresher(Me, AddressOf Fetch, AddressOf Apply)
            _refresher.Start()
        End Sub

        Private Function Fetch() As DataTable
            Dim result As New DataTable()
            result.Columns.Add("Type", GetType(String))
            result.Columns.Add("Detail", GetType(String))
            result.Columns.Add("Severity", GetType(String))
            result.Columns.Add("Time", GetType(String))

            Dim lowGrades = Db.FetchTable(
                "SELECT 'LOW AVERAGE' AS kind, CONCAT(course_code, ' average ', average_grade) AS detail, " &
                "'Warning' AS severity, NOW() AS stamp FROM student_course_averages " &
                "WHERE average_grade < 75 ORDER BY average_grade LIMIT 8")
            AddRows(result, lowGrades)

            Dim audit = Db.FetchTable(
                "SELECT 'AUDIT' AS kind, CONCAT(action_type, ' on ', table_name, ' by ', app_user) AS detail, " &
                "'Info' AS severity, event_time AS stamp FROM audit_log " &
                "ORDER BY event_time DESC LIMIT 6")
            AddRows(result, audit)

            Dim sessions = Db.FetchTable(
                "SELECT 'SESSIONS' AS kind, CONCAT(COUNT(*), ' active session(s)') AS detail, " &
                "'Info' AS severity, NOW() AS stamp FROM user_sessions WHERE status = 'ACTIVE'")
            AddRows(result, sessions)

            Dim courses = Db.FetchTable(
                "SELECT 'COURSE' AS kind, CONCAT(course_code, ' - ', grades_submitted, ' grades') AS detail, " &
                "'Info' AS severity, NOW() AS stamp FROM course_performance_analytics " &
                "ORDER BY grades_submitted DESC LIMIT 5")
            AddRows(result, courses)

            Return result
        End Function

        Private Sub AddRows(target As DataTable, source As DataTable)
            For Each row As DataRow In source.Rows
                target.Rows.Add(
                    Convert.ToString(row("kind")),
                    Convert.ToString(row("detail")),
                    Convert.ToString(row("severity")),
                    Convert.ToString(row("stamp")))
            Next
        End Sub

        Private Sub Apply(table As DataTable)
            If IsDisposed Then Return
            _grid.DataSource = table
            AdvancedUi.ApplyGridFeatures(_grid)
            _status.ForeColor = Color.SeaGreen
            Dim count As Integer = If(_refresher Is Nothing, 0, _refresher.RefreshCount)
            Dim stamp As String = DateTime.Now.ToString("HH:mm:ss")
            _status.Text = table.Rows.Count & " notification(s)  |  Auto-refresh #" & count &
                           "  |  Last: " & stamp
            Dim heading As Label = TryCast(Controls("notificationHeader"), Label)
            If heading IsNot Nothing Then
                heading.Text = "Notification Center  |  Auto-refresh #" & count & "  |  Last: " & stamp
            End If
        End Sub

        Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
            If _refresher IsNot Nothing Then _refresher.StopRefresh()
            MyBase.OnFormClosed(e)
        End Sub

    End Class

End Namespace
