using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BleSendData
    {     
        public BleSendData(){}

        // Send a request to the server with data formatted in CBOR
        public static async Task SendRequestAsync(ICharacteristic characteristic, byte[] cborData)
        {
            if (characteristic == null)
            {
                throw new InvalidOperationException("Characteristic is not initialized.");
            }
            try
            {
                await characteristic.WriteAsync(cborData);              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send data to server: {ex.Message}");
            }
        }         
    }
}
