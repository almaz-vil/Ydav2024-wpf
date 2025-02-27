using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Sockets;

namespace Ydav2024_wpf
{
  
    [Serializable]
    public class Signal
    {
        [JsonPropertyName("rsrp")]
        public long Rsrp { get; set; }
        [JsonPropertyName("rsrq")]
        public long Rsrq { get; set; }
        [JsonPropertyName("rssi")]
        public long Rssi { get; set; }
        [JsonPropertyName("network_type")]
        public string NetworkType { get; set; }
        [JsonPropertyName("sim_county_iso")]
        public string SimCountyIso { get; set; }
        [JsonPropertyName("sim_operator")]
        public string SimOperator { get; set; }
        [JsonPropertyName("sim_operator_name")]
        public string SimOperatorName { get; set; }
    }

    [Serializable]
    public class Battery
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        [JsonPropertyName("level")]
        public double Level { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("charge")]
        public string Charge { get; set; }
    }

    [Serializable]
    public class Phones
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("battery")]
        public Battery Battery { get; set; }
        [JsonPropertyName("signal")]
        public Signal Signal { get; set; }
        [JsonPropertyName("sms")]
        public uint Sms { get; set; }
        [JsonPropertyName("phone")]
        public uint Phone { get; set; }
        public static (Phones, String) Connect(String adress, CommandSend commandSend, String param )
        {
            var sendCommand = new SendCommand();
            var sSend = sendCommand.Command(commandSend);
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(adress, 38300);
            }
            catch (Exception e)
            {
                tcpClient.Close();
                var error = $"Ошибка: {e.Message}!";
                return (null, error);
            }
            NetworkStream stream = tcpClient.GetStream();

            byte[] data = Encoding.UTF8.GetBytes("  " + sSend + "\n");
            stream.Write(data, 0, data.Length);

            var responseData = new byte[512];
            var message = new StringBuilder();
            int bytes; 
            do
            {
                bytes = stream.Read(responseData,0,512);
                message.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
            }
            while (bytes > 0); // пока данные есть в потоке 

            try
            {
                tcpClient.Close();
                Phones phones = JsonSerializer.Deserialize<Phones>(message.ToString());
                return (phones, null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                var error = $"Ошибка: {e.Message}!\n {message}";
                
                return (null, error);
            }
        }
    }

    public class InfoLog
    {
        public Phones Info { get; set; }
        public string Json { get; set; }

        public static InfoLog Connect(string address)
        {

            var (info, json) = Phones.Connect(address, CommandSend.INFO, "");
            return new InfoLog { Info = info, Json = json };
        }
    }

    public class Level
    {
        private double _value;

        public string GetString(double current)
        {
            if (_value == default || _value == current)
            {
                _value = current;
                return $"{current:F1}";
            }

            if (_value < current)
            {
                return $"▲{current:F1}";
            }
            else
            {
                return $"▼{current:F1}";
            }
        }
    }
}
