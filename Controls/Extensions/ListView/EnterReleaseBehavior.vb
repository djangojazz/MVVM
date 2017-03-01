Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows
Imports System.Windows.Media
Imports Prism.Interactivity

Namespace Extensions.ListView

  Public NotInheritable Class EnterReleaseBehavior
    Inherits CommandBehaviorBase(Of Windows.Controls.ListView)

    Public ClickedItem As ListViewItem

    Public Sub New(ByVal Element As Windows.Controls.ListView)
      MyBase.New(Element)

      AddHandler Element.KeyUp, AddressOf Element_KeyReleased
    End Sub

    Private Sub Element_KeyReleased(sender As Object, e As KeyEventArgs)
      If e.Key = Key.Enter Then
        Dim dep As DependencyObject = CType(e.OriginalSource, DependencyObject)
        Do While dep IsNot Nothing AndAlso Not TypeOf (dep) Is ListViewItem
          dep = VisualTreeHelper.GetParent(dep)
        Loop
        ClickedItem = DirectCast(dep, ListViewItem)

        MyBase.ExecuteCommand(Nothing)
      End If
    End Sub

  End Class


  Public NotInheritable Class EnterReleased

    'DEPENDENCY PROPERTIES
    ' Using a DependencyProperty as the backing store for Command. This enables animation, styling, binding, etc... 
    Public Shared ReadOnly CommandProperty As DependencyProperty = DependencyProperty.RegisterAttached("Command", GetType(ICommand), GetType(EnterReleased), New PropertyMetadata(AddressOf OnSetCommandCallback))
    Public Shared ReadOnly CommandParameterProperty As DependencyProperty = DependencyProperty.RegisterAttached("CommandParameter", GetType(Object), GetType(EnterReleased), New PropertyMetadata(AddressOf OnSetCommandParameterCallback))
    Public Shared ReadOnly EnterReleaseBehaviorProperty As DependencyProperty = DependencyProperty.RegisterAttached("EnterReleaseBehavior", GetType(EnterReleaseBehavior), GetType(EnterReleased), Nothing)


    'METHODS
    Private Shared Function GetOrCreateBehavior(ByVal element As Windows.Controls.ListView) As EnterReleaseBehavior
      Dim behavior As EnterReleaseBehavior = TryCast(element.GetValue(EnterReleaseBehaviorProperty), EnterReleaseBehavior)
      If behavior Is Nothing Then
        behavior = New EnterReleaseBehavior(element)
        element.SetValue(EnterReleaseBehaviorProperty, behavior)
      End If
      Return behavior
    End Function

    Private Shared Sub OnSetCommandCallback(ByVal dependencyObject As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
      Dim element As Windows.Controls.ListView = TryCast(dependencyObject, Windows.Controls.ListView)
      If element IsNot Nothing Then
        Dim behavior As EnterReleaseBehavior = GetOrCreateBehavior(element)
        behavior.Command = TryCast(e.NewValue, ICommand)
      End If
    End Sub

    Private Shared Sub OnSetCommandParameterCallback(ByVal dependencyObject As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
      Dim element As Windows.Controls.ListView = TryCast(dependencyObject, Windows.Controls.ListView)
      If element IsNot Nothing Then
        Dim behavior As EnterReleaseBehavior = GetOrCreateBehavior(element)
        behavior.CommandParameter = e.NewValue
      End If
    End Sub

    Public Shared Function GetCommand(ByVal obj As DependencyObject) As ICommand
      Return DirectCast(obj.GetValue(CommandProperty), ICommand)
    End Function

    Public Shared Sub SetCommand(ByVal obj As DependencyObject, ByVal value As ICommand)
      obj.SetValue(CommandProperty, value)
    End Sub

    Public Shared Function GetCommandParameter(ByVal Element As Windows.Controls.ListView) As Object
      Return Element.GetValue(CommandParameterProperty)
    End Function

    Public Shared Sub SetCommandParameter(ByVal Element As Windows.Controls.ListView, ByVal parameter As Object)
      Element.SetValue(CommandParameterProperty, parameter)
    End Sub

    Public Shared Function GetEnterReleaseBehavior(ByVal obj As DependencyObject) As EnterReleaseBehavior
      Return DirectCast(obj.GetValue(EnterReleaseBehaviorProperty), EnterReleaseBehavior)
    End Function

    Public Shared Sub SetEnterReleaseBehavior(ByVal obj As DependencyObject, ByVal value As EnterReleaseBehavior)
      obj.SetValue(EnterReleaseBehaviorProperty, value)
    End Sub

  End Class

End Namespace
