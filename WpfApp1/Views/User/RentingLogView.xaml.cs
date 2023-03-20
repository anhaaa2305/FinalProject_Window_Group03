using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.User;

public partial class RentingLogView : UserControl
{
	public RentingLogView(RentingLogViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}
