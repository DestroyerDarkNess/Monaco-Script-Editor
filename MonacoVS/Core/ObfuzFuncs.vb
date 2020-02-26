Imports System.Text
Imports BatchObfuscator.MiscFuncs

Public Class ObfuzFuncs

    Public Class BatOfuser

        Public Enum BatSettingObz
            RV = 0
            RC = 1
            UR = 2
        End Enum

        Private Shared obfuscatedCode As New StringBuilder
        Private Shared alphaChars As String = "abcdefghijklmnopqrstuvwxyz"
        Private Shared randomGenerator As New Random

        Public Shared Function BatchObfuscator(ByVal Source As String, ByVal OptionObz As BatSettingObz) As String
            If OptionObz = BatSettingObz.RV Then
                Return BatMethod1(Source)
            ElseIf OptionObz = BatSettingObz.RC Then
                Return BatMethod2(Source)
            ElseIf OptionObz = BatSettingObz.UR Then
                BatMethod3(Source)
            End If
        End Function

        Private Shared Function BatMethod1(ByVal Source As String) As String
            Dim ProcessSource As String = LCase(Source)

            obfuscatedCode.Clear() ' Make sure there is no excess code left over from last time.

            '
            ' Jump % Declare
            '
            Dim PorsentageNumbers As Integer = 0
            Dim PorsentageJump As Boolean = False
            Dim FinalizePorsentage As Boolean = False
            Dim SimbolN As Integer = 0
            Dim WritteP As Integer = 0
            Dim PStr As String = String.Empty
            Dim FinalizationP As Boolean = False


            '
            ' GENERATE THE VARIABLE NAMES FOR EACH LOWERCASE ALPHA CHARACTER
            '
            obfuscatedCode.Append("@echo off" & vbNewLine) ' Prepend @echo off so that the variable definitions don't show on screen.
            Dim alphaCharDefinitions(26) As String ' Create a string array that will hold the variable name for each letter of the alphabet a-z.
            Dim loopCounter As Integer
            For loopCounter = 0 To 25 ' Loop 26 times for each character a-z.
                alphaCharDefinitions(loopCounter) = GenerateRandomString(6) ' Assign each character a randomly generated variable name.
                obfuscatedCode.Append("set " & alphaCharDefinitions(loopCounter) & "=" & Mid(alphaChars, loopCounter + 1, 1) & vbNewLine) ' Add the "set asdfjkl=a" into the StringBuilder
            Next


            '
            ' OBFUSCATE THE SOURCE CODE
            '

            For loopCounter = 1 To Len(ProcessSource) ' For each character in TextBox1
                Dim Letter As String = Mid(ProcessSource, loopCounter, 1)


                If Letter = "%" Then
                    If PorsentageJump = True Then
                        PStr = PStr & Letter
                        WritteP += 1
                        If WritteP = SimbolN Then
                            PStr = GeneratePorsentage(SimbolN) & PStr
                            FinalizePorsentage = True
                        End If
                    Else
                        PorsentageNumbers += 1
                        If PorsentageNumbers > 0 Then
                            SimbolN = PorsentageNumbers
                        End If
                    End If
                    GoTo JumpCase
                Else
                    If FinalizePorsentage = True Then
                        obfuscatedCode.Append(PStr)
                        PorsentageNumbers = 0
                        PorsentageJump = False
                        WritteP = 0
                        FinalizationP = False
                        FinalizePorsentage = False
                        SimbolN = 0
                        PStr = String.Empty
                        FinalizePorsentage = False
                    Else
                        If PorsentageNumbers > 0 Then
                            PorsentageJump = True
                            PStr = PStr & Letter
                            GoTo JumpCase
                        Else
                            GoTo ObzLetter
                        End If
                    End If
                End If



ObzLetter:


                Select Case Letter
                    Case "a"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(0) & "%") ' Replace each lowercase alpha character with its variable equivalent.
                    Case "b"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(1) & "%")
                    Case "c"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(2) & "%")
                    Case "d"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(3) & "%")
                    Case "e"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(4) & "%")
                    Case "f"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(5) & "%")
                    Case "g"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(6) & "%")
                    Case "h"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(7) & "%")
                    Case "i"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(8) & "%")
                    Case "j"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(9) & "%")
                    Case "k"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(10) & "%")
                    Case "l"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(11) & "%")
                    Case "m"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(12) & "%")
                    Case "n"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(13) & "%")
                    Case "o"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(14) & "%")
                    Case "p"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(15) & "%")
                    Case "q"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(16) & "%")
                    Case "r"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(17) & "%")
                    Case "s"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(18) & "%")
                    Case "t"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(19) & "%")
                    Case "u"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(20) & "%")
                    Case "v"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(21) & "%")
                    Case "w"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(22) & "%")
                    Case "x"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(23) & "%")
                    Case "y"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(24) & "%")
                    Case "z"
                        obfuscatedCode.Append("%" & alphaCharDefinitions(25) & "%")
                    Case Else
                        obfuscatedCode.Append(Mid(ProcessSource, loopCounter, 1)) ' If it does not match one of the alpha characters, just leave it alone and place it in.S
                End Select

