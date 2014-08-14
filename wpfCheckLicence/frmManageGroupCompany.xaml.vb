Imports System.Data.SqlClient
Imports System.Data

Public Class frmManageGroupCompany

    Private Sub frmManageGroupCompany_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        ListGroupCompany()

        '

    End Sub
 
    Private Sub cmbGroupCompany_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbGroupCompany.SelectionChanged
        ListCompany(cmbGroupCompany.SelectedValue)

        If cmbCompany.Items.Count > 0 Then
            btnDeleteGroupCompany.IsEnabled = False

        Else
            btnDeleteGroupCompany.IsEnabled = True

        End If
    End Sub

    'btn Group Company

    Private Sub btnAddNewGroupCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnAddNewGroupCompany.Click
        Dim newName As String = InputBox("New name for GroupCompany ?", "ADD NEW", "")

        If newName <> "" Then
            addGroupCompany(maxID("tblGroupCompany") + 1, newName)

            ListGroupCompany()

        End If

    End Sub

    Private Sub btnRenameGroupCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnRenameGroupCompany.Click
        Dim newName As String = InputBox("New name for " & cmbGroupCompany.Text & " ?", "", cmbGroupCompany.Text)

        If newName <> "" Then
            renameGroupCompany(cmbGroupCompany.SelectedValue, newName, "tblGroupCompany")

            ListGroupCompany()

        End If

    End Sub

    Private Sub btnDeleteGroupCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnDeleteGroupCompany.Click
        Dim ris As MsgBoxResult = MsgBox("Are you sure ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "DELETING")
        If ris = MsgBoxResult.Yes Then
            deleteGroupCompany(cmbGroupCompany.SelectedValue)

            ListGroupCompany()

        End If

    End Sub

    'btn company

    Private Sub btnAddNewCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnAddNewCompany.Click
        tmpData = "new" & cmbGroupCompany.SelectedValue

        Dim frm As New frmAdmModifyCompany
        frm.ShowDialog()

        ListGroupCompany()

    End Sub

    Private Sub btnModifyCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnModifyCompany.Click

        If cmbCompany.Text <> "" Then
            tmpData = cmbCompany.SelectedValue

            Dim frm As New frmAdmModifyCompany
            frm.ShowDialog()

            ListGroupCompany()

        End If

    End Sub

    Private Sub btnDeleteCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnDeleteCompany.Click
        Dim ris As MsgBoxResult = MsgBox("Are you sure ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "DELETING")
        If ris = MsgBoxResult.Yes Then



        End If

    End Sub

    '***

    'Group Company Function
    Private Function ListGroupCompany() As Integer
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

    Private Function addGroupCompany(newId As Integer, newGroupCompany As String) As Boolean
        Mouse.OverrideCursor = Cursors.Wait

        addGroupCompany = False

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString


        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "INSERT INTO tblGroupCompany ([id], [group_company_code], [description]) VALUES (@id, @group_company_code, @description)"
        cmd.Parameters.Add("@id", SqlDbType.Int)
        cmd.Parameters.Add("@group_company_code", SqlDbType.Int)
        cmd.Parameters.Add("@description", SqlDbType.NVarChar)
 
        cmd.Parameters("@id").Value = newId
        cmd.Parameters("@group_company_code").Value = newId
        cmd.Parameters("@description").Value = newGroupCompany
 

        Try
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()

            End If

            cmd.ExecuteNonQuery()


            addGroupCompany = True

        Catch ex As Exception
            addGroupCompany = False

            MessageBox.Show(ex.Message.ToString)

        Finally
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

    End Function

    Private Function renameGroupCompany(id As Integer, newDescription As String, table As String) As Boolean
        Mouse.OverrideCursor = Cursors.Wait

        renameGroupCompany = False

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString


        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "UPDATE " & table & " SET description = @newDesc WHERE id = @id"

        cmd.Parameters.Add("@id", SqlDbType.Int)
        cmd.Parameters.Add("@newDesc", SqlDbType.NVarChar)

        cmd.Parameters("@id").Value = id
        cmd.Parameters("@newDesc").Value = newDescription


        Try
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()

            End If

            cmd.ExecuteNonQuery()

            renameGroupCompany = True

        Catch ex As Exception
            renameGroupCompany = False

            MessageBox.Show(ex.Message.ToString)

        Finally
            cmd.Dispose()
            sqlCon.Dispose()

        End Try


    End Function

    Private Function deleteGroupCompany(id As Integer) As Boolean
        Mouse.OverrideCursor = Cursors.Wait

        deleteGroupCompany = False

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString


        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "DELETE tblGroupCompany WHERE id = @id"

        cmd.Parameters.Add("@id", SqlDbType.Int) 
        cmd.Parameters("@id").Value = id
   

        Try
            If cmd.Connection.State <> ConnectionState.Open Then
                cmd.Connection.Open()

            End If

            cmd.ExecuteNonQuery()

            deleteGroupCompany = True

        Catch ex As Exception
            deleteGroupCompany = False

            MessageBox.Show(ex.Message.ToString)

        Finally
            cmd.Dispose()
            sqlCon.Dispose()

        End Try


    End Function

    'Company Function

    Private Sub ListCompany(groupcompany As Integer)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT tblCompany.company_code, tblCompany.description, tblCompany.country_code FROM tblCompany INNER JOIN tblCompanyAssignement ON tblCompany.company_code = tblCompanyAssignement.idCompany INNER JOIN tblGroupCompany ON tblCompanyAssignement.idGroupCompany = tblGroupCompany.group_company_code WHERE (tblGroupCompany.group_company_code = @gcompany) ORDER BY tblCompany.company_default desc, tblCompany.description"

        cmd.Parameters.Add("@gcompany", SqlDbType.Int)
        cmd.Parameters.Add("@ser", SqlDbType.NChar)
        cmd.Parameters("@gcompany").Value = groupcompany
        cmd.Parameters("@ser").Value = My.Settings.Item("product_serial")

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            sqlDa.Fill(ds, "tblCompany")

            'Binding the data to the combobox.
            cmbCompany.ItemsSource = ds.Tables("tblCompany").DefaultView


            'To display category name (DisplayMember in Visual Studio 2005)
            cmbCompany.DisplayMemberPath = ds.Tables("tblCompany").Columns("description").ToString()

            'To store the ID as hidden (ValueMember in Visual Studio 2005)
            cmbCompany.SelectedValuePath = ds.Tables("tblCompany").Columns("company_code").ToString()



        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        Finally

            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

        If cmbCompany.Items.Count > 0 Then
            cmbCompany.SelectedIndex = 0

        End If
        Mouse.OverrideCursor = Cursors.Arrow
    End Sub



End Class
