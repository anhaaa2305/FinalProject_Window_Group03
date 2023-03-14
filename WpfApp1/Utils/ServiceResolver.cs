using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1.Utils;

public class ServiceResolver : MarkupExtension
{
	private readonly Type type;
	public ServiceResolver(Type type)
	{
		this.type = type;
	}
	public override object ProvideValue(IServiceProvider serviceProvider) => App.Current.Services.GetRequiredService(type);
}
