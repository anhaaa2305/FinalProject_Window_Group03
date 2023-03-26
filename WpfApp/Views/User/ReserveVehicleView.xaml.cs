using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Contracts;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class ReserveVehicleView : UserControl
{
	public ReserveVehicleView(ReserveVehicleViewModel vm, IServiceProvider provider)
	{
		InitializeComponent();
		Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
		provider.GetRequiredService<IDialogService>().SetDialogControl(reserveDialog);
		DataContext = vm;
	}
}
