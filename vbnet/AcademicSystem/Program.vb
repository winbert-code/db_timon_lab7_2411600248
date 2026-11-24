Imports AcademicSystem.AcademicSystem

Friend Module Program

    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_KEYDOWN As Integer = &H100

    <STAThread()>
    Friend Sub Main(args As String())
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        AppLogger.Initialize()
        AppLogger.Log("INFO", "Application starting")
        GlobalExceptionHandler.Install()
        Application.AddMessageFilter(New ActivityFilter())
        SessionMonitor.Install()
        AddHandler Application.ApplicationExit,
            Sub(sender As Object, e As EventArgs)
                If Not String.IsNullOrEmpty(Session.SessionId) Then Session.EndSession("APP_EXIT")
                AppLogger.Log("INFO", "Application exited")
            End Sub

        Dim splash As SplashForm = SplashForm.ShowStartup()
        splash.Advance(25, "Loading secure configuration...")
        System.Threading.Thread.Sleep(160)
        splash.Advance(55, "Connecting to DB_Timon...")
        System.Threading.Thread.Sleep(160)
        splash.Advance(80, "Preparing MDI workspace...")
        System.Threading.Thread.Sleep(160)
        splash.Advance(100, "Ready - all services connected")
        System.Threading.Thread.Sleep(220)
        splash.Close()

        Application.Run(New LoginForm())
    End Sub

    Private Class ActivityFilter
        Implements IMessageFilter

        Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
            If m.Msg = WM_MOUSEMOVE OrElse m.Msg = WM_LBUTTONDOWN OrElse m.Msg = WM_KEYDOWN Then
                Session.Touch()
            End If
            Return False
        End Function

    End Class

End Module
