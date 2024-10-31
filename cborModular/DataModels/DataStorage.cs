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

        public static T GetData<T>(DataIdentifier identifier)
        {
            if (dataStorage.ContainsKey(identifier) && dataStorage[identifier] is T value)
            {
                return value;
            }
            throw new InvalidOperationException("Data not found or invalid type.");
        }
        private static Type GetExpectedType(DataIdentifier identifier)
        {
            var memberInfo = typeof(DataIdentifier).GetMember(identifier.ToString())[0];
            var attribute = memberInfo.GetCustomAttribute<DataTypeAttribute>();
            return attribute?.ExpectedType ?? throw new InvalidOperationException("Data type not specified.");
        }
    }
}
