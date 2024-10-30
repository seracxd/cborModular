using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Domain
{
    public interface IMotorcycleRepository
    {
        Task SaveAsync(MotorcycleData data);
        Task<MotorcycleData?> GetLastDataAsync();
    }
}
