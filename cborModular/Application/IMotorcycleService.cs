using cborModular.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Application
{
    public interface IMotorcycleService
    {
        Task<MotorcycleData> LoadDataAsync(Guid deviceId);
    }
}
