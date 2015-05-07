<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.WebKitBrowser = New WebKit.WebKitBrowser()
        Me.SuspendLayout()
        '
        'WebKitBrowser
        '
        Me.WebKitBrowser.AllowDrop = True
        Me.WebKitBrowser.AllowNewWindows = False
        Me.WebKitBrowser.AutoScroll = True
        Me.WebKitBrowser.BackColor = System.Drawing.Color.White
        Me.WebKitBrowser.Location = New System.Drawing.Point(1, 1)
        Me.WebKitBrowser.Name = "WebKitBrowser"
        Me.WebKitBrowser.PrivateBrowsing = False
        Me.WebKitBrowser.Size = New System.Drawing.Size(800, 550)
        Me.WebKitBrowser.TabIndex = 0
        Me.WebKitBrowser.Url = Nothing
        Me.WebKitBrowser.UseDefaultContextMenu = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(76, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(802, 552)
        Me.Controls.Add(Me.WebKitBrowser)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MainForm"
        Me.Text = "WiFi管家"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WebKitBrowser As WebKit.WebKitBrowser

End Class
