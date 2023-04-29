Imports System

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

    Public Sub DrawPlayer(playerState As Integer, playerAnchorX As Integer, playerAnchorY As Integer)
        Try
            Select Case playerState
                Case 0
                    WriteAt("@", playerAnchorX + 1, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 2, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 3, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 4, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 5, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 6, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 7, playerAnchorY + 1)
                    WriteAt("@", playerAnchorX + 8, playerAnchorY + 1)

                    WriteAt("@", playerAnchorX + 1, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 2, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 3, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 4, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 5, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 6, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 7, playerAnchorY + 2)
                    WriteAt("@", playerAnchorX + 8, playerAnchorY + 2)

                    WriteAt("@", playerAnchorX + 1, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 2, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 3, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 4, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 5, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 6, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 7, playerAnchorY + 3)
                    WriteAt("@", playerAnchorX + 8, playerAnchorY + 3)

                    WriteAt("@", playerAnchorX + 1, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 2, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 3, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 4, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 5, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 6, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 7, playerAnchorY + 4)
                    WriteAt("@", playerAnchorX + 8, playerAnchorY + 4)

                    WriteAt("@", playerAnchorX + 1, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 2, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 3, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 4, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 5, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 6, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 7, playerAnchorY + 5)
                    WriteAt("@", playerAnchorX + 8, playerAnchorY + 5)
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

    Public Sub DrawGround()
        Try
            For i = 0 To 99
                WriteAt("_", i, 19)
            Next

        Catch e As ArgumentOutOfRangeException
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    Public gameTimer As Integer = 0
    Public gameTimerCache As Integer = 0
    Public fps As Double = 0
    Public Sub DrawTimer()
        WriteAt("Timer:" & gameTimer, 0, 0)
        If gameTimer > gameTimerCache Then
            WriteAt("FPS:" & fps, 0, 1)
            fps = 0
        End If
        fps += 1
        gameTimerCache = gameTimer
    End Sub

    Public Sub ConsoleSetup()
        Dim width, height As Integer
        width = 100
        height = 20
        Console.SetWindowSize(width, height)
        Console.CursorVisible = False
        Console.TreatControlCAsInput = True
    End Sub

    Public Sub PlayerManager()


    End Sub

    Public stayInLoop As Boolean = True
    Public playerAnimationState As Integer
    Public Function AsyncLoop() As Task
        Dim taskA = Task.Run(AddressOf RenderLoop)
        Dim taskB = Task.Run(AddressOf Timer)
        Dim taskC = Task.Run(AddressOf KeyImput)
        Dim taskD = Task.Run(AddressOf PlayerAnimator)

        Task.WaitAll(taskA, taskB)
    End Function

    Public Sub RenderLoop()
        While stayInLoop
            DrawGround()
            DrawTimer()
            DrawPlayer(playerAnimationState, 0, 14)
            Threading.Thread.Sleep(30)
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
            If (cki.Modifiers And ConsoleModifiers.Shift) <> 0 Then

            End If
            If (cki.Modifiers And ConsoleModifiers.Control And cki.Key = 67) <> 0 Then
                stayInLoop = False
            End If
        End While
    End Sub

    Public Sub PlayerAnimator()
        While stayInLoop
            If playerAnimationState = 7 Then
                playerAnimationState = 1
            Else
                playerAnimationState += 1
            End If
            Threading.Thread.Sleep(60)
        End While
    End Sub


    Public Sub Main()
        ConsoleSetup()
        AsyncLoop()
    End Sub
End Module



