Imports System.Globalization

Public Class DateFormatConverter
  Inherits Control
  Implements IValueConverter

  Public Property DateFormat As String = "MM/dd/yyyy"

  Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
    Dim dt = DirectCast(value, DateTime)

    Return dt.ToString(DateFormat)
  End Function

  Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
    Throw New NotImplementedException()
  End Function
End Class
