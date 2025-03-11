using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Sockets;

namespace Ydav2024_wpf
{

    public class ResultStatus
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("sent")]
        public ResultStatus Sent { get; set; }

        [JsonPropertyName("delivery")]
        public ResultStatus Delivery { get; set; }
    }

    public class StatusSMSOutput: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        public static (StatusSMSOutput, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json==null?null:JsonSerializer.Deserialize<StatusSMSOutput>(json), jsonText, error);
        }
    }

    public class SmsOutputParam
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }

        public SmsOutputParam(int id, string phone, string text)
        {
            this.Id = id.ToString();
            this.Phone = phone;
            this.Text = text;
        }

        public string Json()
        {
            var res = "{id:\""+this.Id+"\",phone:\""+this.Phone+ "\",text:\""+this.Text+"\"}";
            return res;
        }
    }


    public class SMSOutputLog
    {
        public StatusSMSOutput StatusLog { get; set; }

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

        public static SMSOutputLog Send(string address, SmsOutputParam smsOutputParam)
        {
            var (status, json, error) = StatusSMSOutput.Connect(address, CommandSend.SmsOutput, smsOutputParam.Json());
            return new SMSOutputLog { StatusLog = status, Json = json, Error = error };
        }
        public static SMSOutputLog Status(string address, string id)
        {
            var (status, json, error) = StatusSMSOutput.Connect(address, CommandSend.SmsOutputStatus, id);
            return new SMSOutputLog { StatusLog = status, Json = json, Error = error };
        }
    }

}
