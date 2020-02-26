Public Class Structures

    Public Enum Lexer
        none
        Lua
        vbs
        Batch
        C
        Cnet
        VB
        JS
        Html
        Php
        Css
        Cplus
        Xml
        Python
    End Enum

    Public Shared Function DetectLexer(ByVal Extension As String) As Lexer
        Select Case LCase(Extension)
            Case ".lua" : Return Lexer.Lua
            Case ".vbs" : Return Lexer.vbs
            Case ".wfs" : Return Lexer.vbs
            Case ".bat" : Return Lexer.Batch
            Case ".cmd" : Return Lexer.Batch
            Case ".cpp" : Return Lexer.Cplus
            Case ".h" : Return Lexer.Cplus
            Case ".html" : Return Lexer.Html
            Case ".js" : Return Lexer.JS
            Case ".php" : Return Lexer.Php
            Case ".py" : Return Lexer.Python
            Case ".xml" : Return Lexer.Xml
            Case ".vb" : Return Lexer.VB
            Case ".cs" : Return Lexer.Cnet
        End Select
        Return Lexer.none
    End Function

End Class
