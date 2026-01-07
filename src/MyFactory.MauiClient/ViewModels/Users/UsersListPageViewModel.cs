using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Users;
using MyFactory.MauiClient.Services.Users;

namespace MyFactory.MauiClient.ViewModels.Users;

public partial class UsersListPageViewModel : ObservableObject
{
    private readonly IUsersService _usersService;
    private List<UserItemViewModel> _allUsers = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? searchText;

    [ObservableProperty]
    private RoleResponse? selectedRole;

    public ObservableCollection<RoleResponse> Roles { get; } = new();
    public ObservableCollection<UserItemViewModel> Users { get; } = new();

    public UsersListPageViewModel(IUsersService usersService)
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
            Users.Clear();

            var roles = await _usersService.GetRolesAsync() ?? Array.Empty<RoleResponse>();
            foreach (var role in roles.OrderBy(r => r.Name))
            {
                Roles.Add(role);
            }

            var users = await _usersService.GetUsersAsync();
            _allUsers = users?.Select(u => new UserItemViewModel(u)).ToList() ?? new List<UserItemViewModel>();

            ApplyFilters();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync("UserDetailsPage");
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(UserItemViewModel? user)
    {
        if (user is null)
        {
            return;
        }

        await Shell.Current.GoToAsync("UserDetailsPage", new Dictionary<string, object>
        {
            { "UserId", user.Id }
        });
    }

    [RelayCommand]
    private async Task EditAsync(UserItemViewModel? user)
    {
        await OpenDetailsAsync(user);
    }

    [RelayCommand]
    private async Task ToggleActiveAsync(UserItemViewModel? user)
    {
        if (user is null || !user.IsActive)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Блокировка", $"Заблокировать пользователя {user.Username}?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _usersService.DeactivateUserAsync(user.Id);
            user.IsActive = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSearchTextChanged(string? value)
    {
        ApplyFilters();
    }

    partial void OnSelectedRoleChanged(RoleResponse? value)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var query = _allUsers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim();
            query = query.Where(u => u.Username.Contains(term, StringComparison.OrdinalIgnoreCase) || u.RoleName.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedRole is not null)
        {
            query = query.Where(u => string.Equals(u.RoleName, SelectedRole.Name, StringComparison.OrdinalIgnoreCase));
        }

        var filtered = query.ToList();

        Users.Clear();
        foreach (var user in filtered)
        {
            Users.Add(user);
        }
    }

    public partial class UserItemViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string Username { get; }
        public string RoleName { get; }

        [ObservableProperty]
        private bool isActive;

        public DateTime CreatedAt { get; }

        public string Status => IsActive ? "Активен" : "Заблокирован";

        partial void OnIsActiveChanged(bool value)
        {
            OnPropertyChanged(nameof(Status));
        }

        public UserItemViewModel(UserListItemResponse dto)
        {
            Id = dto.Id;
            Username = dto.Username;
            RoleName = dto.RoleName;
            IsActive = dto.IsActive;
            CreatedAt = dto.CreatedAt;
        }
    }
}

