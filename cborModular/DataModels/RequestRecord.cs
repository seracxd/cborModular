﻿using cborModular.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    public class RequestRecord
    {
        public MessageType MessageType { get; }  
        public int SequenceNumber { get; }
        public List<DataIdentifier> DataIdentifiers { get; }
        // For set
        public Dictionary<DataIdentifier, object> Values { get; }
        public DateTimeOffset TimeOfRequest { get; }

        public RequestRecord(MessageType messageType, int sequenceNumber, List<DataIdentifier> dataIdentifiers, DateTimeOffset timeOfRequest)
        {
            MessageType = messageType;
            SequenceNumber = sequenceNumber;
            DataIdentifiers = dataIdentifiers;
            TimeOfRequest = timeOfRequest;
            Values = [];
        }
        public RequestRecord(MessageType messageType, int sequenceNumber, Dictionary<DataIdentifier, object> values, DateTimeOffset timeOfRequest)
        {
            MessageType = messageType;
            SequenceNumber = sequenceNumber;
            DataIdentifiers = new List<DataIdentifier>(values.Keys); // Identifikátory získáme z klíčů ve slovníku
            Values = values;
            TimeOfRequest = timeOfRequest;
        }

    }
}
