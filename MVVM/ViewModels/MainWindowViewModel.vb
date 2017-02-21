Imports System.Collections.ObjectModel
Imports Controls
Imports DataAccess

Public NotInheritable Class MainWindowViewModel
  Inherits BaseViewModel

  Private _lastPoints As New List(Of PlotPoints)
  Private _testText As String
  Private _demands As New List(Of DemandTrendOutput)
  Private _grouping As String

  Public Property TestText As String
    Get
      Return _testText
    End Get
    Set(ByVal value As String)
      _testText = value
      OnPropertyChanged(NameOf(TestText))
    End Set
  End Property

  Private _dt As DateTime
  Public Property Dt As DateTime
    Get
      Return _dt
    End Get
    Set(ByVal value As DateTime)
      _dt = value
    End Set
  End Property

  Private _array As New ObservableCollection(Of String)({"Day", "Month", "Year"})
  Public Property Array As ObservableCollection(Of String)
    Get
      Return _array
    End Get
    Set(ByVal value As ObservableCollection(Of String))
      _array = value
      OnPropertyChanged(NameOf(Array))
    End Set
  End Property


  Private _theItem As String
  Public Property TheItem As String
    Get
      Return _theItem
    End Get
    Set(ByVal value As String)
      _theItem = value
      If MyConverter IsNot Nothing Then MyConverter.OptionalHeader = value
      OnPropertyChanged(NameOf(TheItem))
    End Set
  End Property

  Public ReadOnly Property MyConverter As New DecimalConverter


  Public ReadOnly Property ChartData As New ObservableCollectionContentNotifying(Of PlotTrend)

  Public Enum TrendChoices
    FiscalYear
    FiscalWeek
    FiscalPeriod
    FiscalQuarter
    Year
    Quarter
    Month
    Week
    Day
  End Enum

  Public Sub New()
    TheItem = "Month"
    MyConverter.OptionalHeader = TheItem
    TestText = TrendChoices.Month.ToString
    _grouping = TrendChoices.Month.ToString

    _demands = Selects.GetDemandTrends(New DemandTrendInput(2278, New Date(2017, 3, 10), New Date(2017, 5, 20), _grouping, New List(Of Integer)({24, 26}), New List(Of Integer)({2, 25})))
    AddingLinesForLineChart()
  End Sub

  Public ReadOnly Property TestCommand As New DelegateCommand(Of Object)(AddressOf TestCommandExecute)

  Private Sub TestCommandExecute()
    LinePlotAdding()
    TestText = "Line Chart Hello there" + DateTime.Now.ToLongTimeString
  End Sub

  Private _xTicks As Integer

  Public Property XTicks As Integer
    Get
      Return (ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).Distinct.Count - 1)
    End Get
    Set(value As Integer)
      _xTicks = value
      OnPropertyChanged(NameOf(XTicks))
    End Set
  End Property


#Region "Line Graph parts"
  Private Sub AddingLinesForLineChart()
    Dim demand = _demands.Select(Function(x) New PlotPoints(New PlotPoint(Of Double)(x.Grouping), New PlotPoint(Of Double)(x.DemandQty)))
    Dim ad = _demands.Select(Function(x) New PlotPoints(New PlotPoint(Of Double)(x.Grouping), New PlotPoint(Of Double)(x.DemandAdQty)))

    _lastPoints = New List(Of PlotPoints)({demand.Last, ad.Last})

    ChartData.ClearAndAddRange({New PlotTrend("Demand", Brushes.Blue, New Thickness(2), demand, _grouping), New PlotTrend("Ad", Brushes.Red, New Thickness(2), ad, _grouping)})

    '_lastPoints = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 12)), New PlotPoint(Of Double)(1200)),
    '                                      New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 12)), New PlotPoint(Of Double)(1200)),
    '                                      New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 10)), New PlotPoint(Of Double)(950))})

    'Dim o = New ObservableCollection(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(400)),
    '                                                                          New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 11)), New PlotPoint(Of Double)(800)),
    '                                                                          _lastPoints(0)})

    'Dim o2 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(150)),
    '                                New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 11)), New PlotPoint(Of Double)(720)),
    '                                _lastPoints(1)})

    'Dim o3 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 1)), New PlotPoint(Of Double)(300)),
    '                                New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(1720)),
    '                                New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 7)), New PlotPoint(Of Double)(420)),
    '                                New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 8)), New PlotPoint(Of Double)(920)),
    '                                _lastPoints(2)})

    'Dim o4 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(1920))})

    'Dim o5 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(2920))})

    'ChartData.ClearAndAddRange({New PlotTrend("First", Brushes.Blue, New Thickness(2), o), New PlotTrend("Second", Brushes.Red, New Thickness(2), o2),
    '                           New PlotTrend("Third", Brushes.Purple, New Thickness(2), o3), New PlotTrend("Fourth", Brushes.Orange, New Thickness(2), o4),
    '                           New PlotTrend("Fifth", Brushes.Brown, New Thickness(2), o5)})
  End Sub

  Private Sub LinePlotAdding()
    Dim newPoints = New List(Of PlotPoints)

    For i = 1 To _lastPoints.Count
      newPoints.Add(New PlotPoints(New PlotPoint(Of Double)((DirectCast(_lastPoints(i - 1).X, PlotPoint(Of Double)).Point + 1)), New PlotPoint(Of Double)(DirectCast(_lastPoints(i - 1).Y, PlotPoint(Of Double)).Point * 1.95)))
      XTicks = (ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).Distinct.Count - 1)
    Next

    _lastPoints = newPoints
    ChartData(0).AdditionalSeriesInfo = "Day"
    ChartData(0).Points.Add(_lastPoints(0))
    ChartData(1).Points.Add(_lastPoints(1))
    ChartData(1).AdditionalSeriesInfo = "Day"
  End Sub

#End Region

End Class
