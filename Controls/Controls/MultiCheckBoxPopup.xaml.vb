Imports System.Collections.ObjectModel
Imports System.Windows.Controls.Primitives

Public Class MultiCheckBoxPopup

  Public Sub New()
    InitializeComponent()
    PART_Main.DataContext = Me
  End Sub

#Region "IsMultiCheckBoxOpen"
  Public Shared ReadOnly IsMultiCheckBoxOpenProperty As DependencyProperty = DependencyProperty.Register("IsMultiCheckBoxOpen", GetType(Boolean), GetType(MultiCheckBoxPopup), New PropertyMetadata(False))

  Public Property IsMultiCheckBoxOpen As Boolean
    Get
      Return GetValue(IsMultiCheckBoxOpenProperty)
    End Get
    Set(ByVal value As Boolean)
      SetValue(IsMultiCheckBoxOpenProperty, value)
    End Set
  End Property
#End Region

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
End Class
'