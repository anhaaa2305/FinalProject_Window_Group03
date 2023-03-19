using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.User;

public partial class HomeView : UserControl
{
	public HomeView(HomeViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
