using Wpf.Ui.Appearance;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class MainWindow : INavigationWindow
{
	public MainWindow(MainWindowViewModel vm, IPageService pageService, IAppNavigationService navigationService)
	{
		InitializeComponent();

		DataContext = vm;
		SetPageService(pageService);
		navigationService.SetNavigationControl(RootNavigation);

		Loaded += OnLoad;
	}

	public void CloseWindow()
	{
		Close();
	}

	public INavigationView GetNavigation()
	{
		throw new NotImplementedException();
	}

	public bool Navigate(Type pageType)
	{
		return RootNavigation.Navigate(pageType);
	}

	public void SetPageService(IPageService pageService)
	{
		RootNavigation.SetPageService(pageService);
	}

	public void SetServiceProvider(IServiceProvider serviceProvider)
	{
		throw new NotImplementedException();
	}

	public void ShowWindow()
	{
		Show();
	}

	private void OnLoad(object? sender, EventArgs e)
	{
		Navigate(typeof(LoginView));
		Watcher.Watch(this, WindowBackdropType.Mica, true);
	}
}
