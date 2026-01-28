using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Heroes3MapReader.Logic;
using Heroes3MapReader.Logic.Interfaces;
using Heroes3MapReader.Logic.MapSpecificationLogic;
using Heroes3MapReader.UI.ViewModels;
using Heroes3MapReader.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Heroes3MapReader.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();

            var services = new ServiceCollection();
            services.AddSingleton<IMapReader, MapReader>();
            services.AddSingleton<IMapReaderFactory, MapReaderFactory>();
            services.AddSingleton<IMapSpecificationRepository, MapSpecificationRepository>();
            services.AddSingleton<IStreamDecompressor, StreamDecompressor>();
            services.AddTransient<IStorageProvider>(_ => mainWindow.StorageProvider);
            services.AddSingleton<MainWindowViewModel>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            mainWindow.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
