Imports System.Management
Imports System.Text.RegularExpressions

Public Class frmActivation
    Dim serial As Long

    Public Enum INTERNET_Flags
        INTERNET_CONNECTION_CONFIGURED = &H40
        INTERNET_CONNECTION_LAN = &H2
        INTERNET_CONNECTION_MODEM = &H1
        INTERNET_CONNECTION_MODEM_BUSY = &H8
        INTERNET_CONNECTION_OFFLINE = &H20
        INTERNET_CONNECTION_PROXY = &H4
    End Enum


    Declare Auto Function InternetGetConnectedState Lib "wininet.dll" (lpdwFlags As INTERNET_Flags, dwReserved As Integer) As Boolean

    Private Sub frmActivation_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Dim tmpInfo As New clsComputerInfo
        txtSerialCPU.Text = tmpInfo.GetProcessorId
        txtSerialHD.Text = tmpInfo.GetVolumeSerial

        txtApplicationID.Text = My.Settings.application_id

        'txtMachineID.Text = Regex.Replace(GetSerial, ".{4}", "$0-")
        txtMachineID.Text = genNumber("RobyAry" & txtApplicationID.Text)

        Dim cTypea As Long
        Dim InternetConnection As Boolean = InternetGetConnectedState(cTypea, 0&)

        If InternetConnection Then
            lblInternet.Content = "INTERNET CONNECTION : YES"

        Else
            lblInternet.Content = "INTERNET CONNECTION : NO"

        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        'If Long.TryParse(txtActivation.Text.Replace("-", ""), serial) Then
        '    If CheckKey(serial) = True Then

        If authKey(txtActivation.Text, txtMachineID.Text) = True Then

            lblResult.Content = "Yes"
            My.Settings.activated = True
            My.Settings.activated_datetime = Now.ToString
            My.Settings.activated_id = GetSerial()

            My.Settings.Save()

            MsgBox("APPLICATION ACTIVATED", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "")

            Me.Close()

        Else
            lblResult.Content = "Error"

        End If

        'Else
        'lblResult.Content = "No"

        'End If

    End Sub


    Public Function GetSerial() As String
        Dim mc As New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim mac As String = ""
        Dim moc As ManagementObjectCollection = mc.GetInstances

        For Each mo As ManagementObject In moc
            If mo.Item("IPEnabled") Then
                mac = mo.Item("MacAddress").ToString
            End If
        Next

        mc.Dispose()

        Dim sum As Long = 0
        Dim index As Integer = 1
        For Each ch As Char In mac
            If Char.IsDigit(ch) Then
                sum += sum + Integer.Parse(ch) * (index * 2)
            ElseIf Char.IsLetter(ch) Then
                Select Case ch.ToString.ToUpper
                    Case "A"
                        sum += sum + 10 * (index * 2)
                    Case "B"
                        sum += sum + 11 * (index * 2)
                    Case "C"
                        sum += sum + 12 * (index * 2)
                    Case "D"
                        sum += sum + 13 * (index * 2)
                    Case "E"
                        sum += sum + 14 * (index * 2)
                    Case "F"
                        sum += sum + 15 * (index * 2)
                End Select
            End If

            index += 1
        Next

        Return sum
    End Function

    Public Function CheckKey(ByVal key As Long) As Boolean
        Dim x As Long = GetSerial()
        Dim y As Long = x * x + 53 / x + 113 * (x / 4)
        Return y = key

    End Function

    Public Class clsComputerInfo

        Friend Function GetProcessorId() As String
            Dim strProcessorId As String = String.Empty
            Dim query As New SelectQuery("Win32_processor")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject

            For Each info In search.Get()
                strProcessorId = info("processorId").ToString()
            Next
            Return strProcessorId

        End Function

        Friend Function GetMACAddress() As String

            Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim moc As ManagementObjectCollection = mc.GetInstances()
            Dim MACAddress As String = String.Empty
            For Each mo As ManagementObject In moc

                If (MACAddress.Equals(String.Empty)) Then
                    If CBool(mo("IPEnabled")) Then MACAddress = mo("MacAddress").ToString()

                    mo.Dispose()
                End If
                MACAddress = MACAddress.Replace(":", String.Empty)

            Next
            Return MACAddress
        End Function

        Friend Function GetVolumeSerial(Optional ByVal strDriveLetter As String = "C") As String

            Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", strDriveLetter))
            disk.Get()
            Return disk("VolumeSerialNumber").ToString()
        End Function

        Friend Function GetMotherBoardID() As String

            Dim strMotherBoardID As String = String.Empty
            Dim query As New SelectQuery("Win32_BaseBoard")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()

                strMotherBoardID = info("SerialNumber").ToString()

            Next
            Return strMotherBoardID

        End Function



        Friend Function getMD5Hash(ByVal strToHash As String) As String
            Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

            bytesToHash = md5Obj.ComputeHash(bytesToHash)

            Dim strResult As String = ""

            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next

            Return strResult
        End Function


    End Class

    Public Function GenerateKey(ByVal serial As Long) As Long
        Dim x As Long = serial
        Return x * x + 53 / x + 113 * (x / 4)

    End Function

    'Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
    '    'If Long.TryParse(txtMachineID.Text.Replace("-", ""), serial) Then
    '    '    txtActivation.Text = Regex.Replace(GenerateKey(serial).ToString, ".{4}", "$0-")

    '    'End If


    '    Dim hw As New clsComputerInfo

    '    Dim hdd As String
    '    Dim cpu As String
    '    Dim mb As String
    '    Dim mac As String

    '    'cpu = hw.GetProcessorId()
    '    hdd = hw.GetVolumeSerial("C")
    '    'mb = hw.GetMotherBoardID()
    '    mac = hw.GetMACAddress()

    '    'MsgBox(cpu & "   " & hdd & "   " & mb & "   " & mac)

    '    Dim hwid As String = Strings.UCase(hw.getMD5Hash(hdd & mac))

    '    ' MessageBox.Show(Strings.UCase(hwid))

    '    txtActivation.Text = Regex.Replace(hwid, ".{6}", "$0-")
    'End Sub

    Private Sub btnKeyGen2_Click(sender As Object, e As RoutedEventArgs) Handles btnKeyGen2.Click

        txtActivation.Text = genNumber(txtMachineID.Text)

    End Sub

End Class
