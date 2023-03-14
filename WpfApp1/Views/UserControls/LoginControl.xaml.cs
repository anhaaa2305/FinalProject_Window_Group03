using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.UserControls;

public partial class LoginControl : UserControl
{
	public LoginControl(LoginControlViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
