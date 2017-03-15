Imports System.Windows.Threading

Public MustInherit Class BaseChart
  Inherits UserControl

  Private _defaultTimeSpan As New TimeSpan(1000)

  Friend Timer As New DispatcherTimer

  Public Sub New()
    Timer.Interval = _defaultTimeSpan
    FontType = If(FontType IsNot Nothing, FontType, New Windows.Media.FontFamily("Segoe UI"))
  End Sub

#Region "Dependent Properties"

#Region "ChartTitle"
  Public Shared ReadOnly ChartTitleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartTitle), GetType(String), GetType(BaseChart), New UIPropertyMetadata(String.Empty))

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
  Public Shared ReadOnly ChartForegroundProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartForeground), GetType(Brush), GetType(BaseChart), New UIPropertyMetadata(Brushes.Black))

  Public Property ChartForeground As Brush
    Get
      Return DirectCast(GetValue(ChartForegroundProperty), Brush)
    End Get
    Set
      SetValue(ChartForegroundProperty, Value)
    End Set
  End Property
#End Region

#Region "ChartTitleVisibility"
  Public Shared ReadOnly ChartTitleVisibilityProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartTitleVisibility), GetType(Visibility), GetType(BaseChart), New PropertyMetadata(Visibility.Visible))

  Public Property ChartTitleVisibility As Visibility
    Get
      Return GetValue(ChartTitleVisibilityProperty)
    End Get

    Set(ByVal value As Visibility)
      SetValue(ChartTitleVisibilityProperty, value)
    End Set
  End Property
#End Region

#Region "ChartTitleHidden"
  Public Shared ReadOnly ChartTitleHiddenProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartTitleHidden), GetType(Boolean), GetType(BaseChart), New UIPropertyMetadata(False, AddressOf ChartTitleHiddenChanged))

  Public Property ChartTitleHidden As Boolean
    Get
      Return DirectCast(GetValue(ChartTitleHiddenProperty), Boolean)
    End Get
    Set
      SetValue(ChartTitleHiddenProperty, Value)
    End Set
  End Property

  Public Shared Sub ChartTitleHiddenChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim o = DirectCast(d, BaseChart)

    If CBool(e.NewValue) Then
      o.ChartTitleVisibility = Visibility.Collapsed
    Else
      o.ChartTitleVisibility = Visibility.Visible
    End If

  End Sub
#End Region

#Region "BackGroundColor"
  Public Shared ReadOnly BackGroundColorProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackGroundColor), GetType(Brush), GetType(BaseChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly BackGroundColorCanvasProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackGroundColorCanvas), GetType(Brush), GetType(BaseChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly BackGroundColorLegendProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackGroundColorLegend), GetType(Brush), GetType(BaseChart), New UIPropertyMetadata(Brushes.Black))

  Public Property BackGroundColorLegend As Brush
    Get
      Return DirectCast(GetValue(BackGroundColorLegendProperty), Brush)
    End Get
    Set
      SetValue(BackGroundColorLegendProperty, Value)
    End Set
  End Property
#End Region

#Region "LegendVisibility"
  Public Shared ReadOnly LegendVisibilityProperty As DependencyProperty = DependencyProperty.Register(NameOf(LegendVisibility), GetType(Visibility), GetType(BaseChart), New PropertyMetadata(Visibility.Visible))

  Public Property LegendVisibility As Visibility
    Get
      Return GetValue(LegendVisibilityProperty)
    End Get

    Set(ByVal value As Visibility)
      SetValue(LegendVisibilityProperty, value)
    End Set
  End Property
#End Region

#Region "LegendTextVisibility"
  Public Shared ReadOnly LegendTextVisibilityProperty As DependencyProperty = DependencyProperty.Register(NameOf(LegendTextVisibility), GetType(Visibility), GetType(BaseChart), New PropertyMetadata(Visibility.Visible))

  Public Property LegendTextVisibility As Visibility
    Get
      Return GetValue(LegendTextVisibilityProperty)
    End Get

    Set(ByVal value As Visibility)
      SetValue(LegendTextVisibilityProperty, value)
    End Set
  End Property
#End Region

