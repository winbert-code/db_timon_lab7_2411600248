Namespace AcademicSystem

    Partial Public Class StudentDashboard

        Friend header As Label
        Friend WithEvents dgvGrades As DataGridView
        Friend pnlAverages As Panel
        Friend lblAverages As Label
        Friend WithEvents dgvAverages As DataGridView
        Friend pnlSearch As Panel
        Friend WithEvents txtSearch As TextBox
        Friend WithEvents btnSearch As Button
        Friend lblStatus As Label
        Friend WithEvents btnLogout As Button

        Friend Sub InitializeComponent()
            Me.header = New Label()
            Me.dgvGrades = New DataGridView()
            Me.pnlAverages = New Panel()
            Me.lblAverages = New Label()
            Me.dgvAverages = New DataGridView()
            Me.pnlSearch = New Panel()
            Me.txtSearch = New TextBox()
            Me.btnSearch = New Button()
            Me.lblStatus = New Label()
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
            ' dgvGrades
            '
            Me.dgvGrades.AllowUserToAddRows = False
            Me.dgvGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvGrades.Dock = DockStyle.Fill
            Me.dgvGrades.ReadOnly = True
            Me.dgvGrades.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            '
            ' pnlAverages
            '
            Me.pnlAverages.Dock = DockStyle.Bottom
            Me.pnlAverages.Height = 190
            Me.lblAverages.Location = New Point(14, 8)
            Me.lblAverages.Size = New Size(500, 25)
            Me.lblAverages.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
            Me.lblAverages.Text = "Current Overall Average per Course"
            Me.dgvAverages.AllowUserToAddRows = False
            Me.dgvAverages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvAverages.Location = New Point(14, 34)
            Me.dgvAverages.Size = New Size(880, 145)
            Me.dgvAverages.ReadOnly = True
            Me.pnlAverages.Controls.Add(Me.lblAverages)
            Me.pnlAverages.Controls.Add(Me.dgvAverages)
            '
            ' pnlSearch
            '
            Me.pnlSearch.Dock = DockStyle.Bottom
            Me.pnlSearch.Height = 60
            Me.txtSearch.Location = New Point(14, 14)
            Me.txtSearch.Size = New Size(320, 28)
            Me.btnSearch.Location = New Point(350, 12)
            Me.btnSearch.Size = New Size(110, 30)
            Me.btnSearch.Text = "Search Grades"
            Me.lblStatus.Location = New Point(490, 16)
            Me.lblStatus.Size = New Size(270, 30)
            Me.lblStatus.ForeColor = Color.SeaGreen
            Me.btnLogout.Location = New Point(790, 12)
            Me.btnLogout.Size = New Size(120, 30)
            Me.btnLogout.Text = "Logout"
            Me.pnlSearch.Controls.Add(Me.txtSearch)
            Me.pnlSearch.Controls.Add(Me.btnSearch)
            Me.pnlSearch.Controls.Add(Me.lblStatus)
            Me.pnlSearch.Controls.Add(Me.btnLogout)
            '
            ' StudentDashboard
            '
            Me.ClientSize = New Size(1180, 620)
            Me.Controls.Add(Me.dgvGrades)
            Me.Controls.Add(Me.pnlSearch)
            Me.Controls.Add(Me.pnlAverages)
            Me.Controls.Add(Me.header)
            Me.Name = "StudentDashboard"
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "Student Dashboard"
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

    End Class

End Namespace
