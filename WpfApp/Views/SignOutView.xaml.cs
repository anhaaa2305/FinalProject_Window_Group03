using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class SignOutView : UserControl
{
	public SignOutView(SignOutViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
