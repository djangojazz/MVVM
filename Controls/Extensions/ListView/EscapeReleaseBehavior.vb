Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows
Imports System.Windows.Media
Imports Prism.Interactivity

Namespace Extensions.ListView

  Public NotInheritable Class EscapeReleaseBehavior
    Inherits CommandBehaviorBase(Of Windows.Controls.ListView)

    Public ClickedItem As ListViewItem

    Public Sub New(ByVal Element As Windows.Controls.ListView)
      MyBase.New(Element)

      AddHandler Element.KeyUp, AddressOf Element_KeyReleased
    End Sub

    Private Sub Element_KeyReleased(sender As Object, e As KeyEventArgs)
      If e.Key = Key.Escape Then
        Dim dep As DependencyObject = CType(e.OriginalSource, DependencyObject)
        Do While dep IsNot Nothing AndAlso Not TypeOf (dep) Is ListViewItem
          dep = VisualTreeHelper.GetParent(dep)
        Loop
        ClickedItem = DirectCast(dep, ListViewItem)

        MyBase.ExecuteCommand(Nothing)
      End If
    End Sub

  End Class


  Public NotInheritable Class EscapeReleased

    'DEPENDENCY PROPERTIES
    ' Using a DependencyProperty as the backing store for Command. This enables animation, styling, binding, etc... 
    Public Shared ReadOnly CommandProperty As DependencyProperty = DependencyProperty.RegisterAttached("Command", GetType(ICommand), GetType(EscapeReleased), New PropertyMetadata(AddressOf OnSetCommandCallback))
    Public Shared ReadOnly CommandParameterProperty As DependencyProperty = DependencyProperty.RegisterAttached("CommandParameter", GetType(Object), GetType(EscapeReleased), New PropertyMetadata(AddressOf OnSetCommandParameterCallback))
    Public Shared ReadOnly EscapeReleaseBehaviorProperty As DependencyProperty = DependencyProperty.RegisterAttached("EscapeReleaseBehavior", GetType(EscapeReleaseBehavior), GetType(EscapeReleased), Nothing)


    'METHODS
    Private Shared Function GetOrCreateBehavior(ByVal element As Windows.Controls.ListView) As EscapeReleaseBehavior
      Dim behavior As EscapeReleaseBehavior = TryCast(element.GetValue(EscapeReleaseBehaviorProperty), EscapeReleaseBehavior)
      If behavior Is Nothing Then
        behavior = New EscapeReleaseBehavior(element)
        element.SetValue(EscapeReleaseBehaviorProperty, behavior)
      End If
      Return behavior
    End Function

    Private Shared Sub OnSetCommandCallback(ByVal dependencyObject As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
      Dim element As Windows.Controls.ListView = TryCast(dependencyObject, Windows.Controls.ListView)
      If element IsNot Nothing Then
        Dim behavior As EscapeReleaseBehavior = GetOrCreateBehavior(element)
        behavior.Command = TryCast(e.NewValue, ICommand)
      End If
    End Sub

    Private Shared Sub OnSetCommandParameterCallback(ByVal dependencyObject As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
      Dim element As Windows.Controls.ListView = TryCast(dependencyObject, Windows.Controls.ListView)
      If element IsNot Nothing Then
        Dim behavior As EscapeReleaseBehavior = GetOrCreateBehavior(element)
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

    Public Shared Function GetEscapeReleaseBehavior(ByVal obj As DependencyObject) As EscapeReleaseBehavior
      Return DirectCast(obj.GetValue(EscapeReleaseBehaviorProperty), EscapeReleaseBehavior)
    End Function

    Public Shared Sub SetEscapeReleaseBehavior(ByVal obj As DependencyObject, ByVal value As EscapeReleaseBehavior)
      obj.SetValue(EscapeReleaseBehaviorProperty, value)
    End Sub

  End Class

End Namespace

