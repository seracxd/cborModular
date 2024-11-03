using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cborModular.DataIdentifiers;
using cborModular.DataModels;

namespace cborModular.Services
{
    internal static class CborHandler
    {

        /// <summary>
        /// Encodes a list of requested identifiers as a CBOR array.
        /// </summary>     
        /// <returns>CBOR-encoded byte array</returns>
        public static byte[] EncodeRequest()
        {
            var writer = new CborWriter();

            var requestedIdentifiers = DataStorage.GetRequestedIdentifiers();
            writer.WriteStartArray(requestedIdentifiers.Count+1);
            writer.WriteInt32(DataStorage.IncrementSequenceNumber());

            foreach (var identifier in requestedIdentifiers)
            {
                writer.WriteInt32(identifier.Id);
            }

            writer.WriteEndArray();

            // Clear request identifiers after encoding
            DataStorage.ClearRequest();
            return writer.Encode();
        }
        /// <summary>
        /// Decodes a CBOR request to get the list of requested identifiers.
        /// </summary>
        /// <param name="cborData">CBOR-encoded request data</param>
        /// <returns>Sequence number of request, List of requested identifiers</returns>
        public static (int sequenceNumber, List<DataIdentifier>) DecodeRequest(byte[] cborData)
        {
            var reader = new CborReader(cborData);

            reader.ReadStartArray();
            int sequenceNumber = reader.ReadInt32(); // Read sequence number

            var requestedIdentifiers = new List<DataIdentifier>();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var id = reader.ReadInt32();
                var identifier = DataIdentifierRegistry.GetById(id);
                if (identifier != null)
                {
                    requestedIdentifiers.Add(identifier);
                }
                else
                {
                    throw new InvalidOperationException($"No identifier found for ID {id}");
                }
            }

            reader.ReadEndArray();
            return (sequenceNumber, requestedIdentifiers);
        }

        /// <summary>
        /// Encodes response data into CBOR format.
        /// </summary>
        /// <param name="sequenceNumber">Sequence number of request</param>
        /// <param name="responseData">Dictionary of identifiers and their values</param>
        /// <returns>CBOR-encoded byte array</returns>
        public static byte[] EncodeResponse(int sequenceNumber, Dictionary<DataIdentifier, object> responseData)
        {
            var writer = new CborWriter();

            writer.WriteStartMap(responseData.Count + 1);
            writer.WriteTextString("S");
            writer.WriteInt32(sequenceNumber); // Write the sequence number first

            foreach (var entry in responseData)
            {
                writer.WriteInt32(entry.Key.Id);

                switch (entry.Value)
                {
                    case float f:
                        writer.WriteSingle(f);
                        break;
                    case bool b:
                        writer.WriteBoolean(b);
                        break;
                    case int i:
                        writer.WriteInt32(i);
                        break;
                    case uint u:
                        writer.WriteUInt32(u);
                        break;
                    case ushort us:
                        writer.WriteUInt32(us); // Encoded as uint
                        break;
                    case sbyte sb:
                        writer.WriteInt32(sb); // Encoded as int
                        break;
                    case byte bt:
                        writer.WriteUInt32(bt); // Encoded as uint
                        break;
                    case TimeSpan ts:
                        writer.WriteInt64(ts.Ticks); // Encode TimeSpan as ticks (long)
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported data type");
                }
            }

            writer.WriteEndMap();
            return writer.Encode();
        }

        /// <summary>
        /// Decodes a CBOR response and updates the provided storage dictionary with the data.
        /// </summary>
        /// <param name="cborData">CBOR-encoded response data</param>
        /// <returns>S Dictionary of requested data</returns>
        public static void DecodeResponse(byte[] cborData)
        {
            var reader = new CborReader(cborData);

            reader.ReadStartMap();
            string s = reader.ReadTextString();
            int sequenceNumber = reader.ReadInt32();

            var responseData = new Dictionary<DataIdentifier, object>();
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var id = reader.ReadInt32();
                var identifier = DataIdentifierRegistry.GetById(id);

                if (identifier != null)
                {
                    object value = identifier.ExpectedType switch
                    {
                        Type t when t == typeof(float) => reader.ReadSingle(),
                        Type t when t == typeof(bool) => reader.ReadBoolean(),
                        Type t when t == typeof(ushort) => (ushort)reader.ReadUInt32(),
                        Type t when t == typeof(int) => reader.ReadInt32(),
                        Type t when t == typeof(sbyte) => (sbyte)reader.ReadInt32(),
                        Type t when t == typeof(byte) => (byte)reader.ReadUInt32(),
                        Type t when t == typeof(TimeSpan) => TimeSpan.FromTicks(reader.ReadInt64()),
                        _ => throw new InvalidOperationException($"Unsupported data type for identifier with ID {id}")
                    };
                    DataStorage.AddData(identifier, value);                  
                }
                else
                {
                    throw new InvalidOperationException($"No identifier found for ID {id}");
                }
            }

            reader.ReadEndMap();
        }
    }
}
