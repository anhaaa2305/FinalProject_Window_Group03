using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using WpfApp.ViewModels.UserViewModels;

namespace WpfApp.Views.UserViews;

public partial class RentedVehicleView : UserControl
{
	public RentedVehicleView(RentedVehicleViewModel vm)
	{
		InitializeComponent();

		Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
		DataContext = vm;
	}
}
