Imports System.Text
Imports System.IO

Namespace AcademicSystem

    <System.ComponentModel.DesignerCategory("Form")>
    Partial Public Class ReportsForm
        Inherits Form

        Private _currentTable As DataTable

        Public Sub New()
            InitializeComponent()

            Dim designTime As Boolean = (System.ComponentModel.LicenseManager.UsageMode =
                                          System.ComponentModel.LicenseUsageMode.Designtime)

            If designTime Then Return

            If Session.RoleId <> 1 AndAlso Session.RoleId <> 2 Then
                Throw New UnauthorizedAccessException("Admin or Teacher role is required to open reports.")
            End If

            SessionUi.AddStatusBar(Me)
            cboReportType.Items.Add("Student Transcript")
            cboReportType.Items.Add("Course Performance Analytics")
            cboReportType.SelectedIndex = 0

            Dim suggestions As New AutoCompleteStringCollection()
            Dim names = Db.FetchTable(
                "SELECT CONCAT(first_name, ' ', last_name) AS name FROM students " &
                "UNION SELECT course_code FROM courses " &
                "UNION SELECT CONCAT(first_name, ' ', last_name) FROM instructors " &
                "ORDER BY name")
            For Each row As DataRow In names.Rows
                suggestions.Add(Convert.ToString(row("name")))
            Next
            txtSearch.AutoCompleteCustomSource = suggestions
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource

            AddHandler btnGenerate.Click, AddressOf BtnGenerate_Click
            AddHandler btnExport.Click, AddressOf BtnExport_Click
            AddHandler btnLogout.Click, Sub(sender As Object, e As EventArgs) Close()
            AddHandler txtSearch.KeyDown, AddressOf TxtSearch_KeyDown

            GenerateReport()
        End Sub

        Private Sub BtnGenerate_Click(sender As Object, e As EventArgs)
            GenerateReport()
        End Sub

        Private Sub TxtSearch_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                GenerateReport()
                e.Handled = True
            End If
        End Sub

        Private Sub GenerateReport()
            Dim search As String = txtSearch.Text.Trim()
            Dim parameters As New Dictionary(Of String, Object)()
            Dim sql As String

            If cboReportType.SelectedIndex = 1 Then
                sql = "SELECT course_code AS 'Course Code', course_name AS 'Course Name', " &
                      "instructor_name AS 'Instructor', enrolled_students AS 'Enrolled Students', " &
                      "grades_submitted AS 'Grades Submitted', average_grade AS 'Average Grade', " &
                      "minimum_grade AS 'Minimum Grade', maximum_grade AS 'Maximum Grade' " &
                      "FROM course_performance_analytics"

                If search.Length > 0 Then
                    sql &= " WHERE course_code LIKE @q1 OR course_name LIKE @q1 OR instructor_name LIKE @q1"
                    parameters.Add("@q1", InputValidator.SafeLike(search))
                End If
                sql &= " ORDER BY course_code"
            Else
                sql = "SELECT student_name AS 'Student', course_code AS 'Course Code', " &
                      "course_name AS 'Course', assignment_type AS 'Assignment', " &
                      "default_weight AS 'Weight', grade AS 'Grade', " &
                      "enrollment_date AS 'Enrolled On', instructor_name AS 'Instructor' " &
                      "FROM student_transcript"

                If search.Length > 0 Then
                    sql &= " WHERE student_name LIKE @q1 OR course_code LIKE @q1 OR course_name LIKE @q1"
                    parameters.Add("@q1", InputValidator.SafeLike(search))
                End If
                sql &= " ORDER BY student_name, course_code, assignment_type"
            End If

            Try
                _currentTable = Db.FetchTable(sql, parameters)
                dgvReport.DataSource = _currentTable
                lblStatus.ForeColor = Color.SeaGreen
                lblStatus.Text = _currentTable.Rows.Count & " row(s) found."
            Catch ex As Exception
                lblStatus.ForeColor = Color.Firebrick
                lblStatus.Text = "Report failed: " & ex.Message
            End Try
        End Sub

        Private Sub BtnExport_Click(sender As Object, e As EventArgs)
            If _currentTable Is Nothing OrElse _currentTable.Rows.Count = 0 Then
                lblStatus.ForeColor = Color.Firebrick
                lblStatus.Text = "Generate a report with rows before exporting."
                Return
            End If

            Using dialog As New SaveFileDialog()
                dialog.Filter = "Excel workbook (*.xls)|*.xls|CSV files (*.csv)|*.csv|" &
                                "PDF document (*.pdf)|*.pdf|Text files (*.txt)|*.txt"
                dialog.FileName = If(cboReportType.SelectedIndex = 1,
                                     "course_analytics.csv", "student_transcript.csv")

                If dialog.ShowDialog() = DialogResult.OK Then
                    Try
                        ExportHelper.ExportTable(_currentTable, dialog.FileName)
                        lblStatus.ForeColor = Color.SeaGreen
                        lblStatus.Text = "Exported " & _currentTable.Rows.Count & " row(s) to " &
                                         dialog.FileName
                        AppLogger.Log("INFO", "Report exported: " & dialog.FileName)
                    Catch ex As Exception
                        lblStatus.ForeColor = Color.Firebrick
                        lblStatus.Text = "Export failed: " & ex.Message
                        AppLogger.LogException("report export", ex)
                    End Try
                End If
            End Using
        End Sub

        Private Function EscapeCsv(value As String) As String
            If value Is Nothing Then Return ""
            If value.Contains(",") OrElse value.Contains("""") OrElse value.Contains(vbCrLf) Then
                Return """" & value.Replace("""", """""") & """"
            End If
            Return value
        End Function

    End Class

End Namespace
