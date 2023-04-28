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

    Public Sub DrawPlayer(playerAnchorX As Integer, playerAnchorY As Integer)
        Try
            WriteAt("_", playerAnchorX + 3, playerAnchorY + 0)
            WriteAt("_", playerAnchorX + 4, playerAnchorY + 0)
            WriteAt("O", playerAnchorX + 5, playerAnchorY + 0)
            WriteAt("/", playerAnchorX + 2, playerAnchorY + 1)
            WriteAt("/", playerAnchorX + 4, playerAnchorY + 1)
            WriteAt("\", playerAnchorX + 5, playerAnchorY + 1)
            WriteAt("_", playerAnchorX + 6, playerAnchorY + 1)
            WriteAt(",", playerAnchorX + 7, playerAnchorY + 1)
            WriteAt("_", playerAnchorX + 0, playerAnchorY + 2)
            WriteAt("_", playerAnchorX + 1, playerAnchorY + 2)
            WriteAt("_", playerAnchorX + 2, playerAnchorY + 2)
            WriteAt("/", playerAnchorX + 3, playerAnchorY + 2)
            WriteAt("\", playerAnchorX + 4, playerAnchorY + 2)
            WriteAt("/", playerAnchorX + 4, playerAnchorY + 3)
            WriteAt("_", playerAnchorX + 5, playerAnchorY + 3)


        Catch e As ArgumentOutOfRangeException
            Console.Clear()
            Console.WriteLine(e.Message)
        End Try
    End Sub

    Public Sub ConsoleSetup()
        Dim width, height As Integer
        width = 100
        height = 20
        Console.SetWindowSize(width, height)
    End Sub

    Public Sub Main()
        ConsoleSetup()
        DrawPlayer(1, 2)
    End Sub

End Module
