using cborModular.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cborModular.Domain.MotorcycleDataFields;

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
        public Task<List<object?>> GetAllValuesAsync(MotorcycleDataFields field)
        {
            var result = _dataStorage
                 .Select(data => field switch
                 {
                     MotorcycleDataFields.Speed => data.Speed as object,
                     MotorcycleDataFields.Throttle => data.Throttle as object,
                     MotorcycleDataFields.XCoord => data.XCoord as object,
                     MotorcycleDataFields.YCoord => data.YCoord as object,
                     _ => null
                 })
                .Where(value => value != null) // Ensure non-null values
                .ToList();

            return Task.FromResult(result);
        }
    }
}