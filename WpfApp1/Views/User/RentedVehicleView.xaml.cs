using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.User;

public partial class RentedVehicleView : UserControl
{
	public RentedVehicleView(RentedVehicleViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
