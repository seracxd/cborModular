using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    public class DeviceModel
    {
        public string Name { get; set; }
        public bool Connected { get; set; }
        public IService Service { get; set; }
        public List<CharacteristicInfo> Characteristics { get; set; }
    }
}
