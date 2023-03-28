using System.Windows.Controls;
using WpfApp.ViewModels.UserViewModels;

namespace WpfApp.Views.UserViews;

public partial class RentalLogView : UserControl
{
	public RentalLogView(RentalLogViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
