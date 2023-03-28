using System.Windows;
using System.Windows.Controls;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Contracts;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models;
using WpfApp.Services;
using WpfApp.Views.UserViews;

namespace WpfApp.ViewModels.UserViewModels;

public class ReserveVehicleViewModel : ObservableObject
{
	private readonly ReserveVehicleModel model;
	private readonly IVehicleDAO vehicleDAO;
	private readonly ISessionService sessionService;
	private readonly IDialogService dialogService;
	private readonly IAppNavigationService navigator;

	private ViewState state;
	private Vehicle? vehicle;
	private DateTime startDate = DateTime.Now;
	private DateTime endDate = DateTime.Today.AddDays(1);
	private int totalPrice;
	private TimeSpan rentalTimeSpan = TimeSpan.Zero;

	public IRelayCommand ReserveCommand { get; }

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public Vehicle? Vehicle
	{
		get => vehicle;
		set => SetProperty(ref vehicle, value);
	}

	public DateTime StartDate
	{
		get => startDate;
		set
		{
			var changed = SetProperty(ref startDate, value);
			if (changed)
			{
				OnDateChanged();
			}
		}
	}

	public int TotalPrice
	{
		get => totalPrice;
		set => SetProperty(ref totalPrice, value);
	}

	public TimeSpan RentalTimeSpan
	{
		get => rentalTimeSpan;
		set => SetProperty(ref rentalTimeSpan, value);
	}

	public DateTime EndDate
	{
		get => endDate;
		set
		{
			var changed = SetProperty(ref endDate, value);
			if (changed)
			{
				OnDateChanged();
			}
		}
	}

	public ReserveVehicleViewModel(ReserveVehicleModel model, IVehicleDAO vehicleDAO, ISessionService sessionService, IDialogService dialogService, IAppNavigationService navigator)
	{
		this.model = model;
		this.vehicleDAO = vehicleDAO;
		this.sessionService = sessionService;
		this.dialogService = dialogService;
		this.navigator = navigator;
		ReserveCommand = new RelayCommand(Reserve);
		GetVehicleAsync().SafeFireAndForget();
	}

	private async Task GetVehicleAsync()
	{
		State = ViewState.Busy;
		await Task.Delay(500);

		var vehicle = await vehicleDAO
			.GetByIdAsync(model.Vehicle.Id)
			.ConfigureAwait(false);
		App.Current.Dispatcher.Invoke(() =>
		{
			if (vehicle is null)
			{
				State = ViewState.Empty;
			}
			else
			{
				if (string.IsNullOrEmpty(vehicle.ImageUrl))
				{
					vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
				}
				Vehicle = vehicle;
				State = ViewState.Present;
				OnDateChanged();
			}
		});
	}

	private void Reserve()
	{
		if (Vehicle is null || model is null)
		{
			return;
		}
		var dialog = dialogService.GetDialogControl();
		var dict = new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/Resources/Dictionaries/User/ReserveVehicleViewStrings.xaml")
		};
		var content = (string?)dict["ReserveDialog_Content"] ?? string.Empty;
		if (content.Length != 0)
		{
			content = string.Format(content, model.Vehicle.Name, RentalTimeSpan.Days, TotalPrice);
		}
		dialog.DialogHeight = 240;
		dialog.Title = (string?)dict["ReserveDialog_Title"] ?? string.Empty;
		dialog.Content = new TextBlock
		{
			Margin = new Thickness(0, 8, 0, 0),
			TextWrapping = TextWrapping.WrapWithOverflow,
			Text = content
		};
		dialog.ButtonLeftName = (string?)dict["ReserveDialog_LeftButton"] ?? string.Empty;
		dialog.ButtonRightName = (string?)dict["ReserveDialog_RightButton"] ?? string.Empty;
		dialog.ButtonLeftClick += OnReserveDialogLeftClick;
		dialog.ButtonRightClick += OnReserveDialogRightClick;
		dialog.Show();
	}

	private async void OnReserveDialogLeftClick(object? _1, RoutedEventArgs _2)
	{
		var affected = await vehicleDAO.AddReservedVehicleAsync(new ReservedVehicle
		{
			User = sessionService.User!,
			Vehicle = model.Vehicle,
			StartDate = StartDate,
			EndDate = EndDate
		}).ConfigureAwait(false);
		if (affected == 0)
		{
			// TODO: Show error.
			return;
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			dialogService.GetDialogControl().Hide();
			navigator.Navigate<RentedVehicleView>();
		});
	}

	private void OnReserveDialogRightClick(object? _1, RoutedEventArgs _2)
	{
		dialogService.GetDialogControl().Hide();
	}

	private void OnDateChanged()
	{
		if ((StartDate - EndDate).Days >= 1)
		{
			EndDate = StartDate;
		}
		RentalTimeSpan = TimeSpan.FromDays(Math.Max(Math.Ceiling((EndDate - StartDate).TotalDays), 1));
		if (Vehicle is not null)
		{
			TotalPrice = RentalTimeSpan.Days * Vehicle.PricePerDay;
		}
	}
}
