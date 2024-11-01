using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cborModular.DataIdentifiers;

namespace cborModular.DataModels
{
    public static class DataStorage
    {
        private static int sequenceNumber = 0;
        private static readonly Dictionary<DataIdentifier, List<object>> storage = new();
        private static readonly List<DataIdentifier> requestedIdentifiers = new();
        private static readonly List<SetDataIdentifier> setIdentifiers = new();

        public static int GetSequenceNumber() => sequenceNumber;

        public static int IncrementSequenceNumber()
        {
            sequenceNumber++;
            return sequenceNumber;
        }

        public static void AddData(DataIdentifier identifier, object value)
        {
            if (value.GetType() != identifier.ExpectedType)
            {
                throw new InvalidOperationException($"Invalid type for {identifier}. Expected {identifier.ExpectedType}.");
            }
            if (!storage.ContainsKey(identifier))
            {
                storage[identifier] = new List<object>();
            }
            storage[identifier].Add(value);
        }

        public static object GetLastValue(DataIdentifier identifier)
        {
            if (storage.TryGetValue(identifier, out var values) && values.Count > 0)
            {
                return values[^1];
            }
            throw new InvalidOperationException($"No data found for identifier {identifier}.");
        }

        /// <summary>
        /// Adds a variable number of DataIdentifiers to the request list in DataStorage.
        /// </summary>
        /// <param name="identifiers">A variable number of DataIdentifier values</param>
        public static void AddRequest(params DataIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                if (!requestedIdentifiers.Contains(identifier))
                {
                    requestedIdentifiers.Add(identifier);
                }
            }
        }
        /// <summary>
        /// Adds a variable number of SetDataIdentifier values to the set identifiers list.
        /// </summary>
        public static void AddSet(params SetDataIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                if (!setIdentifiers.Contains(identifier))
                {
                    setIdentifiers.Add(identifier);
                }
            }
        }
        /// <summary>
        /// Gets all requested identifiers.
        /// </summary>
        /// <returns>A read-only collection of the requested identifiers.</returns>
        public static IReadOnlyCollection<DataIdentifier> GetRequestedIdentifiers()
        {
            return requestedIdentifiers.AsReadOnly();
        }
        /// <summary>
        /// Gets all set identifiers.
        /// </summary>
        /// <returns>A read-only collection of the requested identifiers.</returns>
        public static IReadOnlyCollection<SetDataIdentifier> GetSetIdentifiers()
        {
            return setIdentifiers.AsReadOnly();
        }

        public static void ClearRequest()
        {
            requestedIdentifiers.Clear();
        }
        public static void ClearSet()
        {
            setIdentifiers.Clear();
        }
    }
}
