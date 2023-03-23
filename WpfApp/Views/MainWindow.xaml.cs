﻿using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Window;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class MainWindow : FluentWindow
{
	public MainWindow(MainWindowViewModel vm)
	{
		InitializeComponent();

		DataContext = vm;
		Loaded += OnLoad;
	}

	private void OnLoad(object? sender, EventArgs e)
	{
		Watcher.Watch(this, WindowBackdropType.Mica, true);
	}
}