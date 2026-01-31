using Avalonia;
using System;

namespace Heroes3MapReader.UI;

internal class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    private static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

#if DEBUG
        builder.AfterSetup(_ =>
        {
            if (builder.Instance is App app)
            {
                app.AttachDevTools();
            }
        });
#endif

        return builder;
    }
}
