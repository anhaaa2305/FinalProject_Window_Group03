using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class HomeView : UserControl
{
	public HomeView(HomeViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
