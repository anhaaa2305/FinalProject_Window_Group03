using System.Windows;
using WpfApp1.Services;
using WpfApp1.ViewModels;
using WpfApp1.Views.UserControls;

namespace WpfApp1.Views;

public partial class MainWindow : Window
{
	public MainWindow(MainWindowViewModel vm, INavigationService navigationService)
	{
		InitializeComponent();

		DataContext = vm;
		navigationService.Navigate<LoginUserControl>();
	}
}
