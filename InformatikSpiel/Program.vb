Imports System
Imports System.IO
Imports System.Linq.Expressions
Imports System.Net.NetworkInformation
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports InformatikSpiel.MCIDEMO

Module Module1
    'Funktion zur Ausgabe eines Zeichens an einer bestimmten Position der Konsole
    's ist das zu schreibende Zeichen; x und y sind die Koordinaten
    Private Sub WriteAt(s As String, x As Integer, y As Integer, Optional CenterHorizontally As Boolean = False, Optional CenterVertically As Boolean = False)
        Try
            If CenterHorizontally Then
                x = (consoleWidth / 2) - (Len(s) / 2) + x
            End If
            If CenterVertically Then
                y = (consoleHeight / 2) + y
            End If
            Console.SetCursorPosition(x, y)
            Console.Write(s)
        Catch e As ArgumentOutOfRangeException 'Fallback (Bei ungültiger Eingabe)
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    'gibt aus ob Zahl ein ganzzahliger Teiler ist
    Private Function isDivisible(x As Integer, d As Integer) As Boolean
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
    Public Sub PlayerManager() 'Ausgabe der animierten Spielfigur an der entsprechenden Position am Bildschirm
        If playerJumpHeight <> playerJumpHeightCache Then DrawPlayer(0, playerXPosition, (consoleHeight - 7) - playerJumpHeightCache)
        DrawPlayer(playerAnimationState, playerXPosition, (consoleHeight - 7) - playerJumpHeight)
        playerJumpHeightCache = playerJumpHeight
    End Sub

    'Auswahl der Bausteine für den Boden (Hindernisse und der Weg)
    Public Sub DrawArray(groundState As Integer, groundAnchorX As Integer, groundAnchorY As Integer)
        Try
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
                    WriteAt("[][]", groundAnchorX, groundAnchorY - 1)
                    WriteAt("`", groundAnchorX, groundAnchorY)
                Case 3
                    WriteAt(" []", groundAnchorX, groundAnchorY - 2)
                    WriteAt("[][]", groundAnchorX, groundAnchorY - 1)
                    WriteAt("`", groundAnchorX, groundAnchorY)
                Case 4
                    WriteAt("[]", groundAnchorX, groundAnchorY - 2)
                    WriteAt("||", groundAnchorX, groundAnchorY - 1)
                    WriteAt("`", groundAnchorX, groundAnchorY)
                Case 5
                    WriteAt(" ", groundAnchorX, groundAnchorY - 2)
                    WriteAt(" ", groundAnchorX, groundAnchorY - 1)
                    WriteAt(" ", groundAnchorX, groundAnchorY)
                Case 6
                    WriteAt("   ___", groundAnchorX, groundAnchorY - 2)
                    WriteAt(" _(   )", groundAnchorX, groundAnchorY - 1)
                    WriteAt("(___)__)", groundAnchorX, groundAnchorY)

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
    Public skyTiles As New List(Of Tile)
    Public Sub ArraySetUp() 'befüllen der Liste mit Bodenstücken beim Rundenstart
        groundTiles.Clear()
        For i = 0 To consoleWidth - 1
            groundTiles.Add(New Tile With {.tileType = 0}) 'Bestückung mit Bodenstücken des Standarttyps (kein Hindernis)
        Next
        skyTiles.Clear()
        For i = 0 To consoleWidth - 1
            skyTiles.Add(New Tile With {.tileType = 5}) 'Bestückung mit Bodenstücken des Standarttyps (kein Hindernis)
        Next
    End Sub

    Public Sub ArrayManager() 'Ausgabe der Bodenstückliste in der Konsole
        For index As Integer = groundTiles.Count - 1 To 0 Step -1
            DrawArray($"{groundTiles(index).tileType} ", index, consoleHeight - 1) 'nicht denken, maaalen
        Next
        For index As Integer = skyTiles.Count - 1 To 0 Step -1
            DrawArray($"{skyTiles(index).tileType} ", index, 5) 'nicht denken, maaalen
        Next

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
        Dim taskE = Task.Run(AddressOf CollisionManager)

        'Dim taskE = Task.Run(AddressOf SkyAnimator)

        Task.WaitAll(taskA)
    End Function

    Public Sub RenderLoop()
        While stayInLoop 'solange true, wird Spieloberfläche gerendert
            DrawStats()
            PlayerManager()
            ArrayManager()
            Threading.Thread.Sleep(1)
        End While
    End Sub
    Public Sub Timer() 'zählt ab Rundenbeginn Spielsekunden hoch
        While stayInLoop
            gameTimer += 1
            Threading.Thread.Sleep(1000)
        End While
    End Sub

    Public Sub CollisionManager()
        Dim overObstacle As Boolean = False
        While stayInLoop
            If groundTiles(6).tileType <> 0 Then 'wenn an Position des Players ein Hindernis ist, wird geprüft ob Spielfigur am Boden ist. Wenn ja, Game Over; Wenn nein, Score +1.
                If playerJumpHeight < 0.2 Then
                    playDethSound = True
                    gameOverBoolean = True
                    stayInLoop = False
                End If
                overObstacle = True
            End If
            If groundTiles(6).tileType = 0 And overObstacle = True Then
                overObstacle = False

                Debug.WriteLine("overObstacle")
                score += 1
                If isDivisible(score, 10) Then
                    playBigScoreSound = True
                    playScoreSound = True
                Else
                    playScoreSound = True
                End If
            End If
            Threading.Thread.Sleep(0.2)
        End While
    End Sub

    Public playJumpSound As Boolean = False
    Public playBackgroundSound As Boolean = False
    Public playDethSound As Boolean = False
    Public playScoreSound As Boolean = False
    Public playBigScoreSound As Boolean = False

    Public inMenu As Boolean = True
    Dim Snds As New MultiSounds

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

                groundTiles.Add(New Tile With {.tileType = CInt(Math.Floor((4 - 1 + 1) * Rnd())) + 1})
                spawnTimer = 0
            Else 'andernfalls wird Bodenstück ohne Hindernis hinzugefügt
                groundTiles.Add(New Tile With {.tileType = 0})
            End If
            Randomize() 'variiere Zeit zwischen Hindernisspawns; Zeit verringert sich über die Spielzeit
            spawnTimer = (spawnTimer + CInt(Math.Floor((randomUpperBound - randomLowerBound + 1) * Rnd())) + randomLowerBound) + (score / 20)

            Threading.Thread.Sleep(20)
        End While
    End Sub

    Public Sub SkyAnimator()
        Dim spawnTimerSky
        While stayInLoop
            skyTiles.RemoveAt(0)
            If spawnTimerSky > 100 Then
                skyTiles.Add(New Tile With {.tileType = 3})
                spawnTimerSky = 0
            Else
                skyTiles.Add(New Tile With {.tileType = 2})
            End If
            Randomize() 'variiere Zeit zwischen Hindernisspawns; Zeit verringert sich über die Spielzeit
            spawnTimerSky = (spawnTimerSky + CInt(Math.Floor((randomUpperBound - randomLowerBound + 1) * Rnd())) + randomLowerBound) + (score / 20)
            Threading.Thread.Sleep(60)
        End While
    End Sub

    Public Function AsyncLoopGame() As Task   'Starten von verschiedenen Asynchronen Schleifen/Prozessen
        Dim task1 = Task.Run(AddressOf KeyImput)
        Dim task2 = Task.Run(AddressOf SoundManager)
    End Function

    Dim initiateSounds As Boolean = True
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
        Dim consoleKey As ConsoleKeyInfo
        While True
            consoleKey = Console.ReadKey()
            If (consoleKey.Key = 32) <> 0 And stayInLoop Then   'Key32=Lertaste; wenn Leertaste gedrückt wird: setzen der Sprungvariable auf True
                playerJump = True
            End If
            If (consoleKey.Key = 32) <> 0 And stayInLoop = False Then
                menuConfirm = True
            End If
            If (consoleKey.Key = 13) <> 0 And stayInLoop = False Then
                menuConfirm = True
            End If
            If (consoleKey.Key = 38) <> 0 And stayInLoop = False Then
                If menuCursorPosition > 0 Then menuCursorPosition -= 1
            End If
            If (consoleKey.Key = 40) <> 0 And stayInLoop = False Then
                If menuCursorPosition < 3 Then menuCursorPosition += 1
            End If
            If (consoleKey.Modifiers And ConsoleModifiers.Control And consoleKey.Key = 67) <> 0 Then stayInLoop = False 'Abfrage bei Tastenkombination "Strg C" beenden der Runde
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
        ArraySetUp()
        AsyncLoopRound()
    End Sub

    Public Sub SaveSettings(ByVal spielZurücksetzen As Boolean)
        Try
            System.IO.File.WriteAllText("musicsettings.txt", spielZurücksetzen)
        Catch ex As Exception
            Debug.WriteLine("musicsettings.txt could not be reached")
        End Try
    End Sub

    Public Function LoadSettings() As Boolean
        Dim line As Boolean = True
        Try
            Using reader As New StreamReader("musicsettings.txt")
                line = Convert.ToBoolean(reader.ReadLine())
            End Using
        Catch ex As Exception
            Debug.WriteLine("musicsettings.txt could not be reached")
        End Try
        Return line
    End Function

    Public Sub GameOver() 'wird ausgeführt bei Berührung der Spielfigur mit einem Hindernis; Ausgeben des Game-Over Bildschirms
        'My.Computer.Audio.Play("23DB05PJ_sfxDeath_v1.01(1).wav", AudioPlayMode.Background) Sound Tod
        If musicEnabled Then
            gamemusicSound.StopPlaying()
            menuloopSound.PlayLoop()
        End If

        If score > highScore Then 'Highscore wird ggf. gesetzt; Überprüfen ob Score der aktuellen Runde höher als Highscore ist
            highScore = score
            WriteAt("New Highscore", x:=0, y:=-9, CenterHorizontally:=True, CenterVertically:=True)
        End If
    End Sub

    Public Sub HowToPlay()
        Console.Clear()
        WriteAt("   __ __             ______       ___  __         ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("  / // /__ _    __  /_  __/__    / _ \/ /__ ___ __", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / _  / _ \ |/|/ /   / / / _ \  / ___/ / _ `/ // /", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/_//_/\___/__,__/   /_/  \___/ /_/  /_/\_,_/\_, / ", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("                                           /___/  ", x:=0, y:=-4, CenterHorizontally:=True, CenterVertically:=True)
        While menuConfirm = False
            Console.ForegroundColor = ConsoleColor.Blue
            WriteAt("> Back", 3, 1)
            Console.ForegroundColor = ConsoleColor.White
            WriteAt("Drücke Leertaste um zu springen und Hindernissen auszuweichen", x:=0, y:=0, CenterHorizontally:=True, CenterVertically:=True)
            WriteAt("Ziel ist es eine möglichst große Strecke zurückzulegen ohne ein Hinderniss zu berühren.", x:=0, y:=1, CenterHorizontally:=True, CenterVertically:=True)
        End While
        menuConfirm = False
        Console.Clear()
    End Sub

    Public Sub Credits()
        Console.Clear()
        WriteAt("  _____           ___ __    ", x:=0, y:=-8, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt(" / ___/______ ___/ (_) /____", x:=0, y:=-7, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("/ /__/ __/ -_) _  / / __(_-<", x:=0, y:=-6, CenterHorizontally:=True, CenterVertically:=True)
        WriteAt("\___/_/  \__/\_,_/_/\__/___/", x:=0, y:=-5, CenterHorizontally:=True, CenterVertically:=True)
        While menuConfirm = False
            Console.ForegroundColor = ConsoleColor.Blue
            WriteAt("> Back", 3, 1)
            Console.ForegroundColor = ConsoleColor.White
            WriteAt("TWE-22", (consoleWidth / 2) - 8, (consoleHeight / 2) - 3)
            WriteAt("Philipp Brocher", (consoleWidth / 2) - 8, (consoleHeight / 2) - 1)
            WriteAt("Erik Siegel", (consoleWidth / 2) - 8, (consoleHeight / 2))
            WriteAt("Linus von Maltzan", (consoleWidth / 2) - 8, (consoleHeight / 2) + 1)
            WriteAt("Musik: Pochers Dude", (consoleWidth / 2) - 8, (consoleHeight / 2) + 2)
        End While
        menuConfirm = False
        Console.Clear()
    End Sub


    Public musicEnabled As Boolean = LoadSettings()
    Public Sub ToggleAudio()
        If musicEnabled Then
            musicEnabled = False
            SaveSettings(False)
            menuloopSound.StopPlaying()
        Else
            musicEnabled = True
            SaveSettings(True)
            menuloopSound.PlayLoop()
        End If
    End Sub


    Dim menuloopSound As New MciPlayer("menuloop.mp3", "1")
    Dim gamemusicSound As New MciPlayer("music.mp3", "2")

    Dim menuCursorPosition As Integer = 0
    Dim menuConfirm As Boolean = False
    Public Sub MenuLoop()
        If musicEnabled Then menuloopSound.PlayLoop()
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

            If gameOverBoolean Then
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

            If menuCursorPosition = 0 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (consoleWidth / 2) - 7, (consoleHeight / 2) - 2)
                If gameOverBoolean Then
                    WriteAt("Play Again   ", (consoleWidth / 2) - 5, (consoleHeight / 2) - 2)
                Else
                    WriteAt("Play   ", (consoleWidth / 2) - 5, (consoleHeight / 2) - 2)
                End If
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (consoleWidth / 2) - 7, (consoleHeight / 2) - 2)
                If gameOverBoolean Then
                    WriteAt("Play Again   ", (consoleWidth / 2) - 5, (consoleHeight / 2) - 2)
                Else
                    WriteAt("Play   ", (consoleWidth / 2) - 5, (consoleHeight / 2) - 2)
                End If
            End If
            If menuCursorPosition = 1 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (consoleWidth / 2) - 7, (consoleHeight / 2))
                WriteAt("How to Play   ", (consoleWidth / 2) - 5, (consoleHeight / 2))
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (consoleWidth / 2) - 7, (consoleHeight / 2))
                WriteAt("How to Play   ", (consoleWidth / 2) - 5, (consoleHeight / 2))
            End If
            If menuCursorPosition = 2 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (consoleWidth / 2) - 7, (consoleHeight / 2) + 1)
                If musicEnabled Then
                    WriteAt("Music Enabled   ", (consoleWidth / 2) - 5, (consoleHeight / 2) + 1)
                Else
                    WriteAt("Music Disabled   ", (consoleWidth / 2) - 5, (consoleHeight / 2) + 1)
                End If
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (consoleWidth / 2) - 7, (consoleHeight / 2) + 1)
                If musicEnabled Then
                    WriteAt("Music Enabled   ", (consoleWidth / 2) - 5, (consoleHeight / 2) + 1)
                Else
                    WriteAt("Music Disabled   ", (consoleWidth / 2) - 5, (consoleHeight / 2) + 1)
                End If
            End If
            If menuCursorPosition = 3 Then
                Console.ForegroundColor = ConsoleColor.Blue
                WriteAt("> ", (consoleWidth / 2) - 7, (consoleHeight / 2) + 2)
                WriteAt("Credits   ", (consoleWidth / 2) - 5, (consoleHeight / 2) + 2)
                Console.ForegroundColor = ConsoleColor.White
            Else
                WriteAt("  ", (consoleWidth / 2) - 7, (consoleHeight / 2) + 2)
                WriteAt("Credits   ", (consoleWidth / 2) - 5, (consoleHeight / 2) + 2)
            End If
        End While
    End Sub

    Public consoleWidth As Integer = 120
    Public consoleHeight As Integer = 22

    Public Sub Main()
        ConsoleSetup(consoleWidth, consoleHeight)
        AsyncLoopGame()
        MenuLoop()
        Console.ReadKey()
    End Sub

End Module
