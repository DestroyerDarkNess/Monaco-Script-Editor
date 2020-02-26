Imports System.IO
Imports System.Text

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        ForceIE.SetBrowserFeatureControl()
        load1()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.Log = LogTextBox.Text
        My.Settings.Save()
    End Sub

#Region " Declare's "

    Public ContarTabPage As Integer = 0
    Public TextControlName As String = "Monaco"
    Public LogTextBox As TextBox = New TextBox

#End Region

#Region "explorer treeview"

    Private Sub load1()
        Tv_ImgList.ImageSize = New Size(20, 20)
        TV_Explorer.ImageList = Tv_ImgList

        AddSpecialAndStandardFolderImages()

        AddSpecialFolderRootNode(SpecialNodeFolders.Desktop)
        AddSpecialFolderRootNode(SpecialNodeFolders.MyDocuments)
        AddDriveRootNodes()
    End Sub
    Private Sub TV_Explorer_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TV_Explorer.BeforeExpand
        Dim DrvIsReady As Boolean = (From d As DriveInfo In DriveInfo.GetDrives Where d.Name = e.Node.ImageKey Select d.IsReady).FirstOrDefault

        If (e.Node.ImageKey <> "Desktop" AndAlso Not e.Node.ImageKey.Contains(":\")) OrElse DrvIsReady OrElse Directory.Exists(e.Node.ImageKey) Then
            e.Node.Nodes.Clear()
            AddChildNodes(e.Node, e.Node.Tag.ToString)

        ElseIf e.Node.ImageKey = "Desktop" Then
            e.Node.Nodes.Clear()
            Dim PublicDesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)
            Dim CurrentUserDesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            AddChildNodes(e.Node, CurrentUserDesktopFolder)
            AddChildNodes(e.Node, PublicDesktopFolder)

        Else
            e.Cancel = True
            MessageBox.Show("The CD or DVD drive is empty.", "Drive Info...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub TV_Explorer_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles TV_Explorer.AfterCollapse
        e.Node.Nodes.Clear()
        e.Node.Nodes.Add("Empty")
    End Sub

    Dim directorios As String = String.Empty

    Private Sub TV_Explorer_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TV_Explorer.AfterSelect
        Try
            directorios = e.Node.Tag.ToString
            ' TxtBx_Path.Text = directorios
            ToolTip1.SetToolTip(Me.TV_Explorer, directorios)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TxtBx_Path_MouseMove(sender As Object, e As MouseEventArgs) 'Handles TxtBx_Path.MouseMove
        ' ToolTip1.SetToolTip(Me.TxtBx_Path, directorios)
    End Sub

    Private Sub TV_Explorer_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TV_Explorer.NodeMouseDoubleClick
        If e.Button = MouseButtons.Left AndAlso File.Exists(e.Node.Tag.ToString) Then
            Try
                Dim extension As String = Path.GetExtension(e.Node.Tag.ToString)
                Dim namefile As String = Path.GetFileName(e.Node.Tag.ToString)
                If FormatOpen(extension) = True Then
                    Newtap(e.Node.Tag.ToString, namefile)
                Else
                    Process.Start(e.Node.Tag.ToString)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub AddSpecialFolderRootNode(SpecialFolder As SpecialNodeFolders)
        Dim SpecialFolderPath As String = Environment.GetFolderPath(CType(SpecialFolder, Environment.SpecialFolder))
        Dim SpecialFolderName As String = Path.GetFileName(SpecialFolderPath)

        AddImageToImgList(SpecialFolderPath, SpecialFolderName)

        Dim DesktopNode As New TreeNode(SpecialFolderName)
        With DesktopNode
            .Tag = SpecialFolderPath
            .ImageKey = SpecialFolderName
            .SelectedImageKey = SpecialFolderName
            .Nodes.Add("Empty")
        End With

        TV_Explorer.Nodes.Add(DesktopNode)
    End Sub

    Private Sub AddDriveRootNodes()
        For Each drv As DriveInfo In DriveInfo.GetDrives
            AddImageToImgList(drv.Name)
            Dim DriveNode As New TreeNode(drv.Name)
            With DriveNode
                .Tag = drv.Name
                .ImageKey = drv.Name
                .SelectedImageKey = drv.Name
                .Nodes.Add("Empty")
            End With
            TV_Explorer.Nodes.Add(DriveNode)
        Next
    End Sub

    Private Sub AddCustomFolderRootNode(folderpath As String)
        If Directory.Exists(folderpath) Then
            Dim FolderName As String = New DirectoryInfo(folderpath).Name
            AddImageToImgList(folderpath)
            Dim rootNode As New TreeNode(FolderName)
            With rootNode
                .Tag = folderpath
                .ImageKey = folderpath
                .SelectedImageKey = folderpath
                If Directory.GetDirectories(folderpath).Count > 0 OrElse Directory.GetFiles(folderpath).Count > 0 Then
                    .Nodes.Add("Empty")
                End If
            End With
            TV_Explorer.Nodes.Add(rootNode) 'add this root node to the treeview
        End If
    End Sub

    Private Sub AddChildNodes(tn As TreeNode, DirPath As String)
        Dim DirInfo As New DirectoryInfo(DirPath)
        Try
            For Each di As DirectoryInfo In DirInfo.GetDirectories
                If Not (di.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    Dim FolderNode As New TreeNode(di.Name)
                    With FolderNode
                        .Tag = di.FullName
                        If Tv_ImgList.Images.Keys.Contains(di.FullName) Then
                            .ImageKey = di.FullName
                            .SelectedImageKey = di.FullName
                        Else
                            .ImageKey = "Folder"
                            .SelectedImageKey = "Folder"
                        End If
                        .Nodes.Add("*Empty*")
                    End With
                    tn.Nodes.Add(FolderNode)
                End If
            Next
            For Each fi As FileInfo In DirInfo.GetFiles
                If Not (fi.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    Dim ImgKey As String = AddImageToImgList(fi.FullName)
                    Dim FileNode As New TreeNode(fi.Name)
                    With FileNode
                        .Tag = fi.FullName
                        .ImageKey = ImgKey
                        .SelectedImageKey = ImgKey
                    End With
                    tn.Nodes.Add(FileNode)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AddSpecialAndStandardFolderImages()
        AddImageToImgList(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Folder")

        Dim SpecialFolders As New List(Of Environment.SpecialFolder)
        With SpecialFolders
            .Add(Environment.SpecialFolder.Desktop)
            .Add(Environment.SpecialFolder.MyDocuments)
            .Add(Environment.SpecialFolder.Favorites)
            .Add(Environment.SpecialFolder.Recent)
            .Add(Environment.SpecialFolder.MyMusic)
            .Add(Environment.SpecialFolder.MyVideos)
            .Add(Environment.SpecialFolder.Fonts)
            .Add(Environment.SpecialFolder.History)
            .Add(Environment.SpecialFolder.MyPictures)
            .Add(Environment.SpecialFolder.UserProfile)
        End With

        For Each sf As Environment.SpecialFolder In SpecialFolders
            AddImageToImgList(Environment.GetFolderPath(sf))
        Next
    End Sub

    Private Function AddImageToImgList(FullPath As String, Optional SpecialImageKeyName As String = "") As String
        Dim ImgKey As String = If(SpecialImageKeyName = "", FullPath, SpecialImageKeyName)
        Dim LoadFromExt As Boolean = False

        If ImgKey = FullPath AndAlso File.Exists(FullPath) Then
            Dim ext As String = Path.GetExtension(FullPath).ToLower
            If ext <> "" AndAlso ext <> ".exe" AndAlso ext <> ".lnk" AndAlso ext <> ".url" Then
                ImgKey = Path.GetExtension(FullPath).ToLower
                LoadFromExt = True
            End If
        End If

        If Not Tv_ImgList.Images.Keys.Contains(ImgKey) Then
            Tv_ImgList.Images.Add(ImgKey, Iconhelper.GetIconImage(If(LoadFromExt, ImgKey, FullPath), IconSizes.Large32x32))
        End If

        Return ImgKey
    End Function

    Private Enum SpecialNodeFolders As Integer
        Desktop = Environment.SpecialFolder.Desktop
        Favorites = Environment.SpecialFolder.Favorites
        History = Environment.SpecialFolder.History
        MyDocuments = Environment.SpecialFolder.MyDocuments
        MyMusic = Environment.SpecialFolder.MyMusic
        MyPictures = Environment.SpecialFolder.MyPictures
        MyVideos = Environment.SpecialFolder.MyVideos
        Recent = Environment.SpecialFolder.Recent
        UserProfile = Environment.SpecialFolder.UserProfile
    End Enum

    Private Function FormatOpen(ByVal formato As String) As Boolean
        If formato = ".txt" Then
            Return True
        ElseIf formato = ".bat" Then
            Return True
        ElseIf formato = ".cs" Then
            Return True
        ElseIf formato = ".vb" Then
            Return True
        ElseIf formato = ".lua" Then
            Return True
        ElseIf formato = ".html" Then
            Return True
        ElseIf formato = ".php" Then
            Return True
        ElseIf formato = ".css" Then
            Return True
        ElseIf formato = ".cpp" Then
            Return True
        ElseIf formato = ".vbproj" Then
            Return True
        ElseIf formato = ".xml" Then
            Return True
        ElseIf formato = ".manifest" Then
            Return True
        ElseIf formato = ".vcxproj" Then
            Return True
        ElseIf formato = ".tlog" Then
            Return True
        ElseIf formato = ".obj" Then
            Return True
        ElseIf formato = ".log" Then
            Return True
        ElseIf formato = ".cmd" Then
            Return True
        ElseIf formato = ".wsf" Then
            Return True
        ElseIf formato = ".exe" Then
            Return True
        ElseIf formato = ".py" Then
            Return True
        ElseIf formato = ".vbs" Then
            Return True
        End If
        Return False
    End Function

    Private Sub Newtap(ByVal Dir As String, ByVal filename As String)
        AddTabpage(filename, Dir)
    End Sub

#End Region

#Region " TabControlManager "

    Public Sub AddTabpage(ByVal Name As String, Optional ByVal Dir As String = "")
        Dim newPage As New TabPage()
        newPage.Text = Name
        newPage.Name = Name
        VisualStudioTabControl1.TabPages.Add(newPage)
        If Not Dir = "" Then
            Dim SR As New IO.StreamReader(Dir)
            Dim extension As String = Path.GetExtension(Dir)
            Dim namePage As String = "Tab" & ContarTabPage
            AddControl(newPage, SR.ReadToEnd, Structures.DetectLexer(extension), Dir)
            SR.Close()
        Else
            AddControl(newPage, "", Structures.Lexer.none, Dir)
        End If
        ContarTabPage += 1
        VisualStudioTabControl1.SelectedTab = newPage
    End Sub

    Public Sub AddControl(ByVal PageName As TabPage, _
                          Optional ByVal Text As String = "", Optional ByVal Lexer As Structures.Lexer = Structures.Lexer.none, _
                          Optional ByVal Dir As String = "")

        Dim labeldirectorio As New Label
        labeldirectorio.Text = Dir
        labeldirectorio.Visible = False
        PageName.Controls.Add(labeldirectorio)

        Select Case LCase(TextControlName)
            Case "monaco" : GoTo Monaco
        End Select
        Exit Sub
Monaco:
        Dim newControlText As New MonacoNET.Monaco
        newControlText.Dock = DockStyle.Fill
        newControlText.Visible = True
        newControlText.Text = Text
        PageName.Controls.Add(newControlText)

    End Sub

#End Region

#Region " IDE FUNCS "

#Region " TapFunctions"

    Public Shared Function GetNametap(ByVal tapcontrol As TabControl) As String
        Return tapcontrol.SelectedTab.Text
    End Function

    Public Shared Function GetTextTap(ByVal tapcontrol As TabControl) As String
        For Each childControl In tapcontrol.SelectedTab.Controls
            If TypeOf childControl Is MonacoNET.Monaco Then
                Return childControl.Text.ToString
            End If
        Next
        Return "Error GetText"
    End Function

    Public Shared Sub SetTapTheme(ByVal tapcontrol As TabControl, ByVal Theme As MonacoNET.MonacoTheme)
        For Each childControl In tapcontrol.SelectedTab.Controls
            If TypeOf childControl Is MonacoNET.Monaco Then
                childControl.SetTheme(Theme)
            End If
        Next
    End Sub

    Public Shared Sub SetTapSyntax(ByVal tapcontrol As TabControl, ByVal Language As MonacoNET.MonacoLang)
        For Each childControl In tapcontrol.SelectedTab.Controls
            If TypeOf childControl Is MonacoNET.Monaco Then
                childControl.SetLang(Language)
            End If
        Next
    End Sub

    Public Shared Sub ChangeTextDirectory(ByVal tapcontrol As TabControl, ByVal newDirectory As String)
        For Each childControl In tapcontrol.SelectedTab.Controls
            If TypeOf childControl Is Label Then
                childControl.Text = newDirectory
            End If
        Next
    End Sub

    Public Shared Function GetTextDirectory(ByVal tapcontrol As TabControl) As String
        For Each childControl In tapcontrol.SelectedTab.Controls
            If TypeOf childControl Is Label Then
                Return childControl.Text.ToString
            End If
        Next
        Return "Error GetTextDirectory"
    End Function

    Public Shared Function IsContainsExtension(ByVal File As String) As Boolean
        Dim VerifiEx As String = Path.GetExtension(File)
        If VerifiEx = String.Empty Then
            Return False
        End If
        Return True
    End Function

    Public Sub Writelog(ByVal texto As String)
        Dim CurrentText As String = My.Settings.Log
        If CurrentText = "none" Then
            My.Settings.Log = (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & "  >>  ") & texto & vbNewLine
        Else
            My.Settings.Log = CurrentText & vbNewLine & (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & "  >>  ") & texto & vbNewLine
        End If
        My.Settings.Save()
    End Sub

#End Region

    Public Sub SaveAs()
        Using sfd As New SaveFileDialog()
            Try
                Dim name As String = GetNametap(VisualStudioTabControl1)
                Dim texto As String = GetTextTap(VisualStudioTabControl1)
                Dim extension As String = Path.GetExtension(name)
                With sfd
                    .AddExtension = True
                    .AutoUpgradeEnabled = True
                    .CheckPathExists = True
                    .FileName = name
                    .Filter = "Open Save file (*.*)|*.*"
                    .FilterIndex = 2
                    .RestoreDirectory = True
                    .Title = "Select a file destination to save"
                End With
                If sfd.ShowDialog() = DialogResult.OK Then
                    If LogTextBox.Text = "" Then
                        LogTextBox.Text = (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "  >>  Save File  " & sfd.FileName & "   |   File :  " & name & "   |   Extension :  " & extension)
                    Else
                        LogTextBox.Text = LogTextBox.Text & vbNewLine & (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "  >>  Save File  " & sfd.FileName & "   |   File :  " & name & "   |   Extension :  " & extension)
                    End If
                    If IsContainsExtension(sfd.FileName) = True Then
                        File.WriteAllText(sfd.FileName, texto)
                        VisualStudioTabControl1.SelectedTab.Text = Path.GetFileName(sfd.FileName)
                        ChangeTextDirectory(VisualStudioTabControl1, sfd.FileName)
                    Else
                        File.WriteAllText(sfd.FileName & extension, texto)
                        VisualStudioTabControl1.SelectedTab.Text = Path.GetFileName(sfd.FileName & extension)
                        ChangeTextDirectory(VisualStudioTabControl1, sfd.FileName & extension)
                    End If
                End If
            Catch ex As Exception
                Writelog("Error ( Control : SaveAs()1 ) : " & ex.Message & "Please Contact : Official Page or ( Email ; s4lsalsoft@gmail.com )")
            End Try
        End Using
    End Sub

    Public Sub Save()
        Try
            Dim patch As String = GetTextDirectory(VisualStudioTabControl1)
            If patch = "" Then
                SaveAs()
            Else
                Dim textos = GetTextTap(VisualStudioTabControl1)
                File.WriteAllText(patch, textos, Encoding.UTF8)
            End If
        Catch ex As Exception
            Writelog("Error ( Control : Save()1 ) : " & ex.Message & "Please Contact : Official Page or ( Email ; s4lsalsoft@gmail.com )")
        End Try
    End Sub

    Public Sub Open()
        OpenFileDialog1.Title = "Selec File"
        OpenFileDialog1.Filter = "Open Select files (*.*)|*.*|Batch files (*.bat)|*.bat|VBS (*.vbs)|*.vbs|Javas Script (*.js)|*.js|CMD files (*.cmd)|*.cmd|HTML files (*.html)|*.html|Visual Basic files (*.vb)|*.vb|CSharp files (*.cs)|*.cs|txt files (*.cmd)|*.txt"
        Try

            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim Filedir() As String = OpenFileDialog1.FileNames

                If Filedir.Count > 1 Then
                    For Each Rute As String In Filedir
                        Dim FileN As String = Path.GetFileName(Rute)
                        AddTabpage(FileN, Rute)
                    Next
                Else
                    Dim FileN As String = Path.GetFileName(Filedir(0))
                    AddTabpage(FileN, Filedir(0))
                End If
            End If

        Catch ex As Exception
            Writelog("Error ( Control : Open()1 ) : " & ex.Message & "Please Contact : Official Page or ( Email ; s4lsalsoft@gmail.com )")
        End Try
    End Sub

    Public Sub CloseTab()
        Dim name As String = GetTextDirectory(VisualStudioTabControl1)
        Dim extension As String = Path.GetExtension(name)
        If VisualStudioTabControl1.TabCount = 1 Then
            If Not VisualStudioTabControl1.SelectedTab.Text = "Untitle" Then
                Writelog(("Close File  " & name & "   |   File :  " & GetNametap(VisualStudioTabControl1) & "   |   Extension :  " & extension))
                VisualStudioTabControl1.TabPages.Remove(VisualStudioTabControl1.SelectedTab)
            End If
        Else
            Writelog(("Close File  " & name & "   |   File :  " & GetNametap(VisualStudioTabControl1) & "   |   Extension :  " & extension))
            VisualStudioTabControl1.TabPages.Remove(VisualStudioTabControl1.SelectedTab)
        End If

    End Sub

#End Region

    Private Sub Timer_Tab1_Tick(sender As Object, e As EventArgs) Handles Timer_Tab1.Tick
        If VisualStudioTabControl1.TabCount = 0 Then
            AddTabpage("Untitle")
        End If
    End Sub

#Region " FileStrip "

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Open()
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        AddTabpage("Untitle")
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Save()
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        SaveAs()
    End Sub

    Private Sub NewWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewWindowsToolStripMenuItem.Click
        Shell(Application.ExecutablePath)
    End Sub

    Private Sub CloseWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseWindowsToolStripMenuItem.Click
        End
    End Sub

    Private Sub CloseFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseFileToolStripMenuItem.Click
        CloseTab()
    End Sub

    Private Sub CloseAllFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllFilesToolStripMenuItem.Click
        VisualStudioTabControl1.TabPages.Clear()
        AddTabpage("Untitle")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

#End Region

#Region " Menu Strip "

    Private Sub LogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogToolStripMenuItem.Click
        Dim newPage As New TabPage()
        newPage.Text = "Log"
        newPage.Name = "Log"
        VisualStudioTabControl1.TabPages.Add(newPage)
        Dim textolog As String = My.Settings.Log
        If textolog = "" Then : textolog = "none" : End If
        AddControl(newPage, textolog, Structures.Lexer.none)
    End Sub

    Private Sub ClearLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearLogToolStripMenuItem.Click
        My.Settings.Log = "none"
        My.Settings.Save()
    End Sub

#End Region

#Region " View Strip "

    Private Sub DarkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DarkToolStripMenuItem.Click
        SetTapTheme(VisualStudioTabControl1, MonacoNET.MonacoTheme.Dark)
    End Sub

    Private Sub LightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LightToolStripMenuItem.Click
        SetTapTheme(VisualStudioTabControl1, MonacoNET.MonacoTheme.Light)
    End Sub

#End Region

#Region " Tools Strip "

    Private Sub ToHTMLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToHTMLToolStripMenuItem.Click
        Dim name As String = Path.GetFileNameWithoutExtension(GetNametap(VisualStudioTabControl1))
        Dim NewTexto As String = ExportFuncs.ToHTML(GetTextTap(VisualStudioTabControl1))

        Dim newPage As New TabPage()
        newPage.Text = name & "toHTML"
        newPage.Name = name & "toHTML"
        VisualStudioTabControl1.TabPages.Add(newPage)

        Dim labeldirectorio As New Label
        labeldirectorio.Text = ""
        labeldirectorio.Visible = False
        newPage.Controls.Add(labeldirectorio)



        AddControl(newPage, NewTexto, Structures.Lexer.Html)
    End Sub

    Private Sub ReplaceLowercaseAlphaCharsWithVariablesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReplaceLowercaseAlphaCharsWithVariablesToolStripMenuItem.Click
        Dim name As String = Path.GetFileNameWithoutExtension(GetNametap(VisualStudioTabControl1))
        Dim NewTexto As String = ObfuzFuncs.BatOfuser.BatchObfuscator(GetTextTap(VisualStudioTabControl1), ObfuzFuncs.BatOfuser.BatSettingObz.RV)

        Dim newPage As New TabPage()
        newPage.Text = name & "Obz1.bat"
        newPage.Name = name & "Obz1.bat"
        VisualStudioTabControl1.TabPages.Add(newPage)

        Dim labeldirectorio As New Label
        labeldirectorio.Text = ""
        labeldirectorio.Visible = False
        newPage.Controls.Add(labeldirectorio)

        AddControl(newPage, NewTexto, Structures.Lexer.Batch)
    End Sub

    Private Sub InsertJunkVariablesInBetweenCharactersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertJunkVariablesInBetweenCharactersToolStripMenuItem.Click
        Dim name As String = Path.GetFileNameWithoutExtension(GetNametap(VisualStudioTabControl1))
        Dim NewTexto As String = ObfuzFuncs.BatOfuser.BatchObfuscator(GetTextTap(VisualStudioTabControl1), ObfuzFuncs.BatOfuser.BatSettingObz.RC)

        Dim newPage As New TabPage()
        newPage.Text = name & "Obz2.bat"
        newPage.Name = name & "Obz2.bat"
        VisualStudioTabControl1.TabPages.Add(newPage)

        Dim labeldirectorio As New Label
        labeldirectorio.Text = ""
        labeldirectorio.Visible = False
        newPage.Controls.Add(labeldirectorio)

        AddControl(newPage, NewTexto, Structures.Lexer.Batch)
    End Sub

    Private Sub MakeTheFileUnreadableInTextEditorsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MakeTheFileUnreadableInTextEditorsToolStripMenuItem.Click
        Dim name As String = Path.GetFileNameWithoutExtension(GetNametap(VisualStudioTabControl1))
        Dim NewTexto As String = ObfuzFuncs.BatOfuser.BatchObfuscator(GetTextTap(VisualStudioTabControl1), ObfuzFuncs.BatOfuser.BatSettingObz.UR)
    End Sub

#End Region

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/DestroyerDarkNess/Monaco-Script-Editor")
    End Sub

End Class
