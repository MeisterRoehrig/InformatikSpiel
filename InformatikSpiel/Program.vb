Imports System.IO
Imports FireSharp.Config
Imports FireSharp.Response
Imports FireSharp.Interfaces
Imports Runner96.MCIDEMO

Module Module1
    Private Const constConsoleWidth As Integer = 120
    Private Const constConsoleHeight As Integer = 22
    Private Const constPlayerXPosition As Integer = 1 'Abstand Spielfigur zum linken Rand
    Private Const constRandomLowerBound As Integer = 0 'Unter Grenze fuer die zufaellige Generierung von Hindernissen und Wolken
    Private Const constRandomUpperBound As Integer = 5 'Obere Grenze fuer die zufaellige Generierung von Hindernissen und Wolken

    Private gameTimer As Integer = 0 'Verstrichene Zeit seit begin der Runde
    Private gameTimerCache As Integer = 0
    Private gameFps As Double = 0 'Frames Pro Sekunde
    Private roundScore As Integer = 0 'Erreichter Highscore der Runde
    Private playerJumpHeight As Integer
    Private playerJumpHeightCache As Integer
    Private groundTiles As New List(Of Tile) 'Liste bestehend aus Bodenstuecken fuer den Untergrund
    Private skyTiles1 As New List(Of Tile) 'Liste bestehend aus Wolken fuer den Himmel
    Private skyTiles2 As New List(Of Tile)
    Private playerInRound As Boolean = False 'Solange True, bis Runde beendet werden soll (Bei Collision mit Hindernissen)
    Private gameOverBoolean As Boolean = False 'Wenn True, ist das Menue im GameOver look
    Private playerAnimationState As Integer 'Speichert den vorherigen Frame der Animation des Players
    Private playerJump As Boolean 'True, wenn Sprunganimation gestartet werden soll
    Private animateJump As Boolean = False 'Variable ist True sofern sich der Spieler aktiv in einem Sprung befindet

    Private Snds As New MultiSounds 'Externer sound Player fuer parallele Sounds
    Private initiateSounds As Boolean = True 'Damit Sounds nur einmal geladen werden
    Private playJumpSound As Boolean = False  'Wenn True wird der jeweilige Sound abgespielt
    Private playBackgroundSound As Boolean = False
    Private playDeathSound As Boolean = False
    Private playScoreSound As Boolean = False
    Private playBigScoreSound As Boolean = False
    Private menuloopSound As New MciPlayer("menuloop.mp3", "1") 'Hintergrundmusik des Menues
    Private gamemusicSound As New MciPlayer("music.mp3", "2") 'Hintergrundmusik werdend der Runde
    Private introSound As New MciPlayer("intro.mp3", "3") 'Soundeffekt des Runner96 Logos

    Private keyInputDelay As Boolean = True ''Fuegt ein kurzes Delay hinzu bevor die naechste Runde gestartet werden kann
    Private selectedMenuOption As Integer = 0 'Welcher Menuepunkt ausgewaehlt ist und im Fall von menuConfirm=True bestaetigt wird
    Private menuConfirm As Boolean = False 'Wird True bei eingabe von Enter oder Leertaste und bestaetigt die Auswahl
    Private musicEnabled As Boolean 'Hintergrund Musik wird abgespielt wenn True
    Private changeNameMenuOpen = False 'True wenn ChangeName Menue geoeffnet ist
    Private nameFieldCursorLocation As Integer = 0 'Position des Cursors des Change Name Fields
    Private playerNameCharacters As New List(Of String) 'Liste welche bei Eingabe die Zeichen beinhaltet aus denen der PlayerName besteht
    Private openFirstTime = LoadSettings(1) 'Ist True wenn die Application zum ersten mal geoeffnet wird

    Private fClient As IFirebaseClient
    Private fConfig As New FirebaseConfig() With 'Firebase Datenbank Zugang
        {
        .AuthSecret = "Ujd5c4MQRC7lrK1DbEPMfAkkSqgk8IqWGQyZYsFo",
        .BasePath = "https://informatikspiel-default-rtdb.europe-west1.firebasedatabase.app/"
        }

    Public Class Tile 'Bodenstueck bestehend aus tileType - Eigentlich ueberfluessig hatte das urspruenglich so gemacht falls man den Bodenstuecken noch weitere eigenschaften geben will
        Public Property tileType As Integer
    End Class

    Public Class GameScore 'GameScore ist das Object welches nach Abschluss der Runde auf die Datenbank geladen wird
        Public Property roundId As String = ""
        Public Property roundDate As String = ""
        Public Property playerName As String = ""
        Public Property playerScore As String = ""
    End Class

    'WirteAt() Ermoeglicht das ausgeben eines Strings an einer bestimmten Position des Konsolenfensters
    'x,y sind absolute Entfernungen zum Konsolen Ursprung es sei denn CenterHorizontally oder CenterVertically ist True, dann geben x,y den Versatz des Strings relativ zur Mitte der Console an
    Public Sub WriteAt(s As String, x As Integer, y As Integer, Optional CenterHorizontally As Boolean = False, Optional CenterVertically As Boolean = False)
        If CenterHorizontally Then
            x = (constConsoleWidth / 2) - (Len(s) / 2) + x 'Len(s)/2 fuer Mitte des Strings
        End If
        If CenterVertically Then
            y = (constConsoleHeight / 2) + y
        End If
        Console.SetCursorPosition(x, y)
        Console.Write(s)
    End Sub

    'isDivisible() returned True wenn x durch d restlos teilbar ist
    Public Function isDivisible(x As Integer, d As Integer) As Boolean
        Return (x Mod d) = 0
    End Function

    Public Sub ConsoleSetup(consoleWidth As Integer, consoleHeight As Integer)
        Console.Clear()
        Console.SetWindowSize(consoleWidth, consoleHeight)
        Console.SetBufferSize(consoleWidth, consoleHeight)
        Console.CursorVisible = False
        Console.Title = "Runner 96"
        Console.TreatControlCAsInput = True 'Deaktiviert standardmaessige Tastenkombinationen wie ctrl + c zum schliessen des Fensters 
        Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory.ToString()) 'File Path zum root folder der Applikation
        musicEnabled = LoadSettings(0) 'Laedt einstellungen ob hintergrund Musik ein oder ausgeschaltet sein soll aus der settings Datei
    End Sub

    'FirbaseLoad() versucht eine Verbindung mit der Firebase datenbank herzustellen und registriert den neuen Client
    Public Sub FirebaseLoad()
        Try
            fClient = New FireSharp.FirebaseClient(fConfig)
        Catch
            Debug.WriteLine("[FirebaseConfig] Could not connect to Firebase services")
        End Try
    End Sub

    'FirebaseSend() Laedt Highscore Daten auf die Firebase Datenbank
    Public Sub FirebaseSend(playerName As String, playerScore As Integer)
        Dim gameScore As New GameScore() With
            {
            .roundId = Convert.ToInt64((Date.UtcNow - New DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
            .roundDate = DateTime.Now.ToString("dd.MM.yy H:mm"),
            .playerName = playerName,
            .playerScore = playerScore.ToString()
            }
        Try
            Dim fSetter = fClient.SetAsync("GameScoreList/" + gameScore.roundId, gameScore)
        Catch
            Debug.WriteLine("[FirebaseSend] High Score could not be uploaded to Firebase.com")
        End Try

    End Sub

    'FirebaseReceive() Returned alle gespeicherter Highscores von der Firebase Datenbank absteigend nach Hoehe der Highscores sortiert
    Public Function FirebaseReceive() As Dictionary(Of String, GameScore)
        Dim fReceiver As FirebaseResponse
        Dim gameScoreList As Dictionary(Of String, GameScore)
        Dim sortedGameScoreDictionary As New Dictionary(Of String, GameScore)
        'Laedt eine Liste der gespeicherten Highscores von der Datenbank
        Try
            fReceiver = fClient.Get("GameScoreList/")
            gameScoreList = fReceiver.ResultAs(Of Dictionary(Of String, GameScore))
            Debug.WriteLine("[FirebaseReceive] GameScoreList was successfully fetched from Firebase.com")
        Catch ex As Exception
            Debug.WriteLine("[FirebaseReceive] GameScoreList could not be loaded")
            sortedGameScoreDictionary.Clear()
            sortedGameScoreDictionary.Add(key:="0", value:=New GameScore With {.roundId = "", .roundDate = "UNABLE TO CONNECT TO DATABASE", .playerName = "", .playerScore = ""})
            Return sortedGameScoreDictionary
        End Try
        'Sortiert diese Liste absteigend nach Groesse der Highsores
        Try
            Dim sortedGameScoreList = From pair In gameScoreList Order By Convert.ToInt64(pair.Value.playerScore) Descending
            sortedGameScoreDictionary = sortedGameScoreList.ToDictionary(Function(p) p.Key, Function(p) p.Value)
            For Each item As KeyValuePair(Of String, GameScore) In sortedGameScoreDictionary
                Debug.WriteLine("[FirebaseReceive] Key = {0}, HighScore = {1}",
                        item.Key, item.Value.playerScore)
            Next item
        Catch ex As Exception
            Debug.WriteLine("[FirebaseReceive] GameScoreList is empty, populate with placeholder data instead")
            sortedGameScoreDictionary.Clear()
            sortedGameScoreDictionary.Add(key:="0", value:=New GameScore With {.roundId = "", .roundDate = "NO DATA", .playerName = "", .playerScore = ""})
        End Try
        Return sortedGameScoreDictionary
    End Function

    'RoundStart() Setzt Variablen zurueck und startet die neue Runde
    Public Sub RoundStart()
        Console.Clear()
        playerInRound = True
        playerAnimationState = 0
        playerJump = False
        animateJump = False
        playerJumpHeight = 0
        playerJumpHeightCache = 0
        gameTimer = 0
        gameTimerCache = 0
        gameOverBoolean = False
        gameFps = 0
        roundScore = 0
        If musicEnabled Then
            menuloopSound.StopPlaying() 'Menuemusik wird abgeschaltet
            gamemusicSound.PlayLoop() 'InGame Music wird eingeschaltet
        End If
        ArraySetUp()
        AsyncLoopRound()
    End Sub

    'SaveSettings() Speichert Settings in settings.txt diese auch nach neustart des Spiels noch nutzen zu koennen
    Public Sub SaveSettings(ByRef lineOfFile As Integer, Optional musicEnabled As Boolean = False, Optional openFirstTime As Boolean = False, Optional playerName As String = "")
        If lineOfFile = 0 Then '0 ist Speicherort fuer musicEnabled
            Try
                Dim data As String() = {musicEnabled.ToString, LoadSettings(1), LoadSettings(2)}
                File.WriteAllLines("settings.txt", data)
            Catch
                Debug.WriteLine("[LoadSettings] musicEnabled could not be written to settings.txt")
            End Try
        End If
        If lineOfFile = 1 Then '1 ist Speicherort fuer openFirstTime
            Try
                Dim data As String() = {LoadSettings(0), openFirstTime.ToString(), LoadSettings(2)}
                File.WriteAllLines("settings.txt", data)
            Catch
                Debug.WriteLine("[LoadSettings] openFirstTime could not be written to settings.txt")
            End Try
        End If
        If lineOfFile = 2 Then '2 ist Speicherort fuer playerName
            Try
                Dim data As String() = {LoadSettings(0), LoadSettings(1), playerName}
                File.WriteAllLines("settings.txt", data)
            Catch
                Debug.WriteLine("[LoadSettings] playerName could not be written to settings.txt")
            End Try
        End If
    End Sub

    'LoadSettings() Laedt gespeichert Settings aus dem settings.txt File
    Public Function LoadSettings(ByRef lineOfFile As Integer)
        If lineOfFile = 0 Then '0 ist Speicherort fuer musicEnabled
            Try
                Return Convert.ToBoolean(File.ReadAllLines("settings.txt")(0))
            Catch
                Debug.WriteLine("[LoadSettings] settings.txt could not be read")
                Debug.WriteLine("[LoadSettings] setting musicEnabled = False")
                Return False
            End Try
        End If
        If lineOfFile = 1 Then '1 ist Speicherort fuer openFirstTime
            Try
                Return Convert.ToBoolean(File.ReadAllLines("settings.txt")(1))
            Catch
                Debug.WriteLine("[LoadSettings] settings.txt could not be read")
                Debug.WriteLine("[LoadSettings] setting openFirstTime = False")
                Return False
            End Try
        End If
        If lineOfFile = 2 Then '2 ist Speicherort fuer playerName
            Try
                Return File.ReadAllLines("settings.txt")(2)
            Catch
                Debug.WriteLine("[LoadSettings] settings.txt could not be read")
                Debug.WriteLine("[LoadSettings] setting basename instead")
                Return "PLAYER-1"
            End Try
        End If
    End Function

    'DrawStats() Schreibt die Stats in der oberen linken Ecke auf die Console
    Public Sub DrawStats()
        WriteAt("Timer:" & gameTimer & "   ", 1, 1) 'Gametimer: Ausgabe der Spielzeit auf dem Bildschirm
        If gameTimer > gameTimerCache Then
            WriteAt("FPS:" & gameFps & "   ", 1, 2) 'nach einer Sekunde Ausgabe der FPS-Variable auf dem Bildschirm
            gameFps = 0
        End If
        gameFps += 1 'erhoehe die FPS Variable jeden gerenderten Frame um 1
        gameTimerCache = gameTimer
        WriteAt("Score:" & roundScore & "   ", 1, 0)
    End Sub

    'DrawPlayer() Speichert die unterschiedlichen Animationsstadien der Spielfigur und schreibt diese bei abfrage auf das Konsolenfenster
    Public Sub DrawPlayer(playerState As Integer, playerAnchorX As Integer, playerAnchorY As Integer)
        Select Case playerState 'playerState: welcher Frame der Animation soll ausgegeben werden
            Case 0
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 1
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt("L", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt(",", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt("`", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt("/", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt("-", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(",", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt("/", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt("/", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 2
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt("/", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt("|", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt("\", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt("/", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt("`", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(":", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt("/", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt("`", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 3
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt("_", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt("_", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt("`", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt("\", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt("_", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(",", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt("_", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt("-", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt("-", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt(",", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt("\", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(".", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 4
                WriteAt(",", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(".", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt("-", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt("-", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt("_", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(".", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt("-", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(",", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt("_", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt("-", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt(".", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt("\", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt("\", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(".", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 5
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(".", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt("-", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt("-", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt(",", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt("-", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt("`", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt("|", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt("\", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt("_", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt(".", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt("^", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(".", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt("|", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 6
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(".", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt("_", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt("_", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt("|", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt("`", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt("`", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt("^", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt("/", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt("-", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(",", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt("`", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
            Case 7
                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 1)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 1)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 3, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 2)
                WriteAt("@", playerAnchorX + 5, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 2)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 2)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 3, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 4, playerAnchorY + 3)
                WriteAt("\", playerAnchorX + 5, playerAnchorY + 3)
                WriteAt("/", playerAnchorX + 6, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 3)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 3)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 4)
                WriteAt(",", playerAnchorX + 2, playerAnchorY + 4)
                WriteAt("_", playerAnchorX + 3, playerAnchorY + 4)
                WriteAt("|", playerAnchorX + 4, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 4)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 4)

                WriteAt(" ", playerAnchorX + 1, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 2, playerAnchorY + 5)
                WriteAt("/", playerAnchorX + 3, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 4, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 5, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 6, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 7, playerAnchorY + 5)
                WriteAt(" ", playerAnchorX + 8, playerAnchorY + 5)
        End Select
    End Sub

    'PlayerManager() Ausgabe der animierten Spielfigur an der entsprechenden Position am Bildschirm
    Public Sub PlayerManager()
        If playerJumpHeight <> playerJumpHeightCache Then DrawPlayer(0, constPlayerXPosition, (constConsoleHeight - 7) - playerJumpHeightCache) 'BugFix: Loescht doppelte Zeichen die entstehen wenn die Spielfigur sich schneller bewegt als das Konsolenfenster refreshed sobald sich die Figur an einer neuen Position befindet
        DrawPlayer(0, constPlayerXPosition, (constConsoleHeight - 7) - 6) 'BugFix: Loescht doppelte Zeichen die entstehen wenn die Spielfigur sich schneller bewegt als das Konsolenfenster refreshed - Funktioniert also besser so lassen
        DrawPlayer(playerAnimationState, constPlayerXPosition, (constConsoleHeight - 7) - playerJumpHeight)
        playerJumpHeightCache = playerJumpHeight
    End Sub

    'DrawArray() Speichert die Bausteine fuer Hindernisse, den Weg, und den Himmel und schreibt diese bei abfrage auf das Konsolenfenster
    Public Sub DrawArray(groundState As Integer, groundAnchorX As Integer, groundAnchorY As Integer)
        Select Case groundState
            Case 0
                WriteAt(" ", groundAnchorX, groundAnchorY - 3)
                WriteAt(" ", groundAnchorX, groundAnchorY - 2)
                WriteAt(" ", groundAnchorX, groundAnchorY - 1)
                WriteAt("`", groundAnchorX, groundAnchorY)
            Case 1
                WriteAt("[]", groundAnchorX, groundAnchorY - 2)
                WriteAt("[]", groundAnchorX, groundAnchorY - 1)
                WriteAt("`", groundAnchorX, groundAnchorY)
            Case 2
                WriteAt("[]", groundAnchorX, groundAnchorY - 2)
                WriteAt("||", groundAnchorX, groundAnchorY - 1)
                WriteAt("`", groundAnchorX, groundAnchorY)
            Case 3
                WriteAt(" []", groundAnchorX, groundAnchorY - 2)
                WriteAt("[][]", groundAnchorX, groundAnchorY - 1)
                WriteAt("`", groundAnchorX, groundAnchorY)
            Case 4
                WriteAt("[]", groundAnchorX, groundAnchorY - 2)
                WriteAt("[][]", groundAnchorX, groundAnchorY - 1)
                WriteAt("`", groundAnchorX, groundAnchorY)
            Case 5
                WriteAt(" ", groundAnchorX, groundAnchorY - 2)
                WriteAt(" ", groundAnchorX, groundAnchorY - 1)
                WriteAt(" ", groundAnchorX, groundAnchorY)
            Case 6
                WriteAt("   ___", groundAnchorX, groundAnchorY - 2)
                WriteAt(" _(   )", groundAnchorX, groundAnchorY - 1)
                WriteAt("(___)__)", groundAnchorX, groundAnchorY)
            Case 7
                WriteAt("   ___", groundAnchorX, groundAnchorY - 2)
                WriteAt(" _(   )", groundAnchorX, groundAnchorY - 1)
                WriteAt("(___(__)", groundAnchorX, groundAnchorY)
            Case 8
                WriteAt("  ___", groundAnchorX, groundAnchorY - 2)
                WriteAt(" (   )_", groundAnchorX, groundAnchorY - 1)
                WriteAt("(______)", groundAnchorX, groundAnchorY)
        End Select
    End Sub

    'ArraySetUp() Befuellt die Array Listen fuer Boden und Himmel, damit auch bei Spielstart schon was zu sehen ist
    Public Sub ArraySetUp()
        groundTiles.Clear()
        For i = 0 To constConsoleWidth - 1
            groundTiles.Add(New Tile With {.tileType = 0}) 'Bestueckung mit Bodenstuecken des Standarttyps (kein Hindernis)
        Next
        skyTiles1.Clear()
        'Verteilt auf der oberen Wolkenebene zufaellig Wolken
        Dim spawnRandom As Integer = 0
        For i = 0 To constConsoleWidth - 1
            If spawnRandom > CInt(Math.Floor((55 - 45 + 1) * Rnd())) + 45 Then
                Dim tyleTypeRnd As Integer = CInt(Math.Floor((8 - 5 + 1) * Rnd())) + 5
                skyTiles1.Add(New Tile With {.tileType = tyleTypeRnd})
                spawnRandom = 0
            Else
                skyTiles1.Add(New Tile With {.tileType = 5})

            End If
            spawnRandom += 1
        Next
        'Verteilt auf der unteren Wolkenebene zufaellig Wolken
        spawnRandom = 0
        skyTiles2.Clear()
        For i = 0 To constConsoleWidth - 1
            If spawnRandom > CInt(Math.Floor((40 - 30 + 1) * Rnd())) + 30 Then
                Dim tyleTypeRnd As Integer = CInt(Math.Floor((8 - 5 + 1) * Rnd())) + 5
                skyTiles2.Add(New Tile With {.tileType = tyleTypeRnd})
                spawnRandom = 0
            Else
                skyTiles2.Add(New Tile With {.tileType = 5})

            End If
            spawnRandom += 1
        Next
    End Sub

    'ArrayManager() Wird im Render Zyklus abgerufen und zeichnet alle Boden und Himmelstuecke 
    Public Sub ArrayManager()
        For index As Integer = groundTiles.Count - 1 To 0 Step -1
            DrawArray($"{groundTiles(index).tileType} ", index, constConsoleHeight - 1)
        Next
        For index As Integer = skyTiles1.Count - 1 To 0 Step -1
            DrawArray($"{skyTiles1(index).tileType} ", index, 5)
        Next
        For index As Integer = skyTiles2.Count - 1 To 0 Step -1
            DrawArray($"{skyTiles2(index).tileType} ", index, 9)
        Next
    End Sub

    'AsyncLoopRound() Startet Parallele Prozesse die waehrend der Runde laufen
    Public Function AsyncLoopRound() As Task
        Dim taskA = Task.Run(AddressOf RenderLoop)
        Dim taskB = Task.Run(AddressOf Timer)
        Dim taskC = Task.Run(AddressOf PlayerAnimator)
        Dim taskD = Task.Run(AddressOf GroundAnimator)
        Dim taskE = Task.Run(AddressOf CollisionManager)
        Dim taskF = Task.Run(AddressOf SkyAnimator)
        Task.WaitAll(taskA) 'Verhindert das die Base Task weiter laeuft
    End Function

    'RenderLoop() Von hier aus werden waehrend einer Runde alle Zeichnenden Funktionen gesteuert
    Public Sub RenderLoop()
        While playerInRound
            DrawStats()
            ArrayManager()
            PlayerManager()
            Threading.Thread.Sleep(1)
        End While
    End Sub

    'Timer() Ein Timer der ab Rundenstart die Sekunden zaehlt
    Public Sub Timer()
        While playerInRound
            gameTimer += 1
            Threading.Thread.Sleep(1000)
        End While
    End Sub

    'CollisionManager() Ueberprueft ob es eine Kollision zwischen Spielfigur und Hindernis gibt
    'Wenn an Position des Players ein Hindernis ist, wird geprueft ob Spielfigur am Boden ist. Wenn ja, Game Over; Wenn nein, Score +1.
    Public Sub CollisionManager()
        Dim overObstacle As Boolean = False
        While playerInRound
            If groundTiles(6).tileType <> 0 Then
                If playerJumpHeight < 0.2 Then
                    playDeathSound = True
                    gameOverBoolean = True
                    playerInRound = False
                End If
                overObstacle = True
            End If
            If groundTiles(6).tileType = 0 And overObstacle = True Then
                overObstacle = False 'overObstacle Wird verwendet damit es erst einen Punkt gibt wenn das Hindernis vollstaendig Ueberwunden ist
                roundScore += 1
                If isDivisible(roundScore, 10) Then 'Alle 10 Ueberwundenen Hindernisse wird ein anderer sound gespielt fuer ein bisschen Abwechslung
                    playBigScoreSound = True
                End If
                playScoreSound = True 'wenn playScoreSound true ist wird der score sound abgespielt
            End If
            Threading.Thread.Sleep(0.2)
        End While
    End Sub

    'PlayerAnimator() Animiert den Sprung der Spielfigur und cycled durch die Laufanimation
    Public Sub PlayerAnimator()
        While playerInRound
            If playerJumpHeight < 0.1 And playerJump Then 'wenn sich der Spieler in Bodennaehe befindet und ein Sprung initiiert wurde, dann startet Sprunganimation
                playJumpSound = True 'Startet den Sprung Sound
                animateJump = True 'Spieler befindet sich im Sprung: animateJump = True
                playerJump = False
            End If
            If animateJump Then 'bewegt Spielfigur nach oben bis maximale Sprunghoehe erreicht ist (springen)
                If playerJumpHeight < 5 Then '5 ist hier die Maximale Sprunghoehe, ist diese erreicht wird die Spielfigur nicht weiter nach oben bewegt und kehrt in die ausgangsposition zurueck
                    playerJumpHeight += 2
                Else
                    animateJump = False
                End If
            Else
                If playerJumpHeight > 0 Then playerJumpHeight -= 1 'Animation der Spielfigur in die Ausgangsposition (landen)
            End If
            If playerAnimationState = 7 Then 'Cycled durch die Laufsequenz
                playerAnimationState = 1
            Else
                playerAnimationState += 1
            End If
            Threading.Thread.Sleep(60)
        End While
    End Sub

    'GroundAnimator() Bewegt die Hindernisse von Rechts nach Links und Spawned zufaellig neue
    Public Sub GroundAnimator()
        Dim spawnTimer As Integer 'Verstrichene Zeit seid letztem Hindernis Spawn
        While playerInRound
            groundTiles.RemoveAt(0) 'Entfernt das links aussen positionierte Bodenstueck der Bodenstueckliste
            If spawnTimer > 150 Then 'Sofern Zeit zwischen Hindernissen verstrichen ist, fuege neues Hindernisstueck zur Bodenstueckliste hinzu
                Dim tyleTypeRnd As Integer = CInt(Math.Floor((4 - 1 + 1) * Rnd())) + 1 'Waehlt einen Zufaelligen Bodenstueck Typ aus
                If tyleTypeRnd = 1 Or tyleTypeRnd = 2 Then
                    groundTiles.Add(New Tile With {.tileType = tyleTypeRnd}) 'Fuegt rechts aussen ein Bodenstueck aus der Bodenstueckliste hinzu
                End If
                If tyleTypeRnd = 3 Or tyleTypeRnd = 4 Then 'Entfernt weitere Bodenstueck damit auch breitere Bodenstueck hinzugefuegt werden koennen ohne das es einen Overflow error gibt
                    groundTiles.RemoveAt(constConsoleWidth - 2)
                    groundTiles.RemoveAt(constConsoleWidth - 3)
                    groundTiles.RemoveAt(constConsoleWidth - 4)
                    groundTiles.Add(New Tile With {.tileType = tyleTypeRnd})
                    groundTiles.Add(New Tile With {.tileType = 0})
                    groundTiles.Add(New Tile With {.tileType = 0})
                    groundTiles.Add(New Tile With {.tileType = 0})
                End If
                spawnTimer = 0
            Else 'Fuer den Fall das kein neues Hindernis gespawned ist, wird ein Bodenstueck ohne Hindernis hinzugefuegt und alle andern rutschen ein Stueck auf
                groundTiles.Add(New Tile With {.tileType = 0})
            End If
            Randomize() 'variiere Zeit zwischen Hindernisspawns; Zeit verringert sich ueber die Spielzeit
            spawnTimer = (spawnTimer + CInt(Math.Floor((constRandomUpperBound - constRandomLowerBound + 1) * Rnd())) + constRandomLowerBound) + (roundScore / 20)
            Threading.Thread.Sleep(20)
        End While
    End Sub

    'SkyAnimator() Eigentlich das gleiche wie der GroundAnimator nur fuer den Himmel
    Public Sub SkyAnimator()
        Dim spawnTimerSky
        While playerInRound
            skyTiles1.RemoveAt(0)
            skyTiles2.RemoveAt(0)
            If spawnTimerSky > 100 Then
                Dim tyleTypeSkyRnd As Integer = CInt(Math.Floor((8 - 6 + 1) * Rnd())) + 6
                Dim skyLayerRnd As Integer = CInt(Math.Floor((2 - 1 + 1) * Rnd())) + 1
                If skyLayerRnd = 1 Then
                    For i = 2 To 8
                        skyTiles1.RemoveAt(constConsoleWidth - i)
                    Next
                    skyTiles1.Add(New Tile With {.tileType = tyleTypeSkyRnd})
                    For i = 0 To 6
                        skyTiles1.Add(New Tile With {.tileType = 5})
                    Next
                    skyTiles2.Add(New Tile With {.tileType = 5})
                Else

                    For i = 2 To 8
                        skyTiles2.RemoveAt(constConsoleWidth - i)
                    Next
                    skyTiles2.Add(New Tile With {.tileType = tyleTypeSkyRnd})
                    For i = 0 To 6
                        skyTiles2.Add(New Tile With {.tileType = 5})
                    Next
                    skyTiles1.Add(New Tile With {.tileType = 5})
                End If
                spawnTimerSky = 0
            Else
                skyTiles1.Add(New Tile With {.tileType = 5})
                skyTiles2.Add(New Tile With {.tileType = 5})
            End If
            Randomize()
            spawnTimerSky = (spawnTimerSky + CInt(Math.Floor((constRandomUpperBound - constRandomLowerBound + 1) * Rnd())) + constRandomLowerBound) + (roundScore / 20)
            Threading.Thread.Sleep(80)
        End While
    End Sub

    'AsyncLoopGame() Startet Parallele Prozesse die waehrend der ganzen Zeit laufen
    Public Function AsyncLoopGame() As Task
        Dim task1 = Task.Run(AddressOf KeyImput)
        Dim task2 = Task.Run(AddressOf SoundManager)
    End Function

    'SoundManager() Spielt Sounds parallel ab
    Public Sub SoundManager()
        If initiateSounds Then 'Laedt Sounds bei start
            Snds.AddSound("Jump", "jump.wav")
            Snds.AddSound("Death", "death.wav")
            Snds.AddSound("Score", "score.wav")
            Snds.AddSound("BigScore", "bigscore.wav")
            initiateSounds = False
        End If

        While True
            If playJumpSound Then
                Snds.Play("Jump")
                playJumpSound = False
            End If
            If playDeathSound Then
                Snds.Play("Death")
                playDeathSound = False
            End If
            If playScoreSound Then
                Snds.Play("Score")
                playScoreSound = False
            End If
            If playBigScoreSound Then
                Snds.Play("BigScore")
                playBigScoreSound = False
            End If
        End While
    End Sub

    'KeyImput() Prueft ob der Spieler eine Taste gedrueckt hat und reagiert gegebenenfalls
    Public Sub KeyImput()
        Dim consoleKey As ConsoleKeyInfo
        Dim fifteenFix As Boolean = False
        While True
            consoleKey = Console.ReadKey(True)
            If playerInRound Then 'Spieler befindet sich in einer Runde
                If (consoleKey.Key = 32) <> 0 Then playerJump = True 'Key32=Leertaste; starte sprung animation
                If (consoleKey.Modifiers And ConsoleModifiers.Control And consoleKey.Key = 67) <> 0 Then playerInRound = False 'Abfrage bei Tastenkombination "Strg C" beenden der Runde - um Entwicklung zu vereinfachen
            Else 'Spieler befindet sich in einem Menue
                If (consoleKey.Key = 32) <> 0 And keyInputDelay Then menuConfirm = True 'Key32=Leertaste; Auswahl bestaetigen
                If (consoleKey.Key = 13) <> 0 Then menuConfirm = True 'Key13=Enter; Auswahl bestaetigen
                If (consoleKey.Key = 38) <> 0 Then If selectedMenuOption > 0 Then selectedMenuOption -= 1 'Key38=Pfeiltaste Hoch; Auswahl nach oben sofern noch nicht der oberste Menuepunkt erreicht ist
                If (consoleKey.Key = 40) <> 0 Then If selectedMenuOption < 4 Then selectedMenuOption += 1 'Key40=Pfeiltaste Runter; Auswahl nach unten sofern noch nicht der letzte Menuepunkt erreicht ist


                If changeNameMenuOpen And selectedMenuOption <> 0 Then 'Spieler befindet sich im SubMenuChangeName Menue und hat das Namensfeld ausgewaehlt
                    If consoleKey.Key >= 48 And consoleKey.Key <= 57 Then 'Wurde eine Zahl zwischen 0 und 9 gedrueckt 
                        playerNameCharacters(nameFieldCursorLocation) = consoleKey.Key.ToString()(1) 'Die Zahlen Tasten beginnen alle mit D, deswegen erst das zweite Zeichen nutzen
                        If nameFieldCursorLocation < 15 Then nameFieldCursorLocation += 1 'Nach eingabe eines neuen Buchstabens Cursor um eins nach rechts verruecken
                    End If
                    If consoleKey.Key >= 96 And consoleKey.Key <= 105 Then 'Wurde eine Zahl zwischen 0 und 9 auf dem NumPad gedrueckt
                        playerNameCharacters(nameFieldCursorLocation) = consoleKey.Key.ToString()(6) 'Die NumPad Tasten beginnen alle mit NumPad, deswegen erst das siebte Zeichen nutzen
                        If nameFieldCursorLocation < 15 Then nameFieldCursorLocation += 1
                    End If
                    If consoleKey.Key = 32 Then 'Wurde die Leertaste im Namensfeld gedrueckt, ersetze das Leerzeichen mit -
                        playerNameCharacters(nameFieldCursorLocation) = "-"
                        If nameFieldCursorLocation < 15 Then nameFieldCursorLocation += 1
                    End If
                    If consoleKey.Key >= 65 And consoleKey.Key <= 90 Then 'Es wurde eine Buchstabentaste gedrueckt
                        playerNameCharacters(nameFieldCursorLocation) = consoleKey.Key.ToString()(0)
                        If nameFieldCursorLocation < 15 Then nameFieldCursorLocation += 1
                    End If
                    If (consoleKey.Key = 39) <> 0 Then 'Key39=Pfeiltaste Rechts; Verschiebt den Cursor um eine Einheit nach Rechts
                        If nameFieldCursorLocation < 15 Then nameFieldCursorLocation += 1
                    End If
                    If (consoleKey.Key = 37) <> 0 Then 'Key37=Pfeiltaste Links; Verschiebt den Cursor um eine Einheit nach Links
                        If nameFieldCursorLocation > 0 Then nameFieldCursorLocation -= 1
                    End If
                    If (consoleKey.Key = 8) <> 0 Then 'Key37=Backspace; Loescht eingetragenes Zeichen und Verschiebt den Cursor um eine Einheit nach Links
                        If nameFieldCursorLocation > 0 Then
                            If nameFieldCursorLocation < 15 Or fifteenFix Then
                                playerNameCharacters(nameFieldCursorLocation - 1) = "_"
                                nameFieldCursorLocation -= 1
                                fifteenFix = False
                            Else
                                playerNameCharacters(nameFieldCursorLocation) = "_"
                                fifteenFix = True 'Ist der Cursor ganz rechts muesste er eigentlich in das Feld 16, das gibt es aber nicht deswegen wird dieser Zustand in einer Variable gespeichert
                            End If
                        End If
                    End If
                    If (consoleKey.Key = 13) <> 0 Then 'Key13=Enter; Da es keine Absaetze gibt kann die Entertaste zusaetzlich zu den Pfeiltasten genutzt werden um die Eingabe zu beenden
                        selectedMenuOption = 0
                        menuConfirm = False
                    End If
                    If (consoleKey.Key = 46) <> 0 Then 'Key13=Entfernen; Loescht gegebenenfalls das Zeichen im ausgewaehlten Feld
                        playerNameCharacters(nameFieldCursorLocation) = "_"
                    End If
                End If
            End If
        End While
    End Sub

    'GameOver() Wird aufgerufen wenn es Kollision mit Hindernis gab und Runde zu Ende ist
    Public Sub GameOver()
        keyInputDelay = False
        If musicEnabled Then 'Wenn Musik aktiviert ist: Beendet die InGame Musik und Startet die Menuemusik 
            gamemusicSound.StopPlaying()
            menuloopSound.PlayLoop()
        End If

        For i = 0 To 5 'Laesst Hindernis mit dem die Spielfigur kollidiert ist Rot flackern
            DrawArray($"{groundTiles(6).tileType} ", 6, constConsoleHeight - 1)
            Threading.Thread.Sleep(20)
            Console.ForegroundColor = ConsoleColor.DarkRed
            DrawArray($"{groundTiles(6).tileType} ", 6, constConsoleHeight - 1)
            Console.ForegroundColor = ConsoleColor.White
            Threading.Thread.Sleep(30)
        Next

        FirebaseSend(LoadSettings(2), roundScore) 'Speichern des Highscores

        'Loescht die Zeichen Hinter dem Menue und GameOver Schriftzug damit es keine Ueberlappung mit Wolken gibt
        WriteAt("                                          ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                          ", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                          ", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                          ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                     ", x:=0, y:=-4, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                     ", x:=0, y:=-3, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                     ", x:=0, y:=-2, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                     ", x:=0, y:=-1, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                     ", x:=0, y:=0, CenterHorizontally:=True, CenterVertically:=True)
    End Sub

    'MenuLoop() Das Spielmenue bzw. angepasste Spielmenue fuer den GameOver Fall
    Public Sub MenuLoop()
        If musicEnabled Then menuloopSound.PlayLoop() 'Wenn Musik aktiviert ist startet Hintergrundmusik des Menues
        While True
            If menuConfirm Then
                menuConfirm = False
                If selectedMenuOption = 0 Then 'Menuepunkt Play bzw. Play Again wurde bestaetigt
                    RoundStart()
                    GameOver()
                End If
                If selectedMenuOption = 1 Then 'Menuepunkt HighScores wurde bestaetigt
                    SubMenuHighScores()
                End If
                If selectedMenuOption = 2 Then 'Menuepunkt SubMenuChangeName wurde bestaetigt
                    SubMenuChangeName()
                End If
                If selectedMenuOption = 3 Then 'Menuepunkt Music Enabled bzw. Music Disabled wurde bestaetigt
                    ToggleAudio()
                End If
                If selectedMenuOption = 4 Then 'Menuepunkt Credits wurde bestaetigt
                    SubMenuCredits()
                End If
            End If

            If gameOverBoolean Then 'Nach Kollision mit Hindernis zeige GameOver anstatt Runner96 an
                Console.ForegroundColor = ConsoleColor.DarkRed
                WriteAt("  _____                 ____              ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
                WriteAt(" / ___/__ ___ _  ___   / __ \_  _____ ____", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
                WriteAt("/ (_ / _ `/  ' \/ -_) / /_/ / |/ / -_) __/", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
                WriteAt("\___/\_,_/_/_/_/\__/  \____/|___/\__/_/   ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("   ___                           ___  ____", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
                WriteAt("  / _ \__ _____  ___  ___ ____  / _ \/ __/", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
                WriteAt(" / , _/ // / _ \/ _ \/ -_) __/  \_, / _ \ ", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
                WriteAt("/_/|_|\_,_/_//_/_//_/\__/_/    /___/\___/ ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
            End If

            If selectedMenuOption = 0 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) - 2)
                If gameOverBoolean Then
                    WriteAt("Play Again", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) - 2) 'Nach Kollision mit Hindernis zeige Play Again anstatt Play an
                Else
                    WriteAt("Play", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) - 2)
                End If
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) - 2)
                If gameOverBoolean Then
                    WriteAt("Play Again", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) - 2)
                Else
                    WriteAt("Play", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) - 2)
                End If
            End If
            If selectedMenuOption = 1 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2))
                WriteAt("High Scores", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2))
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2))
                WriteAt("High Scores", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2))
            End If
            If selectedMenuOption = 2 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) + 1)
                WriteAt("Change Name", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 1)
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) + 1)
                WriteAt("Change Name", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 1)
            End If
            If selectedMenuOption = 3 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) + 2)
                If musicEnabled Then
                    WriteAt("Music Enabled", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 2)
                Else
                    WriteAt("Music Disabled", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 2)
                End If
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) + 2)
                If musicEnabled Then
                    WriteAt("Music Enabled", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 2)
                Else
                    WriteAt("Music Disabled", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 2)
                End If
            End If
            If selectedMenuOption = 4 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) + 3)
                WriteAt("Credits", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 3)
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (constConsoleWidth / 2) - 7, (constConsoleHeight / 2) + 3)
                WriteAt("Credits", (constConsoleWidth / 2) - 5, (constConsoleHeight / 2) + 3)
            End If

            If keyInputDelay = False Then 'Fuegt ein kurzes Delay hinzu bevor die naechste Runde gestartet werden kann
                Threading.Thread.Sleep(300)
                keyInputDelay = True
            End If
        End While
    End Sub

    'SubMenuHighScores() Untermenue mit einer Liste der Highscores
    Public Sub SubMenuHighScores()
        Console.Clear()
        WriteAt("   __ ___      __     ____                   ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / // (_)__ _/ /    / __/______  _______ ___", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / _  / / _ `/ _ \  _\ \/ __/ _ \/ __/ -_|_-<", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_//_/_/\_, /_//_/ /___/\__/\___/_/  \__/___/", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("       /___/                                 ", x:=0, y:=-4, CenterHorizontally:=True, CenterVertically:=True)
        Console.ForegroundColor = ConsoleColor.Blue
        WriteAt("> Back", 3, 1)
        Console.ForegroundColor = ConsoleColor.White
        Dim runLine As Integer = -2
        For Each gameScore As KeyValuePair(Of String, GameScore) In FirebaseReceive() 'Schreibe die besten 8 HighScores auf die Console
            If runLine <= 5 Then
                WriteAt(gameScore.Value.playerName & " - " & gameScore.Value.roundDate & " - " & gameScore.Value.playerScore, x:=0, y:=runLine, CenterHorizontally:=True, CenterVertically:=True)
            End If
            runLine += 1
        Next
        While menuConfirm = False
        End While
        menuConfirm = False
        Console.Clear()
    End Sub

    'SubMenuChangeName() Ermoeglicht das Aendern des Spielernamen bei Auswahl des Menuepunkts bzw. das setzten eines Spielernamen beim ersten Start
    Public Sub SubMenuChangeName()
        Dim playerName As String
        playerNameCharacters.Clear()
        playerName = LoadSettings(2)
        If openFirstTime Then playerName = "" 'Damit beim ersten Start kein Spielernamen angezeigt wird
        For index As Integer = 0 To 15 'Fuegt die Zeichen des playerNames der Liste hinzu und fuellt verbleibende Stellen auf bis 15 erreicht sind
            Try
                playerNameCharacters.Add(playerName(index))
            Catch
                playerNameCharacters.Add("_")
            End Try
        Next
        nameFieldCursorLocation = 0
        selectedMenuOption = 1 'Bei Oeffnen des Menues Auswahl des Eingabefelds
        If openFirstTime = False Then Console.Clear()
        changeNameMenuOpen = True
        If openFirstTime = False Then 'Beim ersten Spielstart soll Change Name nicht angezeigt werden
            WriteAt("  _______                        _  __              ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
            WriteAt(" / ___/ /  ___ ____  ___ ____   / |/ /__ ___ _  ___ ", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
            WriteAt("/ /__/ _ \/ _ `/ _ \/ _ `/ -_) /    / _ `/  ' \/ -_)", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
            WriteAt("\___/_//_/\_,_/_//_/\_, /\__/ /_/|_/\_,_/_/_/_/\__/ ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
            WriteAt("                   /___/                            ", x:=0, y:=-4, CenterHorizontally:=True, CenterVertically:=True)
        End If
        WriteAt("Enter player name:", (constConsoleWidth / 2) - 20, (constConsoleHeight / 2))
        While True
            If selectedMenuOption = 0 Then 'Beim ersten Spielstart soll anstatt von Back, Continue angezeigt werden
                Console.ForegroundColor = ConsoleColor.Blue
                If openFirstTime Then
                    WriteAt("> Continue ", 3, 1)
                Else
                    WriteAt("> Back ", 3, 1)
                End If
                Console.ForegroundColor = ConsoleColor.White
                If menuConfirm Then Exit While
            Else
                If openFirstTime Then
                    WriteAt("  Continue ", 3, 1)
                Else
                    WriteAt("  Back ", 3, 1)
                End If
                menuConfirm = False
            End If
            If selectedMenuOption > 1 Then 'Letzte position der Auswahlmoeglichkeiten im ChangeName Menue schon bei 1
                selectedMenuOption = 1
            End If

            For index As Integer = 0 To 15 'Schreibe die playerNameCharacters an der jeweiligen position auf die Konsole und setzte den Cursor an die richtige Stelle
                If nameFieldCursorLocation = index And selectedMenuOption = 1 Then
                    Console.ForegroundColor = ConsoleColor.Blue
                    WriteAt(playerNameCharacters(index), (constConsoleWidth / 2) + index, (constConsoleHeight / 2))
                    WriteAt("^", (constConsoleWidth / 2) + index, (constConsoleHeight / 2) + 1)
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    WriteAt(playerNameCharacters(index), (constConsoleWidth / 2) + index, (constConsoleHeight / 2))
                    WriteAt(" ", (constConsoleWidth / 2) + index, (constConsoleHeight / 2) + 1)
                End If
            Next
        End While

        Dim characterPosition = 0
        Dim playerNameCharactersCashe As New List(Of String)
        For Each character As String In playerNameCharacters 'Entferne lehre stellen von der Liste
            If character <> "_" Then
                playerNameCharactersCashe.Add(character)
                characterPosition += 1
            End If
        Next

        Dim playerNameCashe As String = Join(playerNameCharactersCashe.Where(Function(s) Not String.IsNullOrEmpty(s)).ToArray(), "") 'Fuege die einzelnen Character der Liste zum PlayerNamen zusammen
        If playerNameCashe <> "" Then
            playerName = playerNameCashe
            SaveSettings(2, playerName:=playerName) 'Speichern des neuen Player Namens
        Else
            playerName = "NONAME" 'Wenn der Spieler keinen Namen eingegeben hat setzte den Namen auf NONAME
            SaveSettings(2, playerName:=playerName)
        End If

        menuConfirm = False
        Console.Clear()
    End Sub

    'SubMenuCredits() Untermenue mit den Namen der beteiligten Personen
    Public Sub SubMenuCredits()
        Console.Clear()
        WriteAt("  _____           ___ __    ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / ___/______ ___/ (_) /____", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/ /__/ __/ -_) _  / / __(_-<", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("\___/_/  \__/\_,_/_/\__/___/", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        While menuConfirm = False
            Console.ForegroundColor = ConsoleColor.Blue
            WriteAt("> Back", 3, 1)
            Console.ForegroundColor = ConsoleColor.White
            WriteAt("TWE-22", (constConsoleWidth / 2) - 8, (constConsoleHeight / 2) - 2)
            WriteAt("Philipp Brocher", (constConsoleWidth / 2) - 8, (constConsoleHeight / 2) - 1)
            WriteAt("Erik Siegel", (constConsoleWidth / 2) - 8, (constConsoleHeight / 2))
            WriteAt("Linus von Maltzan", (constConsoleWidth / 2) - 8, (constConsoleHeight / 2) + 1)
            WriteAt("Musik: David Bock", (constConsoleWidth / 2) - 8, (constConsoleHeight / 2) + 2)
        End While
        menuConfirm = False
        Console.Clear()
    End Sub

    'ToggleAudio() Wird bei Auswahl des Menuepunktes Enable/Disable Audio aufgerufen und wechselt zwischen den beiden
    Public Sub ToggleAudio()
        If musicEnabled Then
            musicEnabled = False
            SaveSettings(0, False)
            menuloopSound.StopPlaying()
        Else
            musicEnabled = True
            SaveSettings(0, True)
            menuloopSound.PlayLoop()
        End If
    End Sub

    'Introduction() Wird bei Spielstart ausgefuehrt und zeigt beim ersten Spielstart das Tutorial an
    Public Sub Introduction()
        Console.Clear()
        introSound.PlayFromStart()
        WriteAt("   ___ ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_| ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___      ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ __", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // /", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/ ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___          ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ _____ ", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // / _ \", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/_//_/", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___               ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ _____  ___ ", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // / _ \/ _ \", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/_//_/_//_/", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___                    ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ _____  ___  ___ ", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // / _ \/ _ \/ -_)", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/_//_/_//_/\__/ ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___                        ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ _____  ___  ___ ____", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // / _ \/ _ \/ -_) __/", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/_//_/_//_/\__/_/   ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___                           ___ ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ _____  ___  ___ ____  / _ \", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // / _ \/ _ \/ -_) __/  \_, /", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/_//_/_//_/\__/_/    /___/ ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        Threading.Thread.Sleep(35)
        WriteAt("   ___                           ___  ____", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / _ \__ _____  ___  ___ ____  / _ \/ __/", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / , _/ // / _ \/ _ \/ -_) __/  \_, / _ \ ", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_/|_|\_,_/_//_/_//_/\__/_/    /___/\___/ ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        If musicEnabled Then menuloopSound.PlayLoop()
        Threading.Thread.Sleep(600)
        If openFirstTime = False Then MenuLoop()
        While menuConfirm = False 'Animation der "Press [Space] to continue" Aufforderung
            Console.ForegroundColor = ConsoleColor.Blue
            WriteAt("Press [Space] to continue", x:=0, y:=0, CenterHorizontally:=True, CenterVertically:=True)
            Console.ForegroundColor = ConsoleColor.White
            If menuConfirm = False Then Threading.Thread.Sleep(600)
            If menuConfirm = False Then Threading.Thread.Sleep(600)
            WriteAt("                         ", x:=0, y:=0, CenterHorizontally:=True, CenterVertically:=True)
            If menuConfirm = False Then Threading.Thread.Sleep(600)
            If menuConfirm = False Then Threading.Thread.Sleep(600)
        End While
        menuConfirm = False
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Blue
        WriteAt("Press [Space] to continue", 3, 1)
        Console.ForegroundColor = ConsoleColor.White
        WriteAt("Willkommen bei Runner 96, dem ultimativen Test fuer", x:=35, y:=-6, CenterVertically:=True)
        WriteAt("Geschwindigkeit, Reflexe und Ausdauer!", x:=35, y:=-5, CenterVertically:=True)
        WriteAt("Ueberwinde unzaehlige Hindernisse und laufe so weit wie", x:=35, y:=-3, CenterVertically:=True)
        WriteAt("moeglich, waehrend du dich in einer tueckischen Welt voller ", x:=35, y:=-2, CenterVertically:=True)
        WriteAt("Barrieren und Hindernissen bewegst. ", x:=35, y:=-1, CenterVertically:=True)
        WriteAt("Praezision und Timing sind der Schluessel, denn jede ", x:=35, y:=1, CenterVertically:=True)
        WriteAt("Beruehrung eines Hindernisses bedeutet das Ende. ", x:=35, y:=2, CenterVertically:=True)
        WriteAt("Tritt gegen dich selbst an oder fordere Freunde heraus, ", x:=35, y:=4, CenterVertically:=True)
        WriteAt("um deinen Highscore zu uebertreffen. ", x:=35, y:=5, CenterVertically:=True)
        While menuConfirm = False
        End While
        menuConfirm = False
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Blue
        WriteAt("Press [Space] to continue", 3, 1)
        Console.ForegroundColor = ConsoleColor.White
        WriteAt("Bevor es losgeht noch ein paar kurze Tipps wie du dich ", x:=35, y:=-6, CenterVertically:=True)
        WriteAt("durch die Welt von Runner 96 bewegst:", x:=35, y:=-5, CenterVertically:=True)
        WriteAt("- Durch druecken der Leertaste kannst du waehrend", x:=35, y:=-3, CenterVertically:=True)
        WriteAt("  einer Runde mit deinem Charakter springen und", x:=35, y:=-2, CenterVertically:=True)
        WriteAt("  so auf dich zukommende Hindernisse ueberwinden.", x:=35, y:=-1, CenterVertically:=True)
        WriteAt("- Durch betaetigen der Pfeiltasten kannst du", x:=35, y:=1, CenterVertically:=True)
        WriteAt("  zwischen verschiedenen menuepunkten Waehlen", x:=35, y:=2, CenterVertically:=True)
        WriteAt("- Mit der Entertaste oder der Leertaste bestaetigst", x:=35, y:=4, CenterVertically:=True)
        WriteAt("  du die, blau hinterlegte, aktuelle Auswahl", x:=35, y:=5, CenterVertically:=True)
        While menuConfirm = False
        End While
        menuConfirm = False
        Console.Clear()
        WriteAt("Damit du starten kannst muessen wir noch wissen unter", x:=35, y:=-6, CenterVertically:=True)
        WriteAt("welchem Namen du antrittst.", x:=35, y:=-5, CenterVertically:=True)
        SubMenuChangeName()
        Console.ForegroundColor = ConsoleColor.Blue
        WriteAt("> Continue", 3, 1)
        Console.ForegroundColor = ConsoleColor.White
        WriteAt("Alles klar " & LoadSettings(2) & " dann kann es ja los gehen!", x:=35, y:=-6, CenterVertically:=True)
        WriteAt("Schnuer deine Schuhe und stuerze dich in das Abenteuer", x:=35, y:=-4, CenterVertically:=True)
        WriteAt("von Runner 96 und zeige, was in dir steckt.", x:=35, y:=-3, CenterVertically:=True)
        While menuConfirm = False
        End While
        menuConfirm = False
        openFirstTime = False
        SaveSettings(1, openFirstTime:=openFirstTime)
        Console.Clear()
    End Sub

    Public Sub Main()
        Debug.WriteLine(Debugger.IsAttached)
        If Debugger.IsAttached = False Then
            ConsoleSetup(constConsoleWidth + 1, constConsoleHeight)
        Else
            ConsoleSetup(constConsoleWidth, constConsoleHeight)
        End If
        FirebaseLoad()
        AsyncLoopGame()
        Introduction()
        MenuLoop()
    End Sub
End Module

