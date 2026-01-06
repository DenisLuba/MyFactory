using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Users;
using MyFactory.MauiClient.Services.Users;

namespace MyFactory.MauiClient.ViewModels.Users;

[QueryProperty(nameof(UserId), "UserId")]
public partial class UserDetailsPageViewModel : ObservableObject
{
    private readonly IUsersService _usersService;

    [ObservableProperty]
    private Guid? userId;

    [ObservableProperty]
    private string? username;

    [ObservableProperty]
    private RoleResponse? selectedRole;

    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private DateTime createdAt;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<RoleResponse> Roles { get; } = new();

    public UserDetailsPageViewModel(IUsersService usersService)
    {
        _usersService = usersService;
        _ = LoadAsync();
    }

    partial void OnUserIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Roles.Clear();
            var roles = await _usersService.GetRolesAsync() ?? Array.Empty<RoleResponse>();
            foreach (var role in roles.OrderBy(r => r.Name))
            {
                Roles.Add(role);
            }

            if (UserId is null)
            {
                Username = string.Empty;
                SelectedRole = Roles.FirstOrDefault();
                IsActive = true;
                CreatedAt = DateTime.Now;
                return;
            }

            var user = await _usersService.GetUserAsync(UserId.Value);
            if (user is not null)
            {
                Username = user.Username;
                SelectedRole = Roles.FirstOrDefault(r => r.Id == user.RoleId) ?? Roles.FirstOrDefault();
                IsActive = user.IsActive;
                CreatedAt = user.CreatedAt;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedRole is null || string.IsNullOrWhiteSpace(Username))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите логин и роль", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (UserId is null)
            {
                var password = await Shell.Current.DisplayPromptAsync("Пароль", "Введите пароль", placeholder: "******", maxLength: 100, keyboard: Keyboard.Text, isPassword: true);
                if (string.IsNullOrWhiteSpace(password))
                {
                    await Shell.Current.DisplayAlertAsync("Ошибка", "Пароль обязателен", "OK");
                    return;
                }

                var request = new CreateUserRequest(Username.Trim(), password, SelectedRole.Id);
                var response = await _usersService.CreateUserAsync(request);
                if (response is not null)
                {
                    await Shell.Current.DisplayAlertAsync("Успех", "Пользователь создан", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
            else
            {
                var request = new UpdateUserRequest(SelectedRole.Id, IsActive);
                await _usersService.UpdateUserAsync(UserId.Value, request);
                await Shell.Current.DisplayAlertAsync("Успех", "Данные сохранены", "OK");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task BlockAsync()
    {
        if (UserId is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Блокировка", $"Заблокировать пользователя {Username}?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _usersService.DeactivateUserAsync(UserId.Value);
            IsActive = false;
            await Shell.Current.DisplayAlertAsync("Готово", "Пользователь заблокирован", "OK");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}

