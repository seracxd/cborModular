using cborModular.DataIdentifiers;
using cborModular.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace cborModular.Services
{
    public class DataProcessor(BluetoothSimulator bluetoothSimulator)
    {
        private readonly BluetoothSimulator _bluetoothSimulator = bluetoothSimulator;

        /// <summary>
        /// Sends a request for the specified identifiers, receives the response, decodes it, and stores it in the provided dictionary.
        /// </summary>
        public void ProcessBluetooth(MessageType messageType)
        {
            // Step 1: Encode the request into CBOR format
            byte[] cborRequest = CborHandler.EncodeRequest(messageType);

            // Step 2: Send the request to the Bluetooth simulator and get the CBOR response
            byte[] cborResponse = _bluetoothSimulator.ProcessBluetoothRequest(cborRequest);

            // Step 3: Decode the CBOR response and store it in the provided storage dictionary
            CborHandler.DecodeResponse(cborResponse);
        }
    }
}
*/