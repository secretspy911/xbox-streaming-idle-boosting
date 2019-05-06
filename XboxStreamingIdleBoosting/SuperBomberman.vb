Imports WindowsInput
Imports WindowsInput.Native

Public Class SuperBomberman
    Public Enum Direction
        Up = 1
        Down = 2
        Left = 3
        Right = 4
    End Enum

    Private Const MoveOneSquareDelay As Integer = 225
    Private Const KeyPressDelay As Integer = 500
    Private Const BombDetonationDelay = 2500
    Private Const PlaceBombDelay = 1 ' The game needs a delay when getting on a square before being able to place a bomb

    ''' <summary>
    ''' For this to work, the Xbox App game must be at the main menu.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BoostBlocks()
        PlayMatch()
    End Sub

    Private Sub Move(direction As Direction, nbSquares As Integer)
        MainForm.XboxController.Move(direction, nbSquares * MoveOneSquareDelay)
    End Sub

    Private Sub PlayMatch()
        ' 1
        Move(Direction.Right, 1)
        PlaceBomb() '1,2
        Move(Direction.Left, 1)
        Move(Direction.Down, 1)
        WaitForBomb() '2,1 

        ' 2
        Move(Direction.Up, 1)
        Move(Direction.Right, 2)
        PlaceBomb() ' 1,3
        Move(Direction.Left, 2)
        Move(Direction.Down, 1)
        WaitForBomb() '2,1

        ' 3
        Move(Direction.Up, 1)
        Move(Direction.Right, 3)
        PlaceBomb() '1,4
        Move(Direction.Left, 1)
        Move(Direction.Down, 1)
        WaitForBomb() '2,3

        ' 4
        PlaceBomb() '2,3
        Move(Direction.Up, 1)
        Move(Direction.Right, 2)
        WaitForBomb() '1,5

        ' 5
        PlaceBomb() '1,5
        Move(Direction.Left, 2)
        Move(Direction.Down, 2)
        WaitForBomb() '3,3

        ' 6
        PlaceBomb() '3,3
        Move(Direction.Up, 2)
        Move(Direction.Left, 2)
        Move(Direction.Down, 1)
        WaitForBomb() '2,1

        ' 7
        'PlaceBomb() '2,1
        'Move(Direction.Down, 1)
        'Move(Direction.Right, 4)
        'PlaceBomb()
        'Move(Direction.Left, 2)
        'Move(Direction.Down, 2)
        'WaitForBomb()

        '' 8
        'PlaceBomb()
        'Move(Direction.Up, 2)
        'Move(Direction.Left, 1)
        'WaitForBomb()

    End Sub

    Private Sub PlaceBomb()
        MainForm.XboxController.PressButton(XboxController.Button.B)
    End Sub

    Private Sub WaitForBomb()
        Threading.Thread.Sleep(BombDetonationDelay)
    End Sub
End Class
