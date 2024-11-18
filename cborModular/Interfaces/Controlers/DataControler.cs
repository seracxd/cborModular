using cborModular.DataIdentifiers;
using cborModular.DataModels;
using cborModular.DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Interfaces.Controlers
{
    internal class DataControler : IDataService
    {
        private readonly RequestSetStorage _requestSetStorage;
        private readonly RequestStorage _requestStorage;
        private readonly BleDeviceStorage _bleDeviceStorage;

        // Konstruktor fasády, kde se vytvářejí interní závislosti
        public DataControler()
        {           
            _requestSetStorage = new RequestSetStorage();
            _requestStorage = new RequestStorage();
            _bleDeviceStorage = new BleDeviceStorage();
        }

        // Implementace metod RequestSetStorage
        public void AddData(DataIdentifier identifier, object value, DateTimeOffset? timestamp = null)
            => _requestSetStorage.AddData(identifier, value, timestamp);

        public object GetLastValue(DataIdentifier identifier)
            => _requestSetStorage.GetLastValue(identifier);

        public void AddRequest(params DataIdentifier[] identifiers)
            => _requestSetStorage.AddRequest(identifiers);

        public IReadOnlyCollection<DataIdentifier> GetRequestedIdentifiers()
            => _requestSetStorage.GetRequestedIdentifiers();

        public IReadOnlyCollection<DataIdentifier> GetSetIdentifiers()
            => _requestSetStorage.GetSetIdentifiers();

        public void ClearRequest()
            => _requestSetStorage.ClearRequest();
        public void ClearSet()
            => _requestSetStorage.ClearSet();


        // Implementace metod RequestStorage
        public void AddRequestRecord(int sequenceNumber, Dictionary<DataIdentifier, object> data, MessageType messageType)
            => _requestStorage.AddRequestRecord(sequenceNumber, data, messageType);

        public RequestRecord GetRequestRecord(int sequenceNumber)
            => _requestStorage.GetRequestRecord(sequenceNumber);

        public List<RequestRecord> GetAllRequestRecords()
            => _requestStorage.GetAllRequestRecords();

        public void RemoveRequestRecord(int sequenceNumber)
            => _requestStorage.RemoveRequestRecord(sequenceNumber);

        public void RemoveRequestRecordsByTimeout(TimeSpan timeout)
            => _requestStorage.RemoveRequestRecordsByTimeout(timeout);

        public Dictionary<DataIdentifier, object> GetSetValues()
            => _requestSetStorage.GetSetValues();

        // Implementace metod SequenceManager
        public int GetSequenceNumber()
            => SequenceManager.GetSequenceNumber();

        public int IncrementSequenceNumber()
            => SequenceManager.IncrementSequenceNumber();


        // iplementace pro BleDeviceStorage
        public void AddBleDevice(DeviceModel device) => _bleDeviceStorage.AddBleDevice(device);
        public void RemoveBleDevice(DeviceModel device)=> _bleDeviceStorage.RemoveBleDevice(device);
        public DeviceModel GetConnectedModel() => _bleDeviceStorage.GetConnectedModel();
        public void SetDeviceConnection(DeviceModel device, bool set = false)=>_bleDeviceStorage.SetDeviceConnection(device, set);

    }
}
