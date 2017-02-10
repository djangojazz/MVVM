﻿Imports System.Windows.Threading

Public Class BaseChart
  Inherits UserControl
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


#Region "Dependent Properties"


#Region "ChartTitle"
  Public Shared ReadOnly ChartTitleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartTitle), GetType(String), GetType(LineChart), New UIPropertyMetadata(String.Empty))

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
  Public Shared ReadOnly ChartForegroundProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartForeground), GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly ChartTitleHiddenProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartTitleHidden), GetType(Boolean), GetType(LineChart), New UIPropertyMetadata(False, AddressOf LineChart.ChartTitleHiddenChanged))

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
  Public Shared ReadOnly BackGroundColorProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackGroundColor), GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly BackGroundColorCanvasProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackGroundColorCanvas), GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly BackGroundColorLegendProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackGroundColorLegend), GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly LegendForegroundProperty As DependencyProperty = DependencyProperty.Register(NameOf(LegendForeground), GetType(Brush), GetType(LineChart), New UIPropertyMetadata(Brushes.Black))

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
  Public Shared ReadOnly LegendHiddenProperty As DependencyProperty = DependencyProperty.Register(NameOf(LegendHidden), GetType(Boolean), GetType(LineChart), New UIPropertyMetadata(False, AddressOf LineChart.ChartLegendHiddenChanged))

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

#Region "NumberOfTicks"
  Public Shared ReadOnly NumberOfTicksProperty As DependencyProperty = DependencyProperty.Register(NameOf(NumberOfTicks), GetType(Integer), GetType(LineChart), New UIPropertyMetadata(0))

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
  Public Shared ReadOnly XValueConverterProperty As DependencyProperty = DependencyProperty.Register(NameOf(XValueConverter), GetType(IValueConverter), GetType(LineChart), Nothing)

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
  Public Shared ReadOnly YValueConverterProperty As DependencyProperty = DependencyProperty.Register(NameOf(YValueConverter), GetType(IValueConverter), GetType(LineChart), Nothing)

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
  Public Shared ReadOnly FontTypeProperty As DependencyProperty = DependencyProperty.Register(NameOf(FontType), GetType(FontFamily), GetType(LineChart), Nothing)

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
