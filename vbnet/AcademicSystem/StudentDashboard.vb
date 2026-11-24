Imports System.Data

Namespace AcademicSystem

    <System.ComponentModel.DesignerCategory("Form")>
    Partial Public Class StudentDashboard
        Inherits Form

        Private _studentId As Integer
        Private _refresher As RealTimeRefresher

        Public Sub New()
            InitializeComponent()

            Dim designTime As Boolean = (System.ComponentModel.LicenseManager.UsageMode =
                                          System.ComponentModel.LicenseUsageMode.Designtime)

            If designTime Then
                header.Text = "Student Dashboard (Design Preview)"
                Return
            End If

            If Session.RoleId <> 3 Then
                Throw New UnauthorizedAccessException("Student role is required to open this form.")
            End If

            Text = "Student Dashboard - " & Session.Username
            SessionUi.AddStatusBar(Me)

            AddHandler btnLogout.Click,
                Sub(sender As Object, e As EventArgs)
                    If TypeOf MdiParent Is MainMdiForm Then MdiParent.Close() Else Close()
                End Sub
            AddHandler btnSearch.Click, AddressOf BtnSearch_Click
            AddHandler txtSearch.KeyDown, AddressOf TxtSearch_KeyDown

            Dim autoLabel As New Label() With {
                .Name = "lblAutoRefresh", .Location = New Point(900, 16),
                .Size = New Size(260, 34), .ForeColor = Color.SeaGreen,
                .Font = New Font("Segoe UI", 9)
            }
            pnlSearch.Controls.Add(autoLabel)

            LoadProfile()
            RefreshGrades()
            RefreshAverages()
            AdvancedUi.ApplyGridFeatures(dgvGrades)
            AdvancedUi.ApplyGridFeatures(dgvAverages, averageColumn:="Average Grade")
            AdvancedUi.ApplyTooltips(Me)

            _refresher = New RealTimeRefresher(Me, AddressOf FetchGradesTable, AddressOf ApplyGrades)
            _refresher.Start()
        End Sub

        Private Sub LoadProfile()
            Dim parameters As New Dictionary(Of String, Object) From {{"@userId", Session.UserId}}
            Dim profile = Db.FetchTable(
                "SELECT student_id, first_name, last_name FROM student_own_profile WHERE user_id = @userId",
                parameters)

            If profile.Rows.Count > 0 Then
                Dim row = profile.Rows(0)
                _studentId = Convert.ToInt32(row("student_id"))
                header.Text = "Welcome, " & Convert.ToString(row("first_name")) & " " &
                              Convert.ToString(row("last_name")) &
                              "  |  Student ID: " & _studentId &
                              "  |  Your grades and course averages"
            Else
                header.Text = "No student profile is linked to this account."
                _studentId = -1
            End If
        End Sub

        Private Function FetchGradesTable() As DataTable
            If _studentId <= 0 Then Return New DataTable()

            Dim sql As String =
                "SELECT course_code AS 'Course Code', course_name AS 'Course', " &
                "type_name AS 'Assignment', grade AS 'Grade', " &
                "date_recorded AS 'Recorded' " &
                "FROM student_grade_summary WHERE student_id = @studentId"

            Dim parameters As New Dictionary(Of String, Object) From {{"@studentId", _studentId}}

            Dim search As String = txtSearch.Text.Trim()
            If search.Length > 0 Then
                sql &= " AND (course_code LIKE @q1 OR course_name LIKE @q1 OR type_name LIKE @q1)"
                parameters.Add("@q1", InputValidator.SafeLike(search))
            End If

            sql &= " ORDER BY course_code, type_name"
            Return Db.FetchTable(sql, parameters)
        End Function

        Private Sub ApplyGrades(table As DataTable)
            If IsDisposed Then Return
            dgvGrades.DataSource = table
            AdvancedUi.ApplyGridFeatures(dgvGrades)
            lblStatus.ForeColor = Color.SeaGreen
            lblStatus.Text = table.Rows.Count & " grade row(s)."

            Dim label As Control = pnlSearch.Controls("lblAutoRefresh")
            If label IsNot Nothing Then
                label.Text = "Auto-refresh #" & If(_refresher Is Nothing, 0, _refresher.RefreshCount) &
                             vbCrLf & "Last: " & DateTime.Now.ToString("HH:mm:ss")
            End If
        End Sub

        Private Sub RefreshGrades()
            ApplyGrades(FetchGradesTable())
        End Sub

        Private Sub RefreshAverages()
            If _studentId <= 0 Then Return

            Dim parameters As New Dictionary(Of String, Object) From {{"@studentId", _studentId}}
            dgvAverages.DataSource = Db.FetchTable(
                "SELECT course_code AS 'Course Code', course_name AS 'Course', " &
                "average_grade AS 'Average Grade', grades_recorded AS 'Grades Recorded' " &
                "FROM student_course_averages " &
                "WHERE student_id = @studentId ORDER BY course_code", parameters)
            AdvancedUi.ApplyGridFeatures(dgvAverages, averageColumn:="Average Grade")
        End Sub

        Private Sub BtnSearch_Click(sender As Object, e As EventArgs)
            RefreshGrades()
            lblStatus.Text = "Search complete."
        End Sub

        Private Sub TxtSearch_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                RefreshGrades()
                e.Handled = True
            End If
        End Sub

        Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
            If _refresher IsNot Nothing Then _refresher.StopRefresh()
            MyBase.OnFormClosed(e)
        End Sub

    End Class

End Namespace
