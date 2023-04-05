using System.Windows.Controls;
using WpfApp.ViewModels.StaffViewModels;

namespace WpfApp.Views.StaffViews;

public partial class RentedVehiclesView : UserControl
{
	public RentedVehiclesView(RentedVehiclesViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
