using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Infrastructure
{
    public interface IBluetoothClient
    {
        Task ConnectToDeviceAsync(Guid deviceId);

        // Čtení dat z charakteristiky
        Task<byte[]> ReadDataAsync();

        // Zapsání dat do charakteristiky
        Task WriteDataAsync(byte[] data);
    }
}
