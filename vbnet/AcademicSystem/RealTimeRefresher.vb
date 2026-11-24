Imports System.Data
Imports System.Threading.Tasks

Namespace AcademicSystem

    ' Runs database reads off the UI thread on a configurable interval and
    ' applies the result back on the UI thread. Safe to stop and restart.
    Friend Class RealTimeRefresher
        Private ReadOnly _owner As Form
        Private ReadOnly _fetch As Func(Of DataTable)
        Private ReadOnly _apply As Action(Of DataTable)
        Private WithEvents _timer As Timer
        Private _busy As Boolean
        Private _count As Integer
        Private _lastRefresh As DateTime

        Friend ReadOnly Property RefreshCount As Integer
            Get
                Return _count
            End Get
        End Property

        Friend ReadOnly Property LastRefresh As DateTime
            Get
                Return _lastRefresh
            End Get
        End Property

        Friend Sub New(owner As Form, fetch As Func(Of DataTable), apply As Action(Of DataTable))
            _owner = owner
            _fetch = fetch
            _apply = apply
        End Sub

        Friend Sub Start(Optional seconds As Integer = 0)
            StopRefresh()
            Dim interval As Integer = If(seconds > 0, seconds, AppSettings.Current.RefreshIntervalSeconds)
            If interval < 3 Then interval = 3
            _timer = New Timer() With {.Interval = interval * 1000}
            AddHandler _timer.Tick, AddressOf Tick
            _timer.Start()
        End Sub

        Friend Sub StopRefresh()
            If _timer IsNot Nothing Then
                _timer.Stop()
                RemoveHandler _timer.Tick, AddressOf Tick
                _timer.Dispose()
                _timer = Nothing
            End If
        End Sub

        Private Sub Tick(sender As Object, e As EventArgs)
            If _busy OrElse _owner.IsDisposed Then Return
            _busy = True

            Dim refreshTask As Task(Of DataTable) = Task.Run(Function() _fetch())
            refreshTask.ContinueWith(
                Sub(t As Task(Of DataTable))
                    If _owner.IsDisposed OrElse _owner.Handle = IntPtr.Zero Then
                        _busy = False
                        Return
                    End If

                    _owner.BeginInvoke(Sub()
                                           Try
                                               If t.Status = TaskStatus.RanToCompletion Then
                                                   _count += 1
                                                   _lastRefresh = DateTime.Now
                                                   _apply(t.Result)
                                               ElseIf t.Exception IsNot Nothing Then
                                                   AppLogger.Log("WARN", "Auto-refresh failed: " & t.Exception.Message)
                                               End If
                                           Finally
                                               _busy = False
                                           End Try
                                       End Sub)
                End Sub)
        End Sub

    End Class

End Namespace
