Imports System.Data

Namespace AcademicSystem

    ' Data visualisation window: custom bar/pie charts plus an advanced grid.
    Public Class VisualizationForm
        Inherits Form

        Private _barChart As AdvancedChartControl
        Private _pieChart As AdvancedChartControl
        Private _grid As DataGridView
        Private _status As Label
        Private WithEvents _timer As Timer
        Private _refreshCount As Integer

        Public Sub New()
            Text = "Academic Visualizations"
            ClientSize = New Size(1140, 600)
            StartPosition = FormStartPosition.CenterScreen
            BackColor = Color.FromArgb(248, 250, 252)

            Dim header As New Label() With {
                .Text = "Academic data visualization - grade distribution and course statistics",
                .Dock = DockStyle.Top, .Height = 54,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(31, 78, 121), .Padding = New Padding(18, 13, 0, 0)
            }

            Dim charts As New TableLayoutPanel() With {
                .Dock = DockStyle.Top, .Height = 320, .ColumnCount = 2,
                .RowCount = 1, .Padding = New Padding(14, 10, 14, 6)
            }
            charts.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 55))
            charts.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45))
            _barChart = New AdvancedChartControl() With {.Dock = DockStyle.Fill}
            _pieChart = New AdvancedChartControl() With {.Dock = DockStyle.Fill}
            charts.Controls.Add(_barChart, 0, 0)
            charts.Controls.Add(_pieChart, 1, 0)

            _grid = New DataGridView() With {.Dock = DockStyle.Fill, .Margin = New Padding(14, 6, 14, 6)}
            AdvancedUi.ApplyGridFeatures(_grid, averageColumn:="Average Grade")

            Dim bottom As New Panel() With {.Dock = DockStyle.Bottom, .Height = 62, .Padding = New Padding(16, 12, 16, 8)}
            _status = New Label() With {
                .Dock = DockStyle.Fill, .Text = "Loading visualizations...",
                .ForeColor = Color.SeaGreen, .Font = New Font("Segoe UI", 10)
            }
            Dim btnRefresh As New Button() With {
                .Text = "Refresh now", .Dock = DockStyle.Right, .Width = 140, .Height = 34
            }
            AddHandler btnRefresh.Click, Sub(sender, e) RefreshAll()
            bottom.Controls.AddRange(New Control() {_status, btnRefresh})

            Dim btnClose As New Button() With {
                .Text = "Close", .Dock = DockStyle.Bottom, .Height = 34,
                .DialogResult = DialogResult.OK
            }
            AcceptButton = btnClose

            Controls.AddRange(New Control() {_grid, charts, header, bottom, btnClose})
            SessionUi.AddStatusBar(Me)
            AdvancedUi.ApplyTooltips(Me)

            RefreshAll()
            _timer = New Timer() With {.Interval = Math.Max(3000, AppSettings.Current.RefreshIntervalSeconds * 1000)}
            _timer.Start()
        End Sub

        Private Sub _timer_Tick(sender As Object, e As EventArgs) Handles _timer.Tick
            RefreshAll()
        End Sub

        Private Sub RefreshAll()
            Try
                Dim courses = Db.FetchTable(
                    "SELECT course_code AS Label, average_grade AS Value " &
                    "FROM course_performance_analytics ORDER BY course_code")
                _barChart.SetData(courses, "Label", "Value",
                                  "Average grade by course", AdvancedChartControl.ChartKind.Bars)

                Dim types = Db.FetchTable(
                    "SELECT at.type_name AS Label, COUNT(*) AS Value " &
                    "FROM grades g JOIN assignment_types at ON at.type_id = g.type_id " &
                    "GROUP BY at.type_name ORDER BY at.type_id")
                _pieChart.SetData(types, "Label", "Value",
                                  "Submissions by assignment type", AdvancedChartControl.ChartKind.Pie)

                _grid.DataSource = Db.FetchTable(
                    "SELECT course_code AS 'Course Code', course_name AS 'Course', " &
                    "enrolled_students AS 'Enrolled', grades_submitted AS 'Grades', " &
                    "average_grade AS 'Average Grade', minimum_grade AS 'Minimum', " &
                    "maximum_grade AS 'Maximum' FROM course_performance_analytics ORDER BY course_code")
                AdvancedUi.ApplyGridFeatures(_grid, averageColumn:="Average Grade")

                _refreshCount += 1
                _status.ForeColor = Color.SeaGreen
                _status.Text = "Auto-refresh #" & _refreshCount & " at " & DateTime.Now.ToString("HH:mm:ss") &
                               "  |  " & courses.Rows.Count & " courses, " & types.Rows.Count & " assignment types"
            Catch ex As Exception
                _status.ForeColor = Color.Firebrick
                _status.Text = "Refresh failed: " & ex.Message
                AppLogger.LogException("visualization refresh", ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
            If _timer IsNot Nothing Then _timer.Stop()
            MyBase.OnFormClosed(e)
        End Sub

    End Class

End Namespace
