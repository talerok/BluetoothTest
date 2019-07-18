using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.DTO
{
    class SendMetering : Measurement
    {
        public double[] Value { get; set; }
    }
}
