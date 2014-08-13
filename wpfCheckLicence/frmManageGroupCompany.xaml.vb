Imports System.Data.SqlClient
Imports System.Data

Public Class frmManageGroupCompany

    Private Sub btnManageCompany_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnManageCompany_Copy.Click

    End Sub

    Private Sub frmManageGroupCompany_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        ListGroupCompany(My.Settings.Item("product_serial"))

     

        '



    End Sub
    Private Function ListGroupCompany(product_serial As String) As Integer
        Mouse.OverrideCursor = Cursors.Wait

        ListGroupCompany = 0

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT id, group_company_code, description FROM tblGroupCompany ORDER BY description"

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            sqlDa.Fill(ds, "tblGroupCompany")

            ListGroupCompany = ds.Tables("tblGroupCompany").Rows.Count

            'Binding the data to the combobox.
            cmbGroupCompany.ItemsSource = ds.Tables("tblGroupCompany").DefaultView


            'To display category name (DisplayMember in Visual Studio 2005)
            cmbGroupCompany.DisplayMemberPath = ds.Tables("tblGroupCompany").Columns("description").ToString()

            'To store the ID as hidden (ValueMember in Visual Studio 2005)
            cmbGroupCompany.SelectedValuePath = ds.Tables("tblGroupCompany").Columns("group_company_code").ToString()


        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)

        Finally

            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

        If cmbGroupCompany.Items.Count > 0 Then
            cmbGroupCompany.SelectedIndex = 0

        End If
        Mouse.OverrideCursor = Cursors.Arrow
    End Function

End Class
