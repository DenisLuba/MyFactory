using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Auth;
using MyFactory.MauiClient.Services.Auth;

namespace MyFactory.MauiClient.ViewModels.Authentication;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public LoginPageViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введите логин и пароль";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var request = new LoginRequest(Email.Trim(), Password);
            await _authService.LoginAsync(request);
            await Shell.Current.DisplayAlert("Успех", "Вход выполнен", "ОК");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToPasswordResetAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.Authentication.PasswordResetPage));
    }
}

