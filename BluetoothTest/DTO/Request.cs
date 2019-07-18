using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.DTO
{
    class Request
    {
        [JsonProperty("EUI")]
        public string EUI { get; set; }
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

    }

    class RfidReq: Request
    {
        [JsonProperty("RFID")]
        public RFID RFID { get; set; }
    }

    class MeasurementReq : Request
    {

    }

    class MeteringReq: Request
    {
        [JsonProperty("Send_Metering")]
        public SendMetering SendMetering { get; set; }
    }


}
