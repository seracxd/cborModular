﻿using cborModular.Application;
using cborModular.Domain;
using cborModular.Infrastructure;
using Microsoft.Extensions.Logging;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;

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

            builder.Services.AddSingleton<IMotorcycleService, MotorcycleService>();
            builder.Services.AddSingleton<IMotorcycleRepository, MotorcycleRepository>();
            builder.Services.AddSingleton<IBluetoothClient, BluetoothClient>();

            builder.Services.AddSingleton<IAdapter>(CrossBluetoothLE.Current.Adapter);
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
