using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting; // Für SkiaSharp-Unterstützung

namespace SaveUp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>() // Diese Methode zuerst aufrufen
                .UseMauiCommunityToolkit() // CommunityToolkit danach hinzufügen
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            builder.Logging.AddDebug(); // Debug-Logging aktivieren
#endif
            return builder.Build();
        }
    }
}

