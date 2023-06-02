Imports System.Runtime.InteropServices

'Quelle [https://social.msdn.microsoft.com/Forums/vstudio/en-US/39b40b77-4d09-4adb-a728-18eb4397a155/play-multiple-audio-files-at-once-visual-basic?forum=vbgeneral]
Public Class MultiSounds
    Public Snds As New Dictionary(Of String, String)
    Public sndcnt As Integer = 0

    <DllImport("winmm.dll", EntryPoint:="mciSendStringW")>
    Public Shared Function mciSendStringW(<MarshalAs(UnmanagedType.LPTStr)> ByVal lpszCommand As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszReturnString As System.Text.StringBuilder, ByVal cchReturn As UInteger, ByVal hwndCallback As IntPtr) As Integer
    End Function

    Public Function AddSound(ByVal SoundName As String, ByVal SndFilePath As String) As Boolean
        If SoundName.Trim = "" Or Not IO.File.Exists(SndFilePath) Then Return False
        If mciSendStringW("open " & Chr(34) & SndFilePath & Chr(34) & " alias " & "Snd_" & sndcnt.ToString, Nothing, 0, IntPtr.Zero) <> 0 Then Return False
        Snds.Add(SoundName, "Snd_" & sndcnt.ToString)
        sndcnt += 1
        Return True
    End Function

    Public Function Play(ByVal SoundName As String) As Boolean
        If Not Snds.ContainsKey(SoundName) Then Return False
        mciSendStringW("seek " & Snds.Item(SoundName) & " to start", Nothing, 0, IntPtr.Zero)
        If mciSendStringW("play " & Snds.Item(SoundName), Nothing, 0, IntPtr.Zero) <> 0 Then Return False
        Return True
    End Function
End Class