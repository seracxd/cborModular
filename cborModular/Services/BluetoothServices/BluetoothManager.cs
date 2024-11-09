using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Services.BluetoothServices
{
    internal class BluetoothManager
    {
        private readonly BluetoothService _bluetoothService;

        public BluetoothManager()
        {
            _bluetoothService = new BluetoothService();

        }

    }
}
