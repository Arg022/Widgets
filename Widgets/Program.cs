using Avalonia;
using System;
using Microsoft.EntityFrameworkCore;
using Widgets.Data;

namespace Widgets;

sealed class Program
{
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        InitializeDatabase();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }
    
    private static void InitializeDatabase()
    {
        var context = DbContextFactory.Create();
        context.Database.Migrate();
        
        var service = new Widgets.Services.ProjectService(context);
        service.CreateProjectAsync("Test Project", "Progetto di prova").Wait();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}