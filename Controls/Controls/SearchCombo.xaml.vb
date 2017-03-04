Imports System.Collections.ObjectModel
Imports System.Windows
Imports System.Linq
Imports System.Windows.Controls.Primitives
Imports System.Windows.Input

Public Class SearchCombo

  'DEPENDENCY PROPERTIES
  Public Shared ReadOnly FilterTextProperty As DependencyProperty = DependencyProperty.Register("FilterText", GetType(String), GetType(SearchCombo), New UIPropertyMetadata(String.Empty, AddressOf ReloadFilteredList))
  Public Shared ReadOnly IsExpandedProperty As DependencyProperty = DependencyProperty.Register("IsExpanded", GetType(Boolean), GetType(SearchCombo), New UIPropertyMetadata(False, AddressOf ExpandChanged))
  Public Shared ReadOnly MaxResultsProperty As DependencyProperty = DependencyProperty.Register("MaxResults", GetType(Integer), GetType(SearchCombo), New UIPropertyMetadata(500I, AddressOf ReloadFilteredList))
  Public Shared ReadOnly CompleteCollectionProperty As DependencyProperty = DependencyProperty.Register("CompleteCollection", GetType(IList), GetType(SearchCombo), New UIPropertyMetadata(New ObservableCollection(Of Object), AddressOf ReloadFilteredList))
  Public Shared ReadOnly FilteredCollectionProperty As DependencyProperty = DependencyProperty.Register("FilteredCollection", GetType(ObservableCollection(Of Object)), GetType(SearchCombo), New UIPropertyMetadata(New ObservableCollection(Of Object), AddressOf ReloadFilteredList))
  Public Shared ReadOnly SelectCommandProperty As DependencyProperty = DependencyProperty.Register("SelectCommand", GetType(Input.ICommand), GetType(SearchCombo), New UIPropertyMetadata(Nothing))
  Public Shared ReadOnly SelectedProperty As DependencyProperty = DependencyProperty.Register("Selected", GetType(Object), GetType(SearchCombo), New UIPropertyMetadata(String.Empty))
  Public Shared ReadOnly SelectedFilterProperty As DependencyProperty = DependencyProperty.Register("SelectedFilter", GetType(String), GetType(SearchCombo), New UIPropertyMetadata(String.Empty))
  Public Shared ReadOnly PlacementProperty As DependencyProperty = DependencyProperty.Register("Placement", GetType(PlacementMode), GetType(SearchCombo), New UIPropertyMetadata(PlacementMode.Bottom))


  'CONSTRUCTOR
  Public Sub New()
    InitializeComponent()

    Me.FilteredCollection = New ObservableCollection(Of Object)
    Part_Layout.DataContext = Me
  End Sub


  'PROPERTIES
  Public Property FilterText As String
    Get
      Return CStr(GetValue(FilterTextProperty))
    End Get
    Set(value As String)
      SetValue(FilterTextProperty, value)
    End Set
  End Property

  Public Property IsExpanded As Boolean
    Get
      Return CBool(GetValue(IsExpandedProperty))
    End Get
    Set(value As Boolean)
      SetValue(IsExpandedProperty, value)
    End Set
  End Property

  Public Property MaxResults As Integer
    Get
      Return CInt(GetValue(MaxResultsProperty))
    End Get
    Set(value As Integer)
      SetValue(MaxResultsProperty, value)
    End Set
  End Property

  Public Property CompleteCollection As IList
    Get
      Return CType(GetValue(CompleteCollectionProperty), IList)
    End Get
    Set(value As IList)
      SetValue(CompleteCollectionProperty, value)
    End Set
  End Property

  Public Property FilteredCollection As ObservableCollection(Of Object)
    Get
      Return CType(GetValue(FilteredCollectionProperty), ObservableCollection(Of Object))
    End Get
    Private Set(value As ObservableCollection(Of Object))
      SetValue(FilteredCollectionProperty, value)
    End Set
  End Property

  Public Property Placement As PlacementMode
    Get
      Return CType(GetValue(PlacementProperty), PlacementMode)
    End Get
    Private Set(value As PlacementMode)
      SetValue(PlacementProperty, value)
    End Set
  End Property

  Public Property SelectCommand As Input.ICommand
    Get
      Return CType(GetValue(SelectCommandProperty), Input.ICommand)
    End Get
    Set(value As Input.ICommand)
      SetValue(SelectCommandProperty, value)
    End Set
  End Property

  Public Property Selected As Object
    Get
      Return GetValue(SelectedProperty)
    End Get
    Set(value As Object)
      SetValue(SelectedProperty, value)
    End Set
  End Property

  Public Property SelectedFilter As String
    Get
      Return CStr(GetValue(SelectedProperty))
    End Get
    Set(value As String)
      SetValue(SelectedProperty, value)
    End Set
  End Property

  Public Property CommandSelect As New DelegateCommand(Of Object)(AddressOf SelectElement)

  Public Property CommandEsc As New DelegateCommand(Of Object)(AddressOf CloseControl)


  'METHODS
  Private Shared Sub ReloadFilteredList(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    DirectCast(d, SearchCombo).LoadFilterSet()
  End Sub

  Private Sub SelectElement(o As Object)
    If IsNothing(o) Then Exit Sub
    Dim EnumerableObjects = From p In CompleteCollection
    Selected = EnumerableObjects.Where(Function(X) X.ToString = o.ToString).FirstOrDefault
    FocusButton()
    If Not IsNothing(SelectCommand) Then SelectCommand.Execute(Selected)
  End Sub

  Private Sub CloseControl(o As Object)
    If IsExpanded Then FocusButton()
  End Sub

  Public Shared Sub ExpandChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    Dim Control = DirectCast(d, SearchCombo)
    If Not Control.IsExpanded Then
      Control.FocusButton()
    Else
      Control.LoadFilterSet()
    End If
  End Sub

  Public Sub FocusButton()
    IsExpanded = False
    Part_Button.Focus()
  End Sub

  Public Sub LoadFilterSet()
    If Not IsExpanded Then Exit Sub
    Dim EnumerableObjects = From p In CompleteCollection
    FilteredCollection.ClearAndAddRange(EnumerableObjects.Where(Function(t) t.ToString.ContainsInvariant(FilterText)).Take(MaxResults))
  End Sub

End Class
