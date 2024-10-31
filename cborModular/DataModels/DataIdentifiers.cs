using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    public enum DataIdentifier
    {  
        // rychlost
        /// <summary>Rychlost vozidla (float)</summary>
        [DataType(typeof(float))] Rychlost = 1,
        /// <summary>Akcelerace vozidla (float)</summary>
        [DataType(typeof(float))] Akcelerace = 2,
        /// <summary>Průměrná rychlost vozidla (float)</summary>
        [DataType(typeof(float))] PrumernaRychlost = 3,
        /// <summary>Maximální rychlost vozidla (float)</summary>
        [DataType(typeof(float))] MaximalniRychlost = 4,
        /// <summary>Přetížení (float)</summary>
        [DataType(typeof(float))] Pretizeni = 5,
        /// <summary>Maximální přetížení (float)</summary>
        [DataType(typeof(float))] MaxPretizeni = 6,

        // motor
        /// <summary>Otáčky motoru (ushort)</summary>
        [DataType(typeof(ushort))] OtackyMotoru = 10,
        /// <summary>Výkon motoru (ushort)</summary>
        [DataType(typeof(ushort))] VykonMotoru = 11,
        /// <summary>Teplota motoru (float)</summary>
        [DataType(typeof(float))] TeplotaMotoru = 12,
        /// <summary>hodnota plynu v % (float)</summary>
        [DataType(typeof(float))] Plyn = 12,
        /// <summary>Převod (sbyte)</summary>
        [DataType(typeof(sbyte))] Prevod = 15,

        // baterie
        /// <summary>Úroveň baterie (byte)</summary>
        [DataType(typeof(byte))] UrovenBaterie = 20,
        /// <summary>Odhadovaný dojezd (float)</summary>
        [DataType(typeof(float))] OdhadovanyDojezd = 21,
        /// <summary>Teplota baterie (float)</summary>
        [DataType(typeof(float))] TeplotaBaterie = 22,
        /// <summary>Čas do nabití (TimeSpan)</summary>
        [DataType(typeof(TimeSpan))] CasDoNabiti = 23,
        /// <summary>Spotřeba na kilometr (float)</summary>
        [DataType(typeof(float))] SpotrebaPerKm = 24,
        /// <summary>Plynulost jízdy (float)</summary>
        [DataType(typeof(float))] PlynulostJizdy = 25,
        /// <summary>Spotřeba na kWh (float)</summary>
        [DataType(typeof(float))] SpotrebaPerKWh = 26,
        /// <summary>Regenerace baterie (float)</summary>
        [DataType(typeof(float))] RegenBaterie = 27,

        // GPS
        /// <summary>GPS Sever (float)</summary>
        [DataType(typeof(float))] GPS_N = 40,
        /// <summary>GPS Východ (float)</summary>
        [DataType(typeof(float))] GPS_E = 41,
        /// <summary>Nadmořská výška (ushort)</summary>
        [DataType(typeof(ushort))] NadmorskaVyska = 42,
        /// <summary>Směr (float)</summary>
        [DataType(typeof(float))] Smer = 43,
        /// <summary>Vzdálenost (float)</summary>
        [DataType(typeof(float))] Vzdalenost = 44,
        /// <summary>Sklon (sbyte)</summary>
        [DataType(typeof(sbyte))] Sklon = 45,
        /// <summary>Gyroskop (object)</summary>
        [DataType(typeof(object))] Gyroskop = 46, // Placeholder type

        // prostredi (environment)
        /// <summary>Teplota prostředí (float)</summary>
        [DataType(typeof(float))] TeplotaProstredi = 60,
        /// <summary>Vlhkost prostředí (float)</summary>
        [DataType(typeof(float))] Vlhkost = 61,
        /// <summary>Tlak prostředí (float)</summary>
        [DataType(typeof(float))] Tlak = 62,
        /// <summary>Čas prostředí (TimeSpan)</summary>
        [DataType(typeof(TimeSpan))] CasProstredi = 63,
        /// <summary>Světelná úroveň (float)</summary>
        [DataType(typeof(float))] SvetelnaUroven = 64,

        // ovladaci prvky (control elements)
        /// <summary>Nastavení výkonu (float)</summary>
        [DataType(typeof(float))] NastaveniVykonu = 70,
        /// <summary>Ochrana baterie (byte)</summary>
        [DataType(typeof(byte))] OchranyBaterie = 71,
        /// <summary>Regenerativní brzdění (bool)</summary>
        [DataType(typeof(bool))] RegenerativniBrzdeni = 72,
        /// <summary>Světla (bool)</summary>
        [DataType(typeof(bool))] Svetla = 73,
        /// <summary>Nouzová světla (bool)</summary>
        [DataType(typeof(bool))] NouzovaSvetla = 74,
        /// <summary>ABS (bool)</summary>
        [DataType(typeof(bool))] ABS = 75,
        /// <summary>Brzdový výkon (bool)</summary>
        [DataType(typeof(bool))] BrzdovyVykon = 76,
        /// <summary>Traction control (Enum)</summary>
        [DataType(typeof(bool))] Trakce = 77,
        /// <summary>Režim jízdy (Enum)</summary>
        [DataType(typeof(string))] RezimJizdy = 78,
        /// <summary>Maximální rychlost (ushort)</summary>
        [DataType(typeof(ushort))] MaxRychlost = 79,
        /// <summary>Ruční brzda (bool)</summary>
        [DataType(typeof(bool))] RucniBrzda = 80,
        /// <summary>Demo režim motorky (bool)</summary>
        [DataType(typeof(bool))] DemoRezim = 81,

        // bezpecnostni funkce (safety features)
        /// <summary>Imobilizér (bool)</summary>
        [DataType(typeof(bool))] Imobilizer = 90,
        /// <summary>Alarm (bool)</summary>
        [DataType(typeof(bool))] Alarm = 91,
        /// <summary>Geofencing (ushort)</summary>
        [DataType(typeof(ushort))] Geofencing = 92,

        // notifikace (notifications)
        /// <summary>aktivní stav motorky (bool)</summary>
        [DataType(typeof(bool))] Aktivni = 100 // Placeholder
    }
}
