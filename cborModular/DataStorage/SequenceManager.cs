using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataStorage
{
    internal static class SequenceManager
    {
        private static int _sequenceNumber = 0;

        public static int GetSequenceNumber() => _sequenceNumber;
        public static int IncrementSequenceNumber() => ++_sequenceNumber;
    }
}
