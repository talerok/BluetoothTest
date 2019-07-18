using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using BluetoothTest.DTO;
using InTheHand.Net.Sockets;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BluetoothTest
{
    class Program
    {
        static object fromJson(string json)
        {
            var regex = Regex.Match(json, @"(\w+): ({[\w\W]+})");
            if (!regex.Success)
                return null;
            var type = Type.GetType($"BluetoothTest.DTO.{regex.Groups[1].Value}");
            if (type == null)
                return null;
            return JsonConvert.DeserializeObject(regex.Groups[2].Value, type);
        }
        
        static byte[] _responseToBytes(object response)
        {
            var json = $"{response.GetType().Name}: {JsonConvert.SerializeObject(response)}";
            return Encoding.ASCII.GetBytes(json);
        }

        private static readonly Guid _uuid = new Guid("9494ee87-585d-4b80-b971-e32462d0e32a");

        private static void _readSocket(Socket socket, Func<string, byte[]> responseFactory)
        {
            var buffer = new byte[1024];
            while (true)
            {
                var size = socket.Receive(buffer);
                if (size == -1)
                    return;

                var data = Encoding.Default.GetString(buffer.Take(size).ToArray());
                Console.WriteLine($"Get: {data}");
                var response = responseFactory(data);
                if (response == null || response.Length == 0)
                    continue;
                socket.Send(response);
            }
        }

        private static Response _getResponse(string data)
        {
            var req = fromJson(data) as Request;
     
            if (req == null)
                return null;

            if (req is RfidReq)
                return new RfidResp(req as RfidReq, "Fail");

            if (req is MeasurementReq)
                return new MeasurementResp(req as MeasurementReq, new Measurement { DateTime = DateTime.Now, Version = 1, Type = new string[] { "Vibration_RMS" } });

            if (req is MeteringReq)
                return new MeteringResp(req as MeteringReq, "Ok");

            return null;
        }

        private static void _server()
        {
            var listner = new BluetoothListener(_uuid);
            listner.Start(); 
            while(true)
            {
                var socket = listner.AcceptSocket();
                Console.WriteLine($"Connected");
                Task.Factory.StartNew(() => _readSocket(
                    socket,
                    (request) => 
                    {
                        var response = _getResponse(request);
                        return request != null ? _responseToBytes(response) : null; 
                    }
                ));
            }
        }

        private static BluetoothDeviceInfo selectDevice(BluetoothClient client)
        {
            var devices = client.DiscoverDevices(9999, true, false, false).ToList();
            Console.WriteLine("Устройства:");
            for (var i = 0; i < devices.Count; i++)
                Console.WriteLine($"{i + 1}){devices[i].DeviceName} ({devices[i].DeviceAddress})");

            return devices[Convert.ToInt32(Console.ReadLine()) - 1];
        }
   
        private static void _sendData(BluetoothClient client, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var res = Encoding.Default.GetBytes($"{data.GetType().Name}: {json}");
            client.GetStream().Write(res, 0, res.Length);
        }

        private static void _client()
        {
            var client = new BluetoothClient();
            var device = selectDevice(client);
            client.Connect(device.DeviceAddress, _uuid);
            _sendData(client, new RfidReq { EUI = "12345667", Sequence = 0, RFID = new RFID {UID = "1231231", Version = 1 } });
            while(true)
            {
                if (client.GetStream().CanRead)
                    Console.Write(client.GetStream().ReadByte());
            }
        }

        static void Main(string[] args)
        {
            _client();
            //_server();
        }
    }
}
