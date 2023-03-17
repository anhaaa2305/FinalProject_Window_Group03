using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.UserControls;

public partial class UserHomeControl : UserControl
{
	public UserHomeControl(UserHomeControlViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
	}
}