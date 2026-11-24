Namespace AcademicSystem

    Partial Public Class TeacherDashboard

        Friend header As Label
        Friend WithEvents dgvOverview As DataGridView
        Friend pnlEntry As Panel
        Friend lblCourse As Label
        Friend WithEvents cboCourse As ComboBox
        Friend lblStudent As Label
        Friend WithEvents cboStudent As ComboBox
        Friend lblType As Label
        Friend WithEvents cboType As ComboBox
        Friend lblGrade As Label
        Friend WithEvents txtGrade As TextBox
        Friend WithEvents btnSaveGrade As Button
        Friend WithEvents btnCalculate As Button
        Friend WithEvents txtSearch As TextBox
        Friend WithEvents btnSearch As Button
        Friend lblStatus As Label
        Friend WithEvents btnReports As Button
        Friend WithEvents btnLogout As Button

        Friend Sub InitializeComponent()
            Me.header = New Label()
            Me.dgvOverview = New DataGridView()
            Me.pnlEntry = New Panel()
            Me.lblCourse = New Label()
            Me.cboCourse = New ComboBox()
            Me.lblStudent = New Label()
            Me.cboStudent = New ComboBox()
            Me.lblType = New Label()
            Me.cboType = New ComboBox()
            Me.lblGrade = New Label()
            Me.txtGrade = New TextBox()
            Me.btnSaveGrade = New Button()
            Me.btnCalculate = New Button()
            Me.txtSearch = New TextBox()
            Me.btnSearch = New Button()
            Me.lblStatus = New Label()
            Me.btnReports = New Button()
            Me.btnLogout = New Button()
            Me.SuspendLayout()
            '
            ' header
            '
            Me.header.Dock = DockStyle.Top
            Me.header.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
            Me.header.Height = 52
            Me.header.Padding = New Padding(14)
            '
            ' dgvOverview
            '
            Me.dgvOverview.AllowUserToAddRows = False
            Me.dgvOverview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvOverview.Dock = DockStyle.Top
            Me.dgvOverview.Height = 230
            Me.dgvOverview.ReadOnly = True
            Me.dgvOverview.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            '
            ' pnlEntry
            '
            Me.pnlEntry.Dock = DockStyle.Fill
            '
            ' entry controls
            '
            Me.lblCourse.Location = New Point(20, 24)
            Me.lblCourse.Size = New Size(110, 25)
            Me.lblCourse.Text = "Course:"
            Me.cboCourse.Location = New Point(130, 20)
            Me.cboCourse.Size = New Size(260, 28)
            Me.cboCourse.DropDownStyle = ComboBoxStyle.DropDownList

            Me.lblStudent.Location = New Point(410, 24)
            Me.lblStudent.Size = New Size(110, 25)
            Me.lblStudent.Text = "Student:"
            Me.cboStudent.Location = New Point(520, 20)
            Me.cboStudent.Size = New Size(260, 28)
            Me.cboStudent.DropDownStyle = ComboBoxStyle.DropDownList

            Me.lblType.Location = New Point(20, 74)
            Me.lblType.Size = New Size(110, 25)
            Me.lblType.Text = "Assignment:"
            Me.cboType.Location = New Point(130, 70)
            Me.cboType.Size = New Size(260, 28)
            Me.cboType.DropDownStyle = ComboBoxStyle.DropDownList

            Me.lblGrade.Location = New Point(410, 74)
            Me.lblGrade.Size = New Size(110, 25)
            Me.lblGrade.Text = "Grade (0-100):"
            Me.txtGrade.Location = New Point(520, 70)
            Me.txtGrade.Size = New Size(130, 28)

            Me.btnSaveGrade.Location = New Point(130, 120)
            Me.btnSaveGrade.Size = New Size(210, 36)
            Me.btnSaveGrade.Text = "Save / Update Grade"

            Me.btnCalculate.Location = New Point(370, 120)
            Me.btnCalculate.Size = New Size(210, 36)
            Me.btnCalculate.Text = "Calculate Final Grades"

            Me.txtSearch.Location = New Point(20, 180)
            Me.txtSearch.Size = New Size(280, 28)
            Me.btnSearch.Location = New Point(310, 178)
            Me.btnSearch.Size = New Size(110, 30)
            Me.btnSearch.Text = "Search"

            Me.lblStatus.Location = New Point(20, 225)
            Me.lblStatus.Size = New Size(760, 60)
            Me.lblStatus.ForeColor = Color.SeaGreen

            Me.btnReports.Location = New Point(650, 178)
            Me.btnReports.Size = New Size(130, 30)
            Me.btnReports.Text = "Reports"
            Me.btnLogout.Location = New Point(800, 178)
            Me.btnLogout.Size = New Size(120, 30)
            Me.btnLogout.Text = "Logout"

            Me.pnlEntry.Controls.Add(Me.lblCourse)
            Me.pnlEntry.Controls.Add(Me.cboCourse)
            Me.pnlEntry.Controls.Add(Me.lblStudent)
            Me.pnlEntry.Controls.Add(Me.cboStudent)
            Me.pnlEntry.Controls.Add(Me.lblType)
            Me.pnlEntry.Controls.Add(Me.cboType)
            Me.pnlEntry.Controls.Add(Me.lblGrade)
            Me.pnlEntry.Controls.Add(Me.txtGrade)
            Me.pnlEntry.Controls.Add(Me.btnSaveGrade)
            Me.pnlEntry.Controls.Add(Me.btnCalculate)
            Me.pnlEntry.Controls.Add(Me.txtSearch)
            Me.pnlEntry.Controls.Add(Me.btnSearch)
            Me.pnlEntry.Controls.Add(Me.lblStatus)
            Me.pnlEntry.Controls.Add(Me.btnReports)
            Me.pnlEntry.Controls.Add(Me.btnLogout)
            '
            ' TeacherDashboard
            '
            Me.ClientSize = New Size(1180, 620)
            Me.Controls.Add(Me.pnlEntry)
            Me.Controls.Add(Me.dgvOverview)
            Me.Controls.Add(Me.header)
            Me.Name = "TeacherDashboard"
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "Teacher Dashboard"
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

    End Class

End Namespace
