using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sy.Avalonia.Material.Themes;

namespace MaterialDemo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private Color _color = Colors.Red;

    private readonly MaterialTheme _theme;

    private static readonly Dictionary<Theme, Color> ThemeColors = new()
    {
        { Theme.Red, Color.Parse("#b33b15") },
        { Theme.Green, Color.Parse("#63a002") },
        { Theme.Blue, Color.Parse("#769cdf") },
        { Theme.Yellow, Color.Parse("#ffde3f") }
    };

    partial void OnColorChanged(Color value) => Console.WriteLine($"Color changed to: {value.ToString()}");

    public MainViewModel()
    {
        _theme = MaterialTheme.GetInstance();
    }

    [RelayCommand]
    public void ChangeTheme(Theme theme)
    {
        if (ThemeColors.TryGetValue(theme, out var color))
        {
            _theme.ChangeTheme(color);
        }
    }
    
    [RelayCommand]
    public void ApplyCustomColor()
    {
        if (Color is Color color)
        {
            _theme.ChangeTheme(color);
        }
    }

    [RelayCommand]
    public void SwitchBaseTheme()
    {
        _theme.SwitchBaseTheme();
    }
}