using cborModular.DataIdentifiers;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    public class CharacteristicInfo
    {
        public ICharacteristic Characteristic { get; set; }
        public BluetoothCharakteristicIdentifiers Identifier { get; set; }
    }
}
