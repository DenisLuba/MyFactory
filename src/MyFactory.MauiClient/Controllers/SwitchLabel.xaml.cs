using System.Windows.Input;

namespace MyFactory.MauiClient.Controllers;

public partial class SwitchLabel : ContentView
{
    #region Text Property
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(CheckLabel),
            defaultValue: default(string));

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion

    #region IsToggled Property
    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(
            propertyName: nameof(IsToggled),
            returnType: typeof(bool),
            declaringType: typeof(CheckLabel),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }
    #endregion

    #region Command Property
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand),
            declaringType: typeof(CheckLabel),
            defaultValue: default(ICommand));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    #endregion

    #region CommandParameter Property
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            propertyName: nameof(CommandParameter),
            returnType: typeof(object),
            declaringType: typeof(CheckLabel),
            defaultValue: default);

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    public SwitchLabel()
    {
        InitializeComponent();
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        IsToggled = !IsToggled;
        if (Command?.CanExecute(CommandParameter) is true)
        {
            Command.Execute(CommandParameter);
        }
    }
}