using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Utils;

public class SpacingSetter
{
	public static Thickness GetSpacing(DependencyObject obj) => (Thickness)obj.GetValue(SpacingProperty);

	public static void SetSpacing(DependencyObject obj, Thickness value) => obj.SetValue(SpacingProperty, value);

	public static readonly DependencyProperty SpacingProperty =
		DependencyProperty.RegisterAttached("Spacing", typeof(Thickness),
			typeof(SpacingSetter), new UIPropertyMetadata(new Thickness(), SpacingChangedCallback));

	public static void SpacingChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
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
		foreach (FrameworkElement fe in panel.Children.OfType<FrameworkElement>().Skip(1))
		{
			var thickness = GetSpacing(panel);
			fe.Margin = new Thickness(thickness.Left, thickness.Top, 0, 0); ;
		}
	}
}
