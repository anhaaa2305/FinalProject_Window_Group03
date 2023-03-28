using System.Windows.Controls;
using WpfApp.ViewModels.UserViewModels;

namespace WpfApp.Views.UserViews;

public partial class RentedVehicleView : UserControl
{
	public RentedVehicleView(RentedVehicleViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