JumpCase:

            Next

            Return obfuscatedCode.ToString

        End Function

        Private Shared Function BatMethod2(ByVal Source As String) As String
            Dim ProcessSource As String = LCase(Source)
            '
            ' Jump % Declare
            '
            Dim PorsentageNumbers As Integer = 0
            Dim PorsentageJump As Boolean = False
            Dim FinalizePorsentage As Boolean = False
            Dim SimbolN As Integer = 0
            Dim WritteP As Integer = 0
            Dim PStr As String = String.Empty
            Dim FinalizationP As Boolean = False

            obfuscatedCode.Clear() ' Make sure there is no excess code left over from last time.
            Dim randomVariableName As String = ""

            '
            ' OBFUSCATE THE SOURCE CODE
            '
            For loopCounter = 1 To Len(ProcessSource) ' For each line in the source code, stick an uninitialized variable in between that expands to ""
                randomVariableName = GenerateRandomString(6)
                Dim Letter As String = Mid(ProcessSource, loopCounter, 1)

                If Letter = "%" Then
                    If PorsentageJump = True Then
                        PStr = PStr & Letter
                        WritteP += 1
                        If WritteP = SimbolN Then
                            PStr = GeneratePorsentage(SimbolN) & PStr
                            FinalizePorsentage = True
                        End If
                    Else
                        PorsentageNumbers += 1
                        If PorsentageNumbers > 0 Then
                            SimbolN = PorsentageNumbers
                        End If
                    End If
                    GoTo JumpCase
                Else
                    If FinalizePorsentage = True Then
                        obfuscatedCode.Append(PStr)
                        PorsentageNumbers = 0
                        PorsentageJump = False
                        WritteP = 0
                        FinalizationP = False
                        FinalizePorsentage = False
                        SimbolN = 0
                        PStr = String.Empty
                        FinalizePorsentage = False
                    Else
                        If PorsentageNumbers > 0 Then
                            PorsentageJump = True
                            PStr = PStr & Letter
                            GoTo JumpCase
                        Else
                            GoTo ObzLetter
                        End If
                    End If
                End If



ObzLetter:
                obfuscatedCode.Append("%" & randomVariableName & "%" & Letter)
JumpCase:
            Next

            Return obfuscatedCode.ToString

        End Function

        Private Shared Sub BatMethod3(ByVal Source As String)
            obfuscatedCode.Clear() ' Make sure there is no excess code left over from last time.

            Dim unicodeHeader() As Byte = New Byte() {&HFF, &HFE, &HD, &HA} ' The bytes that make text editors think it's in Unicode
            Dim obfuscatedCodeByteArray() As Byte = New Byte() {} ' Declare new Byte array for our text to be obfuscated.
            obfuscatedCodeByteArray = System.Text.Encoding.ASCII.GetBytes("cls" + vbNewLine + Source) ' Put TextBox1.Text into byte array
            Dim concatenatedByteArray() As Byte = New Byte(unicodeHeader.Length + obfuscatedCodeByteArray.Length) {} ' Make new array to combine header and code

            unicodeHeader.CopyTo(concatenatedByteArray, 0) ' Copy header to full byte array
            obfuscatedCodeByteArray.CopyTo(concatenatedByteArray, unicodeHeader.Length) ' Copy obfuscated code after the header

            ' Need a custom save function because have to write raw bytes to disk.
            Dim SvFile As String = save("Obz3.bat")
            If Not SvFile = "Error" Then
                IO.File.WriteAllBytes(SvFile, concatenatedByteArray) ' Write raw bytes, not just text, to the disk.
            End If
        End Sub

        Private Shared Function GenerateRandomString(length As Integer) As String
            Randomize()
            Dim i As Integer ' The Counter for the loop to generate random characters.
            Dim temp As String = "" ' A string to hold our randomly generated characters as they're created.
            For x As Integer = 0 To length
                i = CInt(Math.Floor((alphaChars.Length - 1 + 1) * Rnd())) + 1
                temp &= Mid(alphaChars, i, 1) ' Get a character from the alphabet at the position specified by the random number.
            Next
            Return temp
        End Function

        Private Shared Function GeneratePorsentage(ByVal len As Integer) As String
            Dim s As String = "%"
            Dim sb As New StringBuilder
            For i As Integer = 1 To len
                sb.Append(s)
            Next
            Return sb.ToString()
        End Function

    End Class



End Class
