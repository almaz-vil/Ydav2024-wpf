using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ydav2024_wpf
{
    public class SmsCount
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("sms")]
        public uint Sms { get; set; }
        public static (SmsCount, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var sendCommand = new SendCommand();
            var json = new StringBuilder();
            var sSend = sendCommand.Command(commandSend, param);
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(adress, 38300);
                NetworkStream stream = tcpClient.GetStream();

                byte[] data = Encoding.UTF8.GetBytes("  " + sSend + "\n");
                stream.Write(data, 0, data.Length);

                var responseData = new byte[512];
                int bytes;
                do
                {
                    bytes = stream.Read(responseData, 0, 512);
                    json.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                }
                while (bytes > 0); // пока данные есть в потоке 

            }
            catch (Exception e)
            {
                tcpClient.Close();
                return (null, "", $"Ошибка: {e.Message}!");
            }

            try
            {
                tcpClient.Close();
                SmsCount smsCount = JsonSerializer.Deserialize<SmsCount>(json.ToString());
                return (smsCount, json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
        }
    }
    class SMSInputDelete
    {
        public SmsCount SmsCount { get; set; } = new SmsCount();
        public string Json { get; set; } = string.Empty;
        public string Error { get; set; }
        public string VisibilityConnect
        {
            get
            {
                if (this.Error == null)
                {
                    return "Collapsed";
                }
                else
                {
                    return "Visible";
                }
            }
        }

        public static SMSInputDelete Connect(string address, string param)
        {

            var (sms, json, error) = SmsCount.Connect(address, CommandSend.DelSmsInput, param);
            return new SMSInputDelete { SmsCount = sms, Json = json, Error = error };
        }
    }

}

