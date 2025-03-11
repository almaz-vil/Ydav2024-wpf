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

    public class Contacts: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("contact")]
        public List<Contact> Contact { get; set; } = new List<Contact>();
        public static (Contacts, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json == null ? null : JsonSerializer.Deserialize<Contacts>(json), jsonText, error);
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
