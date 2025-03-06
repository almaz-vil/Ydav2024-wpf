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
    public class Sms
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("time")] 
        public string Time { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }

    public class SmsInput
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("sms")]
        public List<Sms> Sms { get; set; } = new List<Sms>();
        public static (SmsInput, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var sendCommand = new SendCommand();
            var json = new StringBuilder();
            var sSend = sendCommand.Command(commandSend);
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
                SmsInput smsInput = JsonSerializer.Deserialize<SmsInput>(json.ToString());
                return (smsInput, json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
        }
    }

    class SMSInputLog
    {
        public SmsInput SmsInput { get; set; } = new SmsInput();
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

        public static SMSInputLog Connect(string address)
        {

            var (smsInput, json, error) = SmsInput.Connect(address, CommandSend.SmsInput, "");
            return new SMSInputLog { SmsInput = smsInput, Json = json, Error = error };
        }
    }

}


