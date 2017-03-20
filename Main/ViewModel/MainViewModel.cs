using CSharpControls.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Main
{
  public class MainViewModel : BaseViewModel
  { 
    public MainViewModel()
    {
      _text = "Test";
      TestCommand = new DelegateCommand<string>(TestCommandExecute);
      CountsTest = 1;
    }

    private int _countsTest;

    public int CountsTest
    {
      get { return _countsTest; }
      set
      {
        _countsTest = value;
        OnPropertyChanged(nameof(CountsTest));
      }
    }
                     
    #region Text
    private string _text;

    public string Text
    {
      get { return _text; }
      set
      {
        _text = value;
        OnPropertyChanged(nameof(Text));
      }
    } 
    #endregion

    public DelegateCommand<string> TestCommand { get; }

    private void TestCommandExecute(string input)
    {
      Text = "Updated Test";
      CountsTest = 20;
    }
  }
}
