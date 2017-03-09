Public NotInheritable Class LineChart




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

  Private ReadOnly Property Ratio As Double
    Get
      Return PART_CanvasBorder.ActualHeight / PART_CanvasBorder.ActualWidth
    End Get
  End Property

  Public Sub New()
    InitializeComponent()
    Part_Layout.DataContext = Me
  End Sub

#Region "XNumberOfTicks"
  Public Shared ReadOnly XNumberOfTicksProperty As DependencyProperty = DependencyProperty.Register(NameOf(XNumberOfTicks), GetType(Integer), GetType(LineChart), New UIPropertyMetadata(0))

  Public Property XNumberOfTicks As Integer
    Get
      Return DirectCast(GetValue(XNumberOfTicksProperty), Integer)
    End Get
    Set
      SetValue(XNumberOfTicksProperty, Value)
    End Set
  End Property
#End Region

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
    o.CalculatePlotTrends()
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

      Me.PART_CanvasPoints.Children.RemoveRange(0, Me.PART_CanvasPoints.Children.Count)
      Me.DrawTrends(ChartData)

      If Me.PART_CanvasXAxisTicks IsNot Nothing And Me.PART_CanvasYAxisTicks IsNot Nothing Then
        If Me.XNumberOfTicks = 0 Then Me.XNumberOfTicks = 1 'I want at the very least to see a beginning and an end
        Me.DrawXAxis(PART_CanvasXAxisTicks, PART_CanvasXAxisLabels, _xCeiling, _xFloor, XNumberOfTicks, _viewWidth, _labelHeight)
        Me.DrawYAxis(PART_CanvasYAxisTicks, PART_CanvasYAxisLabels, _yCeiling, _yFloor, _viewHeight, _labelHeight)
      End If
    End If
  End Sub
#End Region

#Region "Drawing Methods"

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