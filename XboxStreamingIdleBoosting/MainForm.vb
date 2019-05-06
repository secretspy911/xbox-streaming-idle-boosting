Imports WindowsInput
Imports WindowsInput.Native

Public Class MainForm

#Region " Enums "
    ' https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-showwindow
    Private Enum ShowWindowCmd
        Hide = 0
        ShowNormal = 1
        ShowMinimize = 2
        ShowMaximize = 3 ' Or ActivatedMaximized
        ShowNoActivate = 4
        Show = 5
        Minimize = 6
        ShowMinNoActive = 7
        ShowNA = 8
        Restore = 9
        ShowDefault = 10
        ForceMinimize = 11
    End Enum
#End Region

#Region " WinAPI "
    Private Declare Function SetForegroundWindow Lib "USER32.DLL" (hWnd As IntPtr) As Boolean
    Private Declare Function ShowWindow Lib "USER32.DLL" (hWnd As IntPtr, nCmdShow As Integer) As Boolean
    Private Declare Function FindWindowA Lib "USER32.DLL" (lpClassName As String, lpWindowName As String) As IntPtr
#End Region

#Region " Singleton "
    Private Shared _inputSimulator As InputSimulator
    Public Shared ReadOnly Property InputSimulator As InputSimulator
        Get
            If _inputSimulator Is Nothing Then
                _inputSimulator = New InputSimulator
            End If

            Return _inputSimulator
        End Get
    End Property

    Private Shared _xboxController As XboxController
    Public Shared ReadOnly Property XboxController As XboxController
        Get
            If _xboxController Is Nothing Then
                _xboxController = New XboxController
            End If

            Return _xboxController
        End Get
    End Property
#End Region

#Region " Forms / Controls "
    Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
        If FocusXboxApp() Then
            Threading.Thread.Sleep(500) ' Let time for the application to focus and be ready to receive inputs
            Dim superBomberMan As New SuperBomberman()
            superBomberMan.BoostBlocks()
        End If
    End Sub
#End Region

#Region " Misc "
    ''' <summary>
    ''' The Xbox App must already be streaming and the game be at the right place to begin receiving inputs
    ''' </summary>
    Private Function FocusXboxApp() As Boolean
        Dim handle As IntPtr = FindWindowA(Nothing, "Xbox")
        If handle = 0 Then
            MessageBox.Show("L'application Xbox n'est pas démarrée.")
            Return False
        Else
            ShowWindow(handle, ShowWindowCmd.ShowNormal)
            SetForegroundWindow(handle)
            Return True
        End If
    End Function
#End Region

End Class
