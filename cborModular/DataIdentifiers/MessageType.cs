using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataIdentifiers
{
    public enum MessageType
    {
        Request = 1100,
        Set = 1101,
        Notification = 1102,
        Undefined =1103     
    }
}
