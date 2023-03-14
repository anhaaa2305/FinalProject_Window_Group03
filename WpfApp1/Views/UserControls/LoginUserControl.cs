using System.Windows;
using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.UserControls;

public partial class LoginUserControl : UserControl
{
	public LoginUserControl(LoginUserControlViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
