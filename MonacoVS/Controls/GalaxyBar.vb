Imports System.ComponentModel

Public Class GalaxyBar
#Region "Variables"
    Inherits System.Windows.Forms.UserControl
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
    Private _Minimum As Integer = 0
    Private _Use_Trackbar As Boolean = True
    Private Mouse_Down As Boolean
    Private WithEvents overlap As PictureBox
    Public Event Progress_Scroll()
#End Region
#Region "Properties"
    Public Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(value As Integer)
            _Minimum = value

            If value > _Value Then _Value = value
            If value > _Maximum Then _Maximum = value
            Change_Format()
        End Set
    End Property
    <Category("Control")>
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(V As Integer)
            Select Case V
                Case Is < _Value
                    _Value = V
            End Select
            _Maximum = V
            Change_Format()
        End Set
    End Property
    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                Case Else
                    Return _Value
            End Select
        End Get
        Set(V As Integer)
            Select Case V
                Case Is > _Maximum
                    V = _Maximum
            End Select
            _Value = V
            Change_Format()
        End Set
    End Property
    <Category("Control")>
    Public Property Use_Trackbar() As Boolean
        Get
            Return _Use_Trackbar
        End Get

        Set(V As Boolean)
            _Use_Trackbar = V
        End Set
    End Property
#End Region
#Region "Subs"
    Sub Change_Format()
        'This Sub Changes The Overlap Width , That Means The Progress
        'I Use The Try Method To Avoid Exceptions Of Dividing With Zero... If You Find A Better Method Please Forgive Me For Not Knowing It :)
        Try
            overlap.Width = Math.Round((((_Value - _Minimum) / (_Maximum - _Minimum)) * Width), 0)
        Catch ex As Exception

        End Try

    End Sub
    Private Sub Me_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Change_Format()
    End Sub
    'The Following Subs Makes The ProgressBar Work As An Trackbar, You Can Change This  At The Property Window
    Private Sub _MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove, overlap.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If _Use_Trackbar = True AndAlso Mouse_Down = True AndAlso e.X > -1 AndAlso e.X < (Width + 1) Then
                _Value = _Minimum + CInt((_Maximum - _Minimum) * (e.X / Width))
                Change_Format()
                RaiseEvent Progress_Scroll()
            End If
        End If
    End Sub
    Private Sub _MouseDown(sender As Object, e As MouseEventArgs) Handles overlap.MouseDown, Me.MouseDown

        Mouse_Down = True
        If _Use_Trackbar = True AndAlso Mouse_Down = True AndAlso e.X > -1 AndAlso e.X < (Width + 1) Then
            _Value = _Minimum + CInt((_Maximum - _Minimum) * (e.X / Width))
            Change_Format()
            RaiseEvent Progress_Scroll()
        End If


    End Sub
    Private Sub _Bar_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp, overlap.MouseUp
        Mouse_Down = False
    End Sub
#End Region
#Region "Design"
    Sub New()        'Makes The Design Of The Progress Bar
        With Me
            .Controls.Clear()
            .BackColor = Color.Transparent
            .Size = New System.Drawing.Size(300, 8)
            .BackgroundImageLayout = ImageLayout.Tile
            .BackgroundImage = Convert_Text_To_Image(Empty_Image_Resource)
            .MinimumSize = New System.Drawing.Size(0, 8)
            .MaximumSize = New System.Drawing.Size(300, 8)
            overlap = New PictureBox With {.BackgroundImage = Convert_Text_To_Image(Full_Image_Resource), .BackColor = Color.White, .BackgroundImageLayout = ImageLayout.Tile, .Location = New Point(0, 0), .Width = 0, .Height = 8}
            .Controls.Add(overlap)
            Change_Format()
        End With
    End Sub
    Function Convert_Text_To_Image(ByRef Text As String) As Image
        Dim Str As String = Text
        Dim Bytes As Byte() = Convert.FromBase64String(Str)
        Dim Stream As New IO.MemoryStream(Bytes)
        Return Image.FromStream(Stream)
    End Function
    'These Are The Image Resource Converted To Text So The Can Be Stored Into This File.
    Dim Full_Image_Resource As String = "iVBORw0KGgoAAAANSUhEUgAAASwAAAAICAYAAABDN8LhAAAACXBIWXMAAAsSAAALEgHS3X78AAAAB3RJTUUH3QkeFw4UQpKrigAAAWFJREFUaEPt2LtKA0EYxfFvFy8o2qWys1JEEOKtsLDyJWwtfQBFbFLY+x52Yh8lu5s1mwiBQEDwEWxUvCFnPHsxGkmbrc7Aj2Gzs1P+mYkNRttNWgu7XhM1uvIi1/ZCdLyQc4Qkm0VExiXtTNqdCHdsz6XfxJEl2LI+5otKFaPn5izCDhdecOEjgRs4EZFSRX+f8ZSGy5rYM+f8olYcIVb90B0zWC0ueBnaQESkbFm48JGeuvwYh9bBwm+0EmzzxTn1ueht5AYiImUK8Mke9ahmMdZ4C5zJg9Vw6/zxjLpc9DryYxGRsuQnrDRYXT/CibXdst1jOg/WDRZZrwMuvOYpi/fGfx+LiJQO7zxABWzTPoM1m8fqZ9RdxWtkp6wHRusrK9zQH2AiImM26A6eGatbP8RpdroaOXpuii8r1sKSJahagI30usjCVbNZRGRc0s7EnENsWoyVrEV1N1HUicPsG1CfccDBu6a+AAAAAElFTkSuQmCC"
    Dim Empty_Image_Resource As String = "iVBORw0KGgoAAAANSUhEUgAAASwAAAAICAYAAABDN8LhAAAACXBIWXMAAAsSAAALEgHS3X78AAAAB3RJTUUH3QkeFw4tHZcjggAAAOVJREFUaEPt2rsKwjAUBuAguIk4iINSUtILpTjZWQdfwBfwBQQXHRwcvIwiOIjXt/VvPILUzqbDf+AjNDlZf06g6lPGmE4QBJM4jhdRFK2JiBybI5fGWuuWxNS7cGBgF4bhA55ERFWAXLpjkFpikGpLXCmVb+DwUmwmInIN+XTCOrVhlWVZHR/H7wYioqrIpyysKxtYeWFjL5ulF4iIHDrjSTiTuLKBNYJDSSMRkWtbPAsHEle2agisIQ42WG+FZiKiv0MWXfO/FpIk6UtO/RYam2js+b6viYhc8Dyvm6ZpQ2JJSqkXABC20Ja0BDUAAAAASUVORK5CYII="
#End Region
End Class

