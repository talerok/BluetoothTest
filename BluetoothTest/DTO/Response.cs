using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.DTO
{
    class Response
    {
        [JsonProperty("EUI")]
        public string EUI { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        public Response(Request req)
        {
            EUI = req.EUI;
            Sequence = req.Sequence;
        }
    }

    class RfidResp : Response
    {
        [JsonProperty("RFID")]
        public RFIDWithStatus RFID { get; set; }

        public RfidResp(RfidReq req, string status) : base(req)
        {
            RFID = new RFIDWithStatus { Status = status, UID = req.RFID.UID, Version = req.RFID.Version };
        }
    }


    class MeasurementResp : Response
    {
        [JsonProperty("Measurement")]
        public Measurement Measurement { get; set; }

        public MeasurementResp(MeasurementReq req, Measurement meas) : base(req)
        {
            Measurement = meas;
        }
    }

    class MeteringResp : Response
    {
        [JsonProperty("Status_Request")]
        public string Status { get; set; }

        public MeteringResp(MeteringReq req, string status) : base(req)
        {
            Status = status;
        }
    }

}
