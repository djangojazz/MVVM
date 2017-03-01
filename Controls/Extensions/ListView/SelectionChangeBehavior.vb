Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows
Imports System.Windows.Media
Imports Prism.Interactivity

Namespace Extensions.ListView

  Public NotInheritable Class SelectionChangeBehavior
    Inherits CommandBehaviorBase(Of Windows.Controls.ListView)

    Public ClickedItem As ListViewItem

    Public Sub New(ByVal Element As Windows.Controls.ListView)
      MyBase.New(Element)

      AddHandler Element.SelectionChanged, AddressOf Element_Clicked
    End Sub

    Public Sub Element_Clicked(ByVal Sender As Object, ByVal e As SelectionChangedEventArgs)
      Dim dep As DependencyObject = CType(e.OriginalSource, DependencyObject)
      Do While dep IsNot Nothing AndAlso Not TypeOf (dep) Is ListViewItem
        dep = VisualTreeHelper.GetParent(dep)
      Loop
      ClickedItem = TryCast(dep, ListViewItem)

      If Not IsNothing(ClickedItem) Then MyBase.ExecuteCommand(Nothing)
    End Sub

  End Class


  Public NotInheritable Class SelectionChange

    'DEPENDENCY PROPERTIES
    ' Using a DependencyProperty as the backing store for Command. This enables animation, styling, binding, etc... 
    Public Shared ReadOnly CommandProperty As DependencyProperty = DependencyProperty.RegisterAttached("Command", GetType(ICommand), GetType(SelectionChange), New PropertyMetadata(AddressOf OnSetCommandCallback))
    Public Shared ReadOnly CommandParameterProperty As DependencyProperty = DependencyProperty.RegisterAttached("CommandParameter", GetType(Object), GetType(SelectionChange), New PropertyMetadata(AddressOf OnSetCommandParameterCallback))
    Public Shared ReadOnly ClickBehaviorProperty As DependencyProperty = DependencyProperty.RegisterAttached("SelectedIndexChangedBehavior", GetType(SelectionChangeBehavior), GetType(SelectionChange), Nothing)


    'METHODS
    Private Shared Function GetOrCreateBehavior(ByVal element As Windows.Controls.ListView) As SelectionChangeBehavior
      Dim behavior As SelectionChangeBehavior = TryCast(element.GetValue(ClickBehaviorProperty), SelectionChangeBehavior)
      If behavior Is Nothing Then
        behavior = New SelectionChangeBehavior(element)
        element.SetValue(ClickBehaviorProperty, behavior)
      End If
      Return behavior
    End Function

    Private Shared Sub OnSetCommandCallback(ByVal dependencyObject As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
      Dim element As Windows.Controls.ListView = TryCast(dependencyObject, Windows.Controls.ListView)
      If element IsNot Nothing Then
        Dim behavior As SelectionChangeBehavior = GetOrCreateBehavior(element)
        behavior.Command = TryCast(e.NewValue, ICommand)
      End If
    End Sub

    Private Shared Sub OnSetCommandParameterCallback(ByVal dependencyObject As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
      Dim element As Windows.Controls.ListView = TryCast(dependencyObject, Windows.Controls.ListView)
      If element IsNot Nothing Then
        Dim behavior As SelectionChangeBehavior = GetOrCreateBehavior(element)
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

    Public Shared Function GetClickBehavior(ByVal obj As DependencyObject) As SelectionChangeBehavior
      Return DirectCast(obj.GetValue(ClickBehaviorProperty), SelectionChangeBehavior)
    End Function

    Public Shared Sub SetClickBehavior(ByVal obj As DependencyObject, ByVal value As SelectionChangeBehavior)
      obj.SetValue(ClickBehaviorProperty, value)
    End Sub

  End Class

End Namespace
