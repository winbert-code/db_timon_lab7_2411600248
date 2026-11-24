Namespace AcademicSystem

    <System.ComponentModel.DesignerCategory("Form")>
    Partial Public Class LoginForm
        Inherits Form

        Public Sub New()
            InitializeComponent()
            AddHandler btnLogin.Click, AddressOf BtnLogin_Click
        End Sub

        Private Sub BtnLogin_Click(sender As Object, e As EventArgs)
            lblStatus.Text = ""

            Dim username As String = InputValidator.Sanitize(txtUsername.Text)
            Dim password As String = txtPassword.Text

            If username.Length = 0 OrElse password.Length = 0 Then
                lblStatus.Text = "Please enter both username and password."
                Return
            End If
            If Not InputValidator.IsValidUsername(username) Then
                lblStatus.ForeColor = Color.Firebrick
                lblStatus.Text = "Invalid username format (letters, digits, dot, dash, underscore; 3-30 characters)."
                txtPassword.Clear()
                Return
            End If

            Try
                ' Authentication is performed with the least-privileged admin
                ' account; the role-specific account is selected after login.
                Db.UseRole("Admin")

                Dim parameters As New Dictionary(Of String, Object) From {
                    {"@username", username}
                }
                Dim users = Db.FetchTable(
                    "SELECT user_id, username, password_hash, role_id FROM users WHERE username = @username",
                    parameters)

                If users.Rows.Count = 0 Then
                    lblStatus.ForeColor = Color.Firebrick
                    lblStatus.Text = "Invalid username or password."
                    txtPassword.Clear()
                    Return
                End If

                Dim row = users.Rows(0)
                If Not PasswordHasher.VerifyPassword(password, Convert.ToString(row("password_hash"))) Then
                    lblStatus.ForeColor = Color.Firebrick
                    lblStatus.Text = "Invalid username or password."
                    txtPassword.Clear()
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(row("user_id"))
                Dim roleId As Integer = Convert.ToInt32(row("role_id"))
                Dim roleResult = Db.FetchTable(
                    "SELECT role_name FROM roles WHERE role_id = @roleId",
                    New Dictionary(Of String, Object) From {{"@roleId", roleId}})
                Dim roleName As String = If(roleResult.Rows.Count > 0,
                                            Convert.ToString(roleResult.Rows(0)("role_name")),
                                            "Unknown")

                Try
                    Session.StartSession(userId, username, roleId, roleName)
                Catch loginEx As InvalidOperationException
                    lblStatus.ForeColor = Color.Firebrick
                    lblStatus.Text = loginEx.Message
                    txtPassword.Clear()
                    Return
                End Try

                Db.UseRole(roleName)

                Select Case roleId
                    Case 1, 2, 3
                        ' Supported roles open in the Lab 7 MDI workspace.
                    Case Else
                        Session.EndSession("FAILED_ROLE")
                        lblStatus.ForeColor = Color.Firebrick
                        lblStatus.Text = "No application form exists for role: " & roleName
                        txtPassword.Clear()
                        Return
                End Select

                Dim workspace As New MainMdiForm()
                Hide()
                AddHandler workspace.FormClosed,
                    Sub(sender2 As Object, e2 As FormClosedEventArgs)
                        txtPassword.Clear()
                        If Not String.IsNullOrEmpty(Session.SessionId) Then Session.EndSession("LOGOUT")
                        Db.UseRole("Admin")
                        Show()
                    End Sub
                workspace.Show()

            Catch ex As Exception
                AppLogger.LogException("login", ex)
                lblStatus.ForeColor = Color.Firebrick
                lblStatus.Text = "Login failed. Details were written to the application log."
                txtPassword.Clear()
            End Try
        End Sub

    End Class

    End Namespace
