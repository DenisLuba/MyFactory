using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Users;
using MyFactory.MauiClient.Services.Users;

namespace MyFactory.MauiClient.ViewModels.Users;

public partial class RolesPageViewModel : ObservableObject
{
    private readonly IUsersService _usersService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<RoleResponse> Roles { get; } = new();

    public RolesPageViewModel(IUsersService usersService)
    {
        _usersService = usersService;
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
    private async Task AddAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync("Новая роль", "Введите название роли", placeholder: "ROLE");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var response = await _usersService.CreateRoleAsync(new CreateRoleRequest(name.Trim()));
            if (response is not null)
            {
                await LoadAsync();
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
    private async Task OpenDetailsAsync(RoleResponse? role)
    {
        if (role is null)
        {
            return;
        }

        var newName = await Shell.Current.DisplayPromptAsync("Редактировать роль", "Название роли", initialValue: role.Name);
        if (string.IsNullOrWhiteSpace(newName) || string.Equals(newName, role.Name, StringComparison.Ordinal))
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _usersService.UpdateRoleAsync(role.Id, new UpdateRoleRequest(newName.Trim()));
            await LoadAsync();
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
    private async Task DeleteAsync(RoleResponse? role)
    {
        if (role is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить роль '{role.Name}'?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _usersService.RemoveRoleAsync(role.Id);
            Roles.Remove(role);
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

