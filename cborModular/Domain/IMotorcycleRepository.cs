using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cborModular.Domain.MotorcycleDataFields;

namespace cborModular.Domain
{
    public interface IMotorcycleRepository
    {
        Task SaveAsync(MotorcycleData data);
        Task<MotorcycleData?> GetLastDataAsync();
        Task<List<object?>> GetAllValuesAsync(MotorcycleDataFields field);
    }
}
