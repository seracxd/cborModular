using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    public static class DataStorage
    {
        private static readonly Dictionary<DataIdentifier, List<object>> dataStorage = new Dictionary<DataIdentifier, List<object>>();
        private static readonly List<DataIdentifier> requestedIdentifiers = new List<DataIdentifier>();


        public static void AddData(DataIdentifier identifier, object value)
        {
            var expectedType = GetExpectedType(identifier);
            if (value.GetType() != expectedType)
            {
                throw new InvalidOperationException($"Invalid data type. Expected {expectedType}, but got {value.GetType()}.");
            }

            if (!dataStorage.ContainsKey(identifier))
            {
                dataStorage[identifier] = new List<object>();
            }
            dataStorage[identifier].Add(value);
        }

        public static object GetLastValue(DataIdentifier identifier)
        {
            if (dataStorage.ContainsKey(identifier) && dataStorage[identifier].Count > 0)
            {
                // Get the last value in the list for the specified identifier
                return dataStorage[identifier][^1]; // ^1 is the index from the end, accessing the last element
            }
            throw new InvalidOperationException("Data not found for the specified identifier.");
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
        /// Gets all requested identifiers.
        /// </summary>
        /// <returns>A read-only collection of the requested identifiers.</returns>
        public static IReadOnlyCollection<DataIdentifier> GetRequestedIdentifiers()
        {
            return requestedIdentifiers.AsReadOnly();
        }

        public static void ClearRequest()
        {
            requestedIdentifiers.Clear();
        }


        private static Type GetExpectedType(DataIdentifier identifier)
        {
            var memberInfo = typeof(DataIdentifier).GetMember(identifier.ToString())[0];
            var attribute = memberInfo.GetCustomAttribute<DataTypeAttribute>();
            return attribute?.ExpectedType ?? throw new InvalidOperationException("Data type not specified.");
        }
    }
}
