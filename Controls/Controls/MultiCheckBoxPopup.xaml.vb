Imports System.Collections.ObjectModel
Imports System.Windows.Controls.Primitives

Public Class MultiCheckBoxPopup
  Public Sub New()
    InitializeComponent()
    PART_Main.DataContext = Me
  End Sub

#Region "ScrollingHeight"
  Public Shared ReadOnly ScrollingHeightProperty As DependencyProperty = DependencyProperty.Register("ScrollingHeight", GetType(Integer), GetType(MultiCheckBoxPopup), New PropertyMetadata(400))

  Public Property ScrollingHeight As String
    Get
      Return GetValue(ScrollingHeightProperty)
    End Get

    Set(ByVal value As String)
      SetValue(ScrollingHeightProperty, value)
    End Set
  End Property

#End Region

#Region "Header"
  Public Shared ReadOnly HeaderProperty As DependencyProperty = DependencyProperty.Register("Header", GetType(String), GetType(MultiCheckBoxPopup), New PropertyMetadata(String.Empty))

  Public Property Header As String
    Get
      Return GetValue(HeaderProperty)
    End Get

    Set(ByVal value As String)
      SetValue(HeaderProperty, value)
    End Set
  End Property
#End Region


#Region "ItemsCollection"
  Public Shared ReadOnly ItemsCollectionProperty As DependencyProperty = DependencyProperty.Register("ItemsCollection", GetType(IList), GetType(MultiCheckBoxPopup), New UIPropertyMetadata(Nothing))

  Public Property ItemsCollection As IList
    Get
      Return GetValue(ItemsCollectionProperty)
    End Get

    Set(ByVal value As IList)
      SetValue(ItemsCollectionProperty, value)
    End Set
  End Property
#End Region
  'Public Shared ReadOnly ItemsCollectionProperty As DependencyProperty = DependencyProperty.Register("ItemsCollection", GetType(ObservableCollection(Of Object)), GetType(MultiCheckBoxPopup), New UIPropertyMetadata(Nothing, AddressOf ItemCollectionChanged))

  'Private Shared Sub ItemCollectionChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
  '  Dim o = DirectCast(d, MultiCheckBoxPopup)

  '  If Not IsNothing(e.OldValue) Then
  '    Dim OldCollection = TryCast(e.OldValue, ObservableCollection(Of Object))
  '    RemoveHandler OldCollection.CollectionChanged, AddressOf o.CollectionChanged
  '  End If

  '  If Not IsNothing(e.NewValue) Then
  '    Dim NewCollection = TryCast(e.NewValue, ObservableCollection(Of Object))
  '    AddHandler NewCollection.CollectionChanged, AddressOf o.CollectionChanged
  '  End If
  'End Sub

  'Private Sub CollectionChanged()
  '  ItemsCollection = New ObservableCollection(Of Object)
  'End Sub

  'Public Property ItemsCollection As ObservableCollection(Of Object)
  '  Get
  '    Return GetValue(ItemsCollectionProperty)
  '  End Get

  '  Set(ByVal value As ObservableCollection(Of Object))
  '    SetValue(ItemsCollectionProperty, value)
  '  End Set
  'End Property

  Public Shared ReadOnly PlacementProperty As DependencyProperty = DependencyProperty.Register("Placement", GetType(PlacementMode), GetType(MultiCheckBoxPopup), New UIPropertyMetadata(PlacementMode.Bottom))

  Public Property Placement As PlacementMode
    Get
      Return CType(GetValue(PlacementProperty), PlacementMode)
    End Get
    Private Set(value As PlacementMode)
      SetValue(PlacementProperty, value)
    End Set
  End Property

  Public ReadOnly Property CommandUpdateHeader As New DelegateCommand(Of Object)(AddressOf CommandUpdateHeaderExecute)

  Private Sub CommandUpdateHeaderExecute(obj As Object)
    'Dim item To examine = 

    'Dim itemsSelected = ItemsCollection.Where(Function(x) x.Value = True).Select(Function(x) x.Key)
    'Dim headerUpdated = If(itemsSelected.Any, String.Concat(", ", itemsSelected), "No Items")

    'Header = headerUpdated
  End Sub
End Class
'