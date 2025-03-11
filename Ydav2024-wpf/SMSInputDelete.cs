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
    public class SmsCount: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("sms")]
        public uint Sms { get; set; }
        public static (SmsCount, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json == null ? null : JsonSerializer.Deserialize<SmsCount>(json), jsonText, error);
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

