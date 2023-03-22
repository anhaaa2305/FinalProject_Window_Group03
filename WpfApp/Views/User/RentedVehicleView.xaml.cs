using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class RentedVehicleView : UserControl
{
	public RentedVehicleView(RentedVehicleViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
