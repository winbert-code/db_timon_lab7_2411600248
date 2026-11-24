Imports System.Data
Imports MySqlConnector

Namespace AcademicSystem

    <System.ComponentModel.DesignerCategory("Form")>
    Partial Public Class TeacherDashboard
        Inherits Form

        Private _refresher As RealTimeRefresher

        Public Sub New()
            InitializeComponent()

            Dim designTime As Boolean = (System.ComponentModel.LicenseManager.UsageMode =
                                          System.ComponentModel.LicenseUsageMode.Designtime)

            If designTime Then
                header.Text = "Teacher Dashboard (Design Preview)"
                Return
            End If

            If Session.RoleId <> 2 Then
                Throw New UnauthorizedAccessException("Teacher role is required to open this form.")
            End If

            Text = "Teacher Dashboard - " & Session.Username
            header.Text = "Welcome, " & Session.Username &
                          "  |  Course overview and gradebook entry"
            SessionUi.AddStatusBar(Me)

            AddHandler btnLogout.Click,
                Sub(sender As Object, e As EventArgs)
                    If TypeOf MdiParent Is MainMdiForm Then MdiParent.Close() Else Close()
                End Sub
            AddHandler btnReports.Click, AddressOf BtnReports_Click
            AddHandler btnSaveGrade.Click, AddressOf BtnSaveGrade_Click
            AddHandler btnCalculate.Click, AddressOf BtnCalculate_Click
            AddHandler btnSearch.Click, AddressOf BtnSearch_Click
            AddHandler cboCourse.SelectedIndexChanged, AddressOf CboCourse_SelectedIndexChanged
            AddHandler txtSearch.KeyDown, AddressOf TxtSearch_KeyDown

            Dim autoLabel As New Label() With {
                .Name = "lblAutoRefresh", .Location = New Point(900, 178),
                .Size = New Size(260, 36), .ForeColor = Color.SeaGreen,
                .Font = New Font("Segoe UI", 9)
            }
            pnlEntry.Controls.Add(autoLabel)

            LoadCombos()
            RefreshOverview()
            AdvancedUi.ApplyGridFeatures(dgvOverview)
            AdvancedUi.ApplyTooltips(Me)

            _refresher = New RealTimeRefresher(Me, AddressOf FetchOverview, AddressOf ApplyOverview)
            _refresher.Start()
        End Sub

        Private Sub LoadCombos()
            Dim parameters As New Dictionary(Of String, Object) From {{"@userId", Session.UserId}}

            Dim courses = Db.FetchTable(
                "SELECT c.course_id AS Value, " &
                "CONCAT(c.course_code, ' - ', c.course_name) AS Display " &
                "FROM courses c JOIN instructors i ON i.instructor_id = c.instructor_id " &
                "WHERE i.user_id = @userId ORDER BY c.course_code", parameters)
            cboCourse.DataSource = courses
            cboCourse.DisplayMember = "Display"
            cboCourse.ValueMember = "Value"

            Dim types = Db.FetchTable(
                "SELECT type_id AS Value, type_name AS Display " &
                "FROM assignment_types ORDER BY type_id")
            cboType.DataSource = types
            cboType.DisplayMember = "Display"
            cboType.ValueMember = "Value"

            LoadStudents()
        End Sub

        Private Sub LoadStudents()
            If cboCourse.SelectedValue Is Nothing Then Return

            Dim parameters As New Dictionary(Of String, Object) From {
                {"@courseId", Convert.ToInt32(cboCourse.SelectedValue)}
            }
            Dim students = Db.FetchTable(
                "SELECT s.student_id AS Value, " &
                "CONCAT(s.first_name, ' ', s.last_name) AS Display " &
                "FROM enrollments e JOIN students s ON s.student_id = e.student_id " &
                "WHERE e.course_id = @courseId ORDER BY Display", parameters)

            cboStudent.DataSource = students
            cboStudent.DisplayMember = "Display"
            cboStudent.ValueMember = "Value"
        End Sub

        Private Sub CboCourse_SelectedIndexChanged(sender As Object, e As EventArgs)
            If Not IsDesignTime() Then LoadStudents()
        End Sub

        Private Function IsDesignTime() As Boolean
            Return System.ComponentModel.LicenseManager.UsageMode =
                   System.ComponentModel.LicenseUsageMode.Designtime
        End Function

        Private Function FetchOverview() As DataTable
            Dim sql As String =
                "SELECT course_code AS 'Course', type_name AS 'Assignment', " &
                "submissions AS 'Submissions', average_grade AS 'Average Grade' " &
                "FROM teacher_course_overview WHERE user_id = @userId " &
                "ORDER BY course_code, type_name"

            Dim parameters As New Dictionary(Of String, Object) From {{"@userId", Session.UserId}}

            Dim search As String = txtSearch.Text.Trim()
            If search.Length > 0 Then
                sql = sql.Replace("ORDER BY", "AND (course_code LIKE @q1 OR course_name LIKE @q1 OR type_name LIKE @q1) ORDER BY")
                parameters.Add("@q1", InputValidator.SafeLike(search))
            End If

            Return Db.FetchTable(sql, parameters)
        End Function

        Private Sub ApplyOverview(table As DataTable)
            If IsDisposed Then Return
            dgvOverview.DataSource = table
            AdvancedUi.ApplyGridFeatures(dgvOverview)
            lblStatus.ForeColor = Color.SeaGreen
            lblStatus.Text = table.Rows.Count & " overview row(s)."

            Dim label As Control = pnlEntry.Controls("lblAutoRefresh")
            If label IsNot Nothing Then
                label.Text = "Auto-refresh #" & If(_refresher Is Nothing, 0, _refresher.RefreshCount) &
                             vbCrLf & "Last: " & DateTime.Now.ToString("HH:mm:ss")
            End If
        End Sub

        Private Sub RefreshOverview()
            ApplyOverview(FetchOverview())
        End Sub

        Private Sub BtnSearch_Click(sender As Object, e As EventArgs)
            RefreshOverview()
            lblStatus.Text = "Search complete."
        End Sub

        Private Sub TxtSearch_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                RefreshOverview()
                e.Handled = True
            End If
        End Sub

        Private Sub BtnSaveGrade_Click(sender As Object, e As EventArgs)
            lblStatus.ForeColor = Color.Firebrick

            If cboStudent.SelectedValue Is Nothing OrElse cboCourse.SelectedValue Is Nothing OrElse
               cboType.SelectedValue Is Nothing Then
                lblStatus.Text = "Select a course, a student and an assignment type."
                Return
            End If

            Dim grade As Decimal
            If Not Decimal.TryParse(txtGrade.Text.Trim(), grade) Then
                lblStatus.Text = "Enter a numeric grade between 0 and 100."
                Return
            End If
            If grade < 0 OrElse grade > 100 Then
                lblStatus.Text = "The CHECK constraint allows grades only from 0 to 100."
                Return
            End If

            Dim parameters As New Dictionary(Of String, Object) From {
                {"@studentId", Convert.ToInt32(cboStudent.SelectedValue)},
                {"@courseId", Convert.ToInt32(cboCourse.SelectedValue)},
                {"@typeId", Convert.ToInt32(cboType.SelectedValue)},
                {"@grade", grade}
            }

            Dim rows As Integer = Db.Execute(
                "INSERT INTO grades (student_id, course_id, type_id, grade) " &
                "VALUES (@studentId, @courseId, @typeId, @grade) " &
                "ON DUPLICATE KEY UPDATE grade = VALUES(grade), date_recorded = CURRENT_TIMESTAMP",
                parameters)

            lblStatus.ForeColor = Color.SeaGreen
            lblStatus.Text = "Grade saved (rows affected: " & rows & ")."
            txtGrade.Clear()
            RefreshOverview()
            AppLogger.Log("INFO", "Teacher saved a grade in course " & Convert.ToString(cboCourse.SelectedValue))
        End Sub

        Private Sub BtnCalculate_Click(sender As Object, e As EventArgs)
            If cboStudent.SelectedValue Is Nothing Then
                lblStatus.ForeColor = Color.Firebrick
                lblStatus.Text = "Select a student first."
                Return
            End If

            Dim studentId As Integer = Convert.ToInt32(cboStudent.SelectedValue)

            Try
                Using connection = Db.GetConnection()
                    Using command As New MySqlCommand("sp_CalculateCourseGrade", connection)
                        command.CommandType = CommandType.StoredProcedure
                        command.Parameters.AddWithValue("@p_student_id", studentId)

                        Dim outputParam As New MySqlParameter("@p_final_grade", MySqlDbType.Decimal)
                        outputParam.Direction = ParameterDirection.Output
                        outputParam.Scale = 2
                        command.Parameters.Add(outputParam)

                        connection.Open()
                        command.ExecuteNonQuery()
                        Dim finalGrade As Decimal = Convert.ToDecimal(outputParam.Value)
                        lblStatus.ForeColor = Color.SeaGreen
                        lblStatus.Text = "Weighted final grade: " & finalGrade.ToString("0.00") &
                                         "  (override by entering a different grade above)."
                    End Using
                End Using
            Catch ex As Exception
                lblStatus.ForeColor = Color.Firebrick
                lblStatus.Text = "Calculation failed: " & ex.Message
                AppLogger.LogException("grade calculation", ex)
            End Try
        End Sub

        Private Sub BtnReports_Click(sender As Object, e As EventArgs)
            Dim reports As New ReportsForm()
            reports.MdiParent = If(TypeOf MdiParent Is MainMdiForm, MdiParent, Nothing)
            AdvancedUi.CentreChild(reports, If(reports.MdiParent IsNot Nothing, reports.MdiParent, Me))
            If reports.MdiParent Is Nothing Then
                AddHandler reports.FormClosed, Sub(sender2 As Object, e2 As FormClosedEventArgs) Show()
                Hide()
            End If
            reports.Show()
        End Sub

        Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
            If _refresher IsNot Nothing Then _refresher.StopRefresh()
            MyBase.OnFormClosed(e)
        End Sub

    End Class

End Namespace
