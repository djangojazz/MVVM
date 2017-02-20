Imports System.Globalization
Imports System.Collections.ObjectModel

Public NotInheritable Class ChartAdditionalInfoAndPointMultiConverter
  Inherits Control
  Implements IMultiValueConverter

  Public Property DecimalPositions As Integer = 2
  Public Property IncludeComma As Boolean = True

  Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert

    'If values.Count <> 2 Then Return 0

    'Return values(0) + " " + values(1)
  End Function

  Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
    Throw New NotImplementedException()
  End Function

End Class
