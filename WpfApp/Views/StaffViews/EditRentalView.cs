using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using WpfApp.ViewModels.StaffViewModels;

namespace WpfApp.Views.StaffViews;

public partial class EditRentalView : UserControl
{
	public EditRentalView(EditRentalViewModel vm)
	{
		InitializeComponent();

		Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
		DataContext = vm;
	}
}
