Imports System.Data

Namespace AcademicSystem

    ' Holds the logged-in user and manages login/logout, activity tracking,
    ' concurrent-login prevention and the 30-minute inactivity timeout.
    Friend Module Session

        Friend Const InactivityMinutes As Integer = 30

        Private _lastPersistedActivity As DateTime = DateTime.MinValue

        Friend Property UserId As Integer
        Friend Property Username As String = ""
        Friend Property RoleId As Integer
        Friend Property RoleName As String = ""
        Friend Property SessionId As String = ""
        Friend Property LoginTime As DateTime
        Friend Property LastActivity As DateTime

        Friend Sub Clear()
            UserId = 0
            Username = ""
            RoleId = 0
            RoleName = ""
            SessionId = ""
            LoginTime = DateTime.MinValue
            LastActivity = DateTime.MinValue
            _lastPersistedActivity = DateTime.MinValue
        End Sub

        ' Called after the password has been verified. Throws when the same
        ' account already has an ACTIVE row in user_sessions.
        Friend Sub StartSession(pUserId As Integer, pUsername As String,
                                pRoleId As Integer, pRoleName As String)
            ' A crashed/killed client cannot run EndSession, so its ACTIVE row can
        ' outlive the inactivity window. Expire those stale rows before deciding
        ' that a real user is already logged in.
        Dim staleParams As New Dictionary(Of String, Object) From {
            {"@userId", pUserId},
            {"@minutes", InactivityMinutes}
        }
        Dim stale = Db.FetchTable(
                "SELECT session_id FROM user_sessions " &
                "WHERE user_id = @userId AND status = 'ACTIVE' " &
                "AND last_activity < DATE_SUB(NOW(), INTERVAL @minutes MINUTE)",
                staleParams, "Admin")

        If stale.Rows.Count > 0 Then
            For Each row As DataRow In stale.Rows
                AuditLogger.Log("TIMEOUT", "user_sessions", Convert.ToString(row("session_id")),
                                "ACTIVE", "stale session auto-expired at next login")
            Next
            Db.Execute(
                "UPDATE user_sessions SET status = 'TIMED_OUT', logout_time = NOW(), " &
                "last_activity = NOW() WHERE user_id = @userId AND status = 'ACTIVE' " &
                "AND last_activity < DATE_SUB(NOW(), INTERVAL @minutes MINUTE)",
                staleParams, "Admin")
        End If

        Dim existing = Db.FetchTable(
                "SELECT session_id, login_time FROM user_sessions " &
                "WHERE user_id = @userId AND status = 'ACTIVE'",
                New Dictionary(Of String, Object) From {{"@userId", pUserId}},
                "Admin")

            If existing.Rows.Count > 0 Then
                Throw New InvalidOperationException(
                    "This account is already logged in. Close the other session first.")
            End If

            Dim sessionGuid As String = Guid.NewGuid().ToString()
            Dim now As DateTime = DateTime.Now

            Db.Execute(
                "INSERT INTO user_sessions " &
                "(session_id, user_id, username, login_time, last_activity, status, host_name) " &
                "VALUES (@sid, @userId, @username, @loginTime, @lastActivity, 'ACTIVE', @host)",
                New Dictionary(Of String, Object) From {
                    {"@sid", sessionGuid},
                    {"@userId", pUserId},
                    {"@username", pUsername},
                    {"@loginTime", now},
                    {"@lastActivity", now},
                    {"@host", Environment.MachineName}
                }, "Admin")

            Session.UserId = pUserId
            Session.Username = pUsername
            Session.RoleId = pRoleId
            Session.RoleName = pRoleName
            SessionId = sessionGuid
            LoginTime = now
            LastActivity = now
            _lastPersistedActivity = now

            AuditLogger.Log("LOGIN", "users", pUserId.ToString(), "",
                            "session " & sessionGuid)
        End Sub

        ' Called by the global message filter. In-memory activity is updated
        ' on every event; the database is persisted at most every 30 seconds.
        Friend Sub Touch()
            If String.IsNullOrEmpty(SessionId) Then Return

            LastActivity = DateTime.Now
            If (LastActivity - _lastPersistedActivity).TotalSeconds >= 30 Then
                Db.Execute(
                    "UPDATE user_sessions SET last_activity = @lastActivity " &
                    "WHERE session_id = @sid",
                    New Dictionary(Of String, Object) From {
                        {"@lastActivity", LastActivity},
                        {"@sid", SessionId}
                    }, "Admin")
                _lastPersistedActivity = LastActivity
            End If
        End Sub

        Friend Function IsTimedOut() As Boolean
            If String.IsNullOrEmpty(SessionId) Then Return False
            Return (DateTime.Now - LastActivity).TotalMinutes >= InactivityMinutes
        End Function

        Friend Sub EndSession(reason As String)
            If Not String.IsNullOrEmpty(SessionId) Then
                Dim status As String = If(reason = "TIMEOUT", "TIMED_OUT", "CLOSED")
                Db.Execute(
                    "UPDATE user_sessions SET status = @status, logout_time = NOW(), " &
                    "last_activity = NOW() WHERE session_id = @sid",
                    New Dictionary(Of String, Object) From {
                        {"@status", status},
                        {"@sid", SessionId}
                    }, "Admin")

                AuditLogger.Log("LOGOUT", "users", UserId.ToString(), reason,
                                "session " & SessionId)
            End If
            Clear()
        End Sub

    End Module

End Namespace
