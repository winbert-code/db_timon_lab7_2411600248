Imports System.IO

Namespace AcademicSystem

    Public Class LogViewerForm
        Inherits Form

        Public Sub New()
            Text = "Application Log"
            ClientSize = New Size(900, 560)
            StartPosition = FormStartPosition.CenterScreen

            Dim logBox As New TextBox() With {
                .Dock = DockStyle.Fill, .Multiline = True, .ReadOnly = True,
                .ScrollBars = ScrollBars.Both, .Font = New Font("Consolas", 9),
                .BackColor = Color.White
            }
            Try
                logBox.Text = If(File.Exists(AppLogger.LogFilePath),
                                 File.ReadAllText(AppLogger.LogFilePath),
                                 "No log entries yet.")
            Catch ex As Exception
                logBox.Text = "Could not read the log: " & ex.Message
            End Try

            Dim btnClose As New Button() With {
                .Text = "Close", .Dock = DockStyle.Bottom, .Height = 36,
                .DialogResult = DialogResult.OK
            }
            AcceptButton = btnClose
            Controls.AddRange(New Control() {logBox, btnClose})
        End Sub

    End Class

End Namespace
