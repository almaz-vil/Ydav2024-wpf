using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Net.Sockets;
using System.Text.Json;

namespace Ydav2024_wpf
{
    public class PhoneCount
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("phone")]
        public uint Phone { get; set; }
        public static (PhoneCount, String, String) Connect(String adress, CommandSend commandSend, String param)
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
                PhoneCount phoneCount = JsonSerializer.Deserialize<PhoneCount>(json.ToString());
                return (phoneCount, json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
        }
    }
    class PhoneDelete
    {
        public PhoneCount Phone { get; set; } = new PhoneCount();
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

        public static PhoneDelete Connect(string address, string param)
        {

            var (phone, json, error) = PhoneCount.Connect(address, CommandSend.DelPhone, param);
            return new PhoneDelete { Phone = phone, Json = json, Error = error };
        }
    }


}
