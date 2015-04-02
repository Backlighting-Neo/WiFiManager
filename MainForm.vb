Imports WiFiProject.WiFi_ICS

Public Class MainForm
    Private Sub OpenICS()
        Dim ICS = New WiFi_ICS
        ICS.PrivateConnection = "本机虚拟Wifi"
        ICS.PublicConnection = "本地连接"
        ICS.switch = "on"
        ICS.Command()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'WebBrowser1.ObjectForScripting = New ScriptForWebBrowser
        'WebBrowser1.Navigate(Application.StartupPath + "\UI\index.html")
        Dim VirtualInterfaceID As Integer, VirtualInterfaceName As String = String.Empty
        GetVi(VirtualInterfaceID, VirtualInterfaceName)



    End Sub

End Class

<System.Runtime.InteropServices.ComVisible(True)>
Public Class ScriptForWebBrowser
    Public Sub abc()
        MsgBox("abc")
    End Sub



End Class
