﻿Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mail
Imports System.Net.Mail
Imports MySql.Data
Imports MySql.Data.MySqlClient


Public Class clsMySqlDB
    'MySQL
    Public connString As String = "Server=127.0.0.1;Database=sms;Uid=root;Pwd=;"
    Public conn As MySqlConnection
    Public SUCCESS As String = "SUCCESS"
    Public SERROR As String = "ERROR"

    Public Function executeSQL(ByVal sSql As String, ByRef sResult As String) As Data.DataTable
        Dim sReturn As String = ""
        'Dim sr As SqlDataReader = Nothing
        Dim dt As DataTable = New DataTable
        Dim da As New mySqlDataAdapter
        conn = New MySqlConnection
        Try
            conn.ConnectionString = connString
            conn.Open()
            Dim sComm As New MySqlCommand
            sComm.CommandText = sSql
            sComm.Connection = conn
            da.SelectCommand = sComm
            da.Fill(dt)
            conn.Close()
            sResult = SUCCESS
        Catch ex As Exception
            sResult = SERROR & ": " & ex.Message
            If (conn.State = Data.ConnectionState.Open) Then
                conn.Close()
            End If
        End Try
        conn = Nothing
        Return dt
    End Function
    Public Function executeDMLSQL(ByVal sSql As String, ByRef sResult As String) As Integer
        Dim sReturn As String = ""
        Dim irows As Integer = 0
        conn = New MySqlConnection
        Try
            conn.ConnectionString = connString
            conn.Open()
            Dim sComm As New MySqlCommand
            sComm.CommandText = sSql
            sComm.Connection = conn
            irows = sComm.ExecuteNonQuery()
            conn.Close()
            sResult = SUCCESS
        Catch ex As Exception
            sResult = SERROR & ": " & ex.Message
            If (conn.State = Data.ConnectionState.Open) Then
                conn.Close()
            End If
        End Try
        conn = Nothing
        Return irows
    End Function
    Public Sub populateDDList(ByRef ctlDD As DropDownList, ByVal sSql As String)

        Dim res As String = ""
        Dim red As DataTable 'SqlDataReader = Nothing
        Dim drow As DataRow
        red = executeSQL(sSql, res)
        ctlDD.Items.Clear()
        ctlDD.Items.Add(New ListItem("Select", "Select"))
        If res = "SUCCESS" Then
            For Each drow In red.Rows
                ctlDD.Items.Add(New ListItem(drow.Item(0).ToString, drow.Item(1).ToString))
            Next
        Else
        End If
    End Sub

    Public Function executeSQL_dset(ByVal sSql As String, ByRef sResult As String) As Data.DataSet
        Dim sReturn As String = ""
        'Dim sr As SqlDataReader = Nothing
        'Dim dt As DataTable = New DataTable
        Dim dt As DataSet = New DataSet
        Dim da As New MySqlDataAdapter
        conn = New MySqlConnection
        Try
            conn.ConnectionString = connString
            conn.Open()
            Dim sComm As New MySqlCommand
            sComm.CommandText = sSql
            sComm.Connection = conn
            da.SelectCommand = sComm
            da.Fill(dt)
            conn.Close()
            sResult = SUCCESS
        Catch ex As Exception
            sResult = SERROR & ": " & ex.Message
            If (conn.State = Data.ConnectionState.Open) Then
                conn.Close()
            End If
        End Try
        conn = Nothing
        Return dt
    End Function

    Public Function fillmygridview(gdview As WebControls.GridView, selectsql As String) As GridView
        Try
            Dim res As String = ""
            Dim dset As Data.DataSet
            Dim sSql1 As String = selectsql
            dset = executeSQL_dset(sSql1, res)
            gdview.DataSource = dset
            gdview.DataBind()
        Catch ex As Exception
        End Try
        Return gdview
    End Function

    Public Function getAllMonth(ByVal sdate As String) As String
        Dim sSql As String = ""
        'Dim dDate As Date = date.(sdate,"dd/MM/yyyy")
        'dDate.Month = sdate.Substring(3, 2)

        Dim inumDays As Integer = Date.DaysInMonth(sdate.Substring(6, 4), sdate.Substring(3, 2)) 'dDate.AddMonths(1).AddDays(-1).Day
        Dim dd As Integer
        For dd = 1 To inumDays
            sSql &= ", max(case when day(att_date)=" & dd & " then att_flag else '' end) as '" & dd & "' "
        Next

        Return sSql
    End Function

    Public Sub CreateConfirmBox(ByRef btn As WebControls.GridView, _
                                       ByVal strMessage As String)
        btn.Attributes.Add("onclick", "return confirm('" & strMessage & "');")
    End Sub
    Public Sub CreateConfirmBoxButton(ByRef btn As WebControls.Button, _
                                       ByVal strMessage As String)
        btn.Attributes.Add("onclick", "return confirm('" & strMessage & "');")
    End Sub
    Public Sub CreateConfirmBoxlink(ByRef btn As WebControls.LinkButton, _
                                      ByVal strMessage As String)
        btn.Attributes.Add("onclick", "return confirm('" & strMessage & "');")

    End Sub

    Sub sendmail(ByVal body As String, ByVal email As String)
        Try
            'Dim uemail As String = a
            Dim MailObj As New System.Net.Mail.SmtpClient
            Dim basicAuthenticationInfo As New System.Net.NetworkCredential("ServiceOrder@canar.com.sd", "satellite")
            'Put your own, or your ISPs, mail server name onthis next line
            'mailClient.Host = "Mail.RemoteMailServer.com"
            'MailObj.UseDefaultCredentials = False
            MailObj.Credentials = basicAuthenticationInfo
            MailObj.Port = 25
            'MailObj.EnableSsl = True

            MailObj.Host = "canarmail.canar.com.sd"
            MailObj.Send("ServiceOrder@canar.com.sd", email, "Service Order Notification Service", body)
            'MsgBox("done")
        Catch ex As Exception
            'MsgBox(ex.ToString) 'Errorbar.Text = Errorbar.Text + " Email not Sent ..!"
        End Try
    End Sub

End Class

