Imports System.Windows.Threading

Public NotInheritable Class BarChart
  'VARIABLES
  Private _xType As Type
  Private _xFloor As Decimal = 0
  Private _xCeiling As Decimal = 0
  Private _yType As Type
  Private _yFloor As Decimal = 0
  Private _yCeiling As Decimal = 0
  Private _viewHeight As Double = 0
  Private _viewWidth As Double = 0
  Private _tickWidth As Double = 0
  Private _tickHeight As Double = 0
  Private _labelWidth As Double = 0
  Private _labelHeight As Double = 0
  Private _timer As New DispatcherTimer
  Private _defaultTimeSpan As New TimeSpan(1000)

  Private ReadOnly Property Ratio As Double
    Get
      Return PART_CanvasBorder.ActualHeight / PART_CanvasBorder.ActualWidth
    End Get
  End Property

  Public Sub New()
    InitializeComponent()

    Part_Layout.DataContext = Me
    _timer.Interval = _defaultTimeSpan
  End Sub

#Region "DataChangedAndTimingEvents"

  Public Overrides Sub OnTick(o As Object)
    _timer.Stop()
    o.ResizeAndPlotPoints(o)
  End Sub

  Public Overrides Sub Resized()
    _timer.Stop()
    _timer.Start()
  End Sub
#End Region

#Region "ResivingAndPlotPoints"
  Public Overrides Sub ResizeAndPlotPoints(o As Object)
    SetupInternalHeightAndWidths()
    SetupHeightAndWidthsOfObjects()
    o.CalculatePlotTrends()
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
      Me._yFloor = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.YAsDouble).OrderBy(Function(x) x).FirstOrDefault()
      Me._yCeiling = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.YAsDouble).OrderByDescending(Function(x) x).FirstOrDefault()

      'Bar Chart needs a buffer of space around it to visibly show data without it looking bad.
      Dim xType As Type = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.X).FirstOrDefault().PointType

      If (xType Is GetType(Date) Or xType Is GetType(DateTime)) Then
        Dim startDate = New DateTime(_xFloor)
        Dim endDate = New DateTime(_xCeiling)

        Me._xFloor = startDate.AddDays(-1).Ticks
        Me._xCeiling = endDate.AddDays(1).Ticks
      Else
        Me._xFloor *= 1.1
        Me._xCeiling *= 1.1
      End If

      Me.PART_CanvasPoints.Children.RemoveRange(0, Me.PART_CanvasPoints.Children.Count)
        Me.DrawTrends(ChartData)

        If Me.PART_CanvasXAxisTicks IsNot Nothing And Me.PART_CanvasYAxisTicks IsNot Nothing Then
          If Me.NumberOfTicks = 0 Then Me.NumberOfTicks = 1 'I want at the very least to see a beginning and an end
          Me.DrawXAxis(ChartData)
          Me.DrawYAxis(ChartData)
        End If
      End If
  End Sub
#End Region

#Region "Drawing Methods"
  Private Sub DrawXAxis(lineTrends As IList(Of PlotTrend))
    Dim segment = ((_xCeiling - _xFloor) / NumberOfTicks)
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
    Dim totalLength = spacingForText * NumberOfTicks
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

    For i As Integer = 0 To NumberOfTicks
      Dim xSegment = If(i = 0, 0, i * (_viewWidth / NumberOfTicks))
      Dim xSegmentLabel = If(i = 0, _xFloor, _xFloor + (i * segment))
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
        .Margin = New Thickness(xSegment - If(i = 0, 0, If(i = NumberOfTicks, lastSpaceFactor, finalSpacing)), 0, 0, 0)
      }

      PART_CanvasXAxisLabels.Children.Add(labelSegment)
    Next
  End Sub

  Private Sub DrawYAxis(lineTrends As IList(Of PlotTrend))
    Dim segment = ((_yCeiling - _yFloor) / NumberOfTicks)
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
    Dim totalLength = spacingForText * NumberOfTicks
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

    For i As Integer = 0 To NumberOfTicks
      Dim ySegment = If(i = 0, 0, i * (_viewHeight / NumberOfTicks))
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
        .Margin = New Thickness(0, _viewHeight - 20 - (ySegment - If(i = 0, 0, If(i = NumberOfTicks, lastSpaceFactor, finalSpacing))), 0, 0)
      }

      PART_CanvasYAxisLabels.Children.Add(labelSegment)
    Next
  End Sub

  Private Sub DrawTrends(points As IList(Of PlotTrend))

    For Each t In ChartData
      If t.Points IsNot Nothing Then
        Dim xFactor = (_viewWidth / (_xCeiling - _xFloor))
        Dim yFactor = (_viewHeight / (_yCeiling - _yFloor))

        xFactor = If(Double.IsNaN(xFactor) OrElse Double.IsInfinity(xFactor), 1, xFactor)
        yFactor = If(Double.IsNaN(yFactor) OrElse Double.IsInfinity(yFactor), 1, yFactor)

        For i As Integer = 1 To t.Points.Count - 1
          Dim toDraw = New Line With {
            .X1 = (t.Points(i - 1).XAsDouble - _xFloor) * xFactor,
            .Y1 = (t.Points(i - 1).YAsDouble - _yFloor) * yFactor,
            .X2 = (t.Points(i).XAsDouble - _xFloor) * xFactor,
            .Y2 = (t.Points(i).YAsDouble - _yFloor) * yFactor,
            .StrokeThickness = 2,
            .Stroke = t.LineColor}
          PART_CanvasPoints.Children.Add(toDraw)
        Next i
      End If
    Next


  End Sub
#End Region

End Class
