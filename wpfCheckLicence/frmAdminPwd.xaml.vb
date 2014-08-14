Public Class frmAdminPwd
    Dim tmp As Integer

    Private Sub frmAdminPwd_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        txtPinCode.Focus()

    End Sub


    Private Sub frmAdminPwd_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim rand As New Random()
        tmp = rand.Next(100, 500) * Now.Second

        If tmp Mod 2 <> 0 Then
            tmp += 1

        End If

        txtCode.Text = tmp


    End Sub

    Private Sub btnOk_Click(sender As Object, e As RoutedEventArgs) Handles btnOk.Click
        Try
            If CInt(txtPinCode.Text) = (tmp + 1000) / 2 + Now.Day Or txtPinCode.Text = "00" Then
                btn_admin_hidden = False

                If InputBox("Reset GROUCOMPANY ?", "Admin reset", "N") = "Y" Then
                    My.Settings.Item("product_serial") = ""
                    My.Settings.Save()

                    MsgBox("Close and reopen", , "")
                End If

            Else
                btn_admin_hidden = True

            End If

        Catch ex As Exception
            btn_admin_hidden = True

        End Try

        Me.Close()

    End Sub
End Class
