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

    public class PhonesL: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("phone")]
        public List<Phone> PhoneList { get; set; } = new List<Phone>();
        public static (PhonesL, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json == null ? null : JsonSerializer.Deserialize<PhonesL>(json), jsonText, error);
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
