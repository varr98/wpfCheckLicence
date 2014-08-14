Imports System.Data.SqlClient
Imports System.Data

Class MainWindow

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        btnManageGroupCompany.Visibility = btn_admin_hidden

        If My.Settings.Item("product_serial") = "" Then
            Dim ser As String = InputBox("Insert User Code", "UserCode request", "")
            If ser <> "" Then
                If ListGroupCompany(ser) > 0 Then
                    My.Settings.Item("product_serial") = ser
                    My.Settings.Save()

                    ListGroupCompany(ser)

                Else
                    MsgBox("ERROR: UserCode with no result")
                    Close()

                End If

            Else
                Close()

            End If

        Else
            ListGroupCompany(My.Settings.Item("product_serial"))

        End If


    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As RoutedEventArgs) Handles btnRefresh.Click
        ListGroupCompany(My.Settings.Item("product_serial"))

    End Sub

    Private Sub btnAdmin_Click(sender As Object, e As RoutedEventArgs) Handles btnAdmin.Click
        Dim frm As New frmAdminPwd
        frm.ShowDialog()

        btnManageGroupCompany.Visibility = btn_admin_hidden

    End Sub

    Private Sub cmbGroupCompany_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbGroupCompany.SelectionChanged
        ListCompany(cmbGroupCompany.SelectedValue)

    End Sub

    Private Sub btnManageGroupCompany_Click(sender As Object, e As RoutedEventArgs) Handles btnManageGroupCompany.Click
        Dim frm As New frmManageGroupCompany
        frm.ShowDialog()

        ListGroupCompany(My.Settings.Item("product_serial"))
    End Sub

    Private Sub cmbCompany_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbCompany.SelectionChanged
        ListPurchased(cmbCompany.SelectedValue)
        TreeviewAgency(cmbCompany.SelectedValue)


    End Sub


    Private Sub dgPurchased_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dgPurchased.SelectionChanged
        Dim selectedTVI As TreeViewItem = Nothing
        Try
            selectedTVI = CType(twAgency.SelectedItem, TreeViewItem)

        Catch ex As Exception

        End Try

        Dim selectedDG As Integer = Nothing
        Try
            selectedDG = dgPurchased.SelectedItem.Item(4).ToString

        Catch ex As Exception

        End Try

        If IsNothing(selectedTVI) = False And IsNothing(selectedDG) = False Then
            ListInUse(selectedDG, selectedTVI.Name)

        End If

    End Sub

    Private Sub twAgency_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object)) Handles twAgency.SelectedItemChanged
        Dim selectedTVI As TreeViewItem = Nothing
        Try
            selectedTVI = CType(twAgency.SelectedItem, TreeViewItem)

        Catch ex As Exception

        End Try

        Dim selectedDG As Integer = Nothing
        Try
            selectedDG = dgPurchased.SelectedItem.Item(4).ToString

        Catch ex As Exception

        End Try

        If IsNothing(selectedTVI) = False And IsNothing(selectedDG) = False Then
            ListInUse(selectedDG, selectedTVI.Name)

        End If


    End Sub


    '

    Private Function ListGroupCompany(product_serial As String) As Integer
        Mouse.OverrideCursor = Cursors.Wait

        ListGroupCompany = 0

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT distinct tblGroupCompany.group_company_code, tblGroupCompany.description FROM tblProgramUser INNER JOIN tblCompanyAssignement ON tblProgramUser.company_code = tblCompanyAssignement.idCompany INNER JOIN tblGroupCompany ON tblCompanyAssignement.idGroupCompany = tblGroupCompany.group_company_code WHERE (tblProgramUser.serial = @ser) ORDER BY tblGroupCompany.description"
        cmd.Parameters.Add("@ser", SqlDbType.NChar)
        cmd.Parameters("@ser").Value = product_serial

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

    Private Sub ListCompany(groupcompany As Integer)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT tblCompany.company_code, tblCompany.description, tblCompany.country_code FROM tblCompany INNER JOIN tblCompanyAssignement ON tblCompany.company_code = tblCompanyAssignement.idCompany INNER JOIN tblGroupCompany ON tblCompanyAssignement.idGroupCompany = tblGroupCompany.group_company_code INNER JOIN tblProgramUser ON tblCompanyAssignement.idCompany = tblProgramUser.company_code WHERE (tblGroupCompany.group_company_code = @gcompany) AND (tblProgramUser.serial = @ser) ORDER BY tblCompany.company_default desc, tblCompany.description"

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

    Private Sub ListPurchased(company As Integer)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT [prog_cod], [prog_description], [seats_purchased], [seats_in_use], [company_code], [register_mail], [prog_serial], [customer_pwd], pin_code FROM tblSeatsPurchased WHERE company_code = @company"

        cmd.Parameters.Add("@company", SqlDbType.Int)
        cmd.Parameters("@company").Value = company
  
        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            sqlDa.Fill(ds, "tblSeatsPurchased")

            'Binding the data to the combobox.
            dgPurchased.ItemsSource = ds.Tables("tblSeatsPurchased").DefaultView
            dgPurchased.DataContext = ds.Tables("tblSeatsPurchased")

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)

        Finally
            sqlDa.Dispose()
            cmd.Dispose()
            sqlCon.Dispose()

        End Try

        If dgPurchased.Items.Count > 0 Then
            dgPurchased.SelectedIndex = 0

        End If
        Mouse.OverrideCursor = Cursors.Arrow
    End Sub

    Private Sub ListInUse(company As Integer, agency As String)
        Mouse.OverrideCursor = Cursors.Wait

        Dim sqlCon As SqlConnection = New SqlConnection()
        sqlCon.ConnectionString = connectionString

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = sqlCon
        cmd.CommandType = CommandType.Text
        If agency = "ALL" Then
            cmd.CommandText = "SELECT [agency_code],[seat_id], [seat_hw],[user_name],[user_mail],[user_pwd],[pin_code],[first_name],[last_name],[active] FROM [DB_99C452_clients].[dbo].[tblSeatsInUse] WHERE company_code = @company"

        Else
            cmd.CommandText = "SELECT [agency_code],[seat_id], [seat_hw],[user_name],[user_mail],[user_pwd],[pin_code],[first_name],[last_name],[active] FROM [DB_99C452_clients].[dbo].[tblSeatsInUse] WHERE company_code = @company and agency_code = @agency"
            cmd.Parameters.Add("@agency", SqlDbType.Int)
            cmd.Parameters("@agency").Value = agency.Replace("age", "")

        End If

        cmd.Parameters.Add("@company", SqlDbType.Int)
        cmd.Parameters("@company").Value = company

        Dim sqlDa As SqlDataAdapter = New SqlDataAdapter()
        sqlDa.SelectCommand = cmd

        Dim ds As DataSet = New DataSet()

        Try
            sqlDa.Fill(ds, "tblSeatsPurchased")

            'Binding the data to the combobox.
            dgInUse.ItemsSource = ds.Tables("tblSeatsPurchased").DefaultView
            dgInUse.DataContext = ds.Tables("tblSeatsPurchased")




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

            'Binding the data 
            'dgInUse.ItemsSource = ds.Tables("tblSeatsPurchased").DefaultView
            'dgInUse.DataContext = ds.Tables("tblSeatsPurchased")

            Dim treeItem As TreeViewItem = Nothing
            treeItem = New TreeViewItem()
            treeItem.Header = "ALL"
            treeItem.Name = "ALL"

            twAgency.Items.Clear()
            twAgency.Items.Add(treeItem)

            For conta As Integer = 0 To ds.Tables("agency").Rows.Count - 1
                For Each a As TreeViewItem In twAgency.Items
                    If a.Header = "ALL" Then

                        Dim b As New TreeViewItem
                        b.Header = ds.Tables("agency").Rows(conta).Item("description").ToString
                        b.Name = "age" & ds.Tables("agency").Rows(conta).Item("agency_code").ToString

                        a.Items.Add(b)

                        a.IsExpanded = True
                        'a.IsSelected = True

                    End If

                Next
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
