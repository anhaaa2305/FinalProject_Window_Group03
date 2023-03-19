using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.User;

public partial class AvailableVehicleView : UserControl
{
	public AvailableVehicleView(AvailableVehicleViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
