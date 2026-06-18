using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.Controllers;

public partial class SortLabel : ContentView
{
	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text), typeof(string), typeof(SortLabel), default(string));

	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
		nameof(TextColor), typeof(Color), typeof(SortLabel), Colors.SteelBlue);

	public static readonly BindableProperty TextAttributesProperty = BindableProperty.Create(
		nameof(TextAttributes), typeof(FontAttributes), typeof(SortLabel), FontAttributes.Bold);

	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
		nameof(FontSize), typeof(double), typeof(SortLabel), 14d);

	public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
		nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(SortLabel), TextAlignment.Start);

	public static readonly BindableProperty SortKeyProperty = BindableProperty.Create(
		nameof(SortKey), typeof(string), typeof(SortLabel), default(string));

	public static readonly BindableProperty IsDescendingProperty = BindableProperty.Create(
		nameof(IsDescending), typeof(bool), typeof(SortLabel), false, BindingMode.TwoWay);

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(
		nameof(Command), typeof(ICommand), typeof(SortLabel), default(ICommand));

	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
		nameof(CommandParameter), typeof(object), typeof(SortLabel), default(object));

	public SortLabel()
	{
		InitializeComponent();
	}

	public string? Text
	{
		get => (string?)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public Color TextColor
	{
		get => (Color)GetValue(TextColorProperty);
		set => SetValue(TextColorProperty, value);
	}

	public FontAttributes TextAttributes
	{
		get => (FontAttributes)GetValue(TextAttributesProperty);
		set => SetValue(TextAttributesProperty, value);
	}

	public double FontSize
	{
		get => (double)GetValue(FontSizeProperty);
		set => SetValue(FontSizeProperty, value);
	}

	public TextAlignment HorizontalTextAlignment
	{
		get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
		set => SetValue(HorizontalTextAlignmentProperty, value);
	}

	/// <summary>
	/// Logical key the consumer uses to know which field to sort by (e.g. "Login", "Status").
	/// </summary>
	public string? SortKey
	{
		get => (string?)GetValue(SortKeyProperty);
		set => SetValue(SortKeyProperty, value);
	}

	/// <summary>
	/// Current direction; toggles each tap. Two-way so a page can reset it if needed.
	/// </summary>
	public bool IsDescending
	{
		get => (bool)GetValue(IsDescendingProperty);
		set => SetValue(IsDescendingProperty, value);
	}

	public ICommand? Command
	{
		get => (ICommand?)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object? CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	private void OnTapped(object? sender, TappedEventArgs e)
	{
		IsDescending = !IsDescending;

		var parameter = new SortLabelCommandParameter(SortKey, IsDescending, CommandParameter);

		if (Command?.CanExecute(parameter) ?? false)
			Command.Execute(parameter);
	}

	public sealed record SortLabelCommandParameter(string? SortKey, bool IsDescending, object? OriginalParameter = null);
}