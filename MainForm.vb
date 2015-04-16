Imports WiFiProject.WiFi_ICS


Public Class MainForm
    Public Declare Function SendMessageA Lib "user32" (ByVal hWnd As Integer, ByVal Msg As Integer, ByVal wParam As Integer, ByVal IParam As Integer) As Boolean
    Public Declare Function ReleaseCapture Lib "user32" Alias "ReleaseCapture" () As Boolean

    Private Sub OpenICS()
        Dim ICS = New WiFi_ICS
        ICS.PrivateConnection = "本机虚拟Wifi"
        ICS.PublicConnection = "本地连接"
        ICS.switch = "on"
        ICS.Command()
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Support As New ScriptForWebBrowser
        WebKitBrowser.Url = New Uri(Application.StartupPath + "../../../UI/index.html")
        WebKitBrowser.GetScriptManager.ScriptObject = Support

        'Dim VirtualInterfaceID As Integer, VirtualInterfaceName As String = String.Empty
        ' Support.GetVi(VirtualInterfaceID, VirtualInterfaceName)

        '在js中已经Scan


    End Sub

End Class


