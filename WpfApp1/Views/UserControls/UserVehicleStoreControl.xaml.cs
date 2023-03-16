using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.UserControls;

public partial class UserVehicleStoreControl : UserControl
{
	public UserVehicleStoreControl(UserVehicleStoreControlViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
