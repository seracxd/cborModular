using cborModular.DataModels;
using Plugin.BLE.Abstractions.Contracts;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.LocalStorageSqLite
{
    public class MotorcycleModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Unikátní identifikátor pro každý záznam

        public string Name { get; set; } // Název motorky

        public bool Connected { get; set; } // Stav připojení

        // Převod IService a CharacteristicInfo na serializovatelný typ
        public string ServiceSerialized { get; set; } // Serializovaný IService jako string

        public string CharacteristicsSerialized { get; set; } // Serializované charakteristiky jako JSON
    }
}
