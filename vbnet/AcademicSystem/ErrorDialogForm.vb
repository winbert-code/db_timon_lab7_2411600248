Namespace AcademicSystem

    ' Custom, user-friendly error dialog used by GlobalExceptionHandler.
    Public Class ErrorDialogForm
        Inherits Form

        Public Sub New(source As String, ex As Exception)
            Text = "DB_Timon - Unexpected error"
            FormBorderStyle = FormBorderStyle.FixedDialog
            StartPosition = FormStartPosition.CenterScreen
            MaximizeBox = False
            MinimizeBox = False
            ShowInTaskbar = False
            ClientSize = New Size(740, 410)
            BackColor = Color.FromArgb(250, 251, 253)

            Dim panel As New Panel() With {.Dock = DockStyle.Top, .Height = 92, .BackColor = Color.FromArgb(31, 78, 121)}
            Dim iconLabel As New Label() With {
                .Text = "!", .Font = New Font("Segoe UI", 26, FontStyle.Bold),
                .ForeColor = Color.White, .Location = New Point(24, 18), .Size = New Size(50, 50)
            }
            Dim heading As New Label() With {
                .Text = "Something went wrong", .Font = New Font("Segoe UI", 15, FontStyle.Bold),
                .ForeColor = Color.White, .Location = New Point(86, 20), .Size = New Size(600, 30)
            }
            Dim subHeading As New Label() With {
                .Text = "Your work is safe. The issue has been written to the application log.",
                .ForeColor = Color.FromArgb(220, 235, 248), .Location = New Point(86, 52), .Size = New Size(620, 25)
            }
            panel.Controls.AddRange(New Control() {iconLabel, heading, subHeading})

            Dim friendly As New Label() With {
                .Text = "DB_Timon caught an unexpected " & If(source, "application") & " error and continued running." & vbCrLf &
                        "If you were entering data, check the form before saving again.",
                .Location = New Point(28, 112), .Size = New Size(700, 56),
                .Font = New Font("Segoe UI", 10)
            }

            Dim detailsLabel As New Label() With {
                .Text = "Technical details (safe to send to support):",
                .Location = New Point(28, 174), .Size = New Size(700, 22),
                .Font = New Font("Segoe UI", 9, FontStyle.Bold)
            }

            Dim details As New TextBox() With {
                .Location = New Point(28, 200), .Size = New Size(704, 150),
                .Multiline = True, .ReadOnly = True, .ScrollBars = ScrollBars.Vertical,
                .BackColor = Color.White, .Font = New Font("Consolas", 9),
                .Text = "Source: " & If(source, "unknown") & vbCrLf &
                        "Type: " & If(ex Is Nothing, "Exception", ex.GetType().FullName) & vbCrLf &
                        "Message: " & If(ex Is Nothing, "", ex.Message) & vbCrLf &
                        "Stack trace:" & vbCrLf & If(ex Is Nothing, "", ex.StackTrace)
            }

            Dim btnCopy As New Button() With {
                .Text = "Copy details", .Location = New Point(28, 365), .Size = New Size(150, 34)
            }
            AddHandler btnCopy.Click,
                Sub(sender As Object, e As EventArgs)
                    If Not String.IsNullOrEmpty(details.Text) Then Clipboard.SetText(details.Text)
                    btnCopy.Text = "Copied"
                End Sub

            Dim btnClose As New Button() With {
                .Text = "Close", .Location = New Point(600, 365), .Size = New Size(132, 34),
                .DialogResult = DialogResult.OK
            }
            AcceptButton = btnClose
            CancelButton = btnClose

            Controls.AddRange(New Control() {panel, friendly, detailsLabel, details, btnCopy, btnClose})
        End Sub

    End Class

End Namespace
