Imports System.IO
Imports System.Text

Namespace AcademicSystem

    ' File-based application logger used by the global exception handler and
    ' by long-running UI operations. Errors are also mirrored to audit_log.
    Friend Module AppLogger

        Private ReadOnly _path As String = Path.Combine(Application.StartupPath, "App_Data", "application.log")
        Private _sync As New Object()

        Friend Sub Initialize()
            Try
                Dim folder As String = Path.GetDirectoryName(_path)
                If Not Directory.Exists(folder) Then Directory.CreateDirectory(folder)
                If Not File.Exists(_path) Then
                    File.WriteAllText(_path,
                        "=== DB_Timon application log ===" & vbCrLf &
                        "Started: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & vbCrLf,
                        Encoding.UTF8)
                End If
            Catch
                ' Logging must never prevent the application from starting.
            End Try
        End Sub

        Friend Sub Log(level As String, message As String)
            Try
                Initialize()
                Dim line As New StringBuilder()
                line.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Append(" [")
                line.Append(level).Append("] [")
                line.Append(If(String.IsNullOrWhiteSpace(Session.Username), "system", Session.Username))
                line.Append("] ").Append(message)
                SyncLock _sync
                    File.AppendAllText(_path, line.ToString() & vbCrLf, Encoding.UTF8)
                End SyncLock
            Catch
                ' Ignore secondary logging failures.
            End Try
        End Sub

        Friend Sub LogException(source As String, ex As Exception)
            Dim detail As String = ex.GetType().Name & ": " & ex.Message
            If ex.InnerException IsNot Nothing Then detail &= " | Inner: " & ex.InnerException.Message
            Log("ERROR", source & " -> " & detail)

            Try
                AuditLogger.Log("APP_ERROR", "application", source, "", detail)
            Catch
                ' The database may not be available during a fatal error.
            End Try
        End Sub

        Friend ReadOnly Property LogFilePath As String
            Get
                Return _path
            End Get
        End Property

    End Module

End Namespace
