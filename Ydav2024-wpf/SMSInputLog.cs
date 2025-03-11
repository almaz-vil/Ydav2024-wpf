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

    public class SmsInput: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("sms")]
        public List<Sms> Sms { get; set; } = new List<Sms>();
        public static (SmsInput, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json == null ? null : JsonSerializer.Deserialize<SmsInput>(json), jsonText, error);
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


