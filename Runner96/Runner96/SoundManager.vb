Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

' Quelle [https://www.codeproject.com/Articles/781792/Fun-with-Sound]

Namespace MCIDEMO
	Class MciPlayer

		<DllImport("winmm.dll")>
		Private Shared Function mciSendString(strCommand As [String],
									   strReturn As StringBuilder,
											  iReturnLength As Integer,
											  hwndCallback As IntPtr) As Integer
		End Function

		<DllImport("winmm.dll")>
		Public Shared Function mciGetErrorString(errCode As Integer,
												 errMsg As StringBuilder,
												 buflen As Integer) As Integer
		End Function

		<DllImport("winmm.dll")>
		Public Shared Function mciGetDeviceID(lpszDevice As String) As Integer
		End Function


		Public Sub New()
		End Sub

		Public Sub New(filename As String, [alias] As String)
			_medialocation = filename
			_alias = [alias]
			LoadMediaFile(_medialocation, _alias)
		End Sub

		Private _deviceid As Integer = 0

		Public ReadOnly Property Deviceid() As Integer
			Get
				Return _deviceid
			End Get
		End Property

		Private _isloaded As Boolean = False

		Public Property Isloaded() As Boolean
			Get
				Return _isloaded
			End Get
			Set
				_isloaded = Value
			End Set
		End Property

		Private _medialocation As String = ""

		Public Property MediaLocation() As String
			Get
				Return _medialocation
			End Get
			Set
				_medialocation = Value
			End Set
		End Property
		Private _alias As String = ""

		Public Property [Alias]() As String
			Get
				Return _alias
			End Get
			Set
				_alias = Value
			End Set
		End Property


		Public Function LoadMediaFile(filename As String, [alias] As String) As Boolean
			_medialocation = filename
			_alias = [alias]
			StopPlaying()
			CloseMediaFile()
			Dim Pcommand As String = "open """ & filename & """ alias " & [alias]
			Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
			_isloaded = If((ret = 0), True, False)
			If _isloaded Then
				_deviceid = mciGetDeviceID(_alias)
			End If
			Return _isloaded
		End Function

		Public Sub PlayFromStart()
			If _isloaded Then
				Dim Pcommand As String = "play " & [Alias] & " from 0"
				Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
			End If
		End Sub

		Public Sub PlayFromStart(callback As IntPtr)
			If _isloaded Then
				Dim Pcommand As String = "play " & [Alias] & " from 0 notify"
				Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, callback)
			End If
		End Sub


		Public Sub PlayLoop()
			If _isloaded Then
				Dim Pcommand As String = "play " & [Alias] & " repeat"
				Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
			End If
		End Sub

		Public Sub CloseMediaFile()
			Dim Pcommand As String = "close " & [Alias]
			Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
			_isloaded = False

		End Sub

		Public Sub StopPlaying()
			Dim Pcommand As String = "stop " & [Alias]
			Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
		End Sub

	End Class
End Namespace

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