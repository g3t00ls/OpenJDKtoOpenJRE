Imports System.Windows.Forms
'All rights reserved
Module Module1
    'todo: remove System.Windows.Forms dependencies, therefore update rtb loop through lines (faster/easier)
    'todo: advanced string format checking / custom error reports? 
    'Missing easy disclaimer for wrong OpenJDK dir
    Sub Main(ByVal args() As String)
        Try
            If Not args(0) = Nothing Then
                For Each currArg As String In args
                    If currArg.ToLower = "/help".ToLower Then
                        Console.WriteLine(ReturnHelp)
                        Exit For
                    End If
                    If currArg.ToLower = "/convert".ToLower Then
                        Console.WriteLine(ReturnConvert(args))
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Console.WriteLine(ReturnInfo)
        End Try
        'Console.ReadKey()
        Application.Exit()
        Exit Sub
    End Sub

    Private Function ReturnInfo() As String
        Dim s0 As String = "OpenJDKtoOpenJRE converter by getools" & vbNewLine & vbNewLine & "HelpArg = /help"
        Return s0
    End Function

    Private Function ReturnHelp() As String
        Dim s0 As String = "No " & Chr(34) & "\" & Chr(34) & " @ end of dir!" & vbNewLine & "Will create a subfolder " & Chr(34) & "jre-<VersionNumber>" & Chr(34) & " @ " & Chr(34) & "<OpenJREdir_output>" & Chr(34) & "!" & vbNewLine & vbNewLine & "/convert " & Chr(34) & "<OpenJDKdir>" & Chr(34) & " " & Chr(34) & "<OpenJREdir_output>" & Chr(34)
        Return s0
    End Function

    Private Function ReturnConvert(ByVal args() As String) As String
        Try
            Dim OpenJDKdir As String
            Dim OpenJREdir As String
            Dim i As Integer = 0
            For Each currarg As String In args
                If Not currarg.ToLower = "/convert".ToLower Then
                    If i = 0 Then OpenJDKdir = currarg
                    If i = 1 Then OpenJREdir = currarg
                    i = i + 1
                End If
            Next

            'Dim OpenJDKmodules As String = ExecuteAndReturn(OpenJDKdir & "\bin\java.exe", "--list-modules")
            Dim rtb0 As New RichTextBox
            Dim OpenJREmodules As String = Nothing
            Dim currOpenJDKmoduleRemoveVersion() As String
            rtb0.Text = ExecuteAndReturn(OpenJDKdir & "\bin\java.exe", "--list-modules", True) 'OpenJDKmodules
            For Each currOpenJDKmodule As String In rtb0.Lines
                If Not currOpenJDKmodule = Nothing AndAlso Not currOpenJDKmodule.StartsWith("jdk") = True Then 'could exclude jdk. modules / or only include java. modules
                    currOpenJDKmoduleRemoveVersion = currOpenJDKmodule.Split("@")
                    OpenJREmodules = OpenJREmodules & "," & currOpenJDKmoduleRemoveVersion(0)
                End If
            Next
            rtb0.Text = Nothing
            OpenJREmodules = OpenJREmodules.Remove(0, 1)
            Dim OpenJDKversion As String = "UnknownVersion"
            If Not currOpenJDKmoduleRemoveVersion(1) = Nothing Then
                OpenJDKversion = currOpenJDKmoduleRemoveVersion(1)
            End If
            currOpenJDKmoduleRemoveVersion.Clear(currOpenJDKmoduleRemoveVersion, 0, currOpenJDKmoduleRemoveVersion.Length)

            'Dim jlinkArg As String = "--no-header-files --no-man-pages --compress=2 --add-modules " & OpenJREmodules & " --output " & Chr(34) & OpenJREdir & "\jre-" & OpenJDKversion & Chr(34)
            'https://docs.oracle.com/en/java/javase/11/tools/jlink.html
            '--compress={0|1|2}
            '0=No compression;1=Constant string sharing;2=ZIP

            Dim OpenJREoutputDir As String = OpenJREdir & "\jre-" & OpenJDKversion
            OpenJREdir = Nothing
            OpenJDKversion = Nothing
            'jlink Error Reporting
            'Return ExecuteAndReturn(OpenJDKdir & "\bin\jlink.exe", "--no-header-files --no-man-pages --compress=2 --add-modules " & OpenJREmodules & " --output " & Chr(34) & OpenJREdir & "\jre-" & OpenJDKversion & Chr(34), True)
            ExecuteAndReturn(OpenJDKdir & "\bin\jlink.exe", "--no-header-files --no-man-pages --compress=2 --add-modules " & OpenJREmodules & " --output " & Chr(34) & OpenJREoutputDir & Chr(34), False)
            Return OpenJREoutputDir
        Catch ex As Exception
            Return ex.ToString
        End Try
    End Function

    Private Function ExecuteAndReturn(ByVal DirToBin As String, ByVal args As String, Optional ByVal returnOutput As Boolean = False)
        Try
            Dim p0 As Process = New Process
            Dim BinOutput As String
            With p0
                .StartInfo.CreateNoWindow = True
                .StartInfo.RedirectStandardOutput = True
                .StartInfo.UseShellExecute = False
                .StartInfo.FileName = DirToBin
                .StartInfo.Arguments = args
                .Start()
                If returnOutput = True Then BinOutput = .StandardOutput.ReadToEnd
                .WaitForExit()
            End With
            Return BinOutput
        Catch ex As Exception
            Return ex.ToString
        End Try
    End Function
End Module
