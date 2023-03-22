using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.User;

public partial class RentalLogView : UserControl
{
	public RentalLogView(RentalLogViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
