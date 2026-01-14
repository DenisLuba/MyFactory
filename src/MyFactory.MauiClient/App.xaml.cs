using Microsoft.Extensions.DependencyInjection;
using MyFactory.MauiClient.Pages.Authentication;
using MyFactory.MauiClient.Services.Auth;
using System.Diagnostics;

namespace MyFactory.MauiClient;

public partial class App : Application
{
    private readonly IServiceProvider _services;
    private readonly IAuthService _authService;

    public App(IServiceProvider services, IAuthService authService)
    {
        _services = services;
        _authService = authService;
        InitializeComponent();
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Page root = _authService.IsAuthenticated
            ? new AppShell()
            : new NavigationPage(_services.GetRequiredService<LoginPage>());

        var window = new Window(root);
        return window;
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