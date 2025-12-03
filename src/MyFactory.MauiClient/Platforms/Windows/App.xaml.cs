using System;
using System.Diagnostics;
using System.IO;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyFactory.MauiClient.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
		UnhandledException += OnUnhandledException;
	}

	private static readonly string WinUiLogPath = Path.Combine(AppContext.BaseDirectory, "winui-errors.log");

	private static void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
	{
		var message = $"{DateTime.Now:O} {e.Exception}";
		try
		{
			File.AppendAllText(WinUiLogPath, message + Environment.NewLine);
		}
		catch (Exception logError)
		{
			Debug.WriteLine($"Failed to write WinUI error log: {logError}");
		}

		Trace.WriteLine(message);
		Trace.WriteLine("WinUI Unhandled Exception: " + e.Exception);
		// keep the crash visible to highlight the faulty screen but make the reason discoverable
		e.Handled = false;
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

