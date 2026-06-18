using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Auth;
using MyFactory.MauiClient.Pages.Authentication;
using MyFactory.MauiClient.Services.Auth;
using MyFactory.MauiClient.Services.Users;

namespace MyFactory.MauiClient.ViewModels.Authentication;

public partial class RegisterPageViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUsersService _usersService;

    private readonly Window? window;
    private Guid? userRoleId;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public RegisterPageViewModel(IAuthService authService, IServiceProvider serviceProvider, IUsersService usersService)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
        _usersService = usersService;
        window = Application.Current?.Windows.FirstOrDefault();
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Заполните все поля";
            return;
        }

        if (!string.Equals(Password, ConfirmPassword, StringComparison.Ordinal))
        {
            ErrorMessage = "Пароли не совпадают";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var roleId = await GetUserRoleIdAsync();
            if (roleId is null || roleId == Guid.Empty)
            {
                ErrorMessage = "Роль 'User' не найдена";
                return;
            }

            var registerRequest = new RegisterRequest(Username.Trim(), roleId.Value, Password);
            await _authService.RegisterAsync(registerRequest);

            var loginRequest = new LoginRequest(Username.Trim(), Password);
            await _authService.LoginAsync(loginRequest);

            if (window is not null)
            {
                window.Page = new AppShell();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;

            if (window?.Page is not null)
            {
                await window.Page.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<Guid?> GetUserRoleIdAsync()
    {
        if (userRoleId.HasValue && userRoleId != Guid.Empty)
            return userRoleId;

        var roles = await _usersService.GetRolesAsync();
        userRoleId = roles?.FirstOrDefault(r => string.Equals(r.Name, "User", StringComparison.OrdinalIgnoreCase))?.Id;
        return userRoleId;
    }

    [RelayCommand]
    private async Task NavigateBackAsync()
    {
        if (Shell.Current is not null)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        else if (window?.Page is NavigationPage nav)
        {
            var viewModel = _serviceProvider.GetRequiredService<LoginPageViewModel>();
            await nav.PushAsync(new LoginPage(viewModel));
        }
    }
}

