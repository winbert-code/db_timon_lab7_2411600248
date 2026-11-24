Imports System.IO
Imports System.Text.Json

Namespace AcademicSystem

    ' User-customisable behaviour saved as JSON under App_Data.
    Friend Class AppSettings

        Public Property RefreshIntervalSeconds As Integer = 10
        Public Property RowColoring As Boolean = True
        Public Property Theme As String = "Light"
        Public Property ShowNotifications As Boolean = True

        Private Shared _current As AppSettings

        Friend Shared ReadOnly Property Current As AppSettings
            Get
                If _current Is Nothing Then _current = Load()
                Return _current
            End Get
        End Property

        Friend Shared Function Load() As AppSettings
            Try
                Dim path As String = FilePath()
                If File.Exists(path) Then
                    Dim loaded = JsonSerializer.Deserialize(Of AppSettings)(File.ReadAllText(path))
                    If loaded IsNot Nothing Then
                        _current = loaded
                        Return loaded
                    End If
                End If
            Catch
                ' Fall back to defaults when the file is missing or invalid.
            End Try
            _current = New AppSettings()
            Return _current
        End Function

        Friend Sub Save()
            Try
                Dim path As String = FilePath()
                Dim folder As String = System.IO.Path.GetDirectoryName(path)
                If Not Directory.Exists(folder) Then Directory.CreateDirectory(folder)
                Dim json As String = JsonSerializer.Serialize(Me, New JsonSerializerOptions With {.WriteIndented = True})
                File.WriteAllText(path, json)
                _current = Me
            Catch ex As Exception
                Throw New InvalidOperationException("Could not save settings: " & ex.Message, ex)
            End Try
        End Sub

        Private Shared Function FilePath() As String
            Return Path.Combine(Application.StartupPath, "App_Data", "settings.json")
        End Function

    End Class

End Namespace
