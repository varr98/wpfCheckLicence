Imports System.Data.SqlClient
Imports System.Data

Public Class frmManageAgency

    Private Sub frmManageAgency_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        TreeviewAgency(tmpData)

    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As RoutedEventArgs) Handles btnAddNew.Click
        btnAddNew.Visibility = Windows.Visibility.Hidden
        btnModify.Visibility = Windows.Visibility.Hidden
        btnDelete.Visibility = Windows.Visibility.Hidden

        btnSave.Visibility = Windows.Visibility.Visible
        btnCancel.Visibility = Windows.Visibility.Visible


        txtID.Text = maxID("tblAgency") + 1
        txtDescription.Text = ""
        txtAddress.Text = ""
        txtCap.Text = ""
        txtCity.Text = ""
        txtProv.Text = ""
        txtCountry.Text = ""

    End Sub

    Private Sub btnModify_Click(sender As Object, e As RoutedEventArgs) Handles btnModify.Click
        btnAddNew.Visibility = Windows.Visibility.Hidden
        btnModify.Visibility = Windows.Visibility.Hidden
        btnDelete.Visibility = Windows.Visibility.Hidden

        btnSave.Visibility = Windows.Visibility.Visible
        btnCancel.Visibility = Windows.Visibility.Visible

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Dim ris As MsgBoxResult = MsgBox("Are you sure ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "DELETING")
        If ris = MsgBoxResult.Yes Then
            'deleteGroupCompany(cmbGroupCompany.SelectedValue)

            'ListGroupCompany()

        End If

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        btnAddNew.Visibility = Windows.Visibility.Visible
        btnModify.Visibility = Windows.Visibility.Visible
        btnDelete.Visibility = Windows.Visibility.Visible

        btnSave.Visibility = Windows.Visibility.Hidden
        btnCancel.Visibility = Windows.Visibility.Hidden

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        btnAddNew.Visibility = Windows.Visibility.Visible
        btnModify.Visibility = Windows.Visibility.Visible
        btnDelete.Visibility = Windows.Visibility.Visible

        btnSave.Visibility = Windows.Visibility.Hidden
        btnCancel.Visibility = Windows.Visibility.Hidden

        txtID.Text = ""
        txtDescription.Text = ""
        txtAddress.Text = ""
        txtCap.Text = ""
        txtCity.Text = ""
        txtProv.Text = ""
        txtCountry.Text = ""

    End Sub

    Private Sub twAgency_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object)) Handles twAgency.SelectedItemChanged
        Dim selectedTVI As TreeViewItem = Nothing
        Try
            selectedTVI = CType(twAgency.SelectedItem, TreeViewItem)
          
        Catch ex As Exception

        End Try

        agencyDetails(selectedTVI.Name.Replace("age", ""))

    End Sub

    Private Sub agencyDetails(agencyID As Integer)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT agency_code, description, address, cap, city, prov, country FROM tblAgency WHERE agency_code = @agency_code"

        cmd.Parameters.Add("@agency_code", SqlDbType.Int)
        cmd.Parameters("@agency_code").Value = agencyID

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            sqlDa.Fill(ds, "agency")

            For conta As Integer = 0 To ds.Tables("agency").Rows.Count - 1
                txtID.Text = agencyID
                txtDescription.Text = ds.Tables("agency").Rows(conta).Item("description").ToString
                txtAddress.Text = ds.Tables("agency").Rows(conta).Item("address").ToString
                txtCap.Text = ds.Tables("agency").Rows(conta).Item("cap").ToString
                txtCity.Text = ds.Tables("agency").Rows(conta).Item("city").ToString
                txtProv.Text = ds.Tables("agency").Rows(conta).Item("prov").ToString
                txtCountry.Text = ds.Tables("agency").Rows(conta).Item("country").ToString


            Next



        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        Finally

            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

        Mouse.OverrideCursor = Cursors.Arrow

    End Sub

    Private Sub TreeviewAgency(company As Integer)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT tblAgency.agency_code, tblAgency.description, tblAgency.address, tblAgency.cap, tblAgency.city, tblAgency.prov, tblAgency.country FROM tblAgency INNER JOIN tblAgencyAssignement ON tblAgency.agency_code = tblAgencyAssignement.idAgency WHERE(tblAgencyAssignement.idCompany = @company) ORDER BY tblAgency.description"

        cmd.Parameters.Add("@company", SqlDbType.Int)
        cmd.Parameters("@company").Value = company

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            sqlDa.Fill(ds, "agency")

            twAgency.Items.Clear()
  
            For conta As Integer = 0 To ds.Tables("agency").Rows.Count - 1
                Dim b As New TreeViewItem
                b.Header = ds.Tables("agency").Rows(conta).Item("description").ToString
                b.Name = "age" & ds.Tables("agency").Rows(conta).Item("agency_code").ToString

                twAgency.Items.Add(b)

            Next



        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        Finally

            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

        Mouse.OverrideCursor = Cursors.Arrow

    End Sub

 
End Class
