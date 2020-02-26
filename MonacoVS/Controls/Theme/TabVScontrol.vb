Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Windows.Forms

Public Class TabVScontrol
    Inherits TabControl

    Private ReadOnly CenterSringFormat As StringFormat = New StringFormat With {
        .Alignment = StringAlignment.Near,
        .LineAlignment = StringAlignment.Center
    }
    Private activeColor As Color = Color.FromArgb(0, 122, 204)
    Private backTabColor As Color = Color.FromArgb(28, 28, 28)
    Private borderColor As Color = Color.FromArgb(30, 30, 30)
    Private closingButtonColor As Color = Color.WhiteSmoke
    Private closingMessage As String
    Private headerColor As Color = Color.FromArgb(45, 45, 48)
    Private horizLineColor As Color = Color.FromArgb(0, 122, 204)
    Private predraggedTab As TabPage
    Private textColor As Color = Color.FromArgb(255, 255, 255)
    Public Property ShowClosingButton As Boolean
    Public selectedTextColor As Color = Color.FromArgb(255, 255, 255)

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Normal
        ItemSize = New Size(240, 16)
        AllowDrop = True
    End Sub

    <Category("Colors"), Browsable(True), Description("The color of the selected page")>
    Public Property ActiveColor1 As Color
        Get
            Return Me.activeColor
        End Get
        Set(ByVal value As Color)
            Me.activeColor = value
        End Set
    End Property

    <Category("Colors"), Browsable(True), Description("The color of the background of the tab")>
    Public Property BackTabColor1 As Color
        Get
            Return Me.backTabColor
        End Get
        Set(ByVal value As Color)
            Me.backTabColor = value
        End Set
    End Property

    <Category("Colors"), Browsable(True), Description("The color of the border of the control")>
    Public Property BorderColor1 As Color
        Get
            Return Me.borderColor
        End Get
        Set(ByVal value As Color)
            Me.borderColor = value
        End Set
    End Property

    <Category("Colors"), Browsable(True), Description("The color of the closing button")>
    Public Property ClosingButtonColor1 As Color
        Get
            Return Me.closingButtonColor
        End Get
        Set(ByVal value As Color)
            Me.closingButtonColor = value
        End Set
    End Property

    <Category("Options"), Browsable(True), Description("The message that will be shown before closing.")>
    Public Property ClosingMessage1 As String
        Get
            Return Me.closingMessage
        End Get
        Set(ByVal value As String)
            Me.closingMessage = value
        End Set
    End Property

    <Category("Colors"), Browsable(True), Description("The color of the header.")>
    Public Property HeaderColor1 As Color
        Get
            Return Me.headerColor
        End Get
        Set(ByVal value As Color)
            Me.headerColor = value
        End Set
    End Property

    <Category("Colors"), Browsable(True), Description("The color of the horizontal line which is located under the headers of the pages.")>
    Public Property HorizontalLineColor As Color
        Get
            Return Me.horizLineColor
        End Get
        Set(ByVal value As Color)
            Me.horizLineColor = value
        End Set
    End Property

    <Category("Options"), Browsable(True), Description("Show a Yes/No message before closing?")>
    Public Property ShowClosingMessage As Boolean

    <Category("Colors"), Browsable(True), Description("The color of the title of the page")>
    Public Property SelectedTextColor1 As Color
        Get
            Return Me.selectedTextColor
        End Get
        Set(ByVal value As Color)
            Me.selectedTextColor = value
        End Set
    End Property

    <Category("Colors"), Browsable(True), Description("The color of the title of the page")>
    Public Property TextColor1 As Color
        Get
            Return Me.textColor
        End Get
        Set(ByVal value As Color)
            Me.textColor = value
        End Set
    End Property

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

    Protected Overrides Sub OnDragOver(ByVal drgevent As DragEventArgs)
        Dim draggedTab = CType(drgevent.Data.GetData(GetType(TabPage)), TabPage)
        Dim pointedTab = getPointedTab()

        If ReferenceEquals(draggedTab, predraggedTab) AndAlso pointedTab IsNot Nothing Then
            drgevent.Effect = DragDropEffects.Move

            If Not ReferenceEquals(pointedTab, draggedTab) Then
                Me.ReplaceTabPages(draggedTab, pointedTab)
            End If
        End If

        MyBase.OnDragOver(drgevent)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Try
            predraggedTab = getPointedTab()
            Dim p = e.Location

            If Not Me.ShowClosingButton Then
            Else

                For i = 0 To Me.TabCount - 1
                    Dim r = Me.GetTabRect(i)
                    r.Offset(r.Width - 15, 2)
                    r.Width = 10
                    r.Height = 10

                    If Not r.Contains(p) Then
                        Continue For
                    End If

                    If Me.ShowClosingMessage Then

                        If DialogResult.Yes = MessageBox.Show(Me.closingMessage, "Close", MessageBoxButtons.YesNo) Then
                            Me.TabPages.RemoveAt(i)
                        End If
                    Else
                        ' Me.TabPages.RemoveAt(i) ' add custom remove tab
                        Form1.CloseTab()
                    End If
                Next
            End If

            MyBase.OnMouseDown(e)
        Catch ex As Exception

        End Try

    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left AndAlso predraggedTab IsNot Nothing Then
            Me.DoDragDrop(predraggedTab, DragDropEffects.Move)
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        predraggedTab = Nothing
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim g = e.Graphics
        Dim Drawer = g
        Drawer.SmoothingMode = SmoothingMode.HighQuality
        Drawer.PixelOffsetMode = PixelOffsetMode.HighQuality
        Drawer.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        Drawer.Clear(Me.headerColor)

        Try
            SelectedTab.BackColor = Me.backTabColor
        Catch
        End Try

        Try
            SelectedTab.BorderStyle = BorderStyle.None
        Catch
        End Try

        For i = 0 To TabCount - 1
            Dim Header = New Rectangle(New Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y), New Size(GetTabRect(i).Width, GetTabRect(i).Height))
            Dim HeaderSize = New Rectangle(Header.Location, New Size(Header.Width, Header.Height))
            Dim ClosingColorBrush As Brush = New SolidBrush(Me.closingButtonColor)

            If i = SelectedIndex Then
                Drawer.FillRectangle(New SolidBrush(Me.headerColor), HeaderSize)
                Drawer.FillRectangle(New SolidBrush(Me.activeColor), New Rectangle(Header.X - 5, Header.Y - 3, Header.Width, Header.Height + 5))
                Drawer.DrawString(TabPages(i).Text, Font, New SolidBrush(Me.selectedTextColor), HeaderSize, Me.CenterSringFormat)

                If Me.ShowClosingButton Then
                    e.Graphics.DrawString("X", Font, ClosingColorBrush, HeaderSize.Right - 17, 3)
                End If
            Else
                Drawer.DrawString(TabPages(i).Text, Font, New SolidBrush(Me.textColor), HeaderSize, Me.CenterSringFormat)
            End If
        Next

        Drawer.DrawLine(New Pen(Me.horizLineColor, 5), New Point(0, 19), New Point(Width, 19))
        Drawer.FillRectangle(New SolidBrush(Me.backTabColor), New Rectangle(0, 20, Width, Height - 20))
        Drawer.DrawRectangle(New Pen(Me.borderColor, 2), New Rectangle(0, 0, Width, Height))
        Drawer.InterpolationMode = InterpolationMode.HighQualityBicubic
    End Sub

    Private Function getPointedTab() As TabPage
        For i = 0 To Me.TabPages.Count - 1

            If Me.GetTabRect(i).Contains(Me.PointToClient(Cursor.Position)) Then
                Return Me.TabPages(i)
            End If
        Next

        Return Nothing
    End Function

    Private Sub ReplaceTabPages(ByVal Source As TabPage, ByVal Destination As TabPage)
        Try
            Dim SourceIndex = Me.TabPages.IndexOf(Source)
            Dim DestinationIndex = Me.TabPages.IndexOf(Destination)
            Me.TabPages(DestinationIndex) = Source
            Me.TabPages(SourceIndex) = Destination

            If Me.SelectedIndex = SourceIndex Then
                Me.SelectedIndex = DestinationIndex
            ElseIf Me.SelectedIndex = DestinationIndex Then
                Me.SelectedIndex = SourceIndex
            End If
            Me.Refresh()
        Catch ex As Exception

        End Try
    End Sub
End Class
