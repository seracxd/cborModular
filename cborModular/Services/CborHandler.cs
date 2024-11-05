using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using cborModular.DataIdentifiers;
using cborModular.DataModels;

namespace cborModular.Services
{
    internal static class CborHandler
    {
        public static byte[] EncodeRequest(MessageType messageType)
        {
            var writer = new CborWriter();
            int sequenceNumber = DataStorage.IncrementSequenceNumber();
   
            writer.WriteStartMap(2);

            writer.WriteInt32((int)CborIdentifiers.SequenceNumber);
            writer.WriteInt32(sequenceNumber);

            if (messageType == MessageType.Request)
            {
                writer.WriteInt32((int)MessageType.Request); // Key for Request data
                var identifiers = DataStorage.GetRequestedIdentifiers().ToList();
                DataStorage.AddRequestRecord(sequenceNumber, identifiers.ToDictionary(id => id, id => (object)null), messageType);
                EncodeIdentifiers(writer, identifiers); // Encode only identifiers
                DataStorage.ClearRequest();
            }
            else if (messageType == MessageType.Set)
            {
                writer.WriteInt32((int)MessageType.Set); // Key for Set data
                var identifiers = DataStorage.GetSetIdentifiers().Cast<DataIdentifier>().ToList();
                var values = DataStorage.GetSetValues();
                DataStorage.AddRequestRecord(sequenceNumber, values, messageType);
                EncodeIdentifiersWithValues(writer, identifiers, values); // Encode identifiers with values
                DataStorage.ClearSet();
            }
            else
            {
                throw new InvalidOperationException("Unsupported message type");
            }

            writer.WriteEndMap(); // End of map
            return writer.Encode();
        }

        private static void EncodeIdentifiers(CborWriter writer, List<DataIdentifier> identifiers)
        {
            writer.WriteStartArray(identifiers.Count);
            identifiers.ForEach(id => writer.WriteInt32(id.Id));
            writer.WriteEndArray();          
        }

        private static void EncodeIdentifiersWithValues(CborWriter writer, List<DataIdentifier> identifiers, Dictionary<DataIdentifier, object> values)
        {
            writer.WriteStartArray(identifiers.Count+1);
            foreach (var identifier in identifiers)
            {
                writer.WriteInt32(identifier.Id);
                if (values != null && values.TryGetValue(identifier, out var value))
                    WriteValue(writer, value);
            }
            writer.WriteEndArray();          
        }

        private static void WriteValue(CborWriter writer, object value)
        {
            switch (value)
            {
                case float f: writer.WriteSingle(f); break;
                case bool b: writer.WriteBoolean(b); break;
                case int i: writer.WriteInt32(i); break;
                case uint u: writer.WriteUInt32(u); break;
                case ushort us: writer.WriteUInt32(us); break;
                case sbyte sb: writer.WriteInt32(sb); break;
                case byte bt: writer.WriteUInt32(bt); break;
                case TimeSpan ts: writer.WriteInt64(ts.Ticks); break;
                case string str: writer.WriteTextString(str); break;
                case long l: writer.WriteInt64(l); break;
                default: throw new InvalidOperationException("Unsupported data type");
            }
        }
        public static void DecodeResponse(byte[] cborData)
        {
            var reader = new CborReader(cborData);
            reader.ReadStartMap();

            int sequenceNumber = -1;
            long timestamp = -1;
            var responseData = new Dictionary<DataIdentifier, object>();

            // Načtěte sekvenční číslo a časové razítko z odpovědi
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                int key = reader.ReadInt32();

                if (key == (int)CborIdentifiers.SequenceNumber)
                {
                    sequenceNumber = reader.ReadInt32();
                }
                else if (key == (int)CborIdentifiers.Timestamp)
                {
                    timestamp = reader.ReadInt64();
                }
                else
                {
                    var identifier = DataIdentifierRegistry.GetById(key)
                        ?? throw new InvalidOperationException($"No identifier found for ID {key}");

                    object value = ReadValue(reader, identifier.ExpectedType);
                    responseData[identifier] = value;
                }
            }

            reader.ReadEndMap();

            // Kontrola existence požadavku s tímto sekvenčním číslem
            var requestRecord = DataStorage.GetRequestRecord(sequenceNumber);
            if (requestRecord == null)
            {
                Console.WriteLine($"Warning: No matching request found for sequence number {sequenceNumber}. Discarding response.");
                return; // Pokud neexistuje odpovídající požadavek, zahoďte data
            }

