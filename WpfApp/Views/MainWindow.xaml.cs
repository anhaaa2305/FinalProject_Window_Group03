﻿using Wpf.Ui.Appearance;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class MainWindow : INavigationWindow
{
	public MainWindow(MainWindowViewModel vm, IPageService pageService, IAppNavigationService navigationService, IVehicleDAO vehicleDAO, IUserDAO userDAO)
	{
		InitializeComponent();

		DataContext = vm;
		SetPageService(pageService);
		navigationService.SetNavigationControl(RootNavigation);

		Loaded += OnLoad;

		Task.Run(async () =>
		{
			await userDAO.AddAsync(new Data.Models.User
			{
				FullName = "user",
				Password = "user"
			});
			for (var i = 0; i != 50; ++i)
			{
				await vehicleDAO.AddAsync(new Vehicle
				{
					Brand = "Yamaha",
					Name = "Yamaha Sirius",
					LicensePlate = "86-B1-" + (35904 + i),
					PricePerDay = 100_000 + (int)Random.Shared.NextInt64(300_000),
					ImageUrl = "http://yamaha-motor.com.vn/wp/wp-content/uploads/2017/05/500x400-Si-RC-trang-den-04.png"
				});
			}
		});
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
