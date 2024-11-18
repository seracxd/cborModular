using Microsoft.Extensions.Logging;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using cborModular.Interfaces;
using cborModular.Services.BluetoothServices;
using cborModular.Interfaces.Controlers;
using cborModular.LocalStorageSqLite;

namespace cborModular
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            ConfigureServices(builder.Services);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseSqlite>();


            // Registrace hlavní služby BluetoothService a závislostí
            services.AddSingleton<IBluetoothService, BluetoothControler>();
            services.AddSingleton<IDataService, DataControler>();

            // Registrace MainPage a App
            services.AddTransient<MainPage>();
            services.AddSingleton<App>();
        }
    }
}
