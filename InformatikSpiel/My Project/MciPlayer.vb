Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Class MciPlayer

	<DllImport("winmm.dll")>
	Private Shared Function mciSendString(strCommand As [String], strReturn As StringBuilder, iReturnLength As Integer, hwndCallback As IntPtr) As Integer
	End Function

	Public Snds As New Dictionary(Of String, String)
	Public sndcnt As Integer = 0

	Public Sub New()
		End Sub

	Public Sub New(filename As String, [alias] As String)
		_medialocation = filename
		_alias = [alias]
		LoadMediaFile(_medialocation, _alias)
	End Sub


	Public Function AddSound(ByVal SoundName As String, ByVal SndFilePath As String) As Boolean
		LoadMediaFile(SndFilePath, SoundName)
		Return True
	End Function

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
			Debug.WriteLine("LoadMediaFile")
		_medialocation = filename
		Debug.WriteLine(_medialocation)
		_alias = [alias]
		Debug.WriteLine(_alias)
		StopPlaying()
			CloseMediaFile()
			Dim Pcommand As String = "open """ & filename & """ alias " & [alias]
			Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
			_isloaded = If((ret = 0), True, False)
		Debug.WriteLine(_isloaded)
		Return _isloaded
		End Function

		Public Sub PlayFromStart()
			If _isloaded Then
				Dim Pcommand As String = "play " & [Alias] & " from 0"
				Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, IntPtr.Zero)
			End If
		End Sub

		Public Sub PlayFromStart(callback As IntPtr)
			Debug.WriteLine("PlayFromStart")
			If _isloaded Then
				Dim Pcommand As String = "play " & [Alias] & " from 0 notify"
				Dim ret As Integer = mciSendString(Pcommand, Nothing, 0, callback)
			End If
		End Sub


		Public Sub PlayLoop()
			Debug.WriteLine("PlayLoop")
			If _isloaded Then
				Debug.WriteLine("PlayLoop2")

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