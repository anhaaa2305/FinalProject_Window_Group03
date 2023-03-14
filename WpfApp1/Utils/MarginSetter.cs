using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Utils;

public class MarginSetter
{
	public static Thickness GetMargin(DependencyObject obj) => (Thickness)obj.GetValue(MarginProperty);

	public static void SetMargin(DependencyObject obj, Thickness value) => obj.SetValue(MarginProperty, value);

	public static readonly DependencyProperty MarginProperty =
		DependencyProperty.RegisterAttached(nameof(FrameworkElement.Margin), typeof(Thickness),
			typeof(MarginSetter), new UIPropertyMetadata(new Thickness(), MarginChangedCallback));

	public static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (sender is not Panel panel)
		{
			return;
		}
		panel.Loaded += Panel_Loaded;
	}

	private static void Panel_Loaded(object sender, EventArgs e)
	{
		if (sender is not Panel panel)
		{
			return;
		}
		foreach (FrameworkElement fe in panel.Children.OfType<FrameworkElement>())
		{
			fe.Margin = GetMargin(panel);
		}
	}
}
