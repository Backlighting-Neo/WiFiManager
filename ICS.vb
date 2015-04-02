Imports Microsoft.VisualBasic

'类名  WiFi_ICS
'作用  用来控制计算机的ICS（Internet连接共享服务）
'用法  实例化类后，请设置PublicConnection, PrivateConnection, switch三个参数，
'        设置完成后，调用Comomand方法
'参数  PublicConnection    【必选】用于被共享的公网连接名     例如"本地连接"
'         PrivateConnection  【必选】用于共享的私网连接名        例如"本机虚拟WiFi"
'         switch                    【必选】 打开共享/关闭共享             可选值"on"
'作者   逆光 Backlighting
'日期   2015年01月28日
'示例   Dim ICS = New WiFi_ICS
'          ICS.PrivateConnection = "本机虚拟Wifi"
'          ICS.PublicConnection = "本地连接"
'          ICS.switch = "on"
'          ICS.Command()

Public Class WiFi_ICS
    Public PublicConnection, PrivateConnection, switch

    Private ICSSC_DEFAULT = 0
    Private CONNECTION_PUBLIC = 0
    Private CONNECTION_PRIVATE = 1
    Private CONNECTION_ALL = 2

    Private NetSharingManager
    Private EveryConnectionCollection

    Public Sub Command()
        If Initialize() = True Then
            GetConnectionObjects()
            FirewallTestByName(PublicConnection, PrivateConnection)
        End If
    End Sub

    Sub FirewallTestByName(ByVal con1, ByVal con2)
        Dim Item
        Dim EveryConnection
        Dim objNCProps
        Dim bFound1, bFound2
        bFound1 = False
        bFound2 = False
        For Each Item In EveryConnectionCollection
            EveryConnection = NetSharingManager.INetSharingConfigurationForINetConnection(Item)
            objNCProps = NetSharingManager.NetConnectionProps(Item)
            If objNCProps.Name = con1 Then
                bFound1 = True
                If Not (EveryConnection.SharingEnabled) And switch = "on" Then
                    EveryConnection.EnableSharing(CONNECTION_PRIVATE)
                ElseIf (switch = "off") Then
                    EveryConnection.EnableSharing(CONNECTION_ALL)
                End If
            End If
            If objNCProps.Name = con2 Then
                bFound2 = True
                If Not (EveryConnection.SharingEnabled) And switch = "on" Then
                    EveryConnection.EnableSharing(CONNECTION_PUBLIC)
                ElseIf (switch = "off") Then
                    EveryConnection.EnableSharing(CONNECTION_ALL)
                End If
            End If
        Next
    End Sub

    Function Initialize()
        Dim bReturn
        bReturn = False

        NetSharingManager = CreateObject("HNetCfg.HNetShare.1")
        If NetSharingManager Is Nothing Then
        Else
            If NetSharingManager.SharingInstalled Is Nothing Then
            Else
                bReturn = True
            End If
        End If
        Initialize = bReturn
    End Function

    Function GetConnectionObjects()
        Dim bReturn = True
        If GetConnection(CONNECTION_PUBLIC) = False Then bReturn = False
        If GetConnection(CONNECTION_PRIVATE) = False Then bReturn = False
        If GetConnection(CONNECTION_ALL) = False Then bReturn = False
        Return bReturn
    End Function


    Function GetConnection(ByVal CONNECTION_TYPE)
        Dim bReturn = True
        Dim Connection
        Dim Item
        If (CONNECTION_PUBLIC = CONNECTION_TYPE) Then
            Connection = NetSharingManager.EnumPublicConnections(ICSSC_DEFAULT)
            If (Connection.Count > 0) And (Connection.Count < 2) Then
                For Each Item In Connection
                    PublicConnection = NetSharingManager.INetSharingConfigurationForINetConnection(Item)
                Next
            Else
                bReturn = False
            End If
        ElseIf (CONNECTION_PRIVATE = CONNECTION_TYPE) Then
            Connection = NetSharingManager.EnumPrivateConnections(ICSSC_DEFAULT)
            If (Connection.Count > 0) And (Connection.Count < 2) Then
                For Each Item In Connection
                    PrivateConnection = NetSharingManager.INetSharingConfigurationForINetConnection(Item)
                Next
            Else
                bReturn = False
            End If
        ElseIf (CONNECTION_ALL = CONNECTION_TYPE) Then
            Connection = NetSharingManager.EnumEveryConnection
            If (Connection.Count > 0) Then
                EveryConnectionCollection = Connection
            Else
                bReturn = False
            End If
        Else
            bReturn = False
        End If

        If (True = bReturn) Then
            If (Connection.Count = 0) Then
                bReturn = False
            ElseIf (Connection.Count > 1) And (CONNECTION_ALL <> CONNECTION_TYPE) Then
                bReturn = False
            End If
        End If
        GetConnection = bReturn
    End Function

    Function ConvertConnectionTypeToString(ByVal ConnectionID)
        Dim ConnectionString
        If (ConnectionID = CONNECTION_PUBLIC) Then
            ConnectionString = "public"
        ElseIf (ConnectionID = CONNECTION_PRIVATE) Then
            ConnectionString = "private"
        ElseIf (ConnectionID = CONNECTION_ALL) Then
            ConnectionString = "all"
        Else
            ConnectionString = "Unknown: " + CStr(ConnectionID)
        End If
        ConvertConnectionTypeToString = ConnectionString
    End Function
End Class
