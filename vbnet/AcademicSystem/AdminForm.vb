Imports System.Data
Imports MySqlConnector

Namespace AcademicSystem

    <System.ComponentModel.DesignerCategory("Form")>
    Partial Public Class AdminForm
        Inherits Form

        Public Sub New()
            InitializeComponent()

            AddHandler btnCreateTeacher.Click, AddressOf BtnCreateTeacher_Click
            AddHandler btnCreateStudent.Click, AddressOf BtnCreateStudent_Click
            AddHandler btnResetPassword.Click, AddressOf BtnResetPassword_Click
            AddHandler btnLogout.Click,
                Sub(sender As Object, e As EventArgs)
                    If TypeOf MdiParent Is MainMdiForm Then MdiParent.Close() Else Close()
                End Sub
            AddHandler btnReports.Click, AddressOf BtnReports_Click
            AddHandler btnAuditLog.Click, AddressOf BtnAuditLog_Click
            AddHandler btnBackup.Click, AddressOf BtnBackup_Click
            AddHandler btnCreateCourse.Click, AddressOf BtnCreateCourse_Click
            AddHandler btnAddType.Click, AddressOf BtnAddType_Click
            AddHandler btnEditType.Click, AddressOf BtnEditType_Click
            AddHandler btnDeleteType.Click, AddressOf BtnDeleteType_Click
            AddHandler btnEnroll.Click, AddressOf BtnEnroll_Click
            AddHandler btnBulkEnroll.Click, AddressOf BtnBulkEnroll_Click
            AddHandler dgvAssignmentTypes.SelectionChanged, AddressOf DgvAssignmentTypes_SelectionChanged
            AddHandler btnGradeSearch.Click, AddressOf BtnGradeSearch_Click
            AddHandler txtGradeSearch.KeyDown, AddressOf TxtGradeSearch_KeyDown

            Dim designTime As Boolean = (System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime)

            If (Not designTime) AndAlso Session.RoleId <> 1 Then
                Throw New UnauthorizedAccessException("Admin role is required to open this form.")
            End If

            Text = If(designTime, "Admin Management (Design Preview)", "Admin Management - " & Session.Username)

            If Not designTime Then
                SessionUi.AddStatusBar(Me)
                RefreshGrids()
                AdvancedUi.ApplyGridFeatures(dgvUsers)
                AdvancedUi.ApplyGridFeatures(dgvInstructors)
                AdvancedUi.ApplyGridFeatures(dgvStudents)
                AdvancedUi.ApplyGridFeatures(dgvAssignmentTypes)
                AdvancedUi.ApplyGridFeatures(dgvAllGrades)
                AdvancedUi.ApplyTooltips(Me)
            End If
        End Sub

        Private Sub RefreshGrids()
            dgvUsers.DataSource = Db.FetchTable(
                "SELECT u.user_id AS 'User ID', u.username AS Username, r.role_name AS Role " &
                "FROM users u JOIN roles r ON r.role_id = u.role_id ORDER BY u.user_id")

            dgvInstructors.DataSource = Db.FetchTable(
                "SELECT i.instructor_id AS 'Instructor ID', i.user_id AS 'User ID', " &
                "i.first_name AS 'First Name', i.last_name AS 'Last Name', i.department AS Department " &
                "FROM instructors i ORDER BY i.instructor_id")

            dgvStudents.DataSource = Db.FetchTable(
                "SELECT s.student_id AS 'Student ID', s.user_id AS 'User ID', " &
                "s.first_name AS 'First Name', s.last_name AS 'Last Name', s.date_of_birth AS 'Birth Date' " &
                "FROM students s ORDER BY s.student_id")

            RefreshGradebook()
        End Sub

        Private Sub RefreshGradebook()
            dgvAssignmentTypes.DataSource = Db.FetchTable(
                "SELECT type_id AS 'Type ID', type_name AS 'Assignment Type', " &
                "default_weight AS 'Weight %' FROM assignment_types ORDER BY type_id")

            dgvAllGrades.DataSource = FetchGrades("")

            Dim students = Db.FetchTable(
                "SELECT student_id AS Value, CONCAT(first_name, ' ', last_name) AS Display " &
                "FROM students ORDER BY Display")
            cboEnrollStudent.DataSource = students
            cboEnrollStudent.DisplayMember = "Display"
            cboEnrollStudent.ValueMember = "Value"

            Dim courses = Db.FetchTable(
                "SELECT course_id AS Value, CONCAT(course_code, ' - ', course_name) AS Display " &
                "FROM courses ORDER BY course_code")
            cboEnrollCourse.DataSource = courses
            cboEnrollCourse.DisplayMember = "Display"
            cboEnrollCourse.ValueMember = "Value"

            Dim instructors = Db.FetchTable(
                "SELECT instructor_id AS Value, CONCAT(first_name, ' ', last_name) AS Display " &
                "FROM instructors ORDER BY Display")
            cboCourseInstructor.DataSource = instructors
            cboCourseInstructor.DisplayMember = "Display"
            cboCourseInstructor.ValueMember = "Value"
        End Sub

        Private Function FetchGrades(search As String) As DataTable
            Dim sql As String =
                "SELECT g.student_id AS 'Student ID', " &
                "CONCAT(s.first_name, ' ', s.last_name) AS 'Student', " &
                "g.course_id AS 'Course ID', c.course_code AS 'Course', " &
                "at.type_name AS 'Assignment', g.grade AS 'Grade', " &
                "g.date_recorded AS 'Recorded' " &
                "FROM grades g JOIN students s ON s.student_id = g.student_id " &
                "JOIN courses c ON c.course_id = g.course_id " &
                "JOIN assignment_types at ON at.type_id = g.type_id"

            Dim parameters As New Dictionary(Of String, Object)()
            If search.Trim().Length > 0 Then
                sql &= " WHERE s.first_name LIKE @q1 OR s.last_name LIKE @q1 " &
                       "OR c.course_code LIKE @q1 OR at.type_name LIKE @q1"
                parameters.Add("@q1", InputValidator.SafeLike(search))
            End If
            sql &= " ORDER BY g.student_id, c.course_code, at.type_id"

            Return Db.FetchTable(sql, parameters)
        End Function

        Private Sub BtnGradeSearch_Click(sender As Object, e As EventArgs)
            dgvAllGrades.DataSource = FetchGrades(txtGradeSearch.Text)
            lblTypeStatus.ForeColor = Color.SeaGreen
            lblTypeStatus.Text = "Grade search complete."
        End Sub

        Private Sub TxtGradeSearch_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                BtnGradeSearch_Click(sender, EventArgs.Empty)
                e.Handled = True
            End If
        End Sub

        Private Sub DgvAssignmentTypes_SelectionChanged(sender As Object, e As EventArgs)
            If dgvAssignmentTypes.CurrentRow Is Nothing Then Return
            Dim row As DataGridViewRow = dgvAssignmentTypes.CurrentRow
            If row.IsNewRow Then Return
            txtTypeName.Text = Convert.ToString(row.Cells("Assignment Type").Value)
            txtTypeWeight.Text = Convert.ToString(row.Cells("Weight %").Value)
        End Sub

        Private Sub BtnAddType_Click(sender As Object, e As EventArgs)
            lblTypeStatus.ForeColor = Color.Firebrick

            Dim name As String = txtTypeName.Text.Trim()
            Dim weight As Decimal
            If name.Length = 0 OrElse Not Decimal.TryParse(txtTypeWeight.Text.Trim(), weight) Then
                lblTypeStatus.Text = "Enter a type name and a numeric weight."
                Return
            End If

            Try
                Db.Execute(
                    "INSERT INTO assignment_types (type_name, default_weight) VALUES (@name, @weight)",
                    New Dictionary(Of String, Object) From {
                        {"@name", name}, {"@weight", weight}
                    })
                lblTypeStatus.ForeColor = Color.SeaGreen
                lblTypeStatus.Text = "Assignment type '" & name & "' added."
                txtTypeName.Clear()
                txtTypeWeight.Clear()
                RefreshGradebook()
            Catch ex As Exception
                lblTypeStatus.Text = "Could not add type: " & ex.Message
            End Try
        End Sub

        Private Sub BtnEditType_Click(sender As Object, e As EventArgs)
            If dgvAssignmentTypes.CurrentRow Is Nothing Then
                lblTypeStatus.Text = "Select an assignment type to edit."
                Return
            End If

            Dim typeId As Integer = Convert.ToInt32(dgvAssignmentTypes.CurrentRow.Cells("Type ID").Value)
            Dim name As String = txtTypeName.Text.Trim()
            Dim weight As Decimal
            If name.Length = 0 OrElse Not Decimal.TryParse(txtTypeWeight.Text.Trim(), weight) Then
                lblTypeStatus.Text = "Enter a type name and a numeric weight."
                Return
            End If

            Try
                Db.Execute(
                    "UPDATE assignment_types SET type_name = @name, default_weight = @weight " &
                    "WHERE type_id = @id",
                    New Dictionary(Of String, Object) From {
                        {"@name", name}, {"@weight", weight}, {"@id", typeId}
                    })
                lblTypeStatus.ForeColor = Color.SeaGreen
                lblTypeStatus.Text = "Assignment type updated."
                RefreshGradebook()
            Catch ex As Exception
                lblTypeStatus.ForeColor = Color.Firebrick
                lblTypeStatus.Text = "Could not edit type: " & ex.Message
            End Try
        End Sub

        Private Sub BtnDeleteType_Click(sender As Object, e As EventArgs)
            If dgvAssignmentTypes.CurrentRow Is Nothing Then
                lblTypeStatus.Text = "Select an assignment type to delete."
                Return
            End If

            Dim typeId As Integer = Convert.ToInt32(dgvAssignmentTypes.CurrentRow.Cells("Type ID").Value)
            Dim name As String = Convert.ToString(dgvAssignmentTypes.CurrentRow.Cells("Assignment Type").Value)

            Try
                Db.Execute("DELETE FROM assignment_types WHERE type_id = @id",
                           New Dictionary(Of String, Object) From {{"@id", typeId}})
                lblTypeStatus.ForeColor = Color.SeaGreen
                lblTypeStatus.Text = "Assignment type '" & name & "' deleted."
                txtTypeName.Clear()
                txtTypeWeight.Clear()
                RefreshGradebook()
            Catch ex As Exception
                lblTypeStatus.ForeColor = Color.Firebrick
                lblTypeStatus.Text = "Could not delete type (grades may reference it): " & ex.Message
            End Try
        End Sub

        Private Sub BtnEnroll_Click(sender As Object, e As EventArgs)
            lblEnrollStatus.ForeColor = Color.Firebrick

            If cboEnrollStudent.SelectedValue Is Nothing OrElse cboEnrollCourse.SelectedValue Is Nothing Then
                lblEnrollStatus.Text = "Select a student and a course."
                Return
            End If

            Dim message As String = CallEnrollProcedure(
                Convert.ToInt32(cboEnrollStudent.SelectedValue),
                Convert.ToInt32(cboEnrollCourse.SelectedValue))

            lblEnrollStatus.ForeColor = If(message.StartsWith("Error"), Color.Firebrick, Color.SeaGreen)
            lblEnrollStatus.Text = message
            RefreshGradebook()
        End Sub

        Private Function CallEnrollProcedure(studentId As Integer, courseId As Integer) As String
            Using connection = Db.GetConnection()
                Using command As New MySqlCommand("sp_EnrollStudent", connection)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.AddWithValue("@p_student_id", studentId)
                    command.Parameters.AddWithValue("@p_course_id", courseId)

                    Dim messageParam As New MySqlParameter("@p_message", MySqlDbType.VarChar, 255)
                    messageParam.Direction = ParameterDirection.Output
                    command.Parameters.Add(messageParam)

                    connection.Open()
                    command.ExecuteNonQuery()
                    Return Convert.ToString(messageParam.Value)
                End Using
            End Using
        End Function

        Private Sub BtnBulkEnroll_Click(sender As Object, e As EventArgs)
            If cboEnrollStudent.SelectedValue Is Nothing Then
                lblEnrollStatus.Text = "Select a student for bulk enrollment."
                Return
            End If

            Dim studentId As Integer = Convert.ToInt32(cboEnrollStudent.SelectedValue)
            Dim succeeded As Integer = 0
            Dim skipped As Integer = 0

            Try
                Dim courses = Db.FetchTable("SELECT course_id FROM courses ORDER BY course_id")
                For Each row As DataRow In courses.Rows
                    Dim result As String = CallEnrollProcedure(
                        studentId, Convert.ToInt32(row("course_id")))
                    If result.StartsWith("Success") Then
                        succeeded += 1
                    Else
                        skipped += 1
                    End If
                Next

                lblEnrollStatus.ForeColor = Color.SeaGreen
                lblEnrollStatus.Text = "Bulk enrollment complete: " & succeeded &
                                       " enrolled, " & skipped & " skipped (already enrolled)."
                RefreshGradebook()
            Catch ex As Exception
                lblEnrollStatus.ForeColor = Color.Firebrick
                lblEnrollStatus.Text = "Bulk enrollment failed: " & ex.Message
            End Try
        End Sub

        Private Sub BtnReports_Click(sender As Object, e As EventArgs)
            Dim reports As New ReportsForm()
            AddHandler reports.FormClosed,
                Sub(sender2 As Object, e2 As FormClosedEventArgs) Show()
            Hide()
            reports.Show()
        End Sub

        Private Sub BtnAuditLog_Click(sender As Object, e As EventArgs)
            If Session.RoleId <> 1 Then Return
            Dim viewer As New AuditLogForm()
            AddHandler viewer.FormClosed,
                Sub(sender2 As Object, e2 As FormClosedEventArgs) Show()
            Hide()
            viewer.Show()
        End Sub

        Private Sub BtnBackup_Click(sender As Object, e As EventArgs)
            If Session.RoleId <> 1 Then Return
            Dim utility As New BackupForm()
            AddHandler utility.FormClosed,
                Sub(sender2 As Object, e2 As FormClosedEventArgs) Show()
            Hide()
            utility.Show()
        End Sub

        Private Sub BtnCreateCourse_Click(sender As Object, e As EventArgs)
            lblCourseStatus.Text = ""

            Dim code As String = InputValidator.Sanitize(txtCourseCode.Text).ToUpperInvariant()
            Dim name As String = InputValidator.Sanitize(txtCourseName.Text)

            If Not InputValidator.IsValidCourseCode(code) Then
                lblCourseStatus.ForeColor = Color.Firebrick
                lblCourseStatus.Text = "Invalid course code. Use 2-4 letters followed by 3 digits (example: CS101)."
                Return
            End If
            If Not InputValidator.IsUniqueCourseCode(code) Then
                lblCourseStatus.ForeColor = Color.Firebrick
                lblCourseStatus.Text = "Course code '" & code & "' already exists. Codes must be unique."
                Return
            End If
            Dim validationMessage As String = ""
            If Not InputValidator.Required(name, "Course name", validationMessage) Then
                lblCourseStatus.ForeColor = Color.Firebrick
                lblCourseStatus.Text = validationMessage
                Return
            End If
            If cboCourseInstructor.SelectedValue Is Nothing Then
                lblCourseStatus.ForeColor = Color.Firebrick
                lblCourseStatus.Text = "Select an instructor."
                Return
            End If

            Try
                Db.Execute(
                    "INSERT INTO courses (course_code, course_name, instructor_id) " &
                    "VALUES (@code, @name, @instructorId)",
                    New Dictionary(Of String, Object) From {
                        {"@code", code},
                        {"@name", name},
                        {"@instructorId", Convert.ToInt32(cboCourseInstructor.SelectedValue)}
                    })

                AuditLogger.Log("INSERT", "courses", code, "", name)
                lblCourseStatus.ForeColor = Color.SeaGreen
                lblCourseStatus.Text = "Course created: " & code & " - " & name
                txtCourseCode.Clear()
                txtCourseName.Clear()
                RefreshGrids()
            Catch ex As Exception
                lblCourseStatus.ForeColor = Color.Firebrick
                lblCourseStatus.Text = "Could not create course: " & ex.Message
            End Try
        End Sub

        Private Sub BtnCreateTeacher_Click(sender As Object, e As EventArgs)
            lblAddStatus.Text = ""

            Dim username As String = InputValidator.Sanitize(txtNewUsername.Text)
            Dim password As String = txtNewPassword.Text
            Dim first As String = InputValidator.Sanitize(txtFirstName.Text)
            Dim last As String = InputValidator.Sanitize(txtLastName.Text)
            Dim department As String = InputValidator.Sanitize(txtDepartment.Text)
            Dim email As String = InputValidator.Sanitize(txtNewEmail.Text)

            If Not InputValidator.IsValidUsername(username) Then
                lblAddStatus.ForeColor = Color.Firebrick
                lblAddStatus.Text = "Invalid username format (letters, digits, dot, dash, underscore; 3-30 characters)."
                Return
            End If
            If Not InputValidator.IsUniqueUsername(username) Then
                lblAddStatus.ForeColor = Color.Firebrick
                lblAddStatus.Text = "Username '" & username & "' already exists."
                Return
            End If
            If Not InputValidator.IsStrongPassword(password) Then
                lblAddStatus.ForeColor = Color.Firebrick
                lblAddStatus.Text = "Weak password: use at least 8 characters with upper/lower case, a digit and a symbol."
                Return
            End If
            If email.Length > 0 AndAlso Not InputValidator.IsValidEmail(email) Then
                lblAddStatus.ForeColor = Color.Firebrick
                lblAddStatus.Text = "Invalid email format. Example: name@example.com"
                Return
            End If
            If String.IsNullOrWhiteSpace(first) OrElse String.IsNullOrWhiteSpace(last) Then
                lblAddStatus.ForeColor = Color.Firebrick
                lblAddStatus.Text = "First name and last name are required."
                Return
            End If

            Dim hash As String = PasswordHasher.HashPassword(password)

            Try
                Using connection = Db.GetConnection()
                    connection.Open()
                    Using transaction = connection.BeginTransaction()
                        Try
                            Dim userId As Integer

                            ' Step 1: create the login account with the Teacher role (role_id = 2).
                            Using command As New MySqlCommand(
                                "INSERT INTO users (username, password_hash, email, role_id) " &
                                "VALUES (@username, @hash, @email, 2); SELECT LAST_INSERT_ID();",
                                connection, transaction)
                                command.Parameters.AddWithValue("@username", username)
                                command.Parameters.AddWithValue("@hash", hash)
                                command.Parameters.AddWithValue("@email", If(email, DBNull.Value))
                                userId = Convert.ToInt32(command.ExecuteScalar())
                            End Using

                            ' Step 2: create the matching Instructor profile using that user_id.
                            Using command As New MySqlCommand(
                                "INSERT INTO instructors (user_id, first_name, last_name, department) " &
                                "VALUES (@userId, @first, @last, @department)",
                                connection, transaction)
                                command.Parameters.AddWithValue("@userId", userId)
                                command.Parameters.AddWithValue("@first", first)
                                command.Parameters.AddWithValue("@last", last)
                                command.Parameters.AddWithValue("@department", If(department, ""))
                                command.ExecuteNonQuery()
                            End Using

                            transaction.Commit()
                            lblAddStatus.ForeColor = Color.SeaGreen
                            lblAddStatus.Text = "Teacher created: " & username &
                                                " (user_id = " & userId & ")."

                        Catch ex As Exception
                            transaction.Rollback()
                            Throw
                        End Try
                    End Using
                End Using

                txtNewUsername.Clear()
                txtNewPassword.Clear()
                txtNewEmail.Clear()
                txtFirstName.Clear()
                txtLastName.Clear()
                txtDepartment.Clear()
                RefreshGrids()

            Catch ex As Exception
                lblAddStatus.ForeColor = Color.Firebrick
                lblAddStatus.Text = "Could not create teacher: " & ex.Message
            End Try
        End Sub

        Private Sub BtnCreateStudent_Click(sender As Object, e As EventArgs)
            lblStudentStatus.Text = ""

            Dim username As String = InputValidator.Sanitize(txtStudentUsername.Text)
            Dim password As String = txtStudentPassword.Text
            Dim email As String = InputValidator.Sanitize(txtStudentEmail.Text)
            Dim first As String = InputValidator.Sanitize(txtStudentFirstName.Text)
            Dim last As String = InputValidator.Sanitize(txtStudentLastName.Text)
            Dim birth As DateTime = dtpStudentBirthDate.Value.Date

            If Not InputValidator.IsValidUsername(username) Then
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "Invalid username format (letters, digits, dot, dash, underscore; 3-30 characters)."
                Return
            End If
            If Not InputValidator.IsUniqueUsername(username) Then
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "Username '" & username & "' already exists."
                Return
            End If
            If Not InputValidator.IsStrongPassword(password) Then
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "Weak password: use at least 8 characters with upper/lower case, a digit and a symbol."
                Return
            End If
            If email.Length > 0 AndAlso Not InputValidator.IsValidEmail(email) Then
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "Invalid email format. Example: name@example.com"
                Return
            End If
            If String.IsNullOrWhiteSpace(first) OrElse String.IsNullOrWhiteSpace(last) Then
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "First name and last name are required."
                Return
            End If
            If birth > DateTime.Today Then
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "Birth date cannot be in the future."
                Return
            End If

            Dim hash As String = PasswordHasher.HashPassword(password)

            Try
                Using connection = Db.GetConnection()
                    connection.Open()
                    Using transaction = connection.BeginTransaction()
                        Try
                            Dim userId As Integer

                            ' Step 1: create the login account with the Student role (role_id = 3).
                            Using command As New MySqlCommand(
                                "INSERT INTO users (username, password_hash, email, role_id) " &
                                "VALUES (@username, @hash, @email, 3); SELECT LAST_INSERT_ID();",
                                connection, transaction)
                                command.Parameters.AddWithValue("@username", username)
                                command.Parameters.AddWithValue("@hash", hash)
                                command.Parameters.AddWithValue("@email", If(email, DBNull.Value))
                                userId = Convert.ToInt32(command.ExecuteScalar())
                            End Using

                            ' Step 2: create the matching Student profile using that user_id.
                            Using command As New MySqlCommand(
                                "INSERT INTO students (user_id, first_name, last_name, date_of_birth) " &
                                "VALUES (@userId, @first, @last, @birth)",
                                connection, transaction)
                                command.Parameters.AddWithValue("@userId", userId)
                                command.Parameters.AddWithValue("@first", first)
                                command.Parameters.AddWithValue("@last", last)
                                command.Parameters.AddWithValue("@birth", birth)
                                command.ExecuteNonQuery()
                            End Using

                            transaction.Commit()
                            lblStudentStatus.ForeColor = Color.SeaGreen
                            lblStudentStatus.Text = "Student created: " & username &
                                                    " (user_id = " & userId & ")."

                        Catch ex As Exception
                            transaction.Rollback()
                            Throw
                        End Try
                    End Using
                End Using

                AuditLogger.Log("INSERT", "students", username, "", first & " " & last)

                txtStudentUsername.Clear()
                txtStudentPassword.Clear()
                txtStudentEmail.Clear()
                txtStudentFirstName.Clear()
                txtStudentLastName.Clear()
                dtpStudentBirthDate.Value = New DateTime(2005, 1, 1)
                RefreshGrids()

            Catch ex As Exception
                lblStudentStatus.ForeColor = Color.Firebrick
                lblStudentStatus.Text = "Could not create student: " & ex.Message
            End Try
        End Sub

        Private Sub BtnResetPassword_Click(sender As Object, e As EventArgs)
            lblResetStatus.Text = ""

            Dim username As String = txtResetUsername.Text.Trim()
            Dim password As String = txtResetPassword.Text

            If String.IsNullOrWhiteSpace(username) OrElse password.Length = 0 Then
                lblResetStatus.ForeColor = Color.Firebrick
                lblResetStatus.Text = "Enter a username and a new password."
                Return
            End If

            Try
                Dim found = Db.FetchTable(
                    "SELECT user_id FROM users WHERE username = @username",
                    New Dictionary(Of String, Object) From {{"@username", username}})

                If found.Rows.Count = 0 Then
                    lblResetStatus.ForeColor = Color.Firebrick
                    lblResetStatus.Text = "No user named '" & username & "' exists."
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(found.Rows(0)("user_id"))
                Db.Execute("UPDATE users SET password_hash = @hash WHERE user_id = @userId",
                           New Dictionary(Of String, Object) From {
                               {"@hash", PasswordHasher.HashPassword(password)},
                               {"@userId", userId}
                           })

                txtResetUsername.Clear()
                txtResetPassword.Clear()
                lblResetStatus.ForeColor = Color.SeaGreen
                lblResetStatus.Text = "Password reset for " & username & "."
                RefreshGrids()

            Catch ex As Exception
                lblResetStatus.ForeColor = Color.Firebrick
                lblResetStatus.Text = "Could not reset password: " & ex.Message
            End Try
        End Sub

    End Class

End Namespace
