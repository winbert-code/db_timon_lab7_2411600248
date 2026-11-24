Imports System.Data
Imports System.Drawing.Drawing2D
Imports System.Linq

Namespace AcademicSystem

    ' Dependency-free custom chart control: vertical bars or pie slices drawn
    ' with GDI+. Used for grade distributions and course statistics.
    Friend Class AdvancedChartControl
        Inherits Control

        Public Enum ChartKind
            Bars
            Pie
        End Enum

        Private _data As DataTable
        Private _labelColumn As String = "Label"
        Private _valueColumn As String = "Value"
        Private _title As String = "Chart"
        Private _kind As ChartKind = ChartKind.Bars
        Private Shared ReadOnly Palette As Color() = {
            Color.FromArgb(31, 78, 121), Color.FromArgb(91, 192, 222),
            Color.FromArgb(155, 197, 61), Color.FromArgb(255, 187, 51),
            Color.FromArgb(215, 82, 82), Color.FromArgb(121, 91, 180)
        }

        Public Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or
                     ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw, True)
            BackColor = Color.White
            ForeColor = Color.FromArgb(45, 55, 65)
            Font = New Font("Segoe UI", 9)
        End Sub

        Friend Sub SetData(table As DataTable, labelColumn As String, valueColumn As String,
                           title As String, kind As ChartKind)
            _data = table
            _labelColumn = labelColumn
            _valueColumn = valueColumn
            _title = title
            _kind = kind
            Invalidate()
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Dim g As Graphics = e.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.Clear(BackColor)

            Dim border As New Rectangle(0, 0, Width - 1, Height - 1)
            Using pen As New Pen(Color.FromArgb(225, 231, 237))
                g.DrawRectangle(pen, border)
            End Using

            Using titleFont As New Font("Segoe UI", 11, FontStyle.Bold)
                Using titleBrush As New SolidBrush(Color.FromArgb(31, 78, 121))
                    g.DrawString(_title, titleFont, titleBrush, 14, 12)
                End Using
            End Using
            g.DrawLine(New Pen(Color.FromArgb(230, 235, 240)), 14, 38, Width - 14, 38)

            If _data Is Nothing OrElse _data.Rows.Count = 0 Then
                g.DrawString("No data available", Font, Brushes.Gray, 20, 70)
                Return
            End If

            Dim series As New List(Of KeyValuePair(Of String, Decimal))()
            For Each row As DataRow In _data.Rows
                Dim label As String = Convert.ToString(row(_labelColumn))
                Dim value As Decimal = 0
                If Not Decimal.TryParse(Convert.ToString(row(_valueColumn)), value) Then value = 0
                series.Add(New KeyValuePair(Of String, Decimal)(label, value))
            Next

            If series.Count = 0 Then Return
            If _kind = ChartKind.Pie Then
                DrawPie(g, series)
            Else
                DrawBars(g, series)
            End If
        End Sub

        Private Sub DrawBars(g As Graphics, series As List(Of KeyValuePair(Of String, Decimal)))
            Dim plotX As Integer = 58
            Dim plotY As Integer = 58
            Dim plotW As Integer = Math.Max(10, Width - 150)
            Dim plotH As Integer = Math.Max(10, Height - 105)
            Dim maxValue As Decimal = series.Max(Function(item) item.Value)
            If maxValue <= 0 Then maxValue = 1

            For i As Integer = 0 To 4
                Dim y As Integer = plotY + plotH - CInt(plotH * i / 4)
                g.DrawLine(New Pen(Color.FromArgb(238, 242, 246)), plotX, y, plotX + plotW, y)
                g.DrawString(Math.Round(maxValue * i / 4, 0).ToString(), Font, Brushes.Gray, 12, y - 7)
            Next

            Dim slot As Integer = Math.Max(18, plotW \ series.Count)
            Dim barW As Integer = Math.Max(8, Math.Min(46, slot - 12))

            For i As Integer = 0 To series.Count - 1
                Dim value As Decimal = series(i).Value
                Dim barH As Integer = CInt(plotH * value / maxValue)
                If barH < 2 Then barH = 2
                Dim x As Integer = plotX + 12 + i * slot
                Dim y As Integer = plotY + plotH - barH

                Using brush As New LinearGradientBrush(New Rectangle(x, y, barW, barH),
                                                       Palette(i Mod Palette.Length),
                                                       Color.FromArgb(230, 240, 248),
                                                       LinearGradientMode.Vertical)
                    g.FillRectangle(brush, x, y, barW, barH)
                End Using
                g.DrawRectangle(New Pen(Color.FromArgb(210, 220, 230)), x, y, barW, barH)

                Dim label As String = series(i).Key
                If label.Length > 11 Then label = label.Substring(0, 10) & "."
                g.DrawString(label, Font, Brushes.DimGray, x - 6, plotY + plotH + 8)
                Using valueBrush As New SolidBrush(Color.FromArgb(31, 78, 121))
                    g.DrawString(value.ToString("0.0"), Font, valueBrush, x, y - 16)
                End Using
            Next
        End Sub

        Private Sub DrawPie(g As Graphics, series As List(Of KeyValuePair(Of String, Decimal)))
            Dim total As Decimal = series.Sum(Function(item) item.Value)
            If total <= 0 Then total = 1

            Dim rect As New Rectangle(28, 62, Math.Min(240, Width \ 2 - 40), Math.Min(240, Height - 120))
            Dim start As Single = 0
            For i As Integer = 0 To series.Count - 1
                Dim sweep As Single = CSng(360 * series(i).Value / total)
                Using brush As New SolidBrush(Palette(i Mod Palette.Length))
                    g.FillPie(brush, rect, start, sweep)
                End Using
                start += sweep
            Next
            g.DrawEllipse(New Pen(Color.White, 2), rect)

            Dim legendX As Integer = rect.Right + 34
            Dim legendY As Integer = 72
            For i As Integer = 0 To series.Count - 1
                Using brush As New SolidBrush(Palette(i Mod Palette.Length))
                    g.FillRectangle(brush, legendX, legendY, 14, 14)
                End Using
                Dim text As String = series(i).Key & "  (" & series(i).Value.ToString("0") & ")"
                If text.Length > 38 Then text = text.Substring(0, 37) & "."
                g.DrawString(text, Font, Brushes.DimGray, legendX + 22, legendY - 1)
                legendY += 26
                If legendY > Height - 24 Then Exit For
            Next
        End Sub

    End Class

End Namespace
