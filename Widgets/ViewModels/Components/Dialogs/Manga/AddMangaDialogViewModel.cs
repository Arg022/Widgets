using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Widgets.ViewModels.Components.Dialogs.Manga;

public partial class AddMangaDialogViewModel : ViewModelBase
{
    // Risultato — null se annullato
    public bool Confirmed { get; private set; }

    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _author = string.Empty;
    [ObservableProperty] private string _origin = "Manga";
    [ObservableProperty] private string? _artist;
    [ObservableProperty] private int? _totalVolumes;
    [ObservableProperty] private int? _totalChapters;
    [ObservableProperty] private int? _releaseYear;

    public string[] Origins { get; } = { "Manga", "Manhwa", "Manhua", "Western", "Novel" };

    public bool CanConfirm => !string.IsNullOrWhiteSpace(Title)
                              && !string.IsNullOrWhiteSpace(Author);

    partial void OnTitleChanged(string value)  => OnPropertyChanged(nameof(CanConfirm));
    partial void OnAuthorChanged(string value) => OnPropertyChanged(nameof(CanConfirm));

    public event Action? CloseRequested;

    [RelayCommand]
    private void Confirm()
    {
        if (!CanConfirm) return;
        Confirmed = true;
        CloseRequested?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        Confirmed = false;
        CloseRequested?.Invoke();
    }
}