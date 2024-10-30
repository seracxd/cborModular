using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;

namespace cborModular.Infrastructure
{
    internal class BluetoothClient : IBluetoothClient
    {
        private readonly IAdapter _adapter;
        private IDevice _device;
        private IService _service;
        private ICharacteristic _characteristic;

        public BluetoothClient(IAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task ConnectToDeviceAsync(Guid deviceId)
        {
            // Vyhledání zařízení podle jeho ID (nebo skenování všech zařízení)
            _device = await _adapter.ConnectToKnownDeviceAsync(deviceId);

            if (_device == null)
            {
                throw new Exception("Device not found or unable to connect.");
            }

            // Získání služby (například pomocí UUID služby)
            _service = await _device.GetServiceAsync(Guid.Parse("0000180d-0000-1000-8000-00805f9b34fb")); // UUID služby

            if (_service == null)
            {
                throw new Exception("Service not found.");
            }

            // Získání charakteristiky (například pomocí UUID charakteristiky)
            _characteristic = await _service.GetCharacteristicAsync(Guid.Parse("00002a37-0000-1000-8000-00805f9b34fb")); // UUID charakteristiky

            if (_characteristic == null)
            {
                throw new Exception("Characteristic not found.");
            }
        }

        public async Task<byte[]> ReadDataAsync()
        {
            if (_characteristic == null)
            {
                throw new InvalidOperationException("Characteristic is not set.");
            }

            // Čtení dat z charakteristiky
            var (data, resultCode) = await _characteristic.ReadAsync();
            return data;
        }

        public async Task WriteDataAsync(byte[] data)
        {
            if (_characteristic == null)
            {
                throw new InvalidOperationException("Characteristic is not set.");
            }

            // Zapsání dat do charakteristiky
            await _characteristic.WriteAsync(data);
        }
    }
}