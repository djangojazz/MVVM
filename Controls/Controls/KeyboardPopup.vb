Imports System.Windows.Input

Public Class KeyboardPopup
  Inherits Windows.Controls.Primitives.Popup

  Protected Overrides Sub OnOpened(e As EventArgs)
    MyBase.OnOpened(e)

    'MOVE FOCUS INTO POPUP WHEN OPENED
    Me.Child.MoveFocus(New TraversalRequest(FocusNavigationDirection.Next))
  End Sub

End Class
