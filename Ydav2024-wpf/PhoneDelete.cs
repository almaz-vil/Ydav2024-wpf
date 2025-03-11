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
    public class PhoneCount: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("phone")]
        public uint Phone { get; set; }
        public static (PhoneCount, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json == null ? null : JsonSerializer.Deserialize<PhoneCount>(json), jsonText, error);
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
