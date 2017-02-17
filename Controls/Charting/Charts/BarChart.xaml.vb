Imports System.Windows.Threading

Public NotInheritable Class BarChart

  'VARIABLES
  Private _xFloor As Decimal = 0
  Private _xCeiling As Decimal = 0
  Private _yFloor As Decimal = 0
  Private _yCeiling As Decimal = 0
  Private _viewHeight As Double = 0
  Private _viewWidth As Double = 0
  Private _tickWidth As Double = 0
  Private _tickHeight As Double = 0
  Private _labelWidth As Double = 0
  Private _labelHeight As Double = 0
  Private _xNumberOfTicks As Double = 0

  Private _explicitTicks As IEnumerable(Of Double)

  Private ReadOnly Property Ratio As Double
    Get
      Return PART_CanvasBorder.ActualHeight / PART_CanvasBorder.ActualWidth
    End Get
  End Property

  Public Sub New()
    InitializeComponent()
    Part_Layout.DataContext = Me
  End Sub

#Region "DataChangedAndTimingEvents"
  Public Overrides Sub OnTick(o As Object)
    Timer.Stop()
    o.ResizeAndPlotPoints(o)
  End Sub

  Public Overrides Sub Resized()
    Timer.Stop()
    Timer.Start()
  End Sub
#End Region

#Region "ResivingAndPlotPoints"
  Public Overrides Sub ResizeAndPlotPoints(o As Object)
    SetupInternalHeightAndWidths()
    SetupHeightAndWidthsOfObjects()
    ResetTicksForSpecificDateRange()
    o.CalculatePlotTrends()
  End Sub

  Private Sub ResetTicksForSpecificDateRange()
    _explicitTicks = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).Distinct().OrderBy(Function(x) x).ToList()
    _xNumberOfTicks = _explicitTicks.Count
  End Sub

  Private Sub SetupInternalHeightAndWidths()
    Dim margin = 0.99

    'True for Height having a larger aspect, False for Width having a larger aspect.  I am setting a floor for 25, anything smaller and it looks like crap.
    Dim SetHeightOrWidth As Func(Of Integer, Boolean, Double) = Function(x, y)
                                                                  Dim val = If(y, ((x / Ratio) * margin), ((x * Ratio) * margin))
                                                                  Return If(val <= 25, 25, val)
                                                                End Function

    If Ratio > 1 Then
      _viewHeight = 1000
      _viewWidth = SetHeightOrWidth(1000, True)
      _tickHeight = 50
      _tickWidth = SetHeightOrWidth(50, True)
      _labelHeight = 30
      _labelWidth = SetHeightOrWidth(75, True)
    Else
      _viewHeight = SetHeightOrWidth(1000, False)
      _viewWidth = 1000
      _tickHeight = SetHeightOrWidth(50, False)
      _tickWidth = 50
      _labelHeight = SetHeightOrWidth(30, False)
      _labelWidth = 75
    End If
  End Sub

  Private Sub SetupHeightAndWidthsOfObjects()
    PART_CanvasYAxisLabels.Height = _viewHeight
    PART_CanvasYAxisLabels.Width = _labelWidth
    PART_CanvasYAxisTicks.Height = _viewHeight
    PART_CanvasYAxisTicks.Width = _tickWidth

    PART_CanvasXAxisLabels.Height = _labelHeight
    PART_CanvasXAxisLabels.Width = _viewWidth
    PART_CanvasXAxisTicks.Height = _tickHeight
    PART_CanvasXAxisTicks.Width = _viewWidth

    PART_CanvasPoints.Height = _viewHeight
    PART_CanvasPoints.Width = _viewWidth
  End Sub

  Public Overrides Sub CalculatePlotTrends()
    If Me.PART_CanvasPoints IsNot Nothing AndAlso ChartData IsNot Nothing Then
      If ChartData.Count > 1 Then
        ResetTicksForSpecificDateRange()

        'Uniformity check of X and Y types.  EG: You cannot have a DateTime and a Number for different X axis or Y axis sets.
        If ChartData.ToList().Select(Function(x) x.Points(0).X.GetType).Distinct.GroupBy(Function(x) x).Count > 1 Or ChartData.ToList().Select(Function(x) x.Points(0).Y.GetType).Distinct.GroupBy(Function(x) x).Count > 1 Then
          Me.PART_CanvasPoints.LayoutTransform = New ScaleTransform(1, 1)
          Me.PART_CanvasPoints.UpdateLayout()
          Dim fontFamily = If(Me.FontType IsNot Nothing, Me.FontType, New FontFamily("Segoe UI"))
          Dim stackPanel = New StackPanel
          stackPanel.Children.Add(New TextBlock With {.Text = "Type Mismatch cannot render!", .FontSize = 54, .FontFamily = fontFamily})
          stackPanel.Children.Add(New TextBlock With {.Text = "Either the X or Y plot points are of different types.", .FontSize = 32, .FontFamily = fontFamily})
          Me.PART_CanvasPoints.Children.Add(stackPanel)
          Return
        End If
      End If

      Me._xFloor = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).OrderBy(Function(x) x).FirstOrDefault()
      Me._xCeiling = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).OrderByDescending(Function(x) x).FirstOrDefault()
      Me._yFloor = 0
      Me._yCeiling = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.YAsDouble).OrderByDescending(Function(x) x).FirstOrDefault()

      Me.PART_CanvasPoints.Children.RemoveRange(0, Me.PART_CanvasPoints.Children.Count)
      Me.DrawTrends()

      If Me.PART_CanvasXAxisTicks IsNot Nothing And Me.PART_CanvasYAxisTicks IsNot Nothing Then
        If Me._xNumberOfTicks = 0 Then Me._xNumberOfTicks = 1 'I want at the very least to see a beginning and an end
        Me.DrawXAxis()
        Me.DrawYAxis()
      End If
    End If
  End Sub
