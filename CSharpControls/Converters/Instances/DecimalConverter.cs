using Microsoft.VisualBasic;
using System;                  
using System.Globalization; 
using System.Windows.Controls;
using System.Windows.Data;

namespace CSharpControls.Converters.Instances
{
  public sealed class DecimalConverter : Control, IValueConverter
  {
    public int DecimalPositions { get; set; } = 0;
    public bool IncludeComma { get; set; } = true;
    public string OptionalHeader { get; set; } = string.Empty;


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(Information.IsNumeric(value)))
        return string.Empty;

      string FormatString = IncludeComma ? "#,##0" : "0";
      if (DecimalPositions > 0)
        FormatString += "." + Strings.StrDup(DecimalPositions, '0');
                                               
      return $"{OptionalHeader} {System.Convert.ToDecimal(value).ToString(FormatString)}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