#Region "LegendForeground"
  Public Shared ReadOnly LegendForegroundProperty As DependencyProperty = DependencyProperty.Register(NameOf(LegendForeground), GetType(Brush), GetType(BaseChart), New UIPropertyMetadata(Brushes.Black))

  Public Property LegendForeground As Brush
    Get
      Return DirectCast(GetValue(LegendForegroundProperty), Brush)
    End Get
    Set
      SetValue(LegendForegroundProperty, Value)
    End Set
  End Property

#End Region

#Region "LegendHidden"
  Public Shared ReadOnly LegendHiddenProperty As DependencyProperty = DependencyProperty.Register(NameOf(LegendHidden), GetType(Boolean), GetType(BaseChart), New UIPropertyMetadata(False, AddressOf ChartLegendHiddenChanged))

  Public Property LegendHidden As Boolean
    Get
      Return DirectCast(GetValue(LegendHiddenProperty), Boolean)
    End Get
    Set
      SetValue(LegendHiddenProperty, Value)
    End Set
  End Property

  Public Shared Sub ChartLegendHiddenChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim o = DirectCast(d, BaseChart)

    If CBool(e.NewValue) Then
      o.LegendVisibility = Visibility.Collapsed
      o.LegendTextVisibility = Visibility.Collapsed
    Else
      o.LegendVisibility = Visibility.Visible
      o.LegendTextVisibility = Visibility.Visible
    End If
  End Sub
#End Region

#Region "YNumberOfTicks"
  Public Shared ReadOnly YNumberOfTicksProperty As DependencyProperty = DependencyProperty.Register(NameOf(YNumberOfTicks), GetType(Integer), GetType(BaseChart), New UIPropertyMetadata(0))

  Public Property YNumberOfTicks As Integer
    Get
      Return DirectCast(GetValue(YNumberOfTicksProperty), Integer)
    End Get
    Set
      SetValue(YNumberOfTicksProperty, Value)
    End Set
  End Property
#End Region

#Region "XValueConverter"
  Public Shared ReadOnly XValueConverterProperty As DependencyProperty = DependencyProperty.Register(NameOf(XValueConverter), GetType(IValueConverter), GetType(BaseChart), Nothing)

  Public Property XValueConverter As IValueConverter
    Get
      Return CType(GetValue(XValueConverterProperty), IValueConverter)
    End Get
    Set
      SetValue(XValueConverterProperty, Value)
    End Set
  End Property
#End Region

#Region "XValueMultiConverter"
  Public Shared ReadOnly XValueMultiConverterProperty As DependencyProperty = DependencyProperty.Register(NameOf(XValueMultiConverter), GetType(IMultiValueConverter), GetType(BaseChart), Nothing)

  Public Property XValueMultiConverter As IMultiValueConverter
    Get
      Return CType(GetValue(XValueMultiConverterProperty), IMultiValueConverter)
    End Get
    Set
      SetValue(XValueMultiConverterProperty, Value)
    End Set
  End Property
#End Region

#Region "YValueConverter"
  Public Shared ReadOnly YValueConverterProperty As DependencyProperty = DependencyProperty.Register(NameOf(YValueConverter), GetType(IValueConverter), GetType(BaseChart), Nothing)

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
  Public Shared ReadOnly FontTypeProperty As DependencyProperty = DependencyProperty.Register(NameOf(FontType), GetType(FontFamily), GetType(BaseChart), Nothing)

  Public Property FontType As FontFamily
    Get
      Return CType(GetValue(FontTypeProperty), FontFamily)
    End Get
    Set
      SetValue(FontTypeProperty, Value)
    End Set
  End Property
#End Region

#Region "ChartData"
  Public Shared ReadOnly ChartDataProperty As DependencyProperty = DependencyProperty.Register("ChartData", GetType(ObservableCollectionContentNotifying(Of PlotTrend)), GetType(BaseChart), New UIPropertyMetadata(New ObservableCollectionContentNotifying(Of PlotTrend), AddressOf ChartDataChanged))

  Public Property ChartData As ObservableCollectionContentNotifying(Of PlotTrend)
    Get
      Return CType(GetValue(ChartDataProperty), ObservableCollectionContentNotifying(Of PlotTrend))
    End Get
    Set
      SetValue(ChartDataProperty, Value)
    End Set
  End Property
#End Region
#End Region

