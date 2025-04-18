﻿using System;
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
        INFO,
        PHONE,
        CONTACT,
        SmsInput,
        DelSmsInput,
        DelPhone,
        SmsOutput,
        SmsOutputStatus,
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
            public string Command(CommandSend commandSend, string param="")
            {
                switch (commandSend) {
                case CommandSend.INFO:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "INFO",
                        Param = ""
                    });
                case CommandSend.PHONE:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "PHONE",
                        Param = ""
                    });
                case CommandSend.CONTACT:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "CONTACT",
                        Param = ""
                    });
                case CommandSend.DelPhone:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "DELETE_PHONE",
                        Param = param
                    });
                case CommandSend.SmsInput:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "SMS_INPUT",
                        Param = param
                    });
                case CommandSend.DelSmsInput:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "DELETE_SMS_INPUT",
                        Param = param
                    });
                case CommandSend.SmsOutput:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "SMS_OUTPUT",
                        Param = param
                    });
                case CommandSend.SmsOutputStatus:
                    return JsonSerializer.Serialize<SendCommandJson>(new SendCommandJson
                    {
                        Command = "SMS_OUTPUT_STATUS",
                        Param = param
                    });
                default:
                        return "";
                }
            }
        }
}
