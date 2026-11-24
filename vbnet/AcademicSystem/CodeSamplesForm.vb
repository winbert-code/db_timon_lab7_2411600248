Imports System.IO

Namespace AcademicSystem

    ' Displays the two code samples requested by the laboratory activity.
    Public Class CodeSamplesForm
        Inherits Form

        Public Sub New()
            Text = "Lab 7 - Code Samples"
            StartPosition = FormStartPosition.CenterScreen
            ClientSize = New Size(1000, 620)
            BackColor = Color.FromArgb(248, 250, 252)

            Dim header As New Label() With {
                .Text = "Lab 7 implementation highlights", .Dock = DockStyle.Top, .Height = 58,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121), .Padding = New Padding(18, 14, 0, 0)
            }

            Dim tab As New TabControl() With {.Dock = DockStyle.Fill, .Padding = New Point(12, 4)}

            Dim pageGlobal As New TabPage("Global exception handler")
            Dim globalCode As New TextBox() With {
                .Dock = DockStyle.Fill, .Multiline = True, .ReadOnly = True,
                .ScrollBars = ScrollBars.Both, .Font = New Font("Consolas", 10),
                .BackColor = Color.White,
                .Text =
                    "' GlobalExceptionHandler.vb" & vbCrLf &
                    "AddHandler Application.ThreadException," & vbCrLf &
                    "    Sub(sender, e) Handle(""UI thread"", e.Exception)" & vbCrLf &
                    "" & vbCrLf &
                    "AddHandler AppDomain.CurrentDomain.UnhandledException," & vbCrLf &
                    "    Sub(sender, e) Handle(""Background thread"", e.ExceptionObject)" & vbCrLf &
                    "" & vbCrLf &
                    "Friend Sub Handle(source As String, ex As Exception)" & vbCrLf &
                    "    AppLogger.LogException(source, ex)   ' file + audit_log" & vbCrLf &
                    "    Using dialog As New ErrorDialogForm(source, ex)" & vbCrLf &
                    "        dialog.ShowDialog(owner)         ' friendly custom dialog" & vbCrLf &
                    "    End Using" & vbCrLf &
                    "End Sub" & vbCrLf &
                    "" & vbCrLf &
                    "' Benefit: one handler records the error, keeps the app alive," & vbCrLf &
                    "' and gives the user a consistent message instead of a crash."
                }
            pageGlobal.Controls.Add(globalCode)

            Dim pageRealtime As New TabPage("Real-time data synchronization")
            Dim realtimeCode As New TextBox() With {
                .Dock = DockStyle.Fill, .Multiline = True, .ReadOnly = True,
                .ScrollBars = ScrollBars.Both, .Font = New Font("Consolas", 10),
                .BackColor = Color.White,
                .Text =
                    "' RealTimeRefresher.vb" & vbCrLf &
                    "_timer = New Timer() With {.Interval = AppSettings.Current.RefreshIntervalSeconds * 1000}" & vbCrLf &
                    "AddHandler _timer.Tick, AddressOf Tick" & vbCrLf &
                    "" & vbCrLf &
                    "Private Sub Tick(sender As Object, e As EventArgs)" & vbCrLf &
                    "    If _busy Then Return" & vbCrLf &
                    "    Dim task = Task.Run(Function() _fetch())          ' database work off UI thread" & vbCrLf &
                    "    task.ContinueWith(Sub(t)" & vbCrLf &
                    "        _owner.BeginInvoke(Sub()" & vbCrLf &
                    "            _apply(t.Result)                       ' update grid on UI thread" & vbCrLf &
                    "            _count += 1" & vbCrLf &
                    "            _lastRefresh = DateTime.Now" & vbCrLf &
                    "        End Sub)" & vbCrLf &
                    "    End Sub)" & vbCrLf &
                    "End Sub" & vbCrLf &
                    "" & vbCrLf &
                    "' SettingsForm lets the user change the interval without recompiling."
                }
            pageRealtime.Controls.Add(realtimeCode)

            tab.TabPages.Add(pageGlobal)
            tab.TabPages.Add(pageRealtime)

            Dim btnClose As New Button() With {
                .Text = "Close", .Location = New Point(850, 570), .Size = New Size(120, 34),
                .DialogResult = DialogResult.OK
            }
            AcceptButton = btnClose

            Controls.AddRange(New Control() {tab, header, btnClose})
        End Sub

    End Class

End Namespace
