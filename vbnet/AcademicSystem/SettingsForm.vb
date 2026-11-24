Namespace AcademicSystem

    ' Lets the user customise refresh interval, row colouring and theme.
    Public Class SettingsForm
        Inherits Form

        Private _interval As NumericUpDown
        Private _rowColors As CheckBox
        Private _theme As ComboBox
        Private _notifications As CheckBox
        Private _status As Label

        Public Sub New()
            Text = "Settings"
            ClientSize = New Size(760, 560)
            StartPosition = FormStartPosition.CenterScreen
            FormBorderStyle = FormBorderStyle.FixedDialog
            MaximizeBox = False
            BackColor = Color.FromArgb(248, 250, 252)

            Dim header As New Label() With {
                .Text = "Application settings", .Dock = DockStyle.Top, .Height = 58,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121), .Padding = New Padding(20, 14, 0, 0)
            }

            Dim panel As New Panel() With {.Location = New Point(30, 80), .Size = New Size(700, 340), .BackColor = Color.White}

            panel.Controls.Add(New Label() With {.Text = "Auto-refresh interval (seconds):", .Location = New Point(24, 30), .Size = New Size(300, 26)})
            _interval = New NumericUpDown() With {
                .Location = New Point(360, 26), .Size = New Size(150, 30),
                .Minimum = 3, .Maximum = 300, .Value = AppSettings.Current.RefreshIntervalSeconds
            }
            panel.Controls.Add(_interval)

            panel.Controls.Add(New Label() With {
                .Text = "Minimum is 3 seconds to protect the database from excessive queries.",
                .Location = New Point(24, 62), .Size = New Size(600, 22), .ForeColor = Color.DimGray
            })

            _rowColors = New CheckBox() With {
                .Text = "Colour DataGridView rows by grade value",
                .Location = New Point(24, 110), .Size = New Size(560, 28),
                .Checked = AppSettings.Current.RowColoring
            }
            panel.Controls.Add(_rowColors)

            panel.Controls.Add(New Label() With {.Text = "Theme:", .Location = New Point(24, 160), .Size = New Size(300, 26)})
            _theme = New ComboBox() With {
                .Location = New Point(360, 156), .Size = New Size(220, 30),
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            _theme.Items.AddRange(New Object() {"Light", "Blue", "High contrast"})
            _theme.SelectedItem = If(AppSettings.Current.Theme, "Light")
            panel.Controls.Add(_theme)

            _notifications = New CheckBox() With {
                .Text = "Show notification reminder on the status bar",
                .Location = New Point(24, 210), .Size = New Size(600, 28),
                .Checked = AppSettings.Current.ShowNotifications
            }
            panel.Controls.Add(_notifications)

            _status = New Label() With {
                .Text = "Settings are stored in App_Data/settings.json.",
                .Location = New Point(24, 270), .Size = New Size(640, 40),
                .ForeColor = Color.SeaGreen
            }
            panel.Controls.Add(_status)

            Dim btnSave As New Button() With {.Text = "Save settings", .Location = New Point(30, 445), .Size = New Size(170, 38)}
            AddHandler btnSave.Click, Sub(sender, e) SaveSettings()
            Dim btnClose As New Button() With {
                .Text = "Close", .Location = New Point(560, 445), .Size = New Size(140, 38),
                .DialogResult = DialogResult.OK
            }
            AcceptButton = btnClose

            Controls.AddRange(New Control() {panel, btnSave, btnClose, header})
            SessionUi.AddStatusBar(Me)
            AdvancedUi.ApplyTooltips(Me)
        End Sub

        Private Sub SaveSettings()
            Try
                Dim settings As New AppSettings() With {
                    .RefreshIntervalSeconds = Convert.ToInt32(_interval.Value),
                    .RowColoring = _rowColors.Checked,
                    .Theme = Convert.ToString(_theme.SelectedItem),
                    .ShowNotifications = _notifications.Checked
                }
                settings.Save()
                _status.ForeColor = Color.SeaGreen
                _status.Text = "Saved at " & DateTime.Now.ToString("HH:mm:ss") &
                               " - refresh every " & settings.RefreshIntervalSeconds & " seconds."
                AppLogger.Log("INFO", "Settings saved: refresh=" & settings.RefreshIntervalSeconds &
                                       "s, rowColoring=" & settings.RowColoring)
            Catch ex As Exception
                _status.ForeColor = Color.Firebrick
                _status.Text = "Save failed: " & ex.Message
            End Try
        End Sub

    End Class

End Namespace
