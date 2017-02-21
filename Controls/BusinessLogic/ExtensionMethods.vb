Imports System.Runtime.CompilerServices

Public Module ExtensionMethods

  <Extension>
  Public Sub ClearAndAddRange(Of T)(input As IList(Of T), array As IEnumerable(Of T))
    input.Clear()
    For Each o In array
      input.Add(o)
    Next
  End Sub

  <Extension>
  Public Function DisplayNumberWithStringSuffix(inputNumber As Integer) As String
    If inputNumber.ToString().EndsWith("11") Or inputNumber.ToString().EndsWith("12") Or inputNumber.ToString().EndsWith("13") Then Return inputNumber.ToString() + "th"
    If (inputNumber.ToString().EndsWith("1")) Then Return inputNumber.ToString() + "st"
    If (inputNumber.ToString().EndsWith("2")) Then Return inputNumber.ToString() + "nd"
    If (inputNumber.ToString().EndsWith("3")) Then Return inputNumber.ToString() + "rd"
    Return inputNumber.ToString() + "th"
  End Function

End Module
