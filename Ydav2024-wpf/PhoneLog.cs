using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Sockets;

namespace Ydav2024_wpf
{

    public class Phone
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("phone")]
        public string PhoneName { get; set; }
        [JsonPropertyName("status")] 
        public string Status { get; set; }
    }

    public class PhonesL
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("phone")]
        public List<Phone> PhoneList { get; set; } = new List<Phone>();
        public static (PhonesL, String, String) Connect(String adress, CommandSend commandSend, String param)
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
                PhonesL phones = JsonSerializer.Deserialize<PhonesL>(json.ToString());
                return (phones, json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
        }
    }

    public class PhoneLog
    {
        public PhonesL Phones { get; set; } = new PhonesL();
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

        public static PhoneLog Connect(string address)
        {

            var (Phones, json, error) = PhonesL.Connect(address, CommandSend.PHONE, "");
            return new PhoneLog { Phones = Phones, Json = json, Error = error };
        }

    }


}
