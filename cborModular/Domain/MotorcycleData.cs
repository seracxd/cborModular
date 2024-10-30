using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.Domain
{
    public class MotorcycleData
    {
        public MotorcycleDataWithTimestamp<float>? Speed { get; set; }
        public MotorcycleDataWithTimestamp<float>? XCoord { get; set; }
        public MotorcycleDataWithTimestamp<float>? YCoord { get; set; }
        public MotorcycleDataWithTimestamp<float>? Throttle { get; set; }
     //   public DateTime Timestamp { get; private set; } // Časová stopa

        public MotorcycleData()
        {
         //   Timestamp = timestamp;
        }
    }
}
