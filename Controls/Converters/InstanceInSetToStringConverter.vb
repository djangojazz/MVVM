Imports System.Globalization
Imports System.Collections.ObjectModel


Public Class InstanceInSetToStringConverter
  Inherits Control
  Implements IValueConverter

  Public Property FirstPosition As Integer = 0
  Public Property OptionalHeader As String = String.Empty


  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    If Not IsNumeric(value) Or FirstPosition <= 0 Then Return String.Empty
    Dim val = CDbl(value)

    Dim final = If(val = FirstPosition, "Current", CInt(val - FirstPosition + 1).DisplayNumberWithStringSuffix)

    Return $"{final} {OptionalHeader}"
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Throw New NotImplementedException()
  End Function
End Class
