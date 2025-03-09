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

    public class StatusSMSOutput
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        public static (StatusSMSOutput, String, String) Connect(String adress, CommandSend commandSend, String param)
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
                StatusSMSOutput statusSMSOutput = JsonSerializer.Deserialize<StatusSMSOutput>(json.ToString());
                return (statusSMSOutput, json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
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
            return JsonSerializer.Serialize<SmsOutputParam>(this);
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

        public static SMSOutputLog XSend(string address, SmsOutputParam smsOutputParam)
        {
            var (status, json, error) = StatusSMSOutput.Connect(address, CommandSend.SmsOutputStatus, smsOutputParam.Json());
            return new SMSOutputLog { StatusLog = status, Json = json, Error = error };
        }
        public static SMSOutputLog XStatus(string address, string id)
        {
            var (status, json, error) = StatusSMSOutput.Connect(address, CommandSend.SmsOutputStatus, id);
            return new SMSOutputLog { StatusLog = status, Json = json, Error = error };
        }
    }

}