#Region "Override Methods"
  Public Shared Sub ChartDataChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim o = DirectCast(d, BaseChart)

    If Not IsNothing(e.OldValue) Then
      Dim OldCollection = TryCast(e.OldValue, ObservableCollectionContentNotifying(Of PlotTrend))
      'RemoveHandler OldCollection.OnCollectionItemChanged, AddressOf o.CalculatePlotTrends
      RemoveHandler OldCollection.CollectionChanged, AddressOf o.CalculatePlotTrends
    End If

    If Not IsNothing(e.NewValue) Then
      Dim NewCollection = TryCast(e.NewValue, ObservableCollectionContentNotifying(Of PlotTrend))
      'AddHandler NewCollection.OnCollectionItemChanged, AddressOf o.CalculatePlotTrends
      AddHandler NewCollection.CollectionChanged, AddressOf o.CalculatePlotTrends
      AddHandler o.Loaded, Sub() o.ResizeAndPlotPoints(o)
      AddHandler o.SizeChanged, Sub() o.Resized()
      AddHandler o.Timer.Tick, Sub() o.OnTick(o)
    End If
  End Sub

  Public MustOverride Sub OnTick(o As Object)

  Public MustOverride Sub Resized()

  Public MustOverride Sub ResizeAndPlotPoints(o As Object)

  Public MustOverride Sub CalculatePlotTrends()
#End Region

