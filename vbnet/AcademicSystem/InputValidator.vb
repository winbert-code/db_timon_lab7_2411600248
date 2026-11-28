Imports System.Linq
Imports System.Text.RegularExpressions

Namespace AcademicSystem

    ' Shared client-side validation and sanitisation helpers used by all
    ' Lab 6 forms. Database triggers remain the authoritative final check.
    Friend Module InputValidator

        Private ReadOnly UsernamePattern As New Regex("^[A-Za-z][A-Za-z0-9._-]{2,29}$")
        Private ReadOnly EmailPattern As New Regex("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
        Private ReadOnly CourseCodePattern As New Regex("^[A-Z]{2,4}[0-9]{3}$")

        Friend Function Sanitize(value As String) As String
            If value Is Nothing Then Return ""
            Dim result As New System.Text.StringBuilder()
            For Each ch As Char In value.Trim()
                If AscW(ch) >= 32 AndAlso AscW(ch) <> 127 Then result.Append(ch)
            Next
            Return result.ToString()
        End Function

        Friend Function IsValidUsername(value As String) As Boolean
            Return UsernamePattern.IsMatch(Sanitize(value))
        End Function

        Friend Function IsStrongPassword(value As String) As Boolean
            If value Is Nothing OrElse value.Length < 8 Then Return False
            Return value.Any(Function(c As Char) Char.IsUpper(c)) AndAlso
                   value.Any(Function(c As Char) Char.IsLower(c)) AndAlso
                   value.Any(Function(c As Char) Char.IsDigit(c)) AndAlso
                   value.Any(Function(c As Char) Not Char.IsLetterOrDigit(c))
        End Function

        Friend Function IsValidEmail(value As String) As Boolean
            Return EmailPattern.IsMatch(Sanitize(value))
        End Function

        Friend Function IsUniqueUsername(username As String) As Boolean
            Dim found = Db.FetchTable(
                "SELECT COUNT(*) AS total FROM users WHERE username = @username",
                New Dictionary(Of String, Object) From {{"@username", Sanitize(username)}})
            Return Convert.ToInt32(found.Rows(0)("total")) = 0
        End Function

        Friend Function IsValidCourseCode(value As String) As Boolean
            Return CourseCodePattern.IsMatch(Sanitize(value).ToUpperInvariant())
        End Function

        Friend Function IsUniqueCourseCode(code As String) As Boolean
            Dim found = Db.FetchTable(
                "SELECT COUNT(*) AS total FROM courses WHERE course_code = @code",
                New Dictionary(Of String, Object) From {{"@code", Sanitize(code).ToUpperInvariant()}})
            Return Convert.ToInt32(found.Rows(0)("total")) = 0
        End Function

        ' Escapes LIKE wildcards so user text cannot broaden a search.
        Friend Function SafeLike(value As String) As String
            Dim text As String = Sanitize(value)
            If text.Length > 100 Then text = text.Substring(0, 100)
            text = text.Replace("\", "\\").Replace("%", "\%").Replace("_", "\_")
            Return "%" & text & "%"
        End Function

        Friend Function Required(value As String, fieldName As String, ByRef message As String) As Boolean
            If String.IsNullOrWhiteSpace(Sanitize(value)) Then
                message = fieldName & " is required."
                Return False
            End If
            message = ""
            Return True
        End Function

    End Module

End Namespace
