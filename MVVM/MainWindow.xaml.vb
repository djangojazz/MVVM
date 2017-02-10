Class MainWindow
  Public Sub New()
    InitializeComponent()
    Me.DataContext = New MainWindowViewModel()
  End Sub
End Class
