Imports NativeWifi
Imports System.Text
Imports Newtonsoft.Json
Imports System.IO

<System.Runtime.InteropServices.ComVisible(True)>
Public Class ScriptForWebBrowser
    Public Declare Function SendMessageA Lib "user32" (ByVal hWnd As Integer, ByVal Msg As Integer, ByVal wParam As Integer, ByVal IParam As Integer) As Boolean
    Public Declare Function ReleaseCapture Lib "user32" Alias "ReleaseCapture" () As Boolean
    Private WiFiControl As New MyWifi

    Private virtal As Integer

    Public Sub JavaScriptTesting()
        MsgBox("Testing Sucess")
    End Sub

    Public Sub FormMouseDown()
        ReleaseCapture()
        SendMessageA(MainForm.Handle, &HA1, 2, 0)
    End Sub

    Public Sub GetGithub()
        Shell("explorer.exe https://github.com/Backlighting-Neo/WiFiManager")
    End Sub

    Public Sub CloseWindow()
        End
    End Sub

    Public Function GetConsoleReturn(ByVal cmdline As String) As String
        Shell("cmd.exe /c " & cmdline & " >result.log", vbHide, True)
        Dim a As Stream
        a = File.OpenRead(Application.StartupPath & "\result.log")
        Dim sr As StreamReader = New StreamReader(a, System.Text.Encoding.Default)
        sr.BaseStream.Seek(0, SeekOrigin.Begin)
        Dim result As String = String.Empty
        While (sr.Peek() > -1)
            result = result + sr.ReadLine() + vbCrLf
        End While
        a = Nothing
        sr.Close()
        sr = Nothing
        Kill(Application.StartupPath & "\result.log")
        Return result
    End Function

    Public Function GetRv() As Integer
        Dim nics() As System.Net.NetworkInformation.NetworkInterface
        nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
        Return nics(virtal).GetIPv4Statistics.BytesReceived / 1024
    End Function

    Public Function GetSv() As Integer
        Dim nics() As System.Net.NetworkInformation.NetworkInterface
        nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
        Return nics(virtal).GetIPv4Statistics.BytesSent / 1024
    End Function



    '其中编号以VitrualID返回，名称赋值到virtalname的全局变量
    Public Sub GetVi(ByRef VitrualId As Integer, ByRef VitrualName As String)
        Dim nics() As System.Net.NetworkInformation.NetworkInterface
        nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
        Dim vi As Integer
        Dim OsVer As String = (Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor)
        If nics.Length > 0 Then
            If OsVer > 6.1 Then
                For Each netState In nics
                    If InStr(netState.Description, "Microsoft 托管网络虚拟适配器") <> 0 Or InStr(netState.Description, "Microsoft Wi-Fi Direct 虚拟适配器") <> 0 Then
                        VitrualName = netState.Name
                        VitrualId = vi
                    Else
                        vi = vi + 1
                    End If
                    If netState.Name = "Loopback Pseudo-Interface 1" Then
                        VitrualId = -1
                        Exit Sub
                    End If
                Next
            ElseIf OsVer = 6.1 Then
                For Each netState In nics
                    If InStr(netState.Description, "Microsoft Virtual WiFi Miniport Adapter") <> 0 Then
                        VitrualName = netState.Name
                        VitrualId = vi
                    Else
                        vi = vi + 1
                    End If
                    If netState.Name = "Loopback Pseudo-Interface 1" Then
                        VitrualId = -1
                        Exit Sub
                    End If
                Next
            End If
            virtal = vi
        End If
    End Sub

    Public Function ScanWiFi() As String
        Dim ResultString As String = String.Empty
        WiFiControl.ScanSSID()
        ResultString = JsonConvert.SerializeObject(WiFiControl.OutputSSIDS)
        Return ResultString
    End Function

    Public Sub ConnectSSID(ssid As String)
        For Each Item In WiFiControl.ssids
            If Item.SSID = ssid Then
                WiFiControl.ConnectToSSID(Item)
                Exit For
            End If
        Next
    End Sub

    Public Sub cpl(Command As String)
        Select Case Command
            Case "Internet"
                Shell("control.exe inetcpl.cpl")
            Case "Firewall"
                Shell("control.exe Firewall.cpl")
            Case "Connection"
                Shell("control.exe ncpa.cpl")
        End Select
    End Sub
End Class


Public Class MyWifi

