Imports System.Data.SqlClient
Imports System.Data

Public Class frmNewPenpc

    Private Sub frmNewPenpc_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized

        If tmpData.ToLower.Substring(0, 3) = "age" Then
            txtIDAgency.Text = tmpData.Substring(3, tmpData.Length - 3)

        End If

        txtIDPenpc.Text = maxID("tblSeatsInUse") + 1

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If btnSave.Content = "Save" Then
            ' savePenpc()

            Close()

        Else
            If txtPenpcHW.Text.Trim <> "" And txtPincode.Text.Trim <> "" Then
                '  addPenpc()

                Close()

            Else
                MsgBox("Insert all form data", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")

            End If

        End If

    End Sub


    '

    'Private Function addPenpc() As Boolean
    '    Mouse.OverrideCursor = Cursors.Wait

    '    addPenpc = False

    '    Dim sqlCon As SqlConnection = New SqlConnection()
    '    sqlCon.ConnectionString = connectionString
    '    Dim cmd As SqlCommand = sqlCon.CreateCommand()

    '    Dim transaction As SqlTransaction

    '    If cmd.Connection.State <> ConnectionState.Open Then
    '        cmd.Connection.Open()

    '    End If

    '    ' Start a local transaction
    '    transaction = sqlCon.BeginTransaction("SampleTransaction")

    '    ' Must assign both transaction object and connection
    '    ' to Command object for a pending local transaction.
    '    cmd.Connection = sqlCon
    '    cmd.Transaction = transaction

    '    cmd.CommandType = CommandType.Text
    '    cmd.CommandText = "INSERT INTO tblCompany (id, company_code, description, country_code, company_default) VALUES (@id, @company_code, @desc, @country_code, @company_default)"

    '    cmd.Parameters.Add("@id", SqlDbType.Int)
    '    cmd.Parameters.Add("@company_code", SqlDbType.Int)
    '    cmd.Parameters.Add("@desc", SqlDbType.NVarChar)
    '    cmd.Parameters.Add("@country_code", SqlDbType.NVarChar)
    '    cmd.Parameters.Add("@company_default", SqlDbType.NVarChar)

    '    cmd.Parameters("@id").Value = txtID.Text.Trim
    '    cmd.Parameters("@company_code").Value = txtID.Text.Trim
    '    cmd.Parameters("@desc").Value = txtDescription.Text.Trim
    '    cmd.Parameters("@country_code").Value = txtCountryCode.Text.Trim
    '    cmd.Parameters("@company_default").Value = cmbDefault.Text.Substring(0, 1).ToLower


    '    Try

    '        cmd.ExecuteNonQuery()

    '        '
    '        cmd.CommandText = "INSERT INTO tblProgramUser (ID, serial, company_code) VALUES (@id, @serial, @company_code)"
    '        cmd.Parameters.Clear()
    '        cmd.Parameters.Add("@id", SqlDbType.Int)
    '        cmd.Parameters.Add("@serial", SqlDbType.NVarChar)
    '        cmd.Parameters.Add("@company_code", SqlDbType.Int)

    '        cmd.Parameters("@id").Value = maxID("tblProgramUser") + 1
    '        cmd.Parameters("@serial").Value = txtPincode.Text.Trim
    '        cmd.Parameters("@company_code").Value = txtID.Text.Trim

    '        cmd.ExecuteNonQuery()

    '        '
    '        cmd.CommandText = "INSERT INTO tblCompanyAssignement (ID, idGroupCompany, idCompany) VALUES (@id, @idGroupCompany, @idCompany)"
    '        cmd.Parameters.Clear()
    '        cmd.Parameters.Add("@id", SqlDbType.Int)
    '        cmd.Parameters.Add("@idGroupCompany", SqlDbType.NVarChar)
    '        cmd.Parameters.Add("@idCompany", SqlDbType.Int)

    '        cmd.Parameters("@id").Value = maxID("tblCompanyAssignement") + 1
    '        cmd.Parameters("@idGroupCompany").Value = tmpData.Substring(3, tmpData.Trim.Length - 3)
    '        cmd.Parameters("@idCompany").Value = txtID.Text.Trim

    '        cmd.ExecuteNonQuery()


    '        transaction.Commit()

    '        addPenpc = True

    '    Catch ex As Exception
    '        addPenpc = False

    '        Try
    '            transaction.Rollback()

    '        Catch ex2 As Exception
    '            ' This catch block will handle any errors that may have occurred
    '            ' on the server that would cause the rollback to fail, such as
    '            ' a closed connection.
    '            Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType())
    '            Console.WriteLine("  Message: {0}", ex2.Message)
    '        End Try

    '        MessageBox.Show(ex.Message.ToString)

    '    Finally
    '        cmd.Dispose()
    '        sqlCon.Dispose()

    '    End Try

    'End Function

End Class
