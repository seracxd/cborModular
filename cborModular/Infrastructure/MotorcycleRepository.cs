using cborModular.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Infrastructure
{
    internal class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly List<MotorcycleData> _dataStorage = new List<MotorcycleData>();

        public Task SaveAsync(MotorcycleData data)
        {
            _dataStorage.Add(data);
            return Task.CompletedTask;
        }

        public Task<MotorcycleData?> GetLastDataAsync()
        {
            var lastData = _dataStorage.LastOrDefault();
            return Task.FromResult(lastData);
        }
    }
}