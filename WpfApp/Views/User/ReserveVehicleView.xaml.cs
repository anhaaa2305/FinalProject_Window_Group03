using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class ReserveVehicleView : UserControl
{
	public ReserveVehicleView(ReserveVehicleViewModel vm)
	{
		InitializeComponent();
		DataContext = vm;
	}
}