            // Filtrace a uložení pouze těch parametrů, o které bylo požádáno
            var requestTime = timestamp != -1
                ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp)
                : DateTimeOffset.UtcNow;

            foreach (var identifier in requestRecord.DataIdentifiers)
            {
                if (responseData.TryGetValue(identifier, out var value))
                {
                    DataStorage.AddData(identifier, value, requestTime);
                }
            }

            // Odstranění požadavku z `requestRecords`
            DataStorage.RemoveRequestRecord(sequenceNumber);
        }

        public static (int sequenceNumber, MessageType messageType, Dictionary<DataIdentifier, object> data) DecodeRequest(byte[] cborData)
        {
            var reader = new CborReader(cborData);
            reader.ReadStartMap();

            int sequenceNumber = -1;
            MessageType messageType = MessageType.Undefined;
            var data = new Dictionary<DataIdentifier, object>();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                int key = reader.ReadInt32();

                if (key == (int)CborIdentifiers.SequenceNumber)
                {                   
                    sequenceNumber = reader.ReadInt32();
                }
                else if (key == (int)MessageType.Request)
                {
                    messageType = MessageType.Request;
                    data = DecodeIdentifiers(reader);
                }
                else if (key == (int)MessageType.Set)
                {
                    messageType = MessageType.Set;
                    data = DecodeIdentifiersWithValues(reader);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown key in CBOR data: {key}");
                }
            }

            reader.ReadEndMap();
            return (sequenceNumber, messageType, data);
        }

        private static Dictionary<DataIdentifier, object> DecodeIdentifiers(CborReader reader)
        {
            var identifiers = new Dictionary<DataIdentifier, object>();

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var id = reader.ReadInt32();
                var identifier = DataIdentifierRegistry.GetById(id)
                    ?? throw new InvalidOperationException($"No identifier found for ID {id}");

                // Ukládáme pouze identifikátor bez hodnoty (null) pro typ Request
                identifiers[identifier] = null;
            }
            reader.ReadEndArray();

            return identifiers;
        }

        private static Dictionary<DataIdentifier, object> DecodeIdentifiersWithValues(CborReader reader)
        {
            var identifiersWithValues = new Dictionary<DataIdentifier, object>();

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var id = reader.ReadInt32();
                var identifier = DataIdentifierRegistry.GetById(id)
                    ?? throw new InvalidOperationException($"No identifier found for ID {id}");

                // Čteme hodnotu na základě očekávaného typu pro typ Set
                var value = ReadValue(reader, identifier.ExpectedType);
                identifiersWithValues[identifier] = value;
            }
            reader.ReadEndArray();

            return identifiersWithValues;
        }
     

        private static object ReadValue(CborReader reader, Type expectedType)
        {
            return expectedType switch
            {
                Type t when t == typeof(int) => reader.ReadInt32(),
                Type t when t == typeof(float) => reader.ReadSingle(),
                Type t when t == typeof(bool) => reader.ReadBoolean(),
                Type t when t == typeof(string) => reader.ReadTextString(),
                Type t when t == typeof(long) => reader.ReadInt64(),
                Type t when t == typeof(ushort) => (ushort)reader.ReadUInt32(),
                Type t when t == typeof(byte) => (byte)reader.ReadUInt32(),
                Type t when t == typeof(sbyte) => (sbyte)reader.ReadInt32(),
                Type t when t == typeof(TimeSpan) => TimeSpan.FromTicks(reader.ReadInt64()),
                _ => throw new InvalidOperationException($"Unsupported type {expectedType.Name}"),
            };
        }

        public static byte[] EncodeResponse(int sequenceNumber, Dictionary<DataIdentifier, object> responseData)
        {
            var writer = new CborWriter();
            writer.WriteStartMap(responseData.Count + 2);

            writer.WriteInt32((int)CborIdentifiers.SequenceNumber);
            writer.WriteInt32(sequenceNumber);

            writer.WriteInt32((int)CborIdentifiers.Timestamp);
            writer.WriteInt64(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            foreach (var entry in responseData)
            {
                writer.WriteInt32(entry.Key.Id);
                WriteValue(writer, entry.Value);
            }

            writer.WriteEndMap();
            return writer.Encode();
        }
    }
}
