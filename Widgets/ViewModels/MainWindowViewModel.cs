using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Widgets.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isPaneOpen = false;

    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
       if (value is null) return;
       var instance = Activator.CreateInstance(value.ModelType);
       if (instance is null) return; 
       CurrentPage = (ViewModelBase)instance;
    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel),"Home"),
        new ListItemTemplate(typeof(ButtonPageViewModel),"CursoHoverRegular")
    };
    
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    
    [RelayCommand]
    private static void ToggleTheme()
    {
        var app = Application.Current;
        if (app is null) return;

        app.RequestedThemeVariant = 
            app.RequestedThemeVariant == ThemeVariant.Dark 
                ? ThemeVariant.Light 
                : ThemeVariant.Dark;
    }
}

public class ListItemTemplate
{
    public string Label { get; }
    public Type ModelType { get; }
    public StreamGeometry ListItemIcon { get; }
    
    public ListItemTemplate(Type type, string iconKey)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", "");
        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon=(StreamGeometry)res!;
    }
    
}