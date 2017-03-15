Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization.Formatters.Binary

Public Module ExtensionMethods

  <Extension>
  Public Sub ClearAndAddRange(Of T)(input As IList(Of T), array As IEnumerable(Of T))
    input.Clear()
    For Each o In array
      input.Add(o)
    Next
  End Sub

  <Extension>
  Public Sub ClearAndAddRange(Of T)(input As ObservableCollectionContentNotifying(Of T), array As IEnumerable(Of T))
    input.SuspendNotification = True
    input.Clear()
    'For i = 0 To array.Count - 1
    '  If i = (array.Count - 1) Then input.SuspendNotification = False
    '  input.Add(array(i))
    'Next
    For Each o In array
      input.Add(o)
    Next
    input.SuspendNotification = False
  End Sub


  <Extension>
  Public Sub ClearAndAddRange(Of T, T2)(input As IDictionary(Of T, T2), dict As IDictionary(Of T, T2))
    input.Clear()
    For Each o In dict
      input.Add(o)
    Next
  End Sub

  <Extension>
  Public Function ContainsInvariant(input As String, contains As String) As Boolean
    Return input.ToUpper.Trim.Contains(contains.ToUpper.Trim)
  End Function

  <Extension>
  Public Function DisplayNumberWithStringSuffix(inputNumber As Integer) As String
    If inputNumber.ToString().EndsWith("11") Or inputNumber.ToString().EndsWith("12") Or inputNumber.ToString().EndsWith("13") Then Return inputNumber.ToString() + "th"
    If (inputNumber.ToString().EndsWith("1")) Then Return inputNumber.ToString() + "st"
    If (inputNumber.ToString().EndsWith("2")) Then Return inputNumber.ToString() + "nd"
    If (inputNumber.ToString().EndsWith("3")) Then Return inputNumber.ToString() + "rd"
    Return inputNumber.ToString() + "th"
  End Function

  <Extension>
  Public Function DeepClone(Of T)(obj As T) As T
    Dim Formater = New BinaryFormatter
    Dim m As New System.IO.MemoryStream
    Formater.Serialize(m, obj)
    m.Seek(0, IO.SeekOrigin.Begin)
    Return CType(Formater.Deserialize(m), T)
  End Function

End Module
