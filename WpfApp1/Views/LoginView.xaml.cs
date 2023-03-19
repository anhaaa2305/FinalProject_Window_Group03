using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views;

public partial class LoginView : UserControl
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
