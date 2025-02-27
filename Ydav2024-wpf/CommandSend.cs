using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Ydav2024_wpf
{
    public enum CommandSend
    {
        INFO
    }

    public class SendCommandJson
    {

        [JsonPropertyName("command")] 
        public string Command { get; set; }

        [JsonPropertyName("param")]
        public string Param { get; set; }
    }
     public class SendCommand
        {
            public string Command(CommandSend commandSend)
            {
                switch (commandSend) {
                    case CommandSend.INFO:
                        var sendCommandJson = new SendCommandJson {
                            Command = "INFO",
                            Param = ""
                        };
                        return JsonSerializer.Serialize<SendCommandJson>(sendCommandJson);
                    default:
                        return "";
                }
            }
        }
}
