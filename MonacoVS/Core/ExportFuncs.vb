Public Class ExportFuncs

    Public Shared Function ToHTML(ByVal Source As String) As String
        Dim htmlC As String =
<a><![CDATA[<pre><font style="font-family: Courier New, monospace; font-size: 9,75pt; line-height: 14px;">
%texto%
</font></pre>]]></a>.Value
        Dim NewCode As String = htmlC
        NewCode = Replace(NewCode, "%texto%", Source)
        Return NewCode
    End Function

End Class
