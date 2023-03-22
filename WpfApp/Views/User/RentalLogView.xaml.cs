using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class RentalLogView : UserControl
{
	public RentalLogView(RentalLogViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