#Region "类定义"
    Public ssids As New List(Of WIFISSID)()
    Public OutputSSIDS As New List(Of WiFiSSIDForJson)()

    Public Class WIFISSID
        Public SSID As String = String.Empty
        Public dot11DefaultAuthAlgorithm As String = ""
        Public dot11DefaultCipherAlgorithm As String = ""
        Public networkConnectable As Boolean = True
        Public wlanNotConnectableReason As String = ""
        Public wlanSignalQuality As Integer = 0
        Public wlanInterface As WlanClient.WlanInterface = Nothing
    End Class

    Public Class WiFiSSIDForJson
        Public SSID As String = String.Empty
        Public Auth As String = String.Empty
        Public Cipher As String = String.Empty
        Public Quality As Integer = 0
    End Class

    Public Sub New()
        ssids.Clear()
        OutputSSIDS.Clear()
    End Sub
#End Region

#Region "Sub.枚举可供连接的SSID"
    Public Sub ScanSSID()
        ssids.Clear()
        OutputSSIDS.Clear()

        Dim client As New WlanClient()
        For Each wlanIface As WlanClient.WlanInterface In client.Interfaces
            Dim networks As Wlan.WlanAvailableNetwork() = wlanIface.GetAvailableNetworkList(0)
            For Each network As Wlan.WlanAvailableNetwork In networks
                Dim targetSSID As New WIFISSID()
                targetSSID.wlanInterface = wlanIface
                targetSSID.wlanSignalQuality = CInt(network.wlanSignalQuality)
                targetSSID.SSID = GetStringForSSID(network.dot11Ssid)
                targetSSID.dot11DefaultAuthAlgorithm = network.dot11DefaultAuthAlgorithm.ToString()
                targetSSID.dot11DefaultCipherAlgorithm = network.dot11DefaultCipherAlgorithm.ToString()
                ssids.Add(targetSSID)

                Dim targetSSIDForJson As New WiFiSSIDForJson
                targetSSIDForJson.SSID = targetSSID.SSID
                targetSSIDForJson.Auth = targetSSID.dot11DefaultAuthAlgorithm
                targetSSIDForJson.Cipher = targetSSID.dot11DefaultCipherAlgorithm
                targetSSIDForJson.Quality = targetSSID.wlanSignalQuality
                OutputSSIDS.Add(targetSSIDForJson)
            Next
        Next
    End Sub
#End Region

    ' 连接到未加密的SSID
    Public Sub ConnectToSSID(ssid As WIFISSID)
        Dim profileName As String = ssid.SSID
        Dim mac As String = StringToHex(profileName)
        ' 
        'string key = "";
        'string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>New{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><MSM><security><authEncryption><authentication>open</authentication><encryption>none</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>networkKey</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey><keyIndex>0</keyIndex></security></MSM></WLANProfile>", profileName, mac, key);
        'string profileXml2 = "<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>Hacker SSID</name><SSIDConfig><SSID><hex>54502D4C494E4B5F506F636B657441505F433844323632</hex><name>TP-LINK_PocketAP_C8D262</name></SSID>        </SSIDConfig>        <connectionType>ESS</connectionType><connectionMode>manual</connectionMode><MSM> <security><authEncryption><authentication>open</authentication><encryption>none</encryption><useOneX>false</useOneX></authEncryption></security></MSM></WLANProfile>";
        'wlanIface.SetProfile( Wlan.WlanProfileFlags.AllUser, profileXml2, true );
        'wlanIface.Connect( Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName );
        Dim myProfileXML As String = String.Format("<?xml version=""1.0""?><WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1""><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>manual</connectionMode><MSM><security><authEncryption><authentication>open</authentication><encryption>none</encryption><useOneX>false</useOneX></authEncryption></security></MSM></WLANProfile>", profileName, mac)
        ssid.wlanInterface.SetProfile(Wlan.WlanProfileFlags.AllUser, myProfileXML, True)
        ssid.wlanInterface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName)
    End Sub

#Region "Fun.字符串转十六进制"
    Public Shared Function StringToHex(str As String) As String
        Dim sb As New StringBuilder()
        Dim byStr As Byte() = System.Text.Encoding.[Default].GetBytes(str)
        For i As Integer = 0 To byStr.Length - 1
            sb.Append(Convert.ToString(byStr(i), 16))
        Next
        Return (sb.ToString().ToUpper())
    End Function

    Private Shared Function GetStringForSSID(ssid As Wlan.Dot11Ssid) As String
        Return Encoding.UTF8.GetString(ssid.SSID, 0, CInt(ssid.SSIDLength))
    End Function
#End Region


End Class




