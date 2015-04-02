Module Support

    'Function GetVi 用来获取虚拟网卡的编号已经适配器名称，其中编号以返回值的形式返回，名称赋值到virtalname的全局变量
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

        End If

    End Sub


End Module
