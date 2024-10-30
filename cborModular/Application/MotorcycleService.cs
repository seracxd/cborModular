using cborModular.Domain;
using cborModular.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Application
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IBluetoothClient _bluetoothClient;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IBluetoothClient bluetoothClient, IMotorcycleRepository motorcycleRepository)
        {
            _bluetoothClient = bluetoothClient;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<MotorcycleData> LoadDataAsync(Guid deviceId)
        {
            try
            {
                // Nejprve se připojíme k zařízení přes BLE
                await _bluetoothClient.ConnectToDeviceAsync(deviceId);

                // Nyní přečteme data z charakteristiky
                var cborResponse = await _bluetoothClient.ReadDataAsync();

                // Kontrola, zda byla přijata nějaká data
                if (cborResponse == null || cborResponse.Length == 0)
                {
                    throw new InvalidOperationException("No data received from the BLE device.");
                }

                // Použití CborReader k dekódování dat
                var reader = new System.Formats.Cbor.CborReader(cborResponse);
                var motorcycleData = DecodeCborResponse(reader);

                // Uložit data do úložiště
                await _motorcycleRepository.SaveAsync(motorcycleData);

                // Vrátit dekódovaná data
                return motorcycleData;
            }
            catch (Exception ex)
            {
                // Zaznamenání výjimky, pokud dojde k nějakému problému
                Console.WriteLine($"Error while loading motorcycle data: {ex.Message}");
                throw;
            }
        }

            private MotorcycleData DecodeCborResponse(System.Formats.Cbor.CborReader reader)
        {
            int mapLength = reader.ReadStartMap()??0; // Počet klíč-hodnota dvojic

            // První parametr by měl být Timestamp
            string firstKey = reader.ReadTextString();
            if (firstKey != "Timestamp")
            {
                throw new InvalidOperationException("Expected 'Timestamp' as the first key.");
            }

            // Načtení času ve formě ticků a vytvoření DateTime
            long timestampTicks = reader.ReadInt64();
            DateTime timestamp = new DateTime(timestampTicks);

            // Vytvoření MotorcycleData objektu s načteným timestampem
            var motorcycleData = new MotorcycleData(timestamp);

            // Nyní čteme ostatní parametry a přiřazujeme jim stejný timestamp
            for (int i = 1; i < mapLength; i++)  // Už jsme načetli jeden pár (Timestamp), takže začneme od 1
            {
                string key = reader.ReadTextString();

                switch (key)
                {
                    case "Speed":
                        motorcycleData.Speed = new MotorcycleDataWithTimestamp<double>
                        {
                            Value = reader.ReadDouble(),
                            Timestamp = timestamp
                        };
                        break;
                    case "Throttle":
                        motorcycleData.Throttle = new MotorcycleDataWithTimestamp<float>
                        {
                            Value = reader.ReadSingle(),
                            Timestamp = timestamp
                        };
                        break;
                    case "XCoord":
                        motorcycleData.XCoord = new MotorcycleDataWithTimestamp<float>
                        {
                            Value = reader.ReadSingle(),
                            Timestamp = timestamp
                        };
                        break;
                        // Přidej další parametry podle potřeby
                }
            }

            reader.ReadEndMap();
            return motorcycleData;
        }
    }
}
