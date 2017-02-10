Public Class BaseTest
  Inherits UserControl


  Public Shared ReadOnly TestTextProperty As DependencyProperty = DependencyProperty.Register("TestText", GetType(String), GetType(BaseTest), New PropertyMetadata(Nothing))

  Public Property TestText As String
    Get
      Return GetValue(TestTextProperty)
    End Get

    Set(ByVal value As String)
      SetValue(TestTextProperty, value)
    End Set
  End Property
End Class
