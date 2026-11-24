Namespace AcademicSystem

    ' Startup splash with a real progress bar. Help > Show Splash opens a
    ' preview copy so the UX can be demonstrated after login as well.
    Public Class SplashForm
        Inherits Form

        Private _bar As ProgressBar
        Private _message As Label
        Private _preview As Boolean
        Private _step As Integer

        Public Sub New(Optional preview As Boolean = False)
            _preview = preview
            Text = "DB_Timon Startup"
            FormBorderStyle = FormBorderStyle.None
            StartPosition = FormStartPosition.CenterScreen
            ClientSize = New Size(560, 250)
            BackColor = Color.FromArgb(18, 46, 76)

            Dim band As New Panel() With {.Dock = DockStyle.Top, .Height = 8, .BackColor = Color.FromArgb(91, 192, 222)}
            Dim title As New Label() With {
                .Text = "DB_Timon", .Font = New Font("Segoe UI", 28, FontStyle.Bold),
                .ForeColor = Color.White, .Location = New Point(38, 44), .Size = New Size(480, 50)
            }
            Dim subtitle As New Label() With {
                .Text = "Academic Management System", .Font = New Font("Segoe UI", 13),
                .ForeColor = Color.FromArgb(190, 216, 236), .Location = New Point(40, 96), .Size = New Size(480, 30)
            }

            _bar = New ProgressBar() With {
                .Location = New Point(40, 150), .Size = New Size(480, 22),
                .Style = ProgressBarStyle.Continuous, .Minimum = 0, .Maximum = 100
            }
            _message = New Label() With {
                .Text = "Starting application...", .Font = New Font("Segoe UI", 10),
                .ForeColor = Color.FromArgb(220, 236, 248), .Location = New Point(40, 184), .Size = New Size(480, 24)
            }

            Controls.AddRange(New Control() {band, title, subtitle, _bar, _message})

            If preview Then
                _bar.Value = 100
                _message.Text = "Ready - all services connected"
                Dim btn As New Button() With {
                    .Text = "Close preview", .Location = New Point(390, 210), .Size = New Size(130, 28),
                    .DialogResult = DialogResult.OK
                }
                AddHandler btn.Click, Sub(sender, e) Close()
                Controls.Add(btn)
                AcceptButton = btn
            End If
        End Sub

        Friend Sub Advance(value As Integer, message As String)
            If value < 0 Then value = 0
            If value > 100 Then value = 100
            _bar.Value = value
            _message.Text = message
            Application.DoEvents()
        End Sub

        Friend Shared Function ShowStartup() As SplashForm
            Dim splash As New SplashForm(False)
            splash.Show()
            splash.Refresh()
            Return splash
        End Function

    End Class

End Namespace
