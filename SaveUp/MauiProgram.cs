using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SaveUp.Services;

namespace SaveUp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Basis-Konfiguration für Maui App
            builder
                .UseMauiApp<App>() // Setzt die Haupt-App
                .UseMauiCommunityToolkit() // Fügt das MAUI Community Toolkit hinzu
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registriere plattformspezifische Dienste
            RegisterPlatformServices(builder);

#if DEBUG
            builder.Logging.AddDebug(); // Aktiviert Debug-Logging
#endif

            return builder.Build();
        }

        /// <summary>
        /// Registriert plattformspezifische Dienste basierend auf der Zielplattform.
        /// </summary>
        /// <param name="builder">MauiAppBuilder</param>
        private static void RegisterPlatformServices(MauiAppBuilder builder)
        {
            // Plattformabhängige Implementierungen registrieren
#if ANDROID
            builder.Services.AddSingleton<IFilePathProvider, SaveUp.Platforms.Android.FilePathProvider>();
#elif IOS
            builder.Services.AddSingleton<IFilePathProvider, SaveUp.Platforms.iOS.FilePathProvider>();
#elif WINDOWS
            builder.Services.AddSingleton<IFilePathProvider, SaveUp.Platforms.Windows.FilePathProvider>();
#else
            throw new PlatformNotSupportedException(
                "Die aktuelle Plattform wird nicht unterstützt. Implementiere eine spezifische FilePathProvider-Klasse für diese Plattform.");
#endif
        }
    }
}