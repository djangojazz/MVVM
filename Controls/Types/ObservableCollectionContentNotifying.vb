﻿Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel

Public Class ObservableCollectionContentNotifying(Of T)
  Inherits ObservableCollection(Of T)
  Implements IDisposable

  'EVENTS
  Public Event OnCollectionItemChanged(ByVal sender As Object, e As ObservableCollectionContentChangedArgs)

  'VARIABLES
  Private _disposedValue As Boolean
  Private _SuspendNotification As Boolean

  'CONSTRUCTOR
  Public Sub New()
    '  If Not TypeOf GetType(T) Is INotifyPropertyChanged Then Throw New NotImplementedException($"ObservableCollectionContentNotifying Does not support this type of object.  Type is {GetType(T).ToString }")
  End Sub

  'METHODS
  Protected Overrides Sub OnCollectionChanged(e As NotifyCollectionChangedEventArgs)
    If Not SuspendNotification Then
      Select Case True
        Case e.Action = NotifyCollectionChangedAction.Add
          RegisterPropertyChangedItems(e.NewItems)
        Case e.Action = NotifyCollectionChangedAction.Remove
          UnregisterPropertyChangedItems(e.OldItems)
        Case e.Action = NotifyCollectionChangedAction.Replace
          UnregisterPropertyChangedItems(e.OldItems)
          RegisterPropertyChangedItems(e.NewItems)
      End Select
      MyBase.OnCollectionChanged(e)
    End If
  End Sub

  Public Property SuspendNotification As Boolean
    Get
      Return _SuspendNotification
    End Get
    Set(value As Boolean)
      Dim Notify = _SuspendNotification And Not value
      _SuspendNotification = value
      If Notify Then OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
      'OnPropertyChanged(New PropertyChangedEventArgs(NameOf(Me.Count)))
    End Set
  End Property

  Protected Overrides Sub OnPropertyChanged(e As PropertyChangedEventArgs)
    If Not _SuspendNotification Then MyBase.OnPropertyChanged(e)
  End Sub

  Protected Overrides Sub ClearItems()
    UnregisterPropertyChangedItems(Me)
    MyBase.ClearItems()
  End Sub

  Private Sub RegisterPropertyChangedItems(items As IList)
    For Each obj As INotifyPropertyChanged In items
      AddHandler obj.PropertyChanged, AddressOf RaiseNotification
    Next
  End Sub

  Private Sub UnregisterPropertyChangedItems(items As IList)
    For Each obj As INotifyPropertyChanged In items
      RemoveHandler obj.PropertyChanged, AddressOf RaiseNotification
    Next
  End Sub

  Private Sub RaiseNotification(ByVal sender As Object, e As PropertyChangedEventArgs)
    RaiseEvent OnCollectionItemChanged(Me, New ObservableCollectionContentChangedArgs With {.ElementChanged = sender, .PropertyChanged = e})
  End Sub

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
  End Sub
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not _disposedValue Then
      If disposing Then
        UnregisterPropertyChangedItems(Me)
      End If
    End If
    _disposedValue = True
  End Sub
End Class

Public Class ObservableCollectionContentChangedArgs
  Inherits EventArgs

  Public Property ElementChanged As Object
  Public Property PropertyChanged As PropertyChangedEventArgs

End Class