Public Class MiscFuncs

    Public Shared Function open(Optional ByVal Custom_Filter As String = "Executables (*.exe)|*.exe") As String
        Using ofd As New OpenFileDialog
            With ofd
                .AddExtension = True
                .AutoUpgradeEnabled = True
                .CheckPathExists = True
                .Title = "Selec File"
                .Filter = Custom_Filter
                .FileName = ""
                .RestoreDirectory = True
            End With
            If ofd.ShowDialog() = DialogResult.OK Then
                Return ofd.FileName
            End If
        End Using
        Return "Error"
    End Function

    Public Shared Function save(Optional ByVal Custom_Filename As String = " ", Optional ByVal Custom_extension As String = "Batch File (*.bat)|*.bat") As String
        Using sfd As New SaveFileDialog()
            With sfd
                .AddExtension = True
                .AutoUpgradeEnabled = True
                .CheckPathExists = True
                .FileName = Custom_Filename
                .Filter = Custom_extension
                .FilterIndex = 2
                .RestoreDirectory = True
                .Title = "Select a file destination to save"
            End With
            If sfd.ShowDialog() = DialogResult.OK Then
                Return sfd.FileName
            End If
        End Using
        Return "Error"
    End Function

End Class
