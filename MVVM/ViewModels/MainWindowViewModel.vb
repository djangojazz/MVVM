﻿Imports System.Collections.ObjectModel
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


    Dt = New DateTime(2017, 2, 12)
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
                               New PlotTrend("Third", Brushes.Purple, New Thickness(2), o3), New PlotTrend("Fourth", Brushes.Orange, New Thickness(2), o4), 
                               New PlotTrend("Fifth", Brushes.Brown, New Thickness(2), o5) })
  End Sub

  Private Sub LinePlotAdding()
    Dim newPoints = New List(Of PlotPoints)

    For i = 1 To _lastPoints.Count
      newPoints.Add(New PlotPoints(New PlotPoint(Of DateTime)((DirectCast(_lastPoints(i - 1).X, PlotPoint(Of DateTime)).Point).AddDays(1)), New PlotPoint(Of Double)(DirectCast(_lastPoints(i - 1).Y, PlotPoint(Of Double)).Point * 1.95)))
    Next

    _lastPoints = newPoints
    ChartData(0).Points.Add(_lastPoints(0))
    ChartData(1).Points.Add(_lastPoints(1))
  End Sub

#End Region

End Class
