Imports Microsoft.Win32

Public Class MonacoEditor

    Public MainPage As String = Application.StartupPath & "\" & "Monaco\Monaco.html"

    Private Sub MonacoEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        ForceIE.SetBrowserFeatureControl()
        If FileExist(MainPage) = True Then
            WebBrowser1.Url = New Uri(MainPage)
        Else
            MsgBox("Monaco Text Engine not Fount")
        End If
    End Sub

    Public Function FileExist(ByVal File As String) As Boolean
        Return My.Computer.FileSystem.FileExists(File)
    End Function

    Private Sub WebBrowser1_ProgressChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserProgressChangedEventArgs) Handles WebBrowser1.ProgressChanged
        GalaxyBar1.Maximum = e.MaximumProgress
        GalaxyBar1.Value = e.CurrentProgress
    End Sub

    Dim DocumentFinalize As Boolean = False
    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        DocumentFinalize = True
    End Sub

#Region " TimerEfect "

    Dim waitPB As Integer = 0
    Private Sub WaitTimer1_Tick(sender As Object, e As EventArgs) Handles WaitTimer1.Tick
        If DocumentFinalize = True Then
            If waitPB = 20 Then
                GalaxyBar1.Visible = False
                WaitTimer1.Enabled = False
            Else
                waitPB += 1
            End If
        End If
    End Sub

#End Region
   

End Class
