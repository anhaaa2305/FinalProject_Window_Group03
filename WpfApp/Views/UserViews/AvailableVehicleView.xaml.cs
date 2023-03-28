using System.Windows.Controls;
using WpfApp.ViewModels.UserViewModels;

namespace WpfApp.Views.UserViews;

public partial class AvailableVehicleView : UserControl
{
	public AvailableVehicleView(AvailableVehicleViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
