using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class AvailableVehicleView : UserControl
{
	public AvailableVehicleView(AvailableVehicleViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
