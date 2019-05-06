Imports WindowsInput
Imports WindowsInput.Native

Public Class XboxController
    Public Enum Direction
        Up = 1
        Down = 2
        Left = 3
        Right = 4
    End Enum

    Public Enum Button
        A = 1
        B = 2
        X = 3
        Y = 4
    End Enum

    Private Const AButtonKey As VirtualKeyCode = VirtualKeyCode.VK_J
    Private Const BButtonKey As VirtualKeyCode = VirtualKeyCode.VK_K
    Private Const XButtonKey As VirtualKeyCode = VirtualKeyCode.VK_U
    Private Const YButtonKey As VirtualKeyCode = VirtualKeyCode.VK_I
    Private Const InputDelay As Integer = 50 ' The xbox games cannot take inputs too quickly. Must also make up for network latency.
    Private Const MoveDelay As Integer = 250 ' Makes up for network latency.

    Public Sub PressButton(button As Button)
        Dim keyCode As VirtualKeyCode
        Select Case button
            Case XboxController.Button.A
                keyCode = AButtonKey
            Case XboxController.Button.B
                keyCode = BButtonKey
            Case XboxController.Button.X
                keyCode = XButtonKey
            Case XboxController.Button.Y
                keyCode = YButtonKey
        End Select

        Threading.Thread.Sleep(InputDelay)
        MainForm.InputSimulator.Keyboard.KeyDown(keyCode)
        Threading.Thread.Sleep(InputDelay)
        MainForm.InputSimulator.Keyboard.KeyUp(keyCode)
    End Sub

    Public Sub Move(direction As Direction, time As Integer)
        Dim keyCode As WindowsInput.Native.VirtualKeyCode
        Select Case direction
            Case SuperBomberman.Direction.Up
                keyCode = VirtualKeyCode.UP
            Case SuperBomberman.Direction.Down
                keyCode = VirtualKeyCode.DOWN
            Case SuperBomberman.Direction.Left
                keyCode = VirtualKeyCode.LEFT
            Case SuperBomberman.Direction.Right
                keyCode = VirtualKeyCode.RIGHT
        End Select

        Dim timeSplitCount As Integer = Math.Floor(time / MoveDelay)
        For i As Integer = 0 To timeSplitCount
            Threading.Thread.Sleep(InputDelay)
            MainForm.InputSimulator.Keyboard.KeyDown(keyCode)
            Threading.Thread.Sleep(MoveDelay)
            MainForm.InputSimulator.Keyboard.KeyUp(keyCode)
        Next
    End Sub
End Class
