Namespace AcademicSystem

    Partial Public Class AdminForm

        Friend WithEvents dgvUsers As DataGridView
        Friend WithEvents dgvInstructors As DataGridView
        Friend WithEvents dgvStudents As DataGridView

        Friend lblAddHeader As Label
        Friend lblNewUsername As Label
        Friend lblNewPassword As Label
        Friend lblFirstName As Label
        Friend lblLastName As Label
        Friend lblDepartment As Label
        Friend lblNewEmail As Label
        Friend WithEvents txtNewEmail As TextBox
        Friend WithEvents txtNewUsername As TextBox
        Friend WithEvents txtNewPassword As TextBox
        Friend WithEvents txtFirstName As TextBox
        Friend WithEvents txtLastName As TextBox
        Friend WithEvents txtDepartment As TextBox
        Friend WithEvents btnCreateTeacher As Button
        Friend lblAddStatus As Label

        Friend studentEntryPanel As Panel
        Friend lblStudentHeader As Label
        Friend lblStudentUsername As Label
        Friend WithEvents txtStudentUsername As TextBox
        Friend lblStudentPassword As Label
        Friend WithEvents txtStudentPassword As TextBox
        Friend lblStudentEmail As Label
        Friend WithEvents txtStudentEmail As TextBox
        Friend lblStudentFirstName As Label
        Friend WithEvents txtStudentFirstName As TextBox
        Friend lblStudentLastName As Label
        Friend WithEvents txtStudentLastName As TextBox
        Friend lblStudentBirthDate As Label
        Friend WithEvents dtpStudentBirthDate As DateTimePicker
        Friend WithEvents btnCreateStudent As Button
        Friend lblStudentStatus As Label

        Friend lblResetHeader As Label
        Friend lblResetUsername As Label
        Friend lblResetPassword As Label
        Friend WithEvents txtResetUsername As TextBox
        Friend WithEvents txtResetPassword As TextBox
        Friend WithEvents btnResetPassword As Button
        Friend lblResetStatus As Label

        Friend tabControl As TabControl
        Friend pageAdd As TabPage
        Friend pageUsers As TabPage
        Friend pageInstructors As TabPage
        Friend pageStudents As TabPage
        Friend pageReset As TabPage
        Friend pageCourses As TabPage
        Friend pageGradebook As TabPage
        Friend lblCourseHeader As Label
        Friend lblCourseCode As Label
        Friend WithEvents txtCourseCode As TextBox
        Friend lblCourseName As Label
        Friend WithEvents txtCourseName As TextBox
        Friend lblCourseInstructor As Label
        Friend WithEvents cboCourseInstructor As ComboBox
        Friend WithEvents btnCreateCourse As Button
        Friend lblCourseStatus As Label
        Friend WithEvents dgvAssignmentTypes As DataGridView
        Friend WithEvents dgvAllGrades As DataGridView
        Friend lblTypeHeader As Label
        Friend lblTypeName As Label
        Friend WithEvents txtTypeName As TextBox
        Friend lblTypeWeight As Label
        Friend WithEvents txtTypeWeight As TextBox
        Friend WithEvents btnAddType As Button
        Friend WithEvents btnEditType As Button
        Friend WithEvents btnDeleteType As Button
        Friend lblTypeStatus As Label
        Friend lblEnrollHeader As Label
        Friend WithEvents cboEnrollStudent As ComboBox
        Friend WithEvents cboEnrollCourse As ComboBox
        Friend WithEvents btnEnroll As Button
        Friend WithEvents btnBulkEnroll As Button
        Friend lblEnrollStatus As Label
        Friend WithEvents txtGradeSearch As TextBox
        Friend WithEvents btnGradeSearch As Button
        Friend bottomPanel As Panel
        Friend WithEvents btnReports As Button
        Friend WithEvents btnAuditLog As Button
        Friend WithEvents btnBackup As Button
        Friend WithEvents btnLogout As Button

        Friend Sub InitializeComponent()
            dgvUsers = New DataGridView()
            dgvInstructors = New DataGridView()
            dgvStudents = New DataGridView()
            lblAddHeader = New Label()
            lblNewUsername = New Label()
            lblNewPassword = New Label()
            lblFirstName = New Label()
            lblLastName = New Label()
            lblDepartment = New Label()
            lblNewEmail = New Label()
            txtNewEmail = New TextBox()
            txtNewUsername = New TextBox()
            txtNewPassword = New TextBox()
            txtFirstName = New TextBox()
            txtLastName = New TextBox()
            txtDepartment = New TextBox()
            btnCreateTeacher = New Button()
            lblAddStatus = New Label()
            studentEntryPanel = New Panel()
            lblStudentHeader = New Label()
            lblStudentUsername = New Label()
            txtStudentUsername = New TextBox()
            lblStudentPassword = New Label()
            txtStudentPassword = New TextBox()
            lblStudentEmail = New Label()
            txtStudentEmail = New TextBox()
            lblStudentFirstName = New Label()
            txtStudentFirstName = New TextBox()
            lblStudentLastName = New Label()
            txtStudentLastName = New TextBox()
            lblStudentBirthDate = New Label()
            dtpStudentBirthDate = New DateTimePicker()
            btnCreateStudent = New Button()
            lblStudentStatus = New Label()
            lblResetHeader = New Label()
            lblResetUsername = New Label()
            lblResetPassword = New Label()
            txtResetUsername = New TextBox()
            txtResetPassword = New TextBox()
            btnResetPassword = New Button()
            lblResetStatus = New Label()
            tabControl = New TabControl()
            pageAdd = New TabPage()
            pageUsers = New TabPage()
            pageInstructors = New TabPage()
            pageStudents = New TabPage()
            pageReset = New TabPage()
            pageCourses = New TabPage()
            lblCourseHeader = New Label()
            lblCourseCode = New Label()
            txtCourseCode = New TextBox()
            lblCourseName = New Label()
            txtCourseName = New TextBox()
            lblCourseInstructor = New Label()
            cboCourseInstructor = New ComboBox()
            btnCreateCourse = New Button()
            lblCourseStatus = New Label()
            pageGradebook = New TabPage()
            lblTypeHeader = New Label()
            dgvAssignmentTypes = New DataGridView()
            lblTypeName = New Label()
            txtTypeName = New TextBox()
            lblTypeWeight = New Label()
            txtTypeWeight = New TextBox()
            btnAddType = New Button()
            btnEditType = New Button()
            btnDeleteType = New Button()
            lblTypeStatus = New Label()
            lblEnrollHeader = New Label()
            cboEnrollStudent = New ComboBox()
            cboEnrollCourse = New ComboBox()
            btnEnroll = New Button()
            btnBulkEnroll = New Button()
            lblEnrollStatus = New Label()
            txtGradeSearch = New TextBox()
            btnGradeSearch = New Button()
            dgvAllGrades = New DataGridView()
            bottomPanel = New Panel()
            btnLogout = New Button()
            btnReports = New Button()
            btnAuditLog = New Button()
            btnBackup = New Button()
            CType(dgvUsers, ComponentModel.ISupportInitialize).BeginInit()
            CType(dgvInstructors, ComponentModel.ISupportInitialize).BeginInit()
            CType(dgvStudents, ComponentModel.ISupportInitialize).BeginInit()
            tabControl.SuspendLayout()
            pageAdd.SuspendLayout()
            pageUsers.SuspendLayout()
            pageInstructors.SuspendLayout()
            pageStudents.SuspendLayout()
            studentEntryPanel.SuspendLayout()
            pageReset.SuspendLayout()
            pageCourses.SuspendLayout()
            pageGradebook.SuspendLayout()
            CType(dgvAssignmentTypes, ComponentModel.ISupportInitialize).BeginInit()
            CType(dgvAllGrades, ComponentModel.ISupportInitialize).BeginInit()
            bottomPanel.SuspendLayout()
            SuspendLayout()
            ' 
            ' dgvUsers
            ' 
            dgvUsers.AllowUserToAddRows = False
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvUsers.ColumnHeadersHeight = 34
            dgvUsers.Dock = DockStyle.Fill
            dgvUsers.Location = New Point(0, 0)
            dgvUsers.Name = "dgvUsers"
            dgvUsers.ReadOnly = True
            dgvUsers.RowHeadersWidth = 62
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvUsers.Size = New Size(192, 62)
            dgvUsers.TabIndex = 0
            ' 
            ' dgvInstructors
            ' 
            dgvInstructors.AllowUserToAddRows = False
            dgvInstructors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvInstructors.ColumnHeadersHeight = 34
            dgvInstructors.Dock = DockStyle.Fill
            dgvInstructors.Location = New Point(0, 0)
            dgvInstructors.Name = "dgvInstructors"
            dgvInstructors.ReadOnly = True
            dgvInstructors.RowHeadersWidth = 62
            dgvInstructors.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvInstructors.Size = New Size(192, 62)
            dgvInstructors.TabIndex = 0
            ' 
            ' dgvStudents
            ' 
            dgvStudents.AllowUserToAddRows = False
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvStudents.ColumnHeadersHeight = 34
            dgvStudents.Dock = DockStyle.Fill
            dgvStudents.Location = New Point(0, 0)
            dgvStudents.Name = "dgvStudents"
            dgvStudents.ReadOnly = True
            dgvStudents.RowHeadersWidth = 62
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvStudents.Size = New Size(192, 62)
            dgvStudents.TabIndex = 0
            ' 
            ' studentEntryPanel
            ' 
            studentEntryPanel.Dock = DockStyle.Top
            studentEntryPanel.Location = New Point(0, 0)
            studentEntryPanel.Name = "studentEntryPanel"
            studentEntryPanel.Size = New Size(916, 214)
            studentEntryPanel.TabIndex = 1
            ' 
            ' lblStudentHeader
            ' 
            lblStudentHeader.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
            lblStudentHeader.Location = New Point(16, 12)
            lblStudentHeader.Name = "lblStudentHeader"
            lblStudentHeader.Size = New Size(880, 25)
            lblStudentHeader.TabIndex = 0
            lblStudentHeader.Text = "Create a Student login and profile in one transaction."
            ' 
            ' lblStudentUsername
            ' 
            lblStudentUsername.AutoSize = True
            lblStudentUsername.Location = New Point(16, 46)
            lblStudentUsername.Name = "lblStudentUsername"
            lblStudentUsername.Size = New Size(70, 20)
            lblStudentUsername.TabIndex = 1
            lblStudentUsername.Text = "Username:"
            ' 
            ' txtStudentUsername
            ' 
            txtStudentUsername.Location = New Point(16, 66)
            txtStudentUsername.Name = "txtStudentUsername"
            txtStudentUsername.Size = New Size(240, 29)
            txtStudentUsername.TabIndex = 2
            ' 
            ' lblStudentPassword
            ' 
            lblStudentPassword.AutoSize = True
            lblStudentPassword.Location = New Point(280, 46)
            lblStudentPassword.Name = "lblStudentPassword"
            lblStudentPassword.Size = New Size(66, 20)
            lblStudentPassword.TabIndex = 3
            lblStudentPassword.Text = "Password:"
            ' 
            ' txtStudentPassword
            ' 
            txtStudentPassword.Location = New Point(280, 66)
            txtStudentPassword.Name = "txtStudentPassword"
            txtStudentPassword.Size = New Size(240, 29)
            txtStudentPassword.TabIndex = 4
            txtStudentPassword.UseSystemPasswordChar = True
            ' 
            ' lblStudentEmail
            ' 
            lblStudentEmail.AutoSize = True
            lblStudentEmail.Location = New Point(544, 46)
            lblStudentEmail.Name = "lblStudentEmail"
            lblStudentEmail.Size = New Size(46, 20)
            lblStudentEmail.TabIndex = 5
            lblStudentEmail.Text = "Email:"
            ' 
            ' txtStudentEmail
            ' 
            txtStudentEmail.Location = New Point(544, 66)
            txtStudentEmail.Name = "txtStudentEmail"
            txtStudentEmail.Size = New Size(240, 29)
            txtStudentEmail.TabIndex = 6
            ' 
            ' lblStudentFirstName
            ' 
            lblStudentFirstName.AutoSize = True
            lblStudentFirstName.Location = New Point(16, 110)
            lblStudentFirstName.Name = "lblStudentFirstName"
            lblStudentFirstName.Size = New Size(67, 20)
            lblStudentFirstName.TabIndex = 7
            lblStudentFirstName.Text = "First name:"
            ' 
            ' txtStudentFirstName
            ' 
            txtStudentFirstName.Location = New Point(16, 130)
            txtStudentFirstName.Name = "txtStudentFirstName"
            txtStudentFirstName.Size = New Size(240, 29)
            txtStudentFirstName.TabIndex = 8
            ' 
            ' lblStudentLastName
            ' 
            lblStudentLastName.AutoSize = True
            lblStudentLastName.Location = New Point(280, 110)
            lblStudentLastName.Name = "lblStudentLastName"
            lblStudentLastName.Size = New Size(66, 20)
            lblStudentLastName.TabIndex = 9
            lblStudentLastName.Text = "Last name:"
            ' 
            ' txtStudentLastName
            ' 
            txtStudentLastName.Location = New Point(280, 130)
            txtStudentLastName.Name = "txtStudentLastName"
            txtStudentLastName.Size = New Size(240, 29)
            txtStudentLastName.TabIndex = 10
            ' 
            ' lblStudentBirthDate
            ' 
            lblStudentBirthDate.AutoSize = True
            lblStudentBirthDate.Location = New Point(544, 110)
            lblStudentBirthDate.Name = "lblStudentBirthDate"
            lblStudentBirthDate.Size = New Size(72, 20)
            lblStudentBirthDate.TabIndex = 11
            lblStudentBirthDate.Text = "Birth date:"
            ' 
            ' dtpStudentBirthDate
            ' 
            dtpStudentBirthDate.Format = DateTimePickerFormat.Short
            dtpStudentBirthDate.Location = New Point(544, 130)
            dtpStudentBirthDate.MaxDate = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            dtpStudentBirthDate.Name = "dtpStudentBirthDate"
            dtpStudentBirthDate.Size = New Size(240, 29)
            dtpStudentBirthDate.TabIndex = 12
            dtpStudentBirthDate.Value = New DateTime(2005, 1, 1)
            ' 
            ' btnCreateStudent
            ' 
            btnCreateStudent.Location = New Point(810, 124)
            btnCreateStudent.Name = "btnCreateStudent"
            btnCreateStudent.Size = New Size(100, 36)
            btnCreateStudent.TabIndex = 13
            btnCreateStudent.Text = "Create Student"
            ' 
            ' lblStudentStatus
            ' 
            lblStudentStatus.Font = New Font("Segoe UI", 9F)
            lblStudentStatus.Location = New Point(16, 176)
            lblStudentStatus.Name = "lblStudentStatus"
            lblStudentStatus.Size = New Size(880, 26)
            lblStudentStatus.TabIndex = 14
            lblStudentStatus.Text = ""
            ' 
            ' lblAddHeader
            ' 
            lblAddHeader.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
            lblAddHeader.Location = New Point(24, 20)
            lblAddHeader.Name = "lblAddHeader"
            lblAddHeader.Size = New Size(600, 25)
            lblAddHeader.TabIndex = 0
            lblAddHeader.Text = "Create a Teacher login and Instructor profile in one transaction."
            ' 
            ' lblNewUsername
            ' 
            lblNewUsername.Location = New Point(30, 33)
            lblNewUsername.Name = "lblNewUsername"
            lblNewUsername.Size = New Size(140, 25)
            lblNewUsername.TabIndex = 1
            lblNewUsername.Text = "Username:"
            ' 
            ' lblNewPassword
            ' 
            lblNewPassword.Location = New Point(30, 73)
            lblNewPassword.Name = "lblNewPassword"
            lblNewPassword.Size = New Size(140, 25)
            lblNewPassword.TabIndex = 3
            lblNewPassword.Text = "Password:"
            ' 
            ' lblFirstName
            ' 
            lblFirstName.Location = New Point(30, 113)
            lblFirstName.Name = "lblFirstName"
            lblFirstName.Size = New Size(140, 25)
            lblFirstName.TabIndex = 5
            lblFirstName.Text = "First name:"
            ' 
            ' lblLastName
            ' 
            lblLastName.Location = New Point(30, 153)
            lblLastName.Name = "lblLastName"
            lblLastName.Size = New Size(140, 25)
            lblLastName.TabIndex = 7
            lblLastName.Text = "Last name:"
            ' 
            ' lblDepartment
            ' 
            lblDepartment.Location = New Point(30, 233)
            lblDepartment.Name = "lblDepartment"
            lblDepartment.Size = New Size(140, 25)
            lblDepartment.TabIndex = 11
            lblDepartment.Text = "Department:"
            ' 
            ' lblNewEmail
            ' 
            lblNewEmail.Location = New Point(30, 193)
            lblNewEmail.Name = "lblNewEmail"
            lblNewEmail.Size = New Size(140, 25)
            lblNewEmail.TabIndex = 9
            lblNewEmail.Text = "Email:"
            ' 
            ' txtNewEmail
            ' 
            txtNewEmail.Location = New Point(180, 190)
            txtNewEmail.Name = "txtNewEmail"
            txtNewEmail.Size = New Size(320, 31)
            txtNewEmail.TabIndex = 10
            ' 
            ' txtNewUsername
            ' 
            txtNewUsername.Location = New Point(180, 30)
            txtNewUsername.Name = "txtNewUsername"
            txtNewUsername.Size = New Size(320, 31)
            txtNewUsername.TabIndex = 2
            ' 
            ' txtNewPassword
            ' 
            txtNewPassword.Location = New Point(180, 70)
            txtNewPassword.Name = "txtNewPassword"
            txtNewPassword.Size = New Size(320, 31)
            txtNewPassword.TabIndex = 4
            txtNewPassword.UseSystemPasswordChar = True
            ' 
            ' txtFirstName
            ' 
            txtFirstName.Location = New Point(180, 110)
            txtFirstName.Name = "txtFirstName"
            txtFirstName.Size = New Size(320, 31)
            txtFirstName.TabIndex = 6
            ' 
            ' txtLastName
            ' 
            txtLastName.Location = New Point(180, 150)
            txtLastName.Name = "txtLastName"
            txtLastName.Size = New Size(320, 31)
            txtLastName.TabIndex = 8
            ' 
            ' txtDepartment
            ' 
            txtDepartment.Location = New Point(180, 230)
            txtDepartment.Name = "txtDepartment"
            txtDepartment.Size = New Size(320, 31)
            txtDepartment.TabIndex = 12
            ' 
            ' btnCreateTeacher
            ' 
            btnCreateTeacher.Location = New Point(180, 285)
            btnCreateTeacher.Name = "btnCreateTeacher"
            btnCreateTeacher.Size = New Size(230, 36)
            btnCreateTeacher.TabIndex = 13
            btnCreateTeacher.Text = "Create Teacher Account"
            ' 
            ' lblAddStatus
            ' 
            lblAddStatus.ForeColor = Color.SeaGreen
            lblAddStatus.Location = New Point(30, 335)
            lblAddStatus.Name = "lblAddStatus"
            lblAddStatus.Size = New Size(700, 60)
            lblAddStatus.TabIndex = 14
            ' 
            ' lblResetHeader
            ' 
            lblResetHeader.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
            lblResetHeader.Location = New Point(24, 20)
            lblResetHeader.Name = "lblResetHeader"
            lblResetHeader.Size = New Size(700, 25)
            lblResetHeader.TabIndex = 0
            lblResetHeader.Text = "Passwords are never stored in plain text. A new salted PBKDF2 hash is generated."
            ' 
            ' lblResetUsername
            ' 
            lblResetUsername.Location = New Point(30, 63)
            lblResetUsername.Name = "lblResetUsername"
            lblResetUsername.Size = New Size(140, 25)
            lblResetUsername.TabIndex = 1
            lblResetUsername.Text = "Username:"
            ' 
            ' lblResetPassword
            ' 
            lblResetPassword.Location = New Point(30, 103)
            lblResetPassword.Name = "lblResetPassword"
            lblResetPassword.Size = New Size(140, 25)
            lblResetPassword.TabIndex = 3
            lblResetPassword.Text = "New password:"
            ' 
            ' txtResetUsername
            ' 
            txtResetUsername.Location = New Point(180, 60)
            txtResetUsername.Name = "txtResetUsername"
            txtResetUsername.Size = New Size(320, 31)
            txtResetUsername.TabIndex = 2
            ' 
            ' txtResetPassword
            ' 
            txtResetPassword.Location = New Point(180, 100)
            txtResetPassword.Name = "txtResetPassword"
            txtResetPassword.Size = New Size(320, 31)
            txtResetPassword.TabIndex = 4
            txtResetPassword.UseSystemPasswordChar = True
            ' 
            ' btnResetPassword
            ' 
            btnResetPassword.Location = New Point(180, 155)
            btnResetPassword.Name = "btnResetPassword"
            btnResetPassword.Size = New Size(230, 36)
            btnResetPassword.TabIndex = 5
            btnResetPassword.Text = "Reset Password"
            ' 
            ' lblResetStatus
            ' 
            lblResetStatus.ForeColor = Color.SeaGreen
            lblResetStatus.Location = New Point(30, 205)
            lblResetStatus.Name = "lblResetStatus"
            lblResetStatus.Size = New Size(700, 50)
            lblResetStatus.TabIndex = 6
            ' 
            ' tabControl
            ' 
            tabControl.Controls.Add(pageAdd)
            tabControl.Controls.Add(pageUsers)
            tabControl.Controls.Add(pageInstructors)
            tabControl.Controls.Add(pageStudents)
            tabControl.Controls.Add(pageReset)
            tabControl.Controls.Add(pageCourses)
            tabControl.Controls.Add(pageGradebook)
            tabControl.Dock = DockStyle.Fill
            tabControl.Location = New Point(0, 0)
            tabControl.Name = "tabControl"
            tabControl.SelectedIndex = 0
            tabControl.Size = New Size(924, 554)
            tabControl.TabIndex = 0
            ' 
            ' pageAdd
            ' 
            pageAdd.Controls.Add(lblAddHeader)
            pageAdd.Controls.Add(lblNewUsername)
            pageAdd.Controls.Add(txtNewUsername)
            pageAdd.Controls.Add(lblNewPassword)
            pageAdd.Controls.Add(txtNewPassword)
            pageAdd.Controls.Add(lblFirstName)
            pageAdd.Controls.Add(txtFirstName)
            pageAdd.Controls.Add(lblLastName)
            pageAdd.Controls.Add(txtLastName)
            pageAdd.Controls.Add(lblNewEmail)
            pageAdd.Controls.Add(txtNewEmail)
            pageAdd.Controls.Add(lblDepartment)
            pageAdd.Controls.Add(txtDepartment)
            pageAdd.Controls.Add(btnCreateTeacher)
            pageAdd.Controls.Add(lblAddStatus)
            pageAdd.Location = New Point(4, 34)
            pageAdd.Name = "pageAdd"
            pageAdd.Size = New Size(916, 516)
            pageAdd.TabIndex = 0
            pageAdd.Text = "Add Teacher"
            ' 
            ' pageUsers
            ' 
            pageUsers.Controls.Add(dgvUsers)
            pageUsers.Location = New Point(4, 34)
            pageUsers.Name = "pageUsers"
            pageUsers.Size = New Size(192, 62)
            pageUsers.TabIndex = 1
            pageUsers.Text = "Users"
            ' 
            ' pageInstructors
            ' 
            pageInstructors.Controls.Add(dgvInstructors)
            pageInstructors.Location = New Point(4, 34)
            pageInstructors.Name = "pageInstructors"
            pageInstructors.Size = New Size(192, 62)
            pageInstructors.TabIndex = 2
            pageInstructors.Text = "Instructors"
            ' 
            ' pageStudents
            ' 
            pageStudents.Controls.Add(dgvStudents)
            studentEntryPanel.Controls.AddRange(New Control() {
                lblStudentHeader,
                lblStudentUsername, txtStudentUsername,
                lblStudentPassword, txtStudentPassword,
                lblStudentEmail, txtStudentEmail,
                lblStudentFirstName, txtStudentFirstName,
                lblStudentLastName, txtStudentLastName,
                lblStudentBirthDate, dtpStudentBirthDate,
                btnCreateStudent, lblStudentStatus})
            pageStudents.Controls.Add(studentEntryPanel)
            pageStudents.Location = New Point(4, 34)
            pageStudents.Name = "pageStudents"
            pageStudents.Size = New Size(192, 62)
            pageStudents.TabIndex = 3
            pageStudents.Text = "Students"
            ' 
            ' pageReset
            ' 
            pageReset.Controls.Add(lblResetHeader)
            pageReset.Controls.Add(lblResetUsername)
            pageReset.Controls.Add(txtResetUsername)
            pageReset.Controls.Add(lblResetPassword)
            pageReset.Controls.Add(txtResetPassword)
            pageReset.Controls.Add(btnResetPassword)
            pageReset.Controls.Add(lblResetStatus)
            pageReset.Location = New Point(4, 34)
            pageReset.Name = "pageReset"
            pageReset.Size = New Size(916, 516)
            pageReset.TabIndex = 4
            pageReset.Text = "Reset Password"
            ' 
            ' pageCourses
            ' 
            pageCourses.Controls.Add(lblCourseHeader)
            pageCourses.Controls.Add(lblCourseCode)
            pageCourses.Controls.Add(txtCourseCode)
            pageCourses.Controls.Add(lblCourseName)
            pageCourses.Controls.Add(txtCourseName)
            pageCourses.Controls.Add(lblCourseInstructor)
            pageCourses.Controls.Add(cboCourseInstructor)
            pageCourses.Controls.Add(btnCreateCourse)
            pageCourses.Controls.Add(lblCourseStatus)
            pageCourses.Location = New Point(4, 34)
            pageCourses.Name = "pageCourses"
            pageCourses.Size = New Size(916, 516)
            pageCourses.TabIndex = 5
            pageCourses.Text = "Courses"
            ' 
            ' lblCourseHeader
            ' 
            lblCourseHeader.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
            lblCourseHeader.Location = New Point(24, 20)
            lblCourseHeader.Name = "lblCourseHeader"
            lblCourseHeader.Size = New Size(700, 25)
            lblCourseHeader.TabIndex = 0
            lblCourseHeader.Text = "Course codes must match CS101-style format and be unique."
            ' 
            ' lblCourseCode
            ' 
            lblCourseCode.Location = New Point(30, 63)
            lblCourseCode.Name = "lblCourseCode"
            lblCourseCode.Size = New Size(140, 25)
            lblCourseCode.TabIndex = 1
            lblCourseCode.Text = "Course code:"
            ' 
            ' txtCourseCode
            ' 
            txtCourseCode.Location = New Point(180, 60)
            txtCourseCode.Name = "txtCourseCode"
            txtCourseCode.Size = New Size(320, 31)
            txtCourseCode.TabIndex = 2
            ' 
            ' lblCourseName
            ' 
            lblCourseName.Location = New Point(30, 103)
            lblCourseName.Name = "lblCourseName"
            lblCourseName.Size = New Size(140, 25)
            lblCourseName.TabIndex = 3
            lblCourseName.Text = "Course name:"
            ' 
            ' txtCourseName
            ' 
            txtCourseName.Location = New Point(180, 100)
            txtCourseName.Name = "txtCourseName"
            txtCourseName.Size = New Size(320, 31)
            txtCourseName.TabIndex = 4
            ' 
            ' lblCourseInstructor
            ' 
            lblCourseInstructor.Location = New Point(30, 143)
            lblCourseInstructor.Name = "lblCourseInstructor"
            lblCourseInstructor.Size = New Size(140, 25)
            lblCourseInstructor.TabIndex = 5
            lblCourseInstructor.Text = "Instructor:"
            ' 
            ' cboCourseInstructor
            ' 
            cboCourseInstructor.DropDownStyle = ComboBoxStyle.DropDownList
            cboCourseInstructor.Location = New Point(180, 140)
            cboCourseInstructor.Name = "cboCourseInstructor"
            cboCourseInstructor.Size = New Size(320, 33)
            cboCourseInstructor.TabIndex = 6
            ' 
            ' btnCreateCourse
            ' 
            btnCreateCourse.Location = New Point(180, 195)
            btnCreateCourse.Name = "btnCreateCourse"
            btnCreateCourse.Size = New Size(230, 36)
            btnCreateCourse.TabIndex = 7
            btnCreateCourse.Text = "Create Course"
            ' 
            ' lblCourseStatus
            ' 
            lblCourseStatus.ForeColor = Color.SeaGreen
            lblCourseStatus.Location = New Point(30, 245)
            lblCourseStatus.Name = "lblCourseStatus"
            lblCourseStatus.Size = New Size(700, 60)
            lblCourseStatus.TabIndex = 8
            ' 
            ' pageGradebook
            ' 
            pageGradebook.Controls.Add(lblTypeHeader)
            pageGradebook.Controls.Add(dgvAssignmentTypes)
            pageGradebook.Controls.Add(lblTypeName)
            pageGradebook.Controls.Add(txtTypeName)
            pageGradebook.Controls.Add(lblTypeWeight)
            pageGradebook.Controls.Add(txtTypeWeight)
            pageGradebook.Controls.Add(btnAddType)
            pageGradebook.Controls.Add(btnEditType)
            pageGradebook.Controls.Add(btnDeleteType)
            pageGradebook.Controls.Add(lblTypeStatus)
            pageGradebook.Controls.Add(lblEnrollHeader)
            pageGradebook.Controls.Add(cboEnrollStudent)
            pageGradebook.Controls.Add(cboEnrollCourse)
            pageGradebook.Controls.Add(btnEnroll)
            pageGradebook.Controls.Add(btnBulkEnroll)
            pageGradebook.Controls.Add(lblEnrollStatus)
            pageGradebook.Controls.Add(txtGradeSearch)
            pageGradebook.Controls.Add(btnGradeSearch)
            pageGradebook.Controls.Add(dgvAllGrades)
            pageGradebook.Location = New Point(4, 34)
            pageGradebook.Name = "pageGradebook"
            pageGradebook.Size = New Size(916, 516)
            pageGradebook.TabIndex = 6
            pageGradebook.Text = "Gradebook"
            ' 
            ' lblTypeHeader
            ' 
            lblTypeHeader.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
            lblTypeHeader.Location = New Point(14, 12)
            lblTypeHeader.Name = "lblTypeHeader"
            lblTypeHeader.Size = New Size(500, 24)
            lblTypeHeader.TabIndex = 0
            lblTypeHeader.Text = "Assignment Types (add / edit / delete)"
            ' 
            ' dgvAssignmentTypes
            ' 
            dgvAssignmentTypes.AllowUserToAddRows = False
            dgvAssignmentTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvAssignmentTypes.ColumnHeadersHeight = 34
            dgvAssignmentTypes.Location = New Point(14, 38)
            dgvAssignmentTypes.Name = "dgvAssignmentTypes"
            dgvAssignmentTypes.ReadOnly = True
            dgvAssignmentTypes.RowHeadersWidth = 62
            dgvAssignmentTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvAssignmentTypes.Size = New Size(870, 150)
            dgvAssignmentTypes.TabIndex = 1
            ' 
            ' lblTypeName
            ' 
            lblTypeName.Location = New Point(14, 202)
            lblTypeName.Name = "lblTypeName"
            lblTypeName.Size = New Size(110, 25)
            lblTypeName.TabIndex = 2
            lblTypeName.Text = "Type name:"
            ' 
            ' txtTypeName
            ' 
            txtTypeName.Location = New Point(120, 198)
            txtTypeName.Name = "txtTypeName"
            txtTypeName.Size = New Size(210, 31)
            txtTypeName.TabIndex = 3
            ' 
            ' lblTypeWeight
            ' 
            lblTypeWeight.Location = New Point(350, 202)
            lblTypeWeight.Name = "lblTypeWeight"
            lblTypeWeight.Size = New Size(90, 25)
            lblTypeWeight.TabIndex = 4
            lblTypeWeight.Text = "Weight %:"
            ' 
            ' txtTypeWeight
            ' 
            txtTypeWeight.Location = New Point(440, 198)
            txtTypeWeight.Name = "txtTypeWeight"
            txtTypeWeight.Size = New Size(90, 31)
            txtTypeWeight.TabIndex = 5
            ' 
            ' btnAddType
            ' 
            btnAddType.Location = New Point(570, 196)
            btnAddType.Name = "btnAddType"
            btnAddType.Size = New Size(90, 32)
            btnAddType.TabIndex = 6
            btnAddType.Text = "Add"
            ' 
            ' btnEditType
            ' 
            btnEditType.Location = New Point(670, 196)
            btnEditType.Name = "btnEditType"
            btnEditType.Size = New Size(90, 32)
            btnEditType.TabIndex = 7
            btnEditType.Text = "Edit"
            ' 
            ' btnDeleteType
            ' 
            btnDeleteType.Location = New Point(770, 196)
            btnDeleteType.Name = "btnDeleteType"
            btnDeleteType.Size = New Size(90, 32)
            btnDeleteType.TabIndex = 8
            btnDeleteType.Text = "Delete"
            ' 
            ' lblTypeStatus
            ' 
            lblTypeStatus.ForeColor = Color.SeaGreen
            lblTypeStatus.Location = New Point(14, 234)
            lblTypeStatus.Name = "lblTypeStatus"
            lblTypeStatus.Size = New Size(870, 30)
            lblTypeStatus.TabIndex = 9
            ' 
            ' lblEnrollHeader
            ' 
            lblEnrollHeader.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
            lblEnrollHeader.Location = New Point(14, 270)
            lblEnrollHeader.Name = "lblEnrollHeader"
            lblEnrollHeader.Size = New Size(500, 24)
            lblEnrollHeader.TabIndex = 10
            lblEnrollHeader.Text = "Enrollment (uses sp_EnrollStudent with transaction handling)"
            ' 
            ' cboEnrollStudent
            ' 
            cboEnrollStudent.DropDownStyle = ComboBoxStyle.DropDownList
            cboEnrollStudent.Location = New Point(14, 302)
            cboEnrollStudent.Name = "cboEnrollStudent"
            cboEnrollStudent.Size = New Size(280, 33)
            cboEnrollStudent.TabIndex = 11
            ' 
            ' cboEnrollCourse
            ' 
            cboEnrollCourse.DropDownStyle = ComboBoxStyle.DropDownList
            cboEnrollCourse.Location = New Point(310, 302)
            cboEnrollCourse.Name = "cboEnrollCourse"
            cboEnrollCourse.Size = New Size(280, 33)
            cboEnrollCourse.TabIndex = 12
            ' 
            ' btnEnroll
            ' 
            btnEnroll.Location = New Point(610, 300)
            btnEnroll.Name = "btnEnroll"
            btnEnroll.Size = New Size(140, 32)
            btnEnroll.TabIndex = 13
            btnEnroll.Text = "Enroll Student"
            ' 
            ' btnBulkEnroll
            ' 
            btnBulkEnroll.Location = New Point(760, 300)
            btnBulkEnroll.Name = "btnBulkEnroll"
            btnBulkEnroll.Size = New Size(150, 32)
            btnBulkEnroll.TabIndex = 14
            btnBulkEnroll.Text = "Bulk Enroll"
            ' 
            ' lblEnrollStatus
            ' 
            lblEnrollStatus.ForeColor = Color.SeaGreen
            lblEnrollStatus.Location = New Point(14, 342)
            lblEnrollStatus.Name = "lblEnrollStatus"
            lblEnrollStatus.Size = New Size(870, 30)
            lblEnrollStatus.TabIndex = 15
            ' 
            ' txtGradeSearch
            ' 
            txtGradeSearch.Location = New Point(14, 376)
            txtGradeSearch.Name = "txtGradeSearch"
            txtGradeSearch.Size = New Size(320, 31)
            txtGradeSearch.TabIndex = 16
            ' 
            ' btnGradeSearch
            ' 
            btnGradeSearch.Location = New Point(350, 374)
            btnGradeSearch.Name = "btnGradeSearch"
            btnGradeSearch.Size = New Size(140, 32)
            btnGradeSearch.TabIndex = 17
            btnGradeSearch.Text = "Search Grades"
            ' 
            ' dgvAllGrades
            ' 
            dgvAllGrades.AllowUserToAddRows = False
            dgvAllGrades.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            dgvAllGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvAllGrades.ColumnHeadersHeight = 34
            dgvAllGrades.Location = New Point(14, 414)
            dgvAllGrades.Name = "dgvAllGrades"
            dgvAllGrades.ReadOnly = True
            dgvAllGrades.RowHeadersWidth = 62
            dgvAllGrades.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvAllGrades.Size = New Size(1586, 561)
            dgvAllGrades.TabIndex = 18
            ' 
            ' bottomPanel
            ' 
            bottomPanel.Controls.Add(btnLogout)
            bottomPanel.Controls.Add(btnReports)
            bottomPanel.Controls.Add(btnAuditLog)
            bottomPanel.Controls.Add(btnBackup)
            bottomPanel.Dock = DockStyle.Bottom
            bottomPanel.Location = New Point(0, 554)
            bottomPanel.Name = "bottomPanel"
            bottomPanel.Size = New Size(924, 48)
            bottomPanel.TabIndex = 1
            ' 
            ' btnLogout
            ' 
            btnLogout.Location = New Point(14, 10)
            btnLogout.Name = "btnLogout"
            btnLogout.Size = New Size(100, 30)
            btnLogout.TabIndex = 0
            btnLogout.Text = "Logout"
            ' 
            ' btnReports
            ' 
            btnReports.Location = New Point(130, 10)
            btnReports.Name = "btnReports"
            btnReports.Size = New Size(120, 30)
            btnReports.TabIndex = 1
            btnReports.Text = "Reports"
            ' 
            ' btnAuditLog
            ' 
            btnAuditLog.Location = New Point(265, 10)
            btnAuditLog.Name = "btnAuditLog"
            btnAuditLog.Size = New Size(120, 30)
            btnAuditLog.TabIndex = 2
            btnAuditLog.Text = "Audit Trail"
            ' 
            ' btnBackup
            ' 
            btnBackup.Location = New Point(400, 10)
            btnBackup.Name = "btnBackup"
            btnBackup.Size = New Size(110, 30)
            btnBackup.TabIndex = 3
            btnBackup.Text = "Backup"
            ' 
            ' AdminForm
            ' 
            ClientSize = New Size(924, 602)
            Controls.Add(tabControl)
            Controls.Add(bottomPanel)
            Name = "AdminForm"
            StartPosition = FormStartPosition.CenterScreen
            Text = "Admin Management"
            CType(dgvUsers, ComponentModel.ISupportInitialize).EndInit()
            CType(dgvInstructors, ComponentModel.ISupportInitialize).EndInit()
            CType(dgvStudents, ComponentModel.ISupportInitialize).EndInit()
            tabControl.ResumeLayout(False)
            pageAdd.ResumeLayout(False)
            pageAdd.PerformLayout()
            pageUsers.ResumeLayout(False)
            pageInstructors.ResumeLayout(False)
            studentEntryPanel.ResumeLayout(False)
            studentEntryPanel.PerformLayout()
            pageStudents.ResumeLayout(False)
            pageReset.ResumeLayout(False)
            pageReset.PerformLayout()
            pageCourses.ResumeLayout(False)
            pageCourses.PerformLayout()
            pageGradebook.ResumeLayout(False)
            pageGradebook.PerformLayout()
            CType(dgvAssignmentTypes, ComponentModel.ISupportInitialize).EndInit()
            CType(dgvAllGrades, ComponentModel.ISupportInitialize).EndInit()
            bottomPanel.ResumeLayout(False)
            ResumeLayout(False)
        End Sub

    End Class

End Namespace
