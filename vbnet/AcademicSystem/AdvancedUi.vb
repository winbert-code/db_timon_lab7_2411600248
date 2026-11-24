Imports System.Linq

Namespace AcademicSystem

    ' Shared helpers for polished, consistent WinForms UI behaviour.
    Friend Module AdvancedUi

        Friend Sub ApplyGridFeatures(grid As DataGridView,
                                     Optional gradeColumn As String = "Grade",
                                     Optional averageColumn As String = "Average Grade")
            If grid Is Nothing Then Return

            grid.AllowUserToAddRows = False
            grid.AllowUserToDeleteRows = False
            grid.ReadOnly = True
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grid.MultiSelect = False
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            grid.BackgroundColor = Color.White
            grid.GridColor = Color.FromArgb(224, 229, 235)
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 78, 121)
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            grid.EnableHeadersVisualStyles = False
            grid.RowHeadersVisible = False
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252)

            If Convert.ToString(grid.Tag) = "advanced" Then Return
            grid.Tag = "advanced"

            AddHandler grid.CellFormatting,
                Sub(sender As Object, e As DataGridViewCellFormattingEventArgs)
                    If Not AppSettings.Current.RowColoring Then Return
                    Dim name As String = If(e.ColumnIndex >= 0 AndAlso e.ColumnIndex < grid.Columns.Count,
                                            grid.Columns(e.ColumnIndex).Name, "")
                    If Not (name = gradeColumn OrElse name = averageColumn OrElse name.Contains("Grade")) Then Return
                    If e.Value Is Nothing OrElse Not IsNumeric(e.Value) Then Return

                    Dim value As Decimal = Convert.ToDecimal(e.Value)
                    Dim color As Color
                    If value >= 90 Then
                        color = Color.FromArgb(214, 243, 226)
                    ElseIf value >= 75 Then
                        color = Color.FromArgb(231, 246, 226)
                    ElseIf value >= 60 Then
                        color = Color.FromArgb(255, 247, 214)
                    Else
                        color = Color.FromArgb(255, 225, 225)
                    End If
                    e.CellStyle.BackColor = color
                    e.CellStyle.ForeColor = Color.FromArgb(30, 55, 40)
                End Sub
        End Sub

        Friend Sub ApplyTooltips(form As Form)
            If form Is Nothing Then Return
            Dim tip As New ToolTip() With {
                .AutoPopDelay = 5000,
                .InitialDelay = 400,
                .ReshowDelay = 200,
                .ShowAlways = True
            }

            For Each control As Control In AllControls(form)
                Dim text As String = ToolTipFor(control)
                If Not String.IsNullOrWhiteSpace(text) Then tip.SetToolTip(control, text)
            Next
        End Sub

        Private Function AllControls(parent As Control) As IEnumerable(Of Control)
            Dim result As New List(Of Control)()
            For Each child As Control In parent.Controls
                result.Add(child)
                result.AddRange(AllControls(child))
            Next
            Return result
        End Function

        Private Function ToolTipFor(control As Control) As String
            Dim name As String = control.Name.ToLowerInvariant()
            If TypeOf control Is DataGridView Then Return "Right-click a column header to sort. Low grades are shaded automatically."
            If TypeOf control Is Button Then
                If name.Contains("save") Then Return "Save changes to the database."
                If name.Contains("search") Then Return "Run the search with the current criteria."
                If name.Contains("refresh") OrElse name.Contains("reload") Then Return "Reload the latest data from DB_Timon."
                If name.Contains("export") Then Return "Export the current results to CSV, Excel or PDF."
                If name.Contains("undo") Then Return "Undo the last change in this form."
                If name.Contains("redo") Then Return "Redo the last undone change."
                If name.Contains("close") OrElse name.Contains("logout") Then Return "Close this window."
                Return "Click to continue."
            End If
            If TypeOf control Is TextBox Then
                If name.Contains("password") Then Return "Passwords are never stored in plain text."
                If name.Contains("search") Then Return "Type a name, code or keyword and press Enter."
                Return "Enter a value."
            End If
            If TypeOf control Is ComboBox Then Return "Select an option from the list."
            If TypeOf control Is NumericUpDown Then Return "Change the refresh interval in seconds."
            Return ""
        End Function

        Friend Sub CentreChild(child As Form, parent As Form)
            If child Is Nothing OrElse parent Is Nothing Then Return
            child.StartPosition = FormStartPosition.Manual
            Dim x As Integer = parent.Left + Math.Max(0, (parent.ClientSize.Width - child.Width) \ 2)
            Dim y As Integer = parent.Top + Math.Max(0, (parent.ClientSize.Height - child.Height) \ 2)
            child.Location = New Point(x, y)
        End Sub

    End Module

End Namespace
