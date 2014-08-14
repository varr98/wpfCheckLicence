Imports System.Data.SqlClient
Imports System.Data

Module Module1

    Public connectionString As String = "Data Source=SQL5004.Smarterasp.net;Initial Catalog=DB_99C452_clients;Persist Security Info=True;User ID=DB_99C452_clients_admin;password=librolibro"

    Public btn_admin_hidden As Boolean = True

    Public tmpData As String = ""


    'DB FUNCTIONS
    Public Function maxID(table As String) As Integer
        Mouse.OverrideCursor = Cursors.Wait

        maxID = -1

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT MAX(id) FROM " & table

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()

            End If

            Dim val = cmd.ExecuteScalar()
            If Not val Is Nothing Then
                maxID = Convert.ToDecimal(val)

            End If

        Catch ex As Exception
            maxID = -1

            MessageBox.Show(ex.Message.ToString)

        Finally

            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

        Mouse.OverrideCursor = Cursors.Arrow

    End Function


End Module
