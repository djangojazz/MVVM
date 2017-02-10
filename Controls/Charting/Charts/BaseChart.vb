Public Class BaseChart
  Inherits UserControl

#Region "Dependent Properties"


#Region "ChartTitle"
  Public Shared ReadOnly ChartTitleProperty As DependencyProperty = DependencyProperty.Register("ChartTitle", GetType(String), GetType(LineChart), New UIPropertyMetadata(String.Empty))

  Public Property ChartTitle As String
    Get
      Return CType(GetValue(ChartTitleProperty), String)
    End Get
    Set
      SetValue(ChartTitleProperty, Value)
    End Set
  End Property
#End Region

#Region "ChartForeground"
  Public Shared ReadOnly ChartForegroundProperty As DependencyProperty = DependencyProperty.Register("ChartForeground", GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

  Public Property ChartForeground As Brush
    Get
      Return DirectCast(GetValue(ChartForegroundProperty), Brush)
    End Get
    Set
      SetValue(ChartForegroundProperty, Value)
    End Set
  End Property
#End Region

#Region "ChartTitleHidden"
  Public Shared ReadOnly ChartTitleHiddenProperty As DependencyProperty = DependencyProperty.Register("ChartTitleHidden", GetType(Boolean), GetType(LineChart), New UIPropertyMetadata(False, AddressOf LineChart.ChartTitleHiddenChanged))

  Public Property ChartTitleHidden As Boolean
    Get
      Return DirectCast(GetValue(ChartTitleHiddenProperty), Boolean)
    End Get
    Set
      SetValue(ChartTitleHiddenProperty, Value)
    End Set
  End Property

  Public Shared Sub ChartTitleHiddenChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim LC As LineChart = DirectCast(d, LineChart)

    If CBool(e.NewValue) Then
      LC.PART_ChartTitle.Visibility = Visibility.Collapsed
    Else
      LC.PART_ChartTitle.Visibility = Visibility.Visible
    End If

  End Sub
#End Region

#Region "BackGroundColor"
  Public Shared ReadOnly BackGroundColorProperty As DependencyProperty = DependencyProperty.Register("BackGroundColor", GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

  Public Property BackGroundColor As Brush
    Get
      Return DirectCast(GetValue(BackGroundColorProperty), Brush)
    End Get
    Set
      SetValue(BackGroundColorProperty, Value)
    End Set
  End Property
#End Region

#Region "BackGroundColorCanvas"
  Public Shared ReadOnly BackGroundColorCanvasProperty As DependencyProperty = DependencyProperty.Register("BackGroundColorCanvas", GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

  Public Property BackGroundColorCanvas As Brush
    Get
      Return DirectCast(GetValue(BackGroundColorCanvasProperty), Brush)
    End Get
    Set
      SetValue(BackGroundColorCanvasProperty, Value)
    End Set
  End Property
#End Region

#Region "BackGroundColorLegend"
  Public Shared ReadOnly BackGroundColorLegendProperty As DependencyProperty = DependencyProperty.Register("BackGroundColorLegend", GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

  Public Property BackGroundColorLegend As Brush
    Get
      Return DirectCast(GetValue(BackGroundColorLegendProperty), Brush)
    End Get
    Set
      SetValue(BackGroundColorLegendProperty, Value)
    End Set
  End Property
#End Region



#Region "LegendForeground"
  Public Shared ReadOnly LegendForegroundProperty As DependencyProperty = DependencyProperty.Register("LegendForeground", GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black, AddressOf LineChart.ChartLegendTextChanged))

  Public Property LegendForeground As Brush
    Get
      Return DirectCast(GetValue(LegendForegroundProperty), Brush)
    End Get
    Set
      SetValue(LegendForegroundProperty, Value)
    End Set
  End Property

  Public Shared Sub ChartLegendTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim LC As LineChart = DirectCast(d, LineChart)
    LC.PART_LEGENDTEXT.Visibility = Visibility.Visible
  End Sub
#End Region

#Region "LegendHidden"
  Public Shared ReadOnly LegendHiddenProperty As DependencyProperty = DependencyProperty.Register("LegendHidden", GetType(Boolean), GetType(LineChart), New UIPropertyMetadata(False, AddressOf LineChart.ChartLegendHiddenChanged))

  Public Property LegendHidden As Boolean
    Get
      Return DirectCast(GetValue(LegendHiddenProperty), Boolean)
    End Get
    Set
      SetValue(LegendHiddenProperty, Value)
    End Set
  End Property

  Public Shared Sub ChartLegendHiddenChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim LC As LineChart = DirectCast(d, LineChart)

    If CBool(e.NewValue) Then
      LC.PART_LEGEND.Visibility = Visibility.Collapsed
      LC.PART_LEGENDTEXT.Visibility = Visibility.Collapsed
    Else
      LC.PART_LEGENDTEXT.Visibility = Visibility.Visible
      LC.PART_LEGEND.Visibility = Visibility.Visible
    End If

  End Sub
#End Region



#Region "NumberOfTicks"
  Public Shared ReadOnly NumberOfTicksProperty As DependencyProperty = DependencyProperty.Register("NumberOfTicks", GetType(Integer), GetType(LineChart), New UIPropertyMetadata(0))

  Public Property NumberOfTicks As Integer
    Get
      Return DirectCast(GetValue(NumberOfTicksProperty), Integer)
    End Get
    Set
      SetValue(NumberOfTicksProperty, Value)
    End Set
  End Property
#End Region

#Region "XValueConverter"
  Public Shared ReadOnly XValueConverterProperty As DependencyProperty = DependencyProperty.Register("XValueConverter", GetType(IValueConverter), GetType(LineChart), Nothing)

  Public Property XValueConverter As IValueConverter
    Get
      Return CType(GetValue(XValueConverterProperty), IValueConverter)
    End Get
    Set
      SetValue(XValueConverterProperty, Value)
    End Set
  End Property
#End Region

#Region "YValueConverter"
  Public Shared ReadOnly YValueConverterProperty As DependencyProperty = DependencyProperty.Register("YValueConverter", GetType(IValueConverter), GetType(LineChart), Nothing)

  Public Property YValueConverter As IValueConverter
    Get
      Return CType(GetValue(YValueConverterProperty), IValueConverter)
    End Get
    Set
      SetValue(YValueConverterProperty, Value)
    End Set
  End Property
#End Region

#Region "FontType"
  Public Shared ReadOnly FontTypeProperty As DependencyProperty = DependencyProperty.Register("FontType", GetType(FontFamily), GetType(LineChart), Nothing)

  Public Property FontType As FontFamily
    Get
      Return CType(GetValue(FontTypeProperty), FontFamily)
    End Get
    Set
      SetValue(FontTypeProperty, Value)
    End Set
  End Property
#End Region
#End Region
End Class
