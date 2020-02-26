Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace MonacoNET.Settings
    Public Structure MonacoSettings
        Public [ReadOnly] As Boolean
        Public AutoIndent As Boolean
        Public DragAndDrop As Boolean
        Public Folding As Boolean
        Public FontLigatures As Boolean
        Public FormatOnPaste As Boolean
        Public FormatOnType As Boolean
        Public Links As Boolean
        Public MinimapEnabled As Boolean
        Public MatchBrackets As Boolean
        Public LetterSpacing As Integer
        Public LineHeight As Integer
        Public FontSize As Integer
        Public FontFamily As String
        Public RenderWhitespace As String
    End Structure
End Namespace
