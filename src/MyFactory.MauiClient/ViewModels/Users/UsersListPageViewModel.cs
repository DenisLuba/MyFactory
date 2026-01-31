using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Users;
using MyFactory.MauiClient.Pages.Users;
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
    private string? selectedRole;

    [ObservableProperty]
    private bool includeInactive = true;

    [ObservableProperty]
    private bool sortDesk = false; 

    public ObservableCollection<string> Roles { get; } = new();
    public ObservableCollection<UserItemViewModel> Users { get; } = new();

    public UsersListPageViewModel(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [RelayCommand]
    public async Task LoadAsync()
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

            var roles = await _usersService.GetRolesAsync() ?? [];
            foreach (var role in roles.OrderBy(r => r.Name))
            {
                Roles.Add(role.Name);
            }

            var all = "Все";
            Roles.Add(all);
            SelectedRole = all;

            var users = await _usersService.GetUsersAsync(includeInactive: IncludeInactive, sortDesk: SortDesk);
            _allUsers = users?.Select(u => new UserItemViewModel(u)).ToList() ?? new List<UserItemViewModel>();

            ApplyFilters();
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
        await Shell.Current.GoToAsync(nameof(UserDetailsPage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(UserItemViewModel? user)
    {
        if (user is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(UserDetailsPage), new Dictionary<string, object>
        {
            { "UserId", user.Id.ToString() }
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

        var confirm = await Shell.Current.DisplayAlertAsync("Блокировка", $"Заблокировать пользователя {user.Username}?", "Да", "Нет");
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
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
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

    partial void OnSelectedRoleChanged(string? value)
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

        else if (SelectedRole is not null && !string.Equals(SelectedRole, "Все", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(u => string.Equals(u.RoleName, SelectedRole, StringComparison.OrdinalIgnoreCase));
        }

        var filtered = query.ToList();

        Users.Clear();
        foreach (var user in filtered)
        {
            Users.Add(user);
        }
    }

    [RelayCommand]
    private async Task StatusSwitcherAsync()
    {
        IncludeInactive = !IncludeInactive;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task SortByLoginSwitcherAsync()
    {
        SortDesk = !SortDesk;
        await LoadAsync();
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
            CreatedAt = dto.CreatedAt.ToLocalTime();
        }
    }
}

