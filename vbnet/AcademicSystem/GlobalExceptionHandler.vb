Namespace AcademicSystem

    ' Catches unhandled UI and background errors, records them and presents a
    ' consistent, non-technical dialog instead of the default crash window.
    Friend Module GlobalExceptionHandler

        Private _installed As Boolean

        Friend Sub Install()
            If _installed Then Return
            _installed = True

            AddHandler Application.ThreadException,
                Sub(sender As Object, e As Threading.ThreadExceptionEventArgs)
                    Handle("UI thread", e.Exception)
                End Sub

            AddHandler AppDomain.CurrentDomain.UnhandledException,
                Sub(sender As Object, e As UnhandledExceptionEventArgs)
                    Dim ex As Exception = TryCast(e.ExceptionObject, Exception)
                    If ex IsNot Nothing Then Handle("Background thread", ex)
                End Sub
        End Sub

        Friend Sub Handle(source As String, ex As Exception)
            If ex Is Nothing Then Return
            AppLogger.LogException(source, ex)

            Try
                Dim owner As Form = Nothing
                For Each form As Form In Application.OpenForms
                    If form.Visible Then owner = form : Exit For
                Next

                Dim dialog As New ErrorDialogForm(source, ex)
                If owner IsNot Nothing AndAlso owner.InvokeRequired Then
                    owner.BeginInvoke(Sub() dialog.ShowDialog(owner))
                Else
                    dialog.ShowDialog(owner)
                End If
            Catch
                ' Last-resort fallback if even the custom dialog cannot show.
                MessageBox.Show("An unexpected error occurred. Details were written to the application log.",
                                "DB_Timon", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Module

End Namespace
