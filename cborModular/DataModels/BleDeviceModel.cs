using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    internal class BleDeviceModel
    {
        public string Name { get; set; }
        public Guid Id { get; set; }       
        public string Manufacturer { get; set; } // Výrobce zařízení, pokud je k dispozici
        public bool IsConnected { get; set; }

        // Reklamní data
        public List<byte[]> AdvertisementData { get; set; }

        // Služby a charakteristiky
        public List<BluetoothServiceModel> Services { get; set; }

        public BleDeviceModel()
        {
            AdvertisementData = new List<byte[]>();
            Services = new List<BluetoothServiceModel>();
        }
    }

    public class BluetoothServiceModel
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public bool IsPrimary { get; set; } // Označení, zda je služba primární

        // Seznam charakteristik služby
        public List<BluetoothCharacteristicModel> Characteristics { get; set; }

        public BluetoothServiceModel()
        {
            Characteristics = new List<BluetoothCharacteristicModel>();
        }
    }

    public class BluetoothCharacteristicModel
    {
        public Guid CharacteristicId { get; set; }
        public string CharacteristicName { get; set; }      
        public List<BluetoothDescriptorModel> Descriptors { get; set; } // Seznam descriptorů charakteristiky

        public BluetoothCharacteristicModel()
        {
            Descriptors = new List<BluetoothDescriptorModel>();
        }
    }

    public class BluetoothDescriptorModel
    {
        public Guid DescriptorId { get; set; }
        public string DescriptorName { get; set; }
    }
}

