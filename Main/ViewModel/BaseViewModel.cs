using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
  public abstract class BaseViewModel : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(String info)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
    }                                               
    #endregion
  }
}                                                                