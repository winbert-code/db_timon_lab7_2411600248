Imports System.Data

Namespace AcademicSystem

    ' Multi-criteria search across users, students, courses, grades and audit.
    Public Class SearchAdvancedForm
        Inherits Form

        Private _grid As DataGridView
        Private _status As Label

        Public Sub New()
            Text = "Advanced Search"
            ClientSize = New Size(1140, 600)
            StartPosition = FormStartPosition.CenterScreen
            BackColor = Color.FromArgb(248, 250, 252)

            Dim header As New Label() With {
                .Text = "Multi-criteria search across DB_Timon", .Dock = DockStyle.Top, .Height = 52,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121), .Padding = New Padding(18, 12, 0, 0)
            }

            Dim filters As New Panel() With {.Dock = DockStyle.Top, .Height = 92, .BackColor = Color.White}
            Dim lblKeyword As New Label() With {.Text = "Keyword:", .Location = New Point(20, 20), .Size = New Size(80, 24)}
            Dim txtKeyword As New TextBox() With {.Name = "txtSearch", .Location = New Point(100, 16), .Size = New Size(320, 28)}
            Dim lblCategory As New Label() With {.Text = "Category:", .Location = New Point(450, 20), .Size = New Size(80, 24)}
            Dim cboCategory As New ComboBox() With {
                .Location = New Point(530, 16), .Size = New Size(220, 28),
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            cboCategory.Items.AddRange(New Object() {"All data", "Students", "Courses", "Teachers", "Grades", "Audit"})
            cboCategory.SelectedIndex = 0

            Dim btnSearch As New Button() With {
                .Text = "Search", .Location = New Point(790, 14), .Size = New Size(130, 32)
            }
            AddHandler btnSearch.Click, Sub(sender, e) RunSearch(txtKeyword.Text, cboCategory.Text)
            AddHandler txtKeyword.KeyDown,
                Sub(sender As Object, e As KeyEventArgs)
                    If e.KeyCode = Keys.Enter Then RunSearch(txtKeyword.Text, cboCategory.Text) : e.Handled = True
                End Sub

            Dim hint As New Label() With {
                .Text = "Search matches names, codes, usernames and audit actions. Press Enter to run.",
                .Location = New Point(20, 58), .Size = New Size(900, 22), .ForeColor = Color.DimGray
            }
            filters.Controls.AddRange(New Control() {lblKeyword, txtKeyword, lblCategory, cboCategory, btnSearch, hint})

            _grid = New DataGridView() With {.Dock = DockStyle.Fill}
            AdvancedUi.ApplyGridFeatures(_grid)

            Dim bottom As New Panel() With {.Dock = DockStyle.Bottom, .Height = 58, .Padding = New Padding(18, 14, 18, 6)}
            _status = New Label() With {.Dock = DockStyle.Fill, .Text = "Ready.", .ForeColor = Color.SeaGreen}
            Dim btnClose As New Button() With {
                .Text = "Close", .Dock = DockStyle.Right, .Width = 120, .DialogResult = DialogResult.OK
            }
            bottom.Controls.AddRange(New Control() {_status, btnClose})
            AcceptButton = btnClose

            Controls.AddRange(New Control() {_grid, filters, header, bottom})
            SessionUi.AddStatusBar(Me)
            AdvancedUi.ApplyTooltips(Me)
            RunSearch("", "All data")
        End Sub

        Private Sub RunSearch(keyword As String, category As String)
            Try
                Dim q As String = InputValidator.SafeLike(keyword)
                Dim parameters As New Dictionary(Of String, Object) From {{"@q", q}}
                Dim sql As String

                Select Case category
                    Case "Students"
                        sql = "SELECT 'Student' AS Category, CONCAT(first_name,' ',last_name) AS Name, " &
                              "CONCAT('Student ID ', student_id) AS Detail FROM students " &
                              "WHERE first_name LIKE @q OR last_name LIKE @q ORDER BY Name"
                    Case "Courses"
                        sql = "SELECT 'Course' AS Category, course_code AS Name, course_name AS Detail " &
                              "FROM courses WHERE course_code LIKE @q OR course_name LIKE @q ORDER BY Name"
                    Case "Teachers"
                        sql = "SELECT 'Teacher' AS Category, u.username AS Name, " &
                              "CONCAT(i.first_name,' ',i.last_name,' - ',i.department) AS Detail " &
                              "FROM users u JOIN instructors i ON i.user_id = u.user_id " &
                              "WHERE u.username LIKE @q OR i.first_name LIKE @q OR i.last_name LIKE @q ORDER BY Name"
                    Case "Grades"
                        sql = "SELECT 'Grade' AS Category, CONCAT(s.first_name,' ',s.last_name) AS Name, " &
                              "CONCAT(c.course_code,' - ',at.type_name,' = ',g.grade) AS Detail " &
                              "FROM grades g JOIN students s ON s.student_id = g.student_id " &
                              "JOIN courses c ON c.course_id = g.course_id " &
                              "JOIN assignment_types at ON at.type_id = g.type_id " &
                              "WHERE c.course_code LIKE @q OR s.first_name LIKE @q OR s.last_name LIKE @q " &
                              "ORDER BY Name"
                    Case "Audit"
                        sql = "SELECT 'Audit' AS Category, app_user AS Name, " &
                              "CONCAT(action_type,' on ',table_name,' at ',event_time) AS Detail " &
                              "FROM audit_log WHERE app_user LIKE @q OR action_type LIKE @q OR table_name LIKE @q " &
                              "ORDER BY event_time DESC LIMIT 200"
                    Case Else
                        sql =
                            "SELECT 'Student' AS Category, CONCAT(first_name,' ',last_name) AS Name, " &
                            "CONCAT('Student ID ', student_id) AS Detail FROM students WHERE first_name LIKE @q OR last_name LIKE @q " &
                            "UNION ALL SELECT 'Course', course_code, course_name FROM courses WHERE course_code LIKE @q OR course_name LIKE @q " &
                            "UNION ALL SELECT 'Teacher', u.username, CONCAT(i.first_name,' ',i.last_name) FROM users u " &
                            "JOIN instructors i ON i.user_id = u.user_id WHERE u.username LIKE @q OR i.first_name LIKE @q " &
                            "UNION ALL SELECT 'Grade', CONCAT(s.first_name,' ',s.last_name), " &
                            "CONCAT(c.course_code,' = ',g.grade) FROM grades g JOIN students s ON s.student_id = g.student_id " &
                            "JOIN courses c ON c.course_id = g.course_id WHERE c.course_code LIKE @q OR s.first_name LIKE @q " &
                            "ORDER BY Category, Name"
                End Select

                Dim table As DataTable = Db.FetchTable(sql, parameters)
                _grid.DataSource = table
                AdvancedUi.ApplyGridFeatures(_grid)
                _status.ForeColor = Color.SeaGreen
                _status.Text = table.Rows.Count & " result(s) for '" & keyword & "' in " & category & "."
                AppLogger.Log("INFO", "Advanced search: " & category & " / " & keyword & " -> " & table.Rows.Count & " rows")
            Catch ex As Exception
                _status.ForeColor = Color.Firebrick
                _status.Text = "Search failed: " & ex.Message
                AppLogger.LogException("advanced search", ex)
            End Try
        End Sub

    End Class

End Namespace
