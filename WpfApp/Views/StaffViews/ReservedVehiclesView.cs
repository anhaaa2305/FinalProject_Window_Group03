using System.Windows.Controls;
using WpfApp.ViewModels.StaffViewModels;

namespace WpfApp.Views.StaffViews;

public partial class ReservedVehiclesView : UserControl
{
	public ReservedVehiclesView(ReservedVehiclesViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
