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
    public class Contact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phone")]
        public List<string> Phone { get; set; } = new List<string>();

        public string PhoneStr
        {
            get
            {
                return String.Join(",", this.Phone);
            }
        }
    }

    public class Contacts
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("contact")]
        public List<Contact> Contact { get; set; } = new List<Contact>();
        public static (Contacts, String, String) Connect(String adress, CommandSend commandSend, String param)
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
                Contacts contacts = JsonSerializer.Deserialize<Contacts>(json.ToString());
                return (contacts, json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
        }
    }

    public class ContactLog
    {
        public Contacts Contacts { get; set; } = new Contacts();
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

        public static ContactLog Connect(string address)
        {

            var (contacts, json, error) = Contacts.Connect(address, CommandSend.CONTACT, "");
            return new ContactLog { Contacts = contacts, Json = json, Error = error };
        }
    }

}
