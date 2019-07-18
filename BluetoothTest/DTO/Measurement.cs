using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.DTO
{
    class Measurement
    {
        [JsonProperty("Version")]
        public int Version { get; set; }

        [JsonProperty("Date_Time")]
        public DateTime DateTime { get; set; }

        [JsonProperty("Type")]
        public string[] Type { get; set; }
    }
}
