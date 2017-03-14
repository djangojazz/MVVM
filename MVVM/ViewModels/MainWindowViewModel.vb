Imports System.Collections.ObjectModel
Imports Controls
Imports DataAccess

Public NotInheritable Class MainWindowViewModel
  Inherits BaseViewModel

  Private _lastPoints As New List(Of PlotPoints)
  Private _loaded As Boolean = False

  Public Sub New()
    LocationCollection.ClearAndAddRange(Selects.GetDemandLocations().Take(5))
    Dim locs = New List(Of Integer)({18, 55})
    locs.ForEach(Sub(x) LocationCollection.Where(Function(y) y.LocationID = x).Single().IsUsed = True)
    UpdateHeader()
    SelectedItem = TrendChoices.FiscalPeriod
    AddHandler LocationCollection.OnCollectionItemChanged, AddressOf UpdateHeader
  End Sub

  Private Sub UpdateHeader(sender As Object, e As ObservableCollectionContentChangedArgs)
    UpdateHeader()
  End Sub

  Private Sub UpdateHeader()
    Dim itemsSelected = LocationCollection.Where(Function(x) x.IsUsed = True).Select(Function(x) x.ToString)
    Dim headerUpdated = If(itemsSelected.Any, String.Join(", ", itemsSelected), "No Items")
    LocationHeader = headerUpdated
  End Sub

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

  Public ReadOnly Property SelectedLocations As List(Of Integer)
    Get
      Return If(LocationCollection?.Any(), LocationCollection.Where(Function(x) x.IsUsed = True).Select(Function(x) x.LocationID).ToList(), Nothing)
    End Get
  End Property

  Private _open As Boolean
  Public Property Open As Boolean
    Get
      Return _open
    End Get
    Set(ByVal value As Boolean)
      _open = value
      If _loaded And Not Open Then
        'DemandTrend.DemandLocations.ForEach(Sub(x) LocationCollection.Where(Function(y) y.LocationID = x).ForEach(Function(z) z.IsUsed = True))
        Dim s = String.Empty
        SelectedLocations.ForEach(Sub(x) s += x.ToString + Environment.NewLine)
        MessageBox.Show(s)
      End If
      OnPropertyChanged(NameOf(Open))
      _loaded = True
    End Set
  End Property

  Private _testText As String

  Public Property TestText As String
    Get
      Return _testText
    End Get
    Set(ByVal value As String)
      _testText = value
      UpdateChartData()
      If DecimalConverter IsNot Nothing Then DecimalConverter.OptionalHeader = TestText : DecimalConverter.FirstPosition = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).First
      OnPropertyChanged(NameOf(TestText))
      UpdateChartData()
    End Set
  End Property


  Private _LocationHeader As String
  Public Property LocationHeader As String
    Get
      Return _LocationHeader
    End Get
    Set(ByVal value As String)
      _LocationHeader = value
      OnPropertyChanged(NameOf(LocationHeader))
    End Set
  End Property

  Public ReadOnly Property Array As TrendChoices()
    Get
      Return [Enum].GetValues(GetType(TrendChoices))
    End Get
  End Property

  Public Property SelectedItem As TrendChoices

  Public ReadOnly Property DecimalConverter As New InstanceInSetToStringConverter

  Public ReadOnly Property ChartData As New ObservableCollectionContentNotifying(Of PlotTrend)
  Public ReadOnly Property LocationCollection As New ObservableCollectionContentNotifying(Of DemandLocation)
  'Dictionary(Of String, Boolean)
  'ObservableCollection(Of DemandLocation)

  Private _selectedLocation As DemandLocation
  Public Property SelectedLocation As DemandLocation
    Get
      Return _selectedLocation
    End Get
    Set(ByVal value As DemandLocation)
      _selectedLocation = value
      OnPropertyChanged(NameOf(SelectedLocation))
    End Set
  End Property

  Private _xTicks As Integer

  Public Property XTicks As Integer
    Get
      Return _xTicks
    End Get
    Set(value As Integer)
      _xTicks = value
      OnPropertyChanged(NameOf(XTicks))
    End Set
  End Property


  Public ReadOnly Property CommandSelectedLocation As New DelegateCommand(Of DemandLocation)(AddressOf CommandSelectedLocationExecute)

  Private Sub CommandSelectedLocationExecute(obj As DemandLocation)
    SelectedLocation = obj
  End Sub

  Public ReadOnly Property TestCommand As New DelegateCommand(Of Object)(AddressOf TestCommandExecute)

  Private Sub TestCommandExecute()
    'LinePlotAdding()
    LocationCollection.ClearAndAddRange(Selects.GetDemandLocations().Take(10))
  End Sub


