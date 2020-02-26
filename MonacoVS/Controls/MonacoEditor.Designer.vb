<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MonacoEditor
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MonacoEditor))
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.WaitTimer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GalaxyBar1 = New BatchObfuscator.GalaxyBar()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.ScriptErrorsSuppressed = True
        Me.WebBrowser1.ScrollBarsEnabled = False
        Me.WebBrowser1.Size = New System.Drawing.Size(841, 509)
        Me.WebBrowser1.TabIndex = 0
        Me.WebBrowser1.Url = New System.Uri("", System.UriKind.Relative)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GalaxyBar1)
        Me.Panel1.Controls.Add(Me.WebBrowser1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(841, 509)
        Me.Panel1.TabIndex = 1
        '
        'WaitTimer1
        '
        Me.WaitTimer1.Enabled = True
        Me.WaitTimer1.Interval = 10
        '
        'GalaxyBar1
        '
        Me.GalaxyBar1.BackColor = System.Drawing.Color.Transparent
        Me.GalaxyBar1.BackgroundImage = CType(resources.GetObject("GalaxyBar1.BackgroundImage"), System.Drawing.Image)
        Me.GalaxyBar1.Location = New System.Drawing.Point(277, 248)
        Me.GalaxyBar1.Maximum = 100
        Me.GalaxyBar1.MaximumSize = New System.Drawing.Size(300, 8)
        Me.GalaxyBar1.Minimum = 0
        Me.GalaxyBar1.MinimumSize = New System.Drawing.Size(0, 8)
        Me.GalaxyBar1.Name = "GalaxyBar1"
        Me.GalaxyBar1.Size = New System.Drawing.Size(300, 8)
        Me.GalaxyBar1.TabIndex = 1
        Me.GalaxyBar1.Use_Trackbar = False
        Me.GalaxyBar1.Value = 2
        '
        'MonacoEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Controls.Add(Me.Panel1)
        Me.Name = "MonacoEditor"
        Me.Size = New System.Drawing.Size(841, 509)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GalaxyBar1 As BatchObfuscator.GalaxyBar
    Friend WithEvents WaitTimer1 As System.Windows.Forms.Timer

End Class
