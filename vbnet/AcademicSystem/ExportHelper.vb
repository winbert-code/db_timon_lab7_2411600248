Imports System.IO
Imports System.Text

Namespace AcademicSystem

    ' Formatted exports for reports: CSV, Excel-readable HTML, plain text and PDF.
    Friend Module ExportHelper

        Friend Sub ExportTable(table As DataTable, filePath As String)
            If table Is Nothing Then Throw New ArgumentNullException(NameOf(table))
            Dim extension As String = Path.GetExtension(filePath).ToLowerInvariant()

            Select Case extension
                Case ".xls", ".xlsx", ".htm", ".html"
                    WriteExcel(table, filePath)
                Case ".pdf"
                    Dim lines As New List(Of String)()
                    For Each row As DataRow In table.Rows
                        Dim values As New List(Of String)()
                        For Each column As DataColumn In table.Columns
                            values.Add(Convert.ToString(row(column)))
                        Next
                        lines.Add(String.Join(" | ", values))
                    Next
                    PdfExporter.Export("DB_Timon report", lines, filePath)
                Case ".txt"
                    WriteText(table, filePath)
                Case Else
                    WriteCsv(table, filePath)
            End Select
        End Sub

        Private Sub WriteCsv(table As DataTable, filePath As String)
            Dim csv As New StringBuilder()
            Dim headers As New List(Of String)()
            For Each column As DataColumn In table.Columns
                headers.Add(EscapeCsv(column.ColumnName))
            Next
            csv.AppendLine(String.Join(",", headers))

            For Each row As DataRow In table.Rows
                Dim values As New List(Of String)()
                For Each column As DataColumn In table.Columns
                    values.Add(EscapeCsv(Convert.ToString(row(column))))
                Next
                csv.AppendLine(String.Join(",", values))
            Next
            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8)
        End Sub

        Private Sub WriteText(table As DataTable, filePath As String)
            Dim text As New StringBuilder()
            For Each column As DataColumn In table.Columns
                text.Append(column.ColumnName.PadRight(22))
            Next
            text.AppendLine()
            text.AppendLine(New String("-"c, 100))
            For Each row As DataRow In table.Rows
                For Each column As DataColumn In table.Columns
                    Dim value As String = Convert.ToString(row(column))
                    If value.Length > 20 Then value = value.Substring(0, 19) & "."
                    text.Append(value.PadRight(22))
                Next
                text.AppendLine()
            Next
            File.WriteAllText(filePath, text.ToString(), Encoding.UTF8)
        End Sub

        Private Sub WriteExcel(table As DataTable, filePath As String)
            Dim html As New StringBuilder()
            html.AppendLine("<html xmlns:o='urn:schemas-microsoft-com:office:office' ")
            html.AppendLine("xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>")
            html.AppendLine("<head><meta charset='utf-8'><style>")
            html.AppendLine("table{border-collapse:collapse;font-family:Segoe UI;font-size:10pt}")
            html.AppendLine("th{background:#1F4E79;color:white;padding:6px;border:1px solid #9BB7CE}")
            html.AppendLine("td{padding:5px;border:1px solid #C9D6E2}</style></head><body>")
            html.AppendLine("<h3>DB_Timon report - " & DateTime.Now.ToString("yyyy-MM-dd HH:mm") & "</h3>")
            html.AppendLine("<table><tr>")
            For Each column As DataColumn In table.Columns
                html.Append("<th>" & Web(HtmlEncode(column.ColumnName)) & "</th>")
            Next
            html.AppendLine("</tr>")
            For Each row As DataRow In table.Rows
                html.Append("<tr>")
                For Each column As DataColumn In table.Columns
                    html.Append("<td>" & Web(HtmlEncode(Convert.ToString(row(column)))) & "</td>")
                Next
                html.AppendLine("</tr>")
            Next
            html.AppendLine("</table></body></html>")
            File.WriteAllText(filePath, html.ToString(), Encoding.UTF8)
        End Sub

        Private Function HtmlEncode(value As String) As String
            If String.IsNullOrEmpty(value) Then Return ""
            Return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("""", "&quot;")
        End Function

        Private Function Web(value As String) As String
            If String.IsNullOrEmpty(value) Then Return "&nbsp;"
            Return value.Replace(vbCrLf, "<br>")
        End Function

        Private Function EscapeCsv(value As String) As String
            If value Is Nothing Then Return ""
            If value.Contains(",") OrElse value.Contains("""") OrElse value.Contains(vbCrLf) Then
                Return """" & value.Replace("""", """""") & """"
            End If
            Return value
        End Function

    End Module

End Namespace
