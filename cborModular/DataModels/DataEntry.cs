using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataModels
{
    public class DataEntry
    {
        public object Data { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public DataEntry(object data)
        {
            Data = data;            
        }
      
        public DataEntry(object data, DateTimeOffset timestamp)
        {
            Data = data;
            Timestamp = timestamp;
        }
    }
}
