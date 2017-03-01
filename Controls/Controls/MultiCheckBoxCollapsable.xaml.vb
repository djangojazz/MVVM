Imports System.Collections.ObjectModel

Public Class MultiCheckBoxCollapsable
  Public Sub New()
    InitializeComponent()
    Part_Expander.DataContext = Me
  End Sub

  Public Shared ReadOnly HeaderProperty As DependencyProperty = DependencyProperty.Register("Header", GetType(String), GetType(MultiCheckBoxCollapsable), New PropertyMetadata(String.Empty))

  Public Property Header As String
    Get
      Return GetValue(HeaderProperty)
    End Get

    Set(ByVal value As String)
      SetValue(HeaderProperty, value)
    End Set
  End Property

  Public Shared ReadOnly ItemsCollectionProperty As DependencyProperty = DependencyProperty.Register("ItemsCollection", GetType(IList), GetType(MultiCheckBoxCollapsable), New PropertyMetadata(Nothing))

  Public Property ItemsCollection As IList
    Get
      Return GetValue(ItemsCollectionProperty)
    End Get

    Set(ByVal value As IList)
      SetValue(ItemsCollectionProperty, value)
    End Set
  End Property




End Class
