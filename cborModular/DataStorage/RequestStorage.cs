using cborModular.DataIdentifiers;
using cborModular.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cborModular.DataStorage
{
    internal class RequestStorage
    {
        private readonly List<RequestRecord> _requestRecords = new();

        public void AddRequestRecord(int sequenceNumber, Dictionary<DataIdentifier, object> data, MessageType messageType)
        {
            RequestRecord requestRecord = messageType switch
            {
                MessageType.Request => new RequestRecord(
                    messageType,
                    sequenceNumber,
                    data.Keys.ToList(),
                    DateTimeOffset.UtcNow),

                MessageType.Set => new RequestRecord(
                    messageType,
                    sequenceNumber,
                    data,
                    DateTimeOffset.UtcNow),
                _ => throw new InvalidOperationException($"Unsupported message type: {messageType}")
            };

            _requestRecords.Add(requestRecord);
        }

        public RequestRecord GetRequestRecord(int sequenceNumber)
        {
            return _requestRecords.FirstOrDefault(record => record.SequenceNumber == sequenceNumber);
        }

        public List<RequestRecord> GetAllRequestRecords() => new(_requestRecords);

        public RequestRecord GetRequestRecord(MessageType messageType)
        {
            return _requestRecords.FirstOrDefault(record =>
                record.MessageType == messageType);
        }
        public void RemoveRequestRecord(int sequenceNumber)
        {
            _requestRecords.RemoveAll(record => record.SequenceNumber == sequenceNumber);
        }

        public void RemoveRequestRecordsByTimeout(TimeSpan timeout)
        {
            var now = DateTimeOffset.UtcNow;
            _requestRecords.RemoveAll(record => (now - record.TimeOfRequest) > timeout);
        }
    }
}
