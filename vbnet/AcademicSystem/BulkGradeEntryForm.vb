Imports System.Data
Imports MySqlConnector

Namespace AcademicSystem

    ' Bulk grade entry with progress feedback and simple undo/redo snapshots.
    Public Class BulkGradeEntryForm
        Inherits Form

        Private _course As ComboBox
        Private _type As ComboBox
        Private _grid As DataGridView
        Private _progress As ProgressBar
        Private _status As Label
        Private _undo As New Stack(Of DataTable)()
        Private _redo As New Stack(Of DataTable)()

        Public Sub New()
            Text = "Bulk Grade Entry"
            ClientSize = New Size(1100, 600)
            StartPosition = FormStartPosition.CenterScreen
            BackColor = Color.FromArgb(248, 250, 252)

            Dim header As New Label() With {
                .Text = "Bulk grade entry - edit the New Grade column, then save all rows",
                .Dock = DockStyle.Top, .Height = 54,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121), .Padding = New Padding(18, 13, 0, 0)
            }

            Dim filters As New Panel() With {.Dock = DockStyle.Top, .Height = 70, .BackColor = Color.White}
            filters.Controls.Add(New Label() With {.Text = "Course:", .Location = New Point(20, 22), .Size = New Size(70, 24)})
            _course = New ComboBox() With {
                .Location = New Point(95, 18), .Size = New Size(360, 30),
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            filters.Controls.Add(New Label() With {.Text = "Assignment:", .Location = New Point(490, 22), .Size = New Size(90, 24)})
            _type = New ComboBox() With {
                .Location = New Point(585, 18), .Size = New Size(240, 30),
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            filters.Controls.AddRange(New Control() {_course, _type})

            _grid = New DataGridView() With {.Dock = DockStyle.Fill}
            _grid.AllowUserToAddRows = False
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 78, 121)
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            _grid.EnableHeadersVisualStyles = False

            Dim bottom As New Panel() With {.Dock = DockStyle.Bottom, .Height = 96, .Padding = New Padding(16, 10, 16, 6)}
            _progress = New ProgressBar() With {.Dock = DockStyle.Top, .Height = 18, .Maximum = 100}
            _status = New Label() With {
                .Text = "Select a course and assignment type.", .Dock = DockStyle.Fill,
                .ForeColor = Color.SeaGreen, .Font = New Font("Segoe UI", 10)
            }
            Dim btnSave As New Button() With {.Text = "Save all grades", .Location = New Point(18, 34), .Size = New Size(170, 38)}
            Dim btnUndo As New Button() With {.Text = "Undo", .Location = New Point(202, 34), .Size = New Size(100, 38)}
            Dim btnRedo As New Button() With {.Text = "Redo", .Location = New Point(312, 34), .Size = New Size(100, 38)}
            Dim btnClose As New Button() With {
                .Text = "Close", .Location = New Point(950, 34), .Size = New Size(140, 38),
                .DialogResult = DialogResult.OK
            }
            AddHandler btnSave.Click, Sub(sender, e) SaveAll()
            AddHandler btnUndo.Click, Sub(sender, e) Restore(_undo, _redo, "Undo")
            AddHandler btnRedo.Click, Sub(sender, e) Restore(_redo, _undo, "Redo")
            AddHandler _course.SelectedIndexChanged, Sub(sender, e) LoadStudents()
            AddHandler _type.SelectedIndexChanged, Sub(sender, e) LoadStudents()
            bottom.Controls.AddRange(New Control() {_status, _progress, btnSave, btnUndo, btnRedo, btnClose})
            AcceptButton = btnClose

            Controls.AddRange(New Control() {_grid, filters, header, bottom})
            SessionUi.AddStatusBar(Me)
            AdvancedUi.ApplyTooltips(Me)

            LoadCombos()
        End Sub

        Private Sub LoadCombos()
            Dim courses = Db.FetchTable(
                "SELECT course_id AS Value, CONCAT(course_code,' - ',course_name) AS Display " &
                "FROM courses ORDER BY course_code")
            _course.DataSource = courses
            _course.DisplayMember = "Display"
            _course.ValueMember = "Value"

            Dim types = Db.FetchTable(
                "SELECT type_id AS Value, type_name AS Display FROM assignment_types ORDER BY type_id")
            _type.DataSource = types
            _type.DisplayMember = "Display"
            _type.ValueMember = "Value"
        End Sub

        Private Function SelectedId(combo As ComboBox) As Integer
            Dim value As Object = combo.SelectedValue
            If value IsNot Nothing AndAlso Not TypeOf value Is DataRowView Then
                Dim parsed As Integer
                If Integer.TryParse(Convert.ToString(value), parsed) Then Return parsed
            End If

            Dim row As DataRowView = TryCast(combo.SelectedItem, DataRowView)
            If row IsNot Nothing Then
                Dim parsed As Integer
                If Integer.TryParse(Convert.ToString(row("Value")), parsed) Then Return parsed
            End If
            Return 0
        End Function

        Private Sub LoadStudents()
            Dim courseId As Integer = SelectedId(_course)
            Dim typeId As Integer = SelectedId(_type)
            If courseId = 0 OrElse typeId = 0 Then Return
            Try
                Dim parameters As New Dictionary(Of String, Object) From {
                    {"@courseId", courseId},
                    {"@typeId", typeId}
                }
                Dim table As DataTable = Db.FetchTable(
                    "SELECT s.student_id AS 'Student ID', " &
                    "CONCAT(s.first_name,' ',s.last_name) AS 'Student', " &
                    "at.type_name AS 'Assignment', " &
                    "IFNULL(g.grade, '') AS 'Current Grade', " &
                    "CAST(IFNULL(g.grade, '') AS CHAR) AS 'New Grade' " &
                    "FROM enrollments e JOIN students s ON s.student_id = e.student_id " &
                    "JOIN courses c ON c.course_id = e.course_id " &
                    "JOIN assignment_types at ON at.type_id = @typeId " &
                    "LEFT JOIN grades g ON g.student_id = s.student_id AND g.course_id = c.course_id AND g.type_id = at.type_id " &
                    "WHERE e.course_id = @courseId ORDER BY s.last_name, s.first_name", parameters)

                _grid.DataSource = table
                If _grid.Columns.Contains("New Grade") Then _grid.Columns("New Grade").ReadOnly = False
                For Each column As DataGridViewColumn In _grid.Columns
                    If column.Name <> "New Grade" Then column.ReadOnly = True
                Next
                _undo.Clear()
                _redo.Clear()
                _status.ForeColor = Color.SeaGreen
                _status.Text = table.Rows.Count & " enrolled student(s) loaded. Edit New Grade, then save."
                _progress.Value = 0
            Catch ex As Exception
                _status.ForeColor = Color.Firebrick
                _status.Text = "Load failed: " & ex.Message
            End Try
        End Sub

        Private Sub SaveAll()
            Dim table As DataTable = TryCast(_grid.DataSource, DataTable)
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                _status.ForeColor = Color.Firebrick
                _status.Text = "Load students before saving."
                Return
            End If

            _undo.Push(table.Copy())
            _redo.Clear()

            Dim saved As Integer = 0
            Dim skipped As Integer = 0
            Try
                Using connection = Db.GetConnection()
                    connection.Open()
                    Using transaction = connection.BeginTransaction()
                        For index As Integer = 0 To table.Rows.Count - 1
                            Dim raw As String = Convert.ToString(table.Rows(index)("New Grade")).Trim()
                            Dim grade As Decimal
                            If raw.Length = 0 Then
                                skipped += 1
                            ElseIf Not Decimal.TryParse(raw, grade) OrElse grade < 0 OrElse grade > 100 Then
                                skipped += 1
                            Else
                                Dim parameters As New Dictionary(Of String, Object) From {
                                    {"@sid", Convert.ToInt32(table.Rows(index)("Student ID"))},
                                    {"@cid", SelectedId(_course)},
                                    {"@tid", SelectedId(_type)},
                                    {"@grade", grade}
                                }
                                Using command As New MySqlCommand(
                                    "INSERT INTO grades (student_id, course_id, type_id, grade) " &
                                    "VALUES (@sid,@cid,@tid,@grade) " &
                                    "ON DUPLICATE KEY UPDATE grade = VALUES(grade)", connection, transaction)
                                    Db.AddParameters(command, parameters)
                                    command.ExecuteNonQuery()
                                End Using
                                saved += 1
                            End If
                            _progress.Value = CInt((index + 1) * 100 / table.Rows.Count)
                            Application.DoEvents()
                        Next
                        transaction.Commit()
                    End Using
                End Using

                _status.ForeColor = Color.SeaGreen
                _status.Text = "Saved " & saved & " grade(s), skipped " & skipped & " invalid/empty row(s)."
                AuditLogger.Log("BULK_GRADE_ENTRY", "grades", Convert.ToString(SelectedId(_course)), "",
                                saved & " saved, " & skipped & " skipped")
                AppLogger.Log("INFO", "Bulk grade entry: saved=" & saved & " skipped=" & skipped)
            Catch ex As Exception
                _status.ForeColor = Color.Firebrick
                _status.Text = "Save failed; no changes were committed: " & ex.Message
                GlobalExceptionHandler.Handle("bulk grade save", ex)
            End Try
        End Sub

        Private Sub Restore(stack As Stack(Of DataTable), other As Stack(Of DataTable), action As String)
            If stack.Count = 0 Then
                _status.ForeColor = Color.Firebrick
                _status.Text = "Nothing to " & action.ToLowerInvariant() & "."
                Return
            End If
            Dim current As DataTable = TryCast(_grid.DataSource, DataTable)
            If current IsNot Nothing Then other.Push(current.Copy())
            _grid.DataSource = stack.Pop()
            _status.ForeColor = Color.SeaGreen
            _status.Text = action & " completed. Database rows are unchanged until you save again."
        End Sub

    End Class

End Namespace
