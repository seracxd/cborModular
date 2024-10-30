using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Domain
{
    public class MotorcycleDataWithTimestamp<T>
    {      
            public required T Value { get; set; }
            public DateTime Timestamp { get; set; }      
    }
}
