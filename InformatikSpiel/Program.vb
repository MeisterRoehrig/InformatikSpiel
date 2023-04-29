Imports System
Imports System.Net.NetworkInformation
Imports System.Runtime.CompilerServices

Module Program

    Dim origRow, origCol As Integer
    Public Sub WriteAt(s As String, x As Integer, y As Integer)
        Try
            Console.SetCursorPosition(origCol + x, origRow + y)
            Console.Write(s)
        Catch e As ArgumentOutOfRangeException
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    ' Quelle [https://stackoverflow.com/a/4985870/13324025]
    Function isDivisible(x As Integer, d As Integer) As Boolean
        Return (x Mod d) = 0
    End Function


    Public Sub ConsoleSetup(consoleWidth As Integer, consoleHeight As Integer)
        Console.SetWindowSize(consoleWidth, consoleHeight)
        Console.SetBufferSize(consoleWidth, consoleHeight)
        Console.CursorVisible = False
        Console.TreatControlCAsInput = True
    End Sub

    Public gameTimer As Integer = 0
    Public gameTimerCache As Integer = 0
    Public fps As Double = 0
    Public score As Integer = 0
    Public highScore As Integer
    Public Sub DrawStats()
        WriteAt("Timer:" & gameTimer, 1, 1)
        If gameTimer > gameTimerCache Then
            WriteAt("FPS:" & fps, 1, 2)
            fps = 0
        End If
        fps += 1
        gameTimerCache = gameTimer
        WriteAt("Score:" & score, 1, 0)
    End Sub

    Public Sub DrawPlayer(playerState As Integer, playerAnchorX As Integer, playerAnchorY As Integer)
        Try
            Select Case playerState
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
        Catch e As ArgumentOutOfRangeException
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    Public playerJumpHeightCache As Integer
    Dim playerXPosition As Integer = 1
    Dim playerYPosition As Integer = 13
    Public Sub PlayerManager()
        If playerJumpHeight <> playerJumpHeightCache Then DrawPlayer(0, playerXPosition, playerYPosition - playerJumpHeightCache)
        DrawPlayer(playerAnimationState, playerXPosition, playerYPosition - playerJumpHeight)
        playerJumpHeightCache = playerJumpHeight
    End Sub

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

        Catch e As ArgumentOutOfRangeException
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    Public Class Tile
        Public Property tileType As Integer
    End Class

    Public groundTiles As New List(Of Tile)
    Public Sub GroundArraySetUp()
        groundTiles.Clear()
        For i = 0 To 99
            groundTiles.Add(New Tile With {.tileType = 0})
        Next
    End Sub

    Public Sub GroundManager()
        For index As Integer = groundTiles.Count - 1 To 0 Step -1
            DrawGround($"{groundTiles(index).tileType} ", index, 19)
        Next
        If groundTiles(6).tileType = 1 Then
            If playerJumpHeight < 2 Then
                stayInLoop = False
            Else
                score += 1
            End If
        End If
    End Sub


    Public stayInLoop As Boolean = True
    Public playerAnimationState As Integer
    Public playerJump As Boolean
    Public playerJumpHeight As Integer
    Public Function AsyncLoop() As Task
        Dim taskA = Task.Run(AddressOf RenderLoop)
        Dim taskB = Task.Run(AddressOf Timer)
        Dim taskC = Task.Run(AddressOf KeyImput)
        Dim taskD = Task.Run(AddressOf PlayerAnimator)
        Dim taskE = Task.Run(AddressOf GroundAnimator)

        Task.WaitAll(taskA, taskB)
    End Function

    Public Sub RenderLoop()
        While stayInLoop
            DrawStats()
            PlayerManager()
            GroundManager()
            Threading.Thread.Sleep(1)
        End While
    End Sub
    Public Sub Timer()
        While stayInLoop
            gameTimer += 1
            Threading.Thread.Sleep(1000)
        End While
    End Sub
    Public Sub KeyImput()
        Dim cki As ConsoleKeyInfo
        While stayInLoop
            cki = Console.ReadKey()
            If (cki.Key = 32) <> 0 Then playerJump = True
            If (cki.Modifiers And ConsoleModifiers.Control And cki.Key = 67) <> 0 Then stayInLoop = False
        End While
    End Sub

    Public animateJump As Boolean = False
    Public Sub PlayerAnimator()
        While stayInLoop
            If playerJumpHeight < 1 And playerJump Then
                animateJump = True
                playerJump = False
            End If
            If animateJump Then
                If playerJumpHeight < 6 Then playerJumpHeight += 2
            Else
                If playerJumpHeight > 0 Then playerJumpHeight -= 1
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
    Public spawnTimer As Integer
    Dim randomLowerBound As Integer = 0
    Dim randomUpperBound As Integer = 5
    Public Sub GroundAnimator()
        While stayInLoop
            spawnTimer += 1
            groundTiles.RemoveAt(0)
            If spawnTimer > 150 Then
                groundTiles.Add(New Tile With {.tileType = 1})
                spawnTimer = 0
            Else
                groundTiles.Add(New Tile With {.tileType = 0})
            End If
            Randomize()
            spawnTimer = spawnTimer + CInt(Math.Floor((randomUpperBound - randomLowerBound + 1) * Rnd())) + randomLowerBound

            Threading.Thread.Sleep(17)
        End While
    End Sub


    Public Sub GameStart()
        Console.Clear()
        stayInLoop = True
        playerAnimationState = 0
        playerJump = False
        animateJump = False
        playerJumpHeight = 0
        playerJumpHeightCache = 0
        spawnTimer = 0
        gameTimer = 0
        gameTimerCache = 0
        fps = 0
        score = 0
        GroundArraySetUp()
        AsyncLoop()
    End Sub

    Public Sub GameOver()
        Console.ForegroundColor = ConsoleColor.DarkRed
        Beep()
        WriteAt("Game Over", 45, 8)
        Console.ForegroundColor = ConsoleColor.White
        If score > highScore Then
            highScore = score
            WriteAt("New Highscore", 43, 9)
        End If
        WriteAt("Double tap [Space] to Restart", 35, 11)

        'Quelle [https://stackoverflow.com/a/44519918/13324025]
        Dim kp As ConsoleKeyInfo
        Do
            If Console.KeyAvailable Then
                kp = Console.ReadKey(True)
                If kp.Key = 32 Then
                    Exit Do
                End If
            End If
            Threading.Thread.Sleep(100)
        Loop While True
        GameStart()
    End Sub

    Public Sub GameLoop()
        GameStart()
        While True
            GameOver()
        End While
    End Sub

    Public Sub Main()
        ConsoleSetup(100, 20)
        GameLoop()
    End Sub
End Module

