Imports Microsoft.Win32
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports BatchObfuscator.MonacoNET.Settings

Namespace MonacoNET
    Public Enum MonacoTheme
        Light = 0
        Dark = 1
    End Enum

    Public Enum MonacoLang
        Lua = 0
        Bat = 1
    End Enum

    Public Class RegistryException
        Inherits Exception

        Private Msg As String = "A Registry Exception has Occured"

        Public Overrides ReadOnly Property Message As String
            Get
                Return Msg
            End Get
        End Property

        Public Sub New(Optional ByVal ExceptionMessage As String = "A Registry Exception has Occured")
            Msg = ExceptionMessage
        End Sub
    End Class

    <ComVisible(True)>
    Public Class Monaco
        Inherits WebBrowser

        Private tStart As Thread
        Private ReadOnlyObj As Boolean = False

        Public Property [ReadOnly] As Boolean
            Get
                Return ReadOnlyObj
            End Get
            Set(ByVal value As Boolean)
                ReadOnlyObj = value
            End Set
        End Property

        Private MinimapEnabledObj As Boolean = False

        Public Property MinimapEnabled As Boolean
            Get
                Return MinimapEnabledObj
            End Get
            Set(ByVal value As Boolean)
                MinimapEnabledObj = value
            End Set
        End Property

        Private _Text As String = String.Empty
        Public Overrides Property Text As String
            Get
                Return GetText()
            End Get
            Set(ByVal value As String)
                _Text = value
                SetText(value)
            End Set
        End Property

        Private RenderWhitespaceObj As String = "none"

        Public Property RenderWhitespace As String
            Get
                Return RenderWhitespaceObj
            End Get
            Set(ByVal value As String)

                Select Case value
                    Case "none"
                        RenderWhitespaceObj = value
                    Case "all"
                        RenderWhitespaceObj = value
                    Case "boundary"
                        RenderWhitespaceObj = value
                    Case Else
                        RenderWhitespaceObj = "none"
                End Select
            End Set
        End Property

        Public MainPage As String = Application.StartupPath & "\" & "Monaco\Monaco.html"
        Public Sub New()
            Try
                Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", True)
                Dim name As String = AppDomain.CurrentDomain.FriendlyName

                If CObj(key.GetValue(name)) Is Nothing Then
                    key.SetValue(name, 11001, RegistryValueKind.DWord)
                End If

                Me.ScriptErrorsSuppressed = True
                Me.ObjectForScripting = Me
                Me.Size = New Size(841, 509)
            Catch e As Exception
                MessageBox.Show("Error in Monaco Class Constructor: " & e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
            If FileExist(MainPage) = True Then
                Me.Url = New Uri(MainPage)
                AddHandler Me.DocumentCompleted, AddressOf OnDocumentLoaded
            Else
                MsgBox("Monaco Text Engine not Fount")
            End If
        End Sub

        Public Function FileExist(ByVal File As String) As Boolean
            Return My.Computer.FileSystem.FileExists(File)
        End Function

        Public Sub OnDocumentLoaded(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
            Try
                tStart = New Thread(New ThreadStart(AddressOf OnMonacoLoad))
                tStart.Start()
            Catch __unusedException1__ As Exception
                tStart.Start()
            End Try
        End Sub

        Public Sub SetLang(ByVal theme As MonacoLang)
            If Me.Document IsNot Nothing Then

                Select Case CType(theme, MonacoLang)
                    Case MonacoLang.Lua
                        Me.Document.InvokeScript("SetLang", New Object() {"Lua"})
                    Case MonacoLang.Bat
                        Me.Document.InvokeScript("SetLang", New Object() {"Bat"})
                End Select
            Else
                Throw New Exception("Cannot set Monaco theme while Document is null.")
            End If
        End Sub

        Public Sub SetTheme(ByVal theme As MonacoTheme)
            If Me.Document IsNot Nothing Then

                Select Case CType(theme, MonacoTheme)
                    Case MonacoTheme.Dark
                        Me.Document.InvokeScript("SetTheme", New Object() {"Dark"})
                    Case MonacoTheme.Light
                        Me.Document.InvokeScript("SetTheme", New Object() {"Light"})
                End Select
            Else
                Throw New Exception("Cannot set Monaco theme while Document is null.")
            End If
        End Sub

        Public Sub SetText(ByVal text As String)
            If Me.Document IsNot Nothing Then
                Me.Document.InvokeScript("SetText", New Object() {text})
            Else
                Throw New Exception("Cannot set Monaco's text while Document is null.")
            End If
        End Sub

        Public Function GetText() As String
            If Me.Document IsNot Nothing Then
                Return TryCast(Me.Document.InvokeScript("GetText"), String)
            Else
                Throw New Exception("Cannot get Monaco's text while Document is null.")
            End If
        End Function

        Public Sub AppendText(ByVal text As String)
            If Me.Document IsNot Nothing Then
                SetText(GetText() & text)
            Else
                Throw New Exception("Cannot append Monaco's text while Document is null.")
            End If
        End Sub

        Public Sub GoToLine(ByVal lineNumber As Integer)
            If Me.Document IsNot Nothing Then
                Me.Document.InvokeScript("SetScroll", New Object() {lineNumber})
            Else
                Throw New Exception("Cannot go to Line in Monaco's Editor while Document is null.")
            End If
        End Sub

        Public Sub EditorRefresh()
            If Me.Document IsNot Nothing Then
                Me.Document.InvokeScript("Refresh")
            Else
                Throw New Exception("Cannot refresh Monaco's editor while the Document is null.")
            End If
        End Sub

        Public Sub UpdateSettings(ByVal settings As MonacoSettings)
            If Me.Document IsNot Nothing Then
                Me.Document.InvokeScript("SwitchMinimap", New Object() {settings.MinimapEnabled})
                Me.Document.InvokeScript("SwitchReadonly", New Object() {settings.[ReadOnly]})
                Me.Document.InvokeScript("SwitchRenderWhitespace", New Object() {settings.RenderWhitespace})
                Me.Document.InvokeScript("SwitchLinks", New Object() {settings.Links})
                Me.Document.InvokeScript("SwitchLineHeight", New Object() {settings.LineHeight})
                Me.Document.InvokeScript("SwitchFontSize", New Object() {settings.FontSize})
                Me.Document.InvokeScript("SwitchFolding", New Object() {settings.Folding})
                Me.Document.InvokeScript("SwitchAutoIndent", New Object() {settings.AutoIndent})
                Me.Document.InvokeScript("SwitchFontFamily", New Object() {settings.FontFamily})
                Me.Document.InvokeScript("SwitchFontLigatures", New Object() {settings.FontLigatures})
            Else
                Throw New Exception("Cannot change Monaco's settings while Document is null.")
            End If
        End Sub

        Public Sub AddIntellisense(ByVal label As String, ByVal type As String, ByVal description As String, ByVal insert As String)
            If Me.Document IsNot Nothing Then
                Me.Document.InvokeScript("AddIntellisense", New Object() {label, type, description, insert})
            Else
                Throw New Exception("Cannot edit Monaco's Intellisense while Document is null.")
            End If
        End Sub

        Public Sub ShowSyntaxError(ByVal line As Integer, ByVal column As Integer, ByVal endLine As Integer, ByVal endColumn As Integer, ByVal message As String)
            If Me.Document IsNot Nothing Then
                Me.Document.InvokeScript("ShowErr", New Object() {line, column, endLine, endColumn, message})
            Else
                Throw New Exception("Cannot show Syntax Error for Monaco while Document is null.")
            End If
        End Sub

        Protected Overridable Sub OnMonacoLoad()
            Application.DoEvents()
            Thread.Sleep(100)

            Me.BeginInvoke(New MethodInvoker(Function()
                                                 UpdateSettings(New MonacoSettings() With {
                                                     .[ReadOnly] = ReadOnlyObj,
                                                     .MinimapEnabled = MinimapEnabledObj,
                                                     .RenderWhitespace = RenderWhitespaceObj
})
                                                 If Not _Text = String.Empty Then : SetText(_Text) : End If

                                             End Function))
        End Sub
    End Class
End Namespace
