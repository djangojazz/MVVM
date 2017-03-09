Imports System.Globalization
Imports System.Collections.ObjectModel

Public NotInheritable Class DecimalConverter
  Inherits Control
  Implements IValueConverter

  Public Property DecimalPositions As Integer = 0
  Public Property IncludeComma As Boolean = True
  Public Property OptionalHeader As String = String.Empty


  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    If Not IsNumeric(value) Then Return String.Empty

    Dim FormatString As String = If(IncludeComma, "#,##0", "0")
    If DecimalPositions > 0 Then FormatString &= "." & StrDup(DecimalPositions, "0"c)

    Return $"{OptionalHeader} {CType(value, Decimal).ToString(FormatString)}"

  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Throw New NotImplementedException()
  End Function
End Class
