using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.Pages;

public partial class LoginPage : Page
{
	public LoginPage(LoginPageViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
