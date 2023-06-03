'importieren von Bibliotheken
Imports System
Imports System.IO
Imports System.Linq.Expressions
Imports System.Net.Mime.MediaTypeNames
Imports System.Net.NetworkInformation
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports InformatikSpiel.MCIDEMO

Module Module1

    Dim origRow, origCol As Integer
    'Funktion zur Ausgabe eines Zeichens an einer bestimmten Position der Konsole
    's ist das zu schreibende Zeichen; x und y sind die Koordinaten
    Public Sub WriteAt(s As String, x As Integer, y As Integer)
        Try
            Console.SetCursorPosition(origCol + x, origRow + y)
            Console.Write(s)
        Catch e As ArgumentOutOfRangeException 'Fallback (Bei ungültiger Eingabe)
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    ' Quelle [https://stackoverflow.com/a/4985870/13324025]
    'gibt aus ob Zahl ein ganzzahliger Teiler ist
    Function isDivisible(x As Integer, d As Integer) As Boolean
        Return (x Mod d) = 0
    End Function

    'Einrichten der Konsole
    'Größe, Scrollbar wird entfernt, Kursor ausblenden
    Public Sub ConsoleSetup(consoleWidth As Integer, consoleHeight As Integer)
        Console.SetWindowSize(consoleWidth, consoleHeight)
        Console.SetBufferSize(consoleWidth, consoleHeight)
        Console.CursorVisible = False
        Console.TreatControlCAsInput = True 'verhindert das Abbrechen des Programms durch Strg c: Fenster wird nicht geschlossen, GameOver wird angezeigt
        Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory.ToString())
    End Sub

    Public gameTimer As Integer = 0 'verstrichene Zeit seit Spielbeginn (Rundenstart)
    Public gameTimerCache As Integer = 0 'für Ausgabe FPS nötig
    Public fps As Double = 0
    Public score As Integer = 0
    Public highScore As Integer
    Public Sub DrawStats()
        WriteAt("Timer:" & gameTimer, 1, 1) 'Gametimer: Ausgabe der Spielzeit auf dem Bildschirm
        If gameTimer > gameTimerCache Then 'wann ist eine Sekunde vergangen
            WriteAt("FPS:" & fps, 1, 2) 'nach einer Sekunde Ausgabe der FPS-Variable auf dem Bildschirm
            fps = 0
        End If
        fps += 1 'erhöhe die FPS Variable jeden gerenderten Frame um 1
        gameTimerCache = gameTimer
        WriteAt("Score:" & score, 1, 0)
    End Sub

    'Animation der Spielfigur
    Public Sub DrawPlayer(playerState As Integer, playerAnchorX As Integer, playerAnchorY As Integer)
        'Animation der Spielfigur bestehend aus 8 Frames
        Try
            Select Case playerState 'playerState: welcher Frame der Figuranimation soll ausgegeben werden
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
        Catch e As ArgumentOutOfRangeException 'Fallback für Abruf eines nicht vorhandenen Zustands
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    'Sprung der Spielfigur über die Hindernisse
    Public playerJumpHeightCache As Integer 'Sprunghöhe!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11
    Dim playerXPosition As Integer = 1 'Ausgangsposition des Spielers
    Dim playerYPosition As Integer = 13
    Public Sub PlayerManager() 'Ausgabe der animierten Spielfigur an der entsprechenden Position am Bildschirm
        If playerJumpHeight <> playerJumpHeightCache Then DrawPlayer(0, playerXPosition, playerYPosition - playerJumpHeightCache)
        DrawPlayer(playerAnimationState, playerXPosition, playerYPosition - playerJumpHeight)
        playerJumpHeightCache = playerJumpHeight
    End Sub

    'Auswahl der Bausteine für den Boden (Hindernisse und der Weg)
    Public Sub DrawGround(groundState As Integer, groundAnchorX As Integer, groundAnchorY As Integer)
        Try
            Select Case groundState
                Case 0
                    WriteAt(" ", groundAnchorX, groundAnchorY - 2)
                    WriteAt(" ", groundAnchorX, groundAnchorY - 1)
                    WriteAt("`", groundAnchorX, groundAnchorY)
                Case 1
                    WriteAt("V", groundAnchorX, groundAnchorY - 2)
                    WriteAt("|", groundAnchorX, groundAnchorY - 1)
                    WriteAt("`", groundAnchorX, groundAnchorY)
            End Select

        Catch e As ArgumentOutOfRangeException 'Fallback
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    Public Class Tile 'Bodenstück
        Public Property tileType As Integer
    End Class

    Public groundTiles As New List(Of Tile) 'Liste bestehend aus Bodenstücken
    Public Sub GroundArraySetUp() 'befüllen der Liste mit Bodenstücken beim Rundenstart
        groundTiles.Clear()
        For i = 0 To 99
            groundTiles.Add(New Tile With {.tileType = 0}) 'Bestückung mit Bodenstücken des Standarttyps (kein Hindernis)
        Next
    End Sub

    Public Sub GroundManager() 'Ausgabe der Bodenstückliste in der Konsole
        For index As Integer = groundTiles.Count - 1 To 0 Step -1
            DrawGround($"{groundTiles(index).tileType} ", index, 19) 'nicht denken, maaalen
        Next
        If groundTiles(6).tileType = 1 Then 'wenn an Position des Players ein Hindernis ist, wird geprüft ob Spielfigur am Boden ist. Wenn ja, Game Over; Wenn nein, Score +1.
            If playerJumpHeight < 0.2 Then
                playDethSound = True
                gameOverBoolean = True
                stayInLoop = False
            Else
                score += 1
                If isDivisible(score, 10) Then
                    playBigScoreSound = True
                Else
                    playScoreSound = True
                End If
            End If
        End If
    End Sub


    Public stayInLoop As Boolean = False 'solange True, bis Runde beendet werden soll
    Public gameOverBoolean As Boolean = False
    Public playerAnimationState As Integer 'speichert den vorherigen Frame der Animation des Players
    Public playerJump As Boolean 'True, wenn Sprunganimation gestartet werden soll
    Public playerJumpHeight As Integer 'Sprunghöhe des Spielers
    Public Function AsyncLoopRound() As Task   'Starten von verschiedenen Asynchronen Schleifen/Prozessen
        Dim taskA = Task.Run(AddressOf RenderLoop)
        Dim taskB = Task.Run(AddressOf Timer)
        Dim taskC = Task.Run(AddressOf PlayerAnimator)
        Dim taskD = Task.Run(AddressOf GroundAnimator)

        Task.WaitAll(taskA)
    End Function

    Public Function AsyncLoopGame() As Task   'Starten von verschiedenen Asynchronen Schleifen/Prozessen
        Dim taskE = Task.Run(AddressOf KeyImput)
        Dim taskF = Task.Run(AddressOf SoundManager)

    End Function

    Public Sub RenderLoop()
        While stayInLoop 'solange true, wird Spieloberfläche gerendert
            DrawStats()
            PlayerManager()
            GroundManager()
            Threading.Thread.Sleep(1)
        End While
    End Sub
    Public Sub Timer() 'zählt ab Rundenbeginn Spielsekunden hoch
        While stayInLoop
            gameTimer += 1
            Threading.Thread.Sleep(1000)
        End While
    End Sub

    Public playJumpSound As Boolean = False
    Public playBackgroundSound As Boolean = False
    Public playDethSound As Boolean = False
    Public playScoreSound As Boolean = False
    Public playBigScoreSound As Boolean = False
    Public initiateSounds As Boolean = True
    Public inMenu As Boolean = True
    Dim Snds As New MultiSounds

    Public Sub SoundManager()
        If initiateSounds Then
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
            If playBackgroundSound Then
                Snds.Play("Background")
                playJumpSound = False
            End If
            If playDethSound Then
                Snds.Play("Death")
                playDethSound = False
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
    Public Sub KeyImput() 'Abfrage der Tastaturabfrage
        Dim cki As ConsoleKeyInfo
        While True
            cki = Console.ReadKey()
            If (cki.Key = 32) <> 0 And stayInLoop Then
                playerJump = True 'Key32=Lertaste; wenn Leertaste gedrückt wird: setzen der Sprungvariable auf True
            End If
            If (cki.Key = 32) <> 0 And stayInLoop = False Then
                menuConfirm = True
            End If
            If (cki.Key = 13) <> 0 And stayInLoop = False Then
                menuConfirm = True
            End If
            If (cki.Key = 38) <> 0 And stayInLoop = False Then
                If menuCursorPosition > 0 Then menuCursorPosition -= 1
            End If
            If (cki.Key = 40) <> 0 And stayInLoop = False Then
                If menuCursorPosition < 3 Then menuCursorPosition += 1
            End If
            If (cki.Modifiers And ConsoleModifiers.Control And cki.Key = 67) <> 0 Then stayInLoop = False 'Abfrage bei Tastenkombination "Strg C" beenden der Runde
        End While
    End Sub

    Public animateJump As Boolean = False 'Variable ist True sofern sich der Spieler aktiv in einem Sprung befindet
    Public Sub PlayerAnimator()
        While stayInLoop
            If playerJumpHeight < 0.1 And playerJump Then 'wenn sich der Spieler in Bodennähe befindet und ein Sprung initiiert wurde, dann startet Sprunganimation
                '  My.Computer.Audio.Play("23DB05PJ_sfxJump_v1.01_1.wav", AudioPlayMode.Background) 'Sound
                playJumpSound = True

                animateJump = True 'Spieler befindet sich im Sprung: animateJump = True
                playerJump = False
            End If
            If animateJump Then 'bewegt Spielfigur nach oben bis maximale Spielersprunghöhe erreicht ist (springen)
                If playerJumpHeight < 6 Then playerJumpHeight += 2
            Else
                If playerJumpHeight > 0 Then playerJumpHeight -= 1 'Animation der Spielfigur in die Ausgangshöhe (landen)
            End If
            If playerJumpHeight > 5 Then animateJump = False
            If playerAnimationState = 7 Then
                playerAnimationState = 1
            Else
                playerAnimationState += 1
            End If
            Threading.Thread.Sleep(60)
        End While
    End Sub
    Public spawnTimer As Integer 'Zeit zwischen Hindernissen
    Dim randomLowerBound As Integer = 0
    Dim randomUpperBound As Integer = 5
    Public Sub GroundAnimator()
        While stayInLoop
            groundTiles.RemoveAt(0) 'entfernt das links außen positionierte Bodenstück der Bodenstückliste, fügt rechts außen ein Bodenstück aus der Bodenstcükliste hinzu
            If spawnTimer > 150 Then 'sofern Zeit zwischen Hinderissen vestrichen ist, füge neues Hindernisstück zur Bodenstückliste hinzu
                groundTiles.Add(New Tile With {.tileType = 1})
                spawnTimer = 0
            Else 'andernfalls wird Bodenstück ohne Hindernis hinzugefügt
                groundTiles.Add(New Tile With {.tileType = 0})
            End If
            Randomize() 'variiere Zeit zwischen Hindernisspawns; Zeit verringert sich über die Spielzeit
            spawnTimer = (spawnTimer + CInt(Math.Floor((randomUpperBound - randomLowerBound + 1) * Rnd())) + randomLowerBound) + (score / 20)

            Threading.Thread.Sleep(17)
        End While
    End Sub


    Public Sub GameStart() 'wird ausgeführt beim Start einer Runde; Zurücksetzen der Variablen
        Console.Clear()
        stayInLoop = True
        playerAnimationState = 0
        playerJump = False
        animateJump = False
        playJumpSound = False
        playBackgroundSound = False
        playDethSound = False
        playScoreSound = False
        playBigScoreSound = False
        playerJumpHeight = 0
        playerJumpHeightCache = 0
        spawnTimer = 0
        gameTimer = 0
        gameTimerCache = 0
        gameOverBoolean = False
        fps = 0
        score = 0
        If musicEnabled Then
            menuloopSound.StopPlaying()
            gamemusicSound.PlayLoop()
        End If
        GroundArraySetUp()
        AsyncLoopRound()
    End Sub

    Public Sub SaveSettings(ByVal spielZurücksetzen As Boolean)
        ' System.IO.File.WriteAllText("musicsettings.txt", "1")
    End Sub

    Public Function LoadSettings() As Boolean
        Using writer As New StreamWriter("musicsettings.txt", True)
            Dim line As Boolean
            Using reader As New StreamReader("musicsettings.txt")
                line = Convert.ToBoolean(reader.ReadLine().ToString())
            End Using
            Return line
        End Using
    End Function

    Public Function SaveHighscore()
        Using writer As New StreamWriter("highscore.txt", True)
            writer.WriteLine("Important data line 1")
        End Using
    End Function

    Public Function ReadHighscore()
        Dim line As String
        Using reader As New StreamReader("highscore.txt")
            line = reader.ReadLine()
        End Using
        Console.WriteLine(line)
    End Function


    Public Sub GameOver() 'wird ausgeführt bei Berührung der Spielfigur mit einem Hindernis; Ausgeben des Game-Over Bildschirms
        'My.Computer.Audio.Play("23DB05PJ_sfxDeath_v1.01(1).wav", AudioPlayMode.Background) Sound Tod
        If musicEnabled Then
            gamemusicSound.StopPlaying()
            menuloopSound.PlayLoop()
        End If
        Console.ForegroundColor = ConsoleColor.DarkRed
        WriteAt("Game Over", 45, 4)
        Console.ForegroundColor = ConsoleColor.White
        If score > highScore Then 'Highscore wird ggf. gesetzt; Überprüfen ob Score der aktuellen Runde höher als Highscore ist
            highScore = score
            WriteAt("New Highscore", 43, 5)
        End If
    End Sub

    Public Sub HowToPlay()
        Console.Clear()
        While menuConfirm = False
            Console.ForegroundColor = ConsoleColor.Blue
            WriteAt("> Back", 3, 1)
            Console.ForegroundColor = ConsoleColor.White
            WriteAt("Drücke Leertaste um zu springen und Hindernissen auszuweichen", 10, 8)
            WriteAt("Ziel ist es eine möglichst große Strecke zurückzulegen ohne ein Hinderniss zu berühren.", 10, 9)
        End While
        menuConfirm = False
        Console.Clear()
    End Sub

    Public Sub Credits()
        Console.Clear()
        While menuConfirm = False
            Console.ForegroundColor = ConsoleColor.Blue
            WriteAt("> Back", 3, 1)
            Console.ForegroundColor = ConsoleColor.White
            WriteAt("TWE-22", 40, 7)
            WriteAt("Philipp Brocher", 40, 9)
            WriteAt("Erik Siegel", 40, 10)
            WriteAt("Linus von Maltzan", 40, 11)
            WriteAt("Musik: Pochers Dude", 40, 13)
        End While
        menuConfirm = False
        Console.Clear()
    End Sub


    Public musicEnabled As Boolean = True
    Public Sub ToggleAudio()
        If musicEnabled Then
            musicEnabled = False
            menuloopSound.StopPlaying()
        Else
            musicEnabled = True
            menuloopSound.PlayLoop()
        End If
    End Sub


    Public menuloopSound As New MciPlayer("menuloop.mp3", "1")
    Public gamemusicSound As New MciPlayer("music.mp3", "2")

    Public menuCursorPosition As Integer = 0
    Public menuConfirm As Boolean = False
    Public Sub MenuLoop()
        menuloopSound.PlayLoop()
        While True
            If menuConfirm Then
                menuConfirm = False
                If menuCursorPosition = 0 Then
                    GameStart()
                    GameOver()
                End If
                If menuCursorPosition = 1 Then
                    HowToPlay()
                End If
                If menuCursorPosition = 2 Then
                    ToggleAudio()
                End If
                If menuCursorPosition = 3 Then
                    Credits()
                End If
            End If

            If menuCursorPosition = 0 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", 43, 7)
                If gameOverBoolean Then
                    WriteAt("Play Again  ", 45, 7)
                Else
                    WriteAt("Play  ", 45, 7)
                End If
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", 43, 7)
                If gameOverBoolean Then
                    WriteAt("Play Again  ", 45, 7)
                Else
                    WriteAt("Play  ", 45, 7)
                End If
            End If
            If menuCursorPosition = 1 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", 43, 9)
                WriteAt("How to Play  ", 45, 9)
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", 43, 9)
                WriteAt("How to Play  ", 45, 9)
            End If
            If menuCursorPosition = 2 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", 43, 10)
                If musicEnabled Then
                    WriteAt("Music Enabled   ", 45, 10)
                Else
                    WriteAt("Music Disabled   ", 45, 10)
                End If
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", 43, 10)
                If musicEnabled Then
                    WriteAt("Music Enabled   ", 45, 10)
                Else
                    WriteAt("Music Disabled   ", 45, 10)
                End If
            End If
            If menuCursorPosition = 3 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", 43, 11)
                WriteAt("Credits  ", 45, 11)
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", 43, 11)
                WriteAt("Credits  ", 45, 11)
            End If
        End While
    End Sub


    Public Sub Main()
        'SaveSettings(True)
        Debug.WriteLine(LoadSettings())
        ConsoleSetup(100, 21)
        AsyncLoopGame()
        MenuLoop()
    End Sub

End Module
