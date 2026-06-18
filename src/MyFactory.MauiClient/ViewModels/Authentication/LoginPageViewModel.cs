using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Auth;
using MyFactory.MauiClient.Pages.Authentication;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Services.Auth;

namespace MyFactory.MauiClient.ViewModels.Authentication;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;

    private readonly Window? window;

    [ObservableProperty]
    private string login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public LoginPageViewModel(IAuthService authService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;

        window = Application.Current?.Windows.Count > 0
            ? Application.Current.Windows[0]
            : null;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Логин и пароль должны быть заполнены.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var request = new LoginRequest(Login.Trim(), Password);
            await _authService.LoginAsync(request);

            var currentWindow = (Application.Current?.Windows.Count > 0)
                ? Application.Current.Windows[0]
                : null;

            if (Shell.Current is not null)
            {
                await Shell.Current.GoToAsync(nameof(MaterialsListPage));
            }
            else if (currentWindow is not null)
            {
                currentWindow.Page = new AppShell();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;

            var currentWindow = (Application.Current?.Windows.Count > 0)
                ? Application.Current.Windows[0]
                : null;

            if (currentWindow?.Page is not null)
            {
                await currentWindow.Page.DisplayAlertAsync("Ошибка!", ex.Message, "Ок");
            }
            else if (Shell.Current is not null)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "Ок");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToPasswordResetAsync()
    {
        if (Shell.Current is not null)
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
        else if (window?.Page is NavigationPage nav)
        {
            var viewModel = _serviceProvider.GetRequiredService<PasswordResetPageViewModel>();
            await nav.PushAsync(new PasswordResetPage(viewModel));
        }
    }

    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        var currentWindow = (Application.Current?.Windows.Count > 0)
            ? Application.Current.Windows[0]
            : null;

        if (Shell.Current is not null)
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
            return;
        }

        if (currentWindow?.Page is NavigationPage nav)
        {
            var viewModel = _serviceProvider.GetRequiredService<RegisterPageViewModel>();
            await nav.PushAsync(new RegisterPage(viewModel));
            return;
        }

        if (currentWindow is not null)
        {
            var viewModel = _serviceProvider.GetRequiredService<RegisterPageViewModel>();
            currentWindow.Page = new NavigationPage(new RegisterPage(viewModel));
        }
    }
}

