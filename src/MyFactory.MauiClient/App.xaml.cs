using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MyFactory.MauiClient;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	private static readonly string AppLogPath = Path.Combine(AppContext.BaseDirectory, "app-errors.log");

	private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
	{
		WriteLog("Domain", e.ExceptionObject as Exception);
	}

	private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		WriteLog("Task", e.Exception);
		e.SetObserved();
	}

	private static void WriteLog(string source, Exception? exception)
	{
		var message = $"{DateTime.Now:O} [{source}] {exception}";
		try
		{
			File.AppendAllText(AppLogPath, message + Environment.NewLine);
		}
		catch (Exception logError)
		{
			Trace.WriteLine($"Failed to write {source} error log: {logError}");
		}

		Trace.WriteLine(message);
	}
}