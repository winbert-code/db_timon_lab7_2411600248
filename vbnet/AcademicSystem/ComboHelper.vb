Imports System.Data

Namespace AcademicSystem

    ' Safely reads the numeric ValueMember from a data-bound ComboBox.
    ' Depending on binding order WinForms can expose a DataRowView through
    ' SelectedValue, so Convert.ToInt32 would throw InvalidCastException.
    Friend Module ComboHelper

        Friend Function SelectedId(combo As ComboBox) As Integer
            If combo Is Nothing Then Return 0

            Dim value As Object = combo.SelectedValue
            If value IsNot Nothing AndAlso Not TypeOf value Is DataRowView Then
                Dim parsed As Integer
                If Integer.TryParse(Convert.ToString(value), parsed) Then Return parsed
            End If

            Dim row As DataRowView = TryCast(combo.SelectedItem, DataRowView)
            If row IsNot Nothing AndAlso row.Row.Table.Columns.Contains("Value") Then
                Dim parsed As Integer
                If Integer.TryParse(Convert.ToString(row("Value")), parsed) Then Return parsed
            End If

            Return 0
        End Function

    End Module

End Namespace
