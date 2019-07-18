using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.DTO
{
    class RFID
    {
        [JsonProperty("Version")]
        public int Version { get; set; }
        [JsonProperty("UID")]
        public String UID { get; set; }
    }

    class RFIDWithStatus: RFID
    {
        [JsonProperty("Status_Request")]
        public string Status { get; set; }
    }
}