#Region "Protected Methods to be used by inheriting classes"
  Protected Function GetXSegmentText(input As String) As String
    Return If(XValueConverter IsNot Nothing, XValueConverter.Convert(input, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture), input.ToString)
  End Function

  Protected Overridable Sub DrawYAxis(partCanvasYTicks As Canvas, partCanvasYLabels As Canvas, yCeiling As Double, yFloor As Double, viewHeight As Double, labelHeight As Double)
    Dim segment = ((yCeiling - yFloor) / YNumberOfTicks)
    partCanvasYTicks.Children.RemoveRange(0, partCanvasYTicks.Children.Count)
    partCanvasYLabels.Children.RemoveRange(0, partCanvasYLabels.Children.Count)

    partCanvasYTicks.Children.Add(New Line With {
                                   .X1 = 0,
                                   .X2 = 0,
                                   .Y1 = 0,
                                   .Y2 = viewHeight,
                                   .StrokeThickness = 2,
                                   .Stroke = Brushes.Black
                                   })


    'Sizing should be done from the ceiling
    Dim lastText = New String(If(YValueConverter Is Nothing, yCeiling.ToString, YValueConverter.Convert(yCeiling, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture)))
    Dim spacingForText = lastText.Count
    Dim fontSize = 0
    Dim finalSpacing = 0
    Dim lastSpaceFactor = 0

    Select Case spacingForText
      Case <= 7
        fontSize = 30
        finalSpacing = spacingForText * 0.3
        lastSpaceFactor = finalSpacing * 1.2
      Case <= 9
        fontSize = 24
        finalSpacing = spacingForText * 0.5
        lastSpaceFactor = finalSpacing * 1.5
      Case <= 11
        fontSize = 18
        finalSpacing = spacingForText * 0.68
        lastSpaceFactor = finalSpacing * 1.45
      Case <= 13
        fontSize = 16
        finalSpacing = spacingForText * 0.7
        lastSpaceFactor = finalSpacing * 1.44
      Case Else
        fontSize = 14
        finalSpacing = spacingForText * 0.7
        lastSpaceFactor = finalSpacing * 1.44
    End Select

    For i As Integer = 0 To YNumberOfTicks
      Dim ySegment = If(i = 0, 0, i * (viewHeight / YNumberOfTicks))
      Dim ySegmentLabel = If(i = 0, yFloor, yFloor + (i * segment))
      Dim textForLabel = New String(If(YValueConverter Is Nothing, ySegmentLabel.ToString, YValueConverter.Convert(ySegmentLabel, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture)))

      Dim lineSegment = New Line With {
          .X1 = 0,
          .X2 = labelHeight,
          .Y1 = ySegment,
          .Y2 = ySegment,
          .StrokeThickness = 2,
          .Stroke = Brushes.Black}
      partCanvasYTicks.Children.Add(lineSegment)

      Dim labelSegment = New TextBlock With {
        .Text = textForLabel,
        .FontSize = fontSize,
        .Margin = New Thickness(0, viewHeight - 20 - (ySegment - If(i = 0, 0, If(i = YNumberOfTicks, lastSpaceFactor, finalSpacing))), 0, 0)
      }

      partCanvasYLabels.Children.Add(labelSegment)
    Next
  End Sub

  Protected Overridable Sub DrawXAxis(partCanvasXTicks As Canvas, partCanvasXLabels As Canvas, xCeiling As Double, xFloor As Double, xTicks As Integer, viewWidth As Double, labelHeight As Double)
    Dim segment = ((xCeiling - xFloor) / xTicks)
    partCanvasXTicks.Children.RemoveRange(0, partCanvasXTicks.Children.Count)
    partCanvasXLabels.Children.RemoveRange(0, partCanvasXLabels.Children.Count)

    partCanvasXTicks.Children.Add(New Line With {
                                   .X1 = 0,
                                   .X2 = viewWidth,
                                   .Y1 = 0,
                                   .Y2 = 0,
                                   .StrokeThickness = 2,
                                   .Stroke = Brushes.Black
                                   })

    'Sizing should be done from the ceiling
    Dim lastText = GetXSegmentText(xCeiling.ToString)

    Dim spacingForText = lastText.Count * 6
    Dim totalLength = spacingForText * xTicks
    Dim fontSize = 0
    Dim finalSpacing = 0
    Dim lastSpaceFactor = 0

    Select Case totalLength
      Case <= 200
        fontSize = 30
        finalSpacing = spacingForText * 1.2
        lastSpaceFactor = finalSpacing * 2
      Case <= 250
        fontSize = 20
        finalSpacing = spacingForText * 0.9
        lastSpaceFactor = finalSpacing * 1.75
      Case <= 500
        fontSize = 16
        finalSpacing = spacingForText * 0.6
        lastSpaceFactor = finalSpacing * 2
      Case <= 750
        fontSize = 12
        finalSpacing = spacingForText * 0.45
        lastSpaceFactor = finalSpacing * 1.8
      Case Else
        fontSize = 8
        finalSpacing = spacingForText * 0.3
        lastSpaceFactor = finalSpacing * 2
    End Select

    For i As Integer = 0 To xTicks
      Dim xSegment = If(i = 0, 0, i * (viewWidth / xTicks))
      Dim xSegmentLabel = If(i = 0, xFloor, xFloor + (i * segment))
      Dim textForLabel = GetXSegmentText(xSegmentLabel)

      Dim lineSegment = New Line With {
          .X1 = xSegment,
          .X2 = xSegment,
          .Y1 = 0,
          .Y2 = labelHeight,
          .StrokeThickness = 2,
          .Stroke = Brushes.Black}
      partCanvasXTicks.Children.Add(lineSegment)

      Dim labelSegment = New TextBlock With {
        .Text = textForLabel,
        .FontSize = fontSize,
        .Margin = New Thickness(xSegment - If(i = 0, 0, If(i = xTicks, lastSpaceFactor, finalSpacing)), 0, 0, 0)
      }

      partCanvasXLabels.Children.Add(labelSegment)
    Next
  End Sub

  Protected Overridable Sub DrawTrends(partCanvas As Canvas, viewWidth As Double, viewHeight As Double, xCeiling As Double, xFloor As Double, yCeiling As Double, yFloor As Double)
    For Each t In ChartData
      If t.Points IsNot Nothing Then
        Dim xFactor = (viewWidth / (xCeiling - xFloor))
        Dim yFactor = (viewHeight / (yCeiling - yFloor))

        xFactor = If(Double.IsNaN(xFactor) OrElse Double.IsInfinity(xFactor), 1, xFactor)
        yFactor = If(Double.IsNaN(yFactor) OrElse Double.IsInfinity(yFactor), 1, yFactor)

        For i As Integer = 1 To t.Points.Count - 1
          Dim toDraw = New Line With {
            .X1 = (t.Points(i - 1).XAsDouble - xFloor) * xFactor,
            .Y1 = (t.Points(i - 1).YAsDouble - yFloor) * yFactor,
            .X2 = (t.Points(i).XAsDouble - xFloor) * xFactor,
            .Y2 = (t.Points(i).YAsDouble - yFloor) * yFactor,
            .StrokeThickness = 2,
            .Stroke = t.LineColor}
          partCanvas.Children.Add(toDraw)
        Next i
      End If
    Next
  End Sub
#End Region

End Class