#End Region

#Region "Drawing Methods"
  Private Sub DrawXAxis()
    PART_CanvasXAxisTicks.Children.RemoveRange(0, PART_CanvasXAxisTicks.Children.Count)
    PART_CanvasXAxisLabels.Children.RemoveRange(0, PART_CanvasXAxisLabels.Children.Count)

    PART_CanvasXAxisTicks.Children.Add(New Line With {
                                   .X1 = 0,
                                   .X2 = _viewWidth,
                                   .Y1 = 0,
                                   .Y2 = 0,
                                   .StrokeThickness = 2,
                                   .Stroke = Brushes.Black
                                   })

    'Sizing should be done from the ceiling
    Dim lastText = New String(If(XValueConverter Is Nothing, _xCeiling.ToString, XValueConverter.Convert(_xCeiling, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture)))
    Dim spacingForText = lastText.Count * 6
    Dim totalLength = spacingForText * _xNumberOfTicks
    Dim fontSize = 0
    Dim spacing = 0

    Select Case totalLength
      Case <= 200
        fontSize = 30
        spacing = spacingForText * 1.2
      Case <= 250
        fontSize = 20
        spacing = spacingForText * 0.9
      Case <= 500
        fontSize = 16
        spacing = spacingForText * 0.6
      Case <= 750
        fontSize = 12
        spacing = spacingForText * 0.45
      Case Else
        fontSize = 8
        spacing = spacingForText * 0.3
    End Select

    For i As Integer = 0 To _xNumberOfTicks - 1
      Dim xSegment = (i * (_viewWidth / _xNumberOfTicks) + ((_viewWidth / _xNumberOfTicks) / 2))
      Dim xSegmentLabel = _explicitTicks(i)
      Dim textForLabel = New String(If(XValueConverter Is Nothing, xSegmentLabel.ToString, XValueConverter.Convert(xSegmentLabel, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture)))

      Dim lineSegment = New Line With {
          .X1 = xSegment,
          .X2 = xSegment,
          .Y1 = 0,
          .Y2 = _labelHeight,
          .StrokeThickness = 2,
          .Stroke = Brushes.Black}
      PART_CanvasXAxisTicks.Children.Add(lineSegment)

      Dim labelSegment = New TextBlock With {
        .Text = textForLabel,
        .FontSize = fontSize,
        .Margin = New Thickness(xSegment - spacing, 0, 0, 0)
      }

      PART_CanvasXAxisLabels.Children.Add(labelSegment)
    Next
  End Sub

  Private Sub DrawYAxis()
    Dim segment = ((_yCeiling - _yFloor) / YNumberOfTicks)
    PART_CanvasYAxisTicks.Children.RemoveRange(0, PART_CanvasYAxisTicks.Children.Count)
    PART_CanvasYAxisLabels.Children.RemoveRange(0, PART_CanvasYAxisLabels.Children.Count)

    PART_CanvasYAxisTicks.Children.Add(New Line With {
                                   .X1 = 0,
                                   .X2 = 0,
                                   .Y1 = 0,
                                   .Y2 = _viewHeight,
                                   .StrokeThickness = 2,
                                   .Stroke = Brushes.Black
                                   })


    'Sizing should be done from the ceiling
    Dim lastText = New String(If(YValueConverter Is Nothing, _yCeiling.ToString, YValueConverter.Convert(_yCeiling, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture)))
    Dim spacingForText = lastText.Count * 5
    Dim totalLength = spacingForText * YNumberOfTicks
    Dim fontSize = 0
    Dim finalSpacing = 0
    Dim lastSpaceFactor = 0

    Select Case totalLength
      Case <= 200
        fontSize = 30
        finalSpacing = spacingForText * 0.5
        lastSpaceFactor = finalSpacing * 1.9
      Case <= 250
        fontSize = 20
        finalSpacing = spacingForText * 0.5
        lastSpaceFactor = finalSpacing * 1.5
      Case Else
        fontSize = 16
        finalSpacing = spacingForText * 0.25
        lastSpaceFactor = finalSpacing * 1
    End Select

    For i As Integer = 0 To YNumberOfTicks
      Dim ySegment = If(i = 0, 0, i * (_viewHeight / YNumberOfTicks))
      Dim ySegmentLabel = If(i = 0, _yFloor, _yFloor + (i * segment))
      Dim textForLabel = New String(If(YValueConverter Is Nothing, ySegmentLabel.ToString, YValueConverter.Convert(ySegmentLabel, GetType(String), Nothing, Globalization.CultureInfo.InvariantCulture)))

      Dim lineSegment = New Line With {
          .X1 = 0,
          .X2 = _labelHeight,
          .Y1 = ySegment,
          .Y2 = ySegment,
          .StrokeThickness = 2,
          .Stroke = Brushes.Black}
      PART_CanvasYAxisTicks.Children.Add(lineSegment)

      Dim labelSegment = New TextBlock With {
        .Text = textForLabel,
        .FontSize = fontSize,
        .Margin = New Thickness(0, _viewHeight - 20 - (ySegment - If(i = 0, 0, If(i = YNumberOfTicks, lastSpaceFactor, finalSpacing))), 0, 0)
      }

      PART_CanvasYAxisLabels.Children.Add(labelSegment)
    Next
  End Sub

  Private Sub DrawTrends()
    Dim widthOfBar = _viewWidth / ((_xNumberOfTicks + 2) * ChartData.Count)
    Dim yFactor = (_viewHeight / (_yCeiling - _yFloor))

    yFactor = If(Double.IsNaN(yFactor) OrElse Double.IsInfinity(yFactor), 1, yFactor)

    For i As Integer = 0 To _xNumberOfTicks - 1
      Dim xValue = _explicitTicks(i)

      DrawTrendsFromDate(xValue, widthOfBar, yFactor, i)
    Next i
  End Sub

  Private Sub DrawTrendsFromDate(xSegmentValue As Double, widthOfBar As Double, yFactor As Double, segmentIndex As Integer)
    ChartData.SelectMany(Function(x) x.Points) _
                      .Where(Function(x) x.XAsDouble = xSegmentValue) _
                      .Select(Function(pnt, ind) New With {.YAsDouble = pnt.YAsDouble, .XAsDouble = pnt.XAsDouble, .Index = ind}) _
                      .ToList() _
                      .ForEach(Sub(t)

                                 'This magic formula basically determines a segment, so 1000 and 4 sets would be 250, however I want to have ticks never start and always pad
                                 'so I then add the same thing back to itself but with a slight buffer of one half
                                 Dim segment = (segmentIndex * _viewWidth / _xNumberOfTicks + (_viewWidth / _xNumberOfTicks) / 2)

                                 'If I have two sets or more on the same day I need to see that
                                 segment = segment + If(t.Index > 0, (t.Index) * widthOfBar, 0)

                                 Dim matches = ChartData.Where(Function(x, ind) x.Points.ToList().Exists(Function(y) y.XAsDouble = t.XAsDouble And y.YAsDouble = t.YAsDouble))
                                 Dim color =  If(matches.Count > 1, matches.Skip(t.Index).Take(1).First.LineColor, matches.First.LineColor)

                                 Dim toDraw = New Line With {
                                                    .X1 = segment,
                                                    .Y1 = 0,
                                                    .X2 = segment,
                                                    .Y2 = (t.YAsDouble - _yFloor) * yFactor,
                                                    .StrokeThickness = widthOfBar,
                                                    .Stroke = color}
                                 PART_CanvasPoints.Children.Add(toDraw)
                               End Sub)
  End Sub
#End Region

End Class
