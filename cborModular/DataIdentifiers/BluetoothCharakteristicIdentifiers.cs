﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.DataIdentifiers
{
    public enum BluetoothCharakteristicIdentifiers
    {
        Unknown=-1,
        Read = 0,
        Write = 1,
        DataNotification = 2,
        WarningNotificacion = 3,

        Service =1000,
    }
}
