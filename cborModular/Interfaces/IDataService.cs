using cborModular.DataIdentifiers;
using cborModular.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Interfaces
{
    public interface IDataService
    {
        // Metody pro RequestSetStorage
        void AddData(DataIdentifier identifier, object value, DateTimeOffset? timestamp = null);
        object GetLastValue(DataIdentifier identifier);
        void AddRequest(params DataIdentifier[] identifiers);
        IReadOnlyCollection<DataIdentifier> GetRequestedIdentifiers();
        IReadOnlyCollection<DataIdentifier> GetSetIdentifiers();
        Dictionary<DataIdentifier, object> GetSetValues();
        void ClearSet();
        void ClearRequest();

        // Metody pro RequestStorage
        void AddRequestRecord(int sequenceNumber, Dictionary<DataIdentifier, object> data, MessageType messageType);
        RequestRecord GetRequestRecord(int sequenceNumber);
        List<RequestRecord> GetAllRequestRecords();
        void RemoveRequestRecord(int sequenceNumber);
        void RemoveRequestRecordsByTimeout(TimeSpan timeout);
 

        // Metody pro SequenceManager
        int GetSequenceNumber();
        int IncrementSequenceNumber();

        // Metody pro BleDeviceStorage
         void AddBleDevice(DeviceModel device);
         void RemoveBleDevice(DeviceModel device);
         DeviceModel GetConnectedModel();
         void SetDeviceConnection(DeviceModel device, bool set = false);
    }
}
