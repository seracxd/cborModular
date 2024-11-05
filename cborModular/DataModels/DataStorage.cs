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
        private static readonly Dictionary<DataIdentifier, List<DataEntry>> storage = [];
        private static readonly List<DataIdentifier> requestedIdentifiers = [];
        private static readonly List<SetDataIdentifier> setIdentifiers = [];
        private static readonly List<RequestRecord> requestRecords = [];




        public static int GetSequenceNumber() => sequenceNumber;
        public static int IncrementSequenceNumber()
        {
            sequenceNumber++;
            return sequenceNumber;
        }

        public static void AddData(DataIdentifier identifier, object value, DateTimeOffset? timestamp = null)
        {
            if (value.GetType() != identifier.ExpectedType)
            {
                throw new InvalidOperationException($"Invalid type for {identifier}. Expected {identifier.ExpectedType}.");
            }
            if (!storage.ContainsKey(identifier))
            {
                storage[identifier] = [];
            }

            // Použití časového údaje, pokud je k dispozici, jinak aktuálního času
            var entry = timestamp.HasValue ? new DataEntry(value, timestamp.Value) : new DataEntry(value);
            storage[identifier].Add(entry);
        }


        public static object GetLastValue(DataIdentifier identifier, Func<DataEntry, object> selector = null)

        {
            if (storage.TryGetValue(identifier, out var entries) && entries.Count > 0)
            {
                var lastEntry = entries[^1];
                selector ??= entry => entry.Data;

                return selector(lastEntry); // Vybere buď Data, nebo Timestamp
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


        public static void AddSet(SetDataIdentifier identifier, bool overrideValue)
        {
            // Check if the identifier already exists in the list
            var existingIdentifier = setIdentifiers.FirstOrDefault(x => x.Id == identifier.Id);

            if (existingIdentifier != null)
            {
                // Update the value and override flag for the existing identifier
                existingIdentifier.Value = overrideValue;
            }
            else
            {
                // If the identifier does not exist, set the value and override flag, then add it
                identifier.Value = overrideValue;
                setIdentifiers.Add(identifier);
            }
        }

        public static void AddRequestRecord(int sequenceNumber, Dictionary<DataIdentifier, object> data, MessageType messageType)
        {
            RequestRecord requestRecord;

            if (messageType == MessageType.Request)
            {
                // Pro Request vytvoříme RequestRecord s identifikátory a bez hodnot
                var identifiers = data.Keys.ToList();
                requestRecord = new RequestRecord(
                    messageType.ToString(),
                    sequenceNumber,
                    identifiers,
                    DateTimeOffset.UtcNow);
            }
            else if (messageType == MessageType.Set)
            {
                // Pro Set vytvoříme RequestRecord s identifikátory a hodnotami
                requestRecord = new RequestRecord(
                    messageType.ToString(),
                    sequenceNumber,
                    data,
                    DateTimeOffset.UtcNow);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported message type: {messageType}");
            }

            requestRecords.Add(requestRecord);
        }

        internal static RequestRecord GetRequestRecord(int sequenceNumber)
        {
            return requestRecords.FirstOrDefault(record => record.SequenceNumber == sequenceNumber);
        }

        internal static List<RequestRecord> GetAllRequestRecords()
        {
            return requestRecords;
        }

        // Odstranění záznamu na základě sequenceNumber (po obdržení odpovědi)
        public static void RemoveRequestRecord(int sequenceNumber)
        {
            requestRecords.RemoveAll(record => record.SequenceNumber == sequenceNumber);
        }

        // Odstranění záznamů na základě timeoutu
        public static void RemoveRequestRecordsByTimeout(TimeSpan timeout)
        {
            var now = DateTimeOffset.UtcNow;
            requestRecords.RemoveAll(record => (now - record.TimeOfRequest) > timeout);
        }

        public static Dictionary<DataIdentifier, object> GetSetValues()
        {
            var values = new Dictionary<DataIdentifier, object>();

            foreach (var identifier in setIdentifiers)
            {
                if (identifier.Value != null)
                {
                    values[identifier] = identifier.Value;
                }
            }

            return values;
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
