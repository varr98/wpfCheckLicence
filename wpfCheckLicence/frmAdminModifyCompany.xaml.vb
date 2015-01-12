Imports System.Data.SqlClient
Imports System.Data

Public Class frmAdmModifyCompany


    Private Sub frmAdmModifyCompany_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        If tmpData.Length >= 3 Then
            If tmpData.ToLower.Substring(0, 3) = "new" Then
                btnSave.Content = "Add"
                txtID.Text = maxID("tblCompany") + 1

            Else
                btnSave.Content = "Save"

                If IsNumeric(tmpData) Then
                    loadData(tmpData)

                End If
            End If
        Else
            btnSave.Content = "Save"

            If IsNumeric(tmpData) Then
                loadData(tmpData)

            End If

        End If


    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If btnSave.Content = "Save" Then
            saveCompany()

            Close()

        Else
            If txtDescription.Text.Trim <> "" And txtPinCode.Text.Trim <> "" And txtCountryCode.Text.Trim <> "" Then
                addCompany()

                Close()

            Else
                MsgBox("Insert all form data", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error")

            End If

        End If

    End Sub




    Private Sub loadData(id As Integer)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT  tblCompany.id, tblCompany.company_code, tblCompany.description, tblCompany.country_code, tblCompany.company_default, tblProgramUser.serial FROM tblProgramUser INNER JOIN tblCompany ON tblProgramUser.company_code = tblCompany.company_code WHERE tblCompany.id = @id"

        cmd.Parameters.Add("@id", SqlDbType.Int)
        cmd.Parameters("@id").Value = id

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()

            End If

            sqlDa.Fill(ds, "tblCompany")

            txtID.Text = ds.Tables("tblCompany").Rows(0).Item("id").ToString
            txtDescription.Text = ds.Tables("tblCompany").Rows(0).Item("description").ToString()
            txtCountryCode.Text = ds.Tables("tblCompany").Rows(0).Item("country_code").ToString()

            If ds.Tables("tblCompany").Rows(0).Item("company_default").ToString() = "y" Then
                cmbDefault.SelectedIndex = 0

            Else
                cmbDefault.SelectedIndex = 1


            End If

            txtPinCode.Text = ds.Tables("tblCompany").Rows(0).Item("serial").ToString().Trim


        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        Finally

            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try


        Mouse.OverrideCursor = Cursors.Arrow

    End Sub

    Private Function saveCompany() As Boolean
        Mouse.OverrideCursor = Cursors.Wait

        saveCompany = False

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString


        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "UPDATE tblCompany SET description = @desc, country_code = @country_code, company_default = @company_default WHERE id = @id"

        cmd.Parameters.Add("@id", SqlDbType.Int)
        cmd.Parameters.Add("@desc", SqlDbType.NVarChar)
        cmd.Parameters.Add("@country_code", SqlDbType.NVarChar)
        cmd.Parameters.Add("@company_default", SqlDbType.NVarChar)

        cmd.Parameters("@id").Value = txtID.Text.Trim
        cmd.Parameters("@desc").Value = txtDescription.Text.Trim
        cmd.Parameters("@country_code").Value = txtCountryCode.Text.Trim
        cmd.Parameters("@company_default").Value = cmbDefault.Text.Substring(0, 1).ToLower


        Try
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()

            End If

            cmd.ExecuteNonQuery()

            '
            cmd.CommandText = "UPDATE tblProgramUser SET serial = @serial WHERE company_code = @company_code"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@serial", SqlDbType.NVarChar)
            cmd.Parameters.Add("@company_code", SqlDbType.Int)

            cmd.Parameters("@serial").Value = txtPinCode.Text.Trim
            cmd.Parameters("@company_code").Value = txtID.Text.Trim

            cmd.ExecuteNonQuery()

            saveCompany = True

        Catch ex As Exception
            saveCompany = False

            MessageBox.Show(ex.Message.ToString)

        Finally
            cmd.Dispose()
            sqlCon.Dispose()

        End Try


    End Function


    Private Function addCompany() As Boolean
        Mouse.OverrideCursor = Cursors.Wait

        addCompany = False

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString
        Dim cmd As SqlCommand = sqlCon.CreateCommand()

        Dim transaction As SqlTransaction

        If cmd.Connection.State <> ConnectionState.Open Then
            cmd.Connection.Open()

        End If

        ' Start a local transaction
        transaction = sqlCon.BeginTransaction("SampleTransaction")

        ' Must assign both transaction object and connection
        ' to Command object for a pending local transaction.
        cmd.Connection = sqlCon
        cmd.Transaction = transaction

        cmd.CommandType = CommandType.Text
        cmd.CommandText = "INSERT INTO tblCompany (id, company_code, description, country_code, company_default) VALUES (@id, @company_code, @desc, @country_code, @company_default)"

        cmd.Parameters.Add("@id", SqlDbType.Int)
        cmd.Parameters.Add("@company_code", SqlDbType.Int)
        cmd.Parameters.Add("@desc", SqlDbType.NVarChar)
        cmd.Parameters.Add("@country_code", SqlDbType.NVarChar)
        cmd.Parameters.Add("@company_default", SqlDbType.NVarChar)

        cmd.Parameters("@id").Value = txtID.Text.Trim
        cmd.Parameters("@company_code").Value = txtID.Text.Trim
        cmd.Parameters("@desc").Value = txtDescription.Text.Trim
        cmd.Parameters("@country_code").Value = txtCountryCode.Text.Trim
        cmd.Parameters("@company_default").Value = cmbDefault.Text.Substring(0, 1).ToLower


        Try
 
            cmd.ExecuteNonQuery()

            '
            cmd.CommandText = "INSERT INTO tblProgramUser (ID, serial, company_code) VALUES (@id, @serial, @company_code)"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@id", SqlDbType.Int)
            cmd.Parameters.Add("@serial", SqlDbType.NVarChar)
            cmd.Parameters.Add("@company_code", SqlDbType.Int)

            cmd.Parameters("@id").Value = maxID("tblProgramUser") + 1
            cmd.Parameters("@serial").Value = txtPinCode.Text.Trim
            cmd.Parameters("@company_code").Value = txtID.Text.Trim

            cmd.ExecuteNonQuery()

            '
            cmd.CommandText = "INSERT INTO tblCompanyAssignement (ID, idGroupCompany, idCompany) VALUES (@id, @idGroupCompany, @idCompany)"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@id", SqlDbType.Int)
            cmd.Parameters.Add("@idGroupCompany", SqlDbType.NVarChar)
            cmd.Parameters.Add("@idCompany", SqlDbType.Int)

            cmd.Parameters("@id").Value = maxID("tblCompanyAssignement") + 1
            cmd.Parameters("@idGroupCompany").Value = tmpData.Substring(3, tmpData.Trim.Length - 3)
            cmd.Parameters("@idCompany").Value = txtID.Text.Trim

            cmd.ExecuteNonQuery()


            transaction.Commit()

            addCompany = True

        Catch ex As Exception
            addCompany = False

            Try
                transaction.Rollback()

            Catch ex2 As Exception
                ' This catch block will handle any errors that may have occurred
                ' on the server that would cause the rollback to fail, such as
                ' a closed connection.
                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType())
                Console.WriteLine("  Message: {0}", ex2.Message)
            End Try

            MessageBox.Show(ex.Message.ToString)

        Finally
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

    End Function
End Class