#Region "Line Graph parts"
  Public Sub UpdateChartData()
    Dim demands = Selects.GetDemandTrends(New DemandTrendInput(2278, New Date(2017, 2, 25), New Date(2017, 5, 1), TestText, New List(Of Integer)({2, 25})))

    Dim demand = demands.Select(Function(x) New PlotPoints(New PlotPoint(Of Double)(x.Grouping), New PlotPoint(Of Double)(x.DemandQty)))
    Dim ad = demands.Select(Function(x) New PlotPoints(New PlotPoint(Of Double)(x.Grouping), New PlotPoint(Of Double)(x.DemandAdQty)))

    _lastPoints = New List(Of PlotPoints)({demand.Last, ad.Last})

    ChartData.ClearAndAddRange({New PlotTrend("Demand", Brushes.Blue, New Thickness(2), demand), New PlotTrend("Ad", Brushes.Red, New Thickness(2), ad)})
    Dim distinctCounts = (ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).Distinct.Count - 1)
    XTicks = If(distinctCounts > 0, distinctCounts, 1)
  End Sub

  Private Sub AddingLinesForLineChart()

    _lastPoints = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 12)), New PlotPoint(Of Double)(1200)),
                                          New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 12)), New PlotPoint(Of Double)(1200)),
                                          New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 10)), New PlotPoint(Of Double)(950))})

    Dim o = New ObservableCollection(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(400)),
                                                                              New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 11)), New PlotPoint(Of Double)(800)),
                                                                              _lastPoints(0)})

    Dim o2 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(150)),
                                    New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 11)), New PlotPoint(Of Double)(720)),
                                    _lastPoints(1)})

    Dim o3 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 1)), New PlotPoint(Of Double)(300)),
                                    New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(1720)),
                                    New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 7)), New PlotPoint(Of Double)(420)),
                                    New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 8)), New PlotPoint(Of Double)(920)),
                                    _lastPoints(2)})

    Dim o4 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(1920))})

    Dim o5 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(New DateTime(2017, 2, 5)), New PlotPoint(Of Double)(2920))})

    ChartData.ClearAndAddRange({New PlotTrend("First", Brushes.Blue, New Thickness(2), o), New PlotTrend("Second", Brushes.Red, New Thickness(2), o2),
                               New PlotTrend("Thrid", Brushes.Purple, New Thickness(2), o3), New PlotTrend("Fourth", Brushes.Brown, New Thickness(2), o4)})
    Dim distinctCounts = (ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).Distinct.Count - 1)
    XTicks = If(distinctCounts > 0, distinctCounts, 1)
  End Sub

  Private Sub LinePlotAdding()
    Dim newPoints = New List(Of PlotPoints)

    For i = 1 To _lastPoints.Count
      newPoints.Add(New PlotPoints(New PlotPoint(Of DateTime)((DirectCast(_lastPoints(i - 1).X, PlotPoint(Of DateTime)).Point).AddDays(1)), New PlotPoint(Of Double)(DirectCast(_lastPoints(i - 1).Y, PlotPoint(Of Double)).Point * 1.95)))
    Next

    _lastPoints = newPoints
    ChartData(0).Points.Add(_lastPoints(0))
    ChartData(1).Points.Add(_lastPoints(1))

    Dim distinctCounts = (ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).Distinct.Count - 1)
    XTicks = If(distinctCounts > 0, distinctCounts, 1)
  End Sub

#End Region
End Class
