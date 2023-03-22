using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class LoginView : UserControl
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
