using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyFactory.MauiClient.ViewModels.Authentication;

public partial class PasswordResetPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task ResetPasswordAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "”кажите email";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {

            //  TODO: Implement password reset logic here

            await Task.Delay(300); // Simulate call
            Message = "—сылка дл€ сброса отправлена";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}

