using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Widgets.Data.Models;
using Widgets.Services;
using AddMangaDialogView = Widgets.Views.Components.Dialogs.Manga.AddMangaDialogView;
using AddMangaDialogViewModel = Widgets.ViewModels.Components.Dialogs.Manga.AddMangaDialogViewModel;

namespace Widgets.ViewModels;

public partial class MangaPageViewModel : ViewModelBase
{
    private readonly MangaService _mangaService;
    private readonly ItemService  _itemService;
    
    [ObservableProperty]
    private ObservableCollection<Manga> _mangas = new();

    public bool IsEmpty => Mangas.Count == 0;
    
    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        var list = await _mangaService.GetAllAsync();
        Mangas = new ObservableCollection<Manga>(list);
        OnPropertyChanged(nameof(IsEmpty));
        IsLoading = false;
    }
    

    [ObservableProperty]
    private Manga? _selectedManga;

    [ObservableProperty]
    private bool _isLoading;

    public MangaPageViewModel(MangaService mangaService, ItemService itemService)
    {
        _mangaService = mangaService;
        _itemService  = itemService;
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var vm = new AddMangaDialogViewModel();
        var dialog = new AddMangaDialogView { DataContext = vm };

        vm.CloseRequested += () => dialog.Close();

        var lifetime = Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime;

        var owner = lifetime?.MainWindow;

        if (owner == null)
            throw new InvalidOperationException("MainWindow not found.");

        await dialog.ShowDialog(owner);

        if (!vm.Confirmed) return;

        var manga = await _mangaService.AddAsync(
            vm.Title,
            vm.Author,
            vm.Origin,
            vm.Artist,
            vm.TotalVolumes,
            vm.TotalChapters,
            vm.ReleaseYear
        );

        Mangas.Add(manga);
        OnPropertyChanged(nameof(IsEmpty));
        SelectedManga = manga;
    }

    [RelayCommand]
    private async Task DeleteAsync(Manga manga)
    {
        await _itemService.DeleteAsync(manga.Id);
        Mangas.Remove(manga);
    }
}