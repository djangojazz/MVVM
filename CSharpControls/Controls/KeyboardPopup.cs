using System;      
using System.Windows.Input;

namespace CSharpControls
{
  public class KeyboardPopup : System.Windows.Controls.Primitives.Popup
  {
    protected override void OnOpened(EventArgs e)
    {
      base.OnOpened(e);
      this.Child.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }
  }
}