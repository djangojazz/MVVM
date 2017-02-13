Imports System.Collections.ObjectModel
Imports Controls

Public NotInheritable Class MainWindowViewModel
  Inherits BaseViewModel

  Private _lastPoints As List(Of PlotPoints)
  Private _testText As String

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

  Public ReadOnly Property ChartData As New ObservableCollectionContentNotifying(Of PlotTrend)
  Public ReadOnly Property ChartData2 As New ObservableCollectionContentNotifying(Of PlotTrend)

  Public Sub New()
    TestText = "Hello there"
    AddingLinesForLineChart()
    Dt = DateTime.Now.AddDays(-3)
  End Sub

  Public ReadOnly Property TestCommand As New DelegateCommand(Of Object)(AddressOf TestCommandExecute)

  Private Sub TestCommandExecute()
    'Items.Add(New Stuff With {.Id = 3, .ShipType = ShipType.Other, .Value = "Boat3"})
    'TestText += " some more stuff"
    LinePlotAdding()
    TestText = "Line Chart Hello there" + DateTime.Now.ToLongTimeString
  End Sub

#Region "Line Graph parts"
  Private Sub AddingLinesForLineChart()
    _lastPoints = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(DateTime.Now), New PlotPoint(Of Double)(930)), New PlotPoints(New PlotPoint(Of DateTime)(DateTime.Now), New PlotPoint(Of Double)(950))})

    Dim o = New ObservableCollection(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(DateTime.Now.AddDays(-10)), New PlotPoint(Of Double)(930)),
                                                                              New PlotPoints(New PlotPoint(Of DateTime)(DateTime.Now.AddDays(-5)), New PlotPoint(Of Double)(850)),
                                                                              _lastPoints(0)})

    Dim o2 = New List(Of PlotPoints)({New PlotPoints(New PlotPoint(Of DateTime)(DateTime.Now.AddDays(-8)), New PlotPoint(Of Double)(600)),
                                    New PlotPoints(New PlotPoint(Of DateTime)(DateTime.Now.AddDays(-4)), New PlotPoint(Of Double)(720)),
                                    _lastPoints(0)})

    ChartData.ClearAndAddRange({New PlotTrend("First", Brushes.Blue, New Thickness(2), o)})
    ', New PlotTrend("Second", Brushes.Red, New Thickness(2), o2)})
  End Sub

  Private Sub LinePlotAdding()
    Dim newPoints = New List(Of PlotPoints)

    For i = 1 To _lastPoints.Count
      newPoints.Add(New PlotPoints(New PlotPoint(Of DateTime)((DirectCast(_lastPoints(i - 1).X, PlotPoint(Of DateTime)).Point).AddDays(1)), New PlotPoint(Of Double)(DirectCast(_lastPoints(i - 1).Y, PlotPoint(Of Double)).Point * 1.95)))
    Next

    _lastPoints = newPoints
    ChartData(0).Points.Add(_lastPoints(0))
    'ChartData(1).Points.Add(_lastPoints(1))
  End Sub

#End Region

End Class
