using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Sockets;

namespace Ydav2024_wpf
{

    [Serializable]
    public class Signal
    {
        [JsonPropertyName("rsrp")]
        public long Rsrp { get; set; }
        [JsonPropertyName("rsrq")]
        public long Rsrq { get; set; }
        [JsonPropertyName("rssi")]
        public long Rssi { get; set; }
        [JsonPropertyName("network_type")]
        public string NetworkType { get; set; }
        [JsonPropertyName("sim_county_iso")]
        public string SimCountyIso { get; set; }
        [JsonPropertyName("sim_operator")]
        public string SimOperator { get; set; }
        [JsonPropertyName("sim_operator_name")]
        public string SimOperatorName { get; set; }
    }

    [Serializable]
    public class Battery
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        [JsonPropertyName("level")]
        public double Level { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("charge")]
        public string Charge { get; set; }
    }

    [Serializable]
    public class Phones: AndroidConnect
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [JsonPropertyName("battery")]
        public Battery Battery { get; set; }
        [JsonPropertyName("signal")]
        public Signal Signal { get; set; }
        [JsonPropertyName("sms")]
        public uint Sms { get; set; }
        [JsonPropertyName("phone")]
        public uint Phone { get; set; }
        public static (Phones, String, String) Connect(String adress, CommandSend commandSend, String param)
        {
            var (json, jsonText, error) = ConnectBase(adress, commandSend, param);
            return (json == null ? null : JsonSerializer.Deserialize<Phones>(json), jsonText, error);
        }
    }

    public class InfoLog
    {
        public InfoLog()
        {
            Info = null;
            Json = null;
            Error = "Введите IP сервера и соединитесь!";
        }
        public Phones Info { get; set; }
        public string Json { get; set; }
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
        public string VisibilitySMS
        {
            get
            {
                if (this.Info!=null)
                {
                    if (this.Info.Sms == 0)
                    {
                        return "Collapsed";
                    }
                    else
                    {
                        return "Visible";
                    }

                }
                else
                {
                    return "Collapsed";
                }

            }
        }
        public string VisibilityPHONE
        {
            get
            {
                if (this.Info != null)
                {
                    if (this.Info.Phone == 0)
                    {
                        return "Collapsed";
                    }
                    else
                    {
                        return "Visible";
                    }
                }
                else
                {
                    return "Collapsed";
                }

            }
        }

        public static InfoLog Connect(string address)
        {

            var (info, json, error) = Phones.Connect(address, CommandSend.INFO, "");
            return new InfoLog { Info = info, Json = json, Error=error};
        }
    }

    public class Level
    {
        private double _value;

        public string GetString(double current)
        {
            if (_value == default || _value == current)
            {
                _value = current;
                return $"{current:F1}";
            }

            if (_value < current)
            {
                return $"▲{current:F1}";
            }
            else
            {
                return $"▼{current:F1}";
            }
        }
    }
}