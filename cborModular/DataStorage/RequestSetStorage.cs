using cborModular.DataIdentifiers;
using cborModular.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataStorage
{
    internal class RequestSetStorage
    {
        private readonly Dictionary<DataIdentifier, List<DataEntry>> _storage = new();
        private readonly List<DataIdentifier> _requestedIdentifiers = new();
        private readonly List<SetDataIdentifier> _setIdentifiers = new();

        public void AddData(DataIdentifier identifier, object value, DateTimeOffset? timestamp = null)
        {
            if (value.GetType() != identifier.ExpectedType)
                throw new InvalidOperationException($"Invalid type for {identifier}. Expected {identifier.ExpectedType}.");

            if (!_storage.ContainsKey(identifier))
                _storage[identifier] = new List<DataEntry>();

            var entry = timestamp.HasValue ? new DataEntry(value, timestamp.Value) : new DataEntry(value);
            _storage[identifier].Add(entry);
        }

        public object GetLastValue(DataIdentifier identifier, Func<DataEntry, object> selector = null)
        {
            if (_storage.TryGetValue(identifier, out var entries) && entries.Count > 0)
            {
                var lastEntry = entries[^1];
                selector ??= entry => entry.Data;
                return selector(lastEntry);
            }

            throw new InvalidOperationException($"No data found for identifier {identifier}.");
        }

        public void AddRequest(params DataIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                if (!_requestedIdentifiers.Contains(identifier))
                    _requestedIdentifiers.Add(identifier);
            }
        }

        public IReadOnlyCollection<DataIdentifier> GetRequestedIdentifiers() => _requestedIdentifiers.AsReadOnly();
        public IReadOnlyCollection<SetDataIdentifier> GetSetIdentifiers() => _setIdentifiers.AsReadOnly();
        public void ClearRequest() => _requestedIdentifiers.Clear();
        public void ClearSet() => _setIdentifiers.Clear();


        public void AddSet(SetDataIdentifier identifier, bool overrideValue)
        {
            var existingIdentifier = _setIdentifiers.FirstOrDefault(x => x.Id == identifier.Id);
            if (existingIdentifier != null)
                existingIdentifier.Value = overrideValue;
            else
            {
                identifier.Value = overrideValue;
                _setIdentifiers.Add(identifier);
            }
        }

        public Dictionary<DataIdentifier, object> GetSetValues()
        {
            var values = new Dictionary<DataIdentifier, object>();
            foreach (var identifier in _setIdentifiers)
            {
                if (identifier.Value != null)
                    values[identifier] = identifier.Value;
            }

            return values;
        }

       
     
    }
}
