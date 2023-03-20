using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.DAOs;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.ViewModels;

public class RentingLogViewModel : ObservableObject
{
	private ObservableCollection<UserRentingLogModel>? records;
	private ViewState state;

	public IRelayCommand<UserRentingLogModel> ViewItemDetailsCommand { get; }

	public ObservableCollection<UserRentingLogModel>? Records
	{
		get => records;
		set => SetProperty(ref records, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	private readonly IUserDAO userDAO;
	public RentingLogViewModel(IUserDAO userDAO)
	{
		this.userDAO = userDAO;
		ViewItemDetailsCommand = new RelayCommand<UserRentingLogModel>(ViewItemDetails);

		GetRentingLogsAsync().SafeFireAndForget();
	}

	private async Task GetRentingLogsAsync()
	{
		State = ViewState.Busy;
		var records = await userDAO.GetAllRentingLogsAsync(new UserModel { }).ConfigureAwait(false);
		App.Current.Dispatcher.Invoke(() =>
		{
			Records = new ObservableCollection<UserRentingLogModel>(records);
			if (Records.Count == 0)
			{
				State = ViewState.Empty;
			}
			else
			{
				State = ViewState.Present;
			}
		});
	}

	private void ViewItemDetails(UserRentingLogModel? model)
	{
		if (Records is null || model is null)
		{
			return;
		}
	}
}
