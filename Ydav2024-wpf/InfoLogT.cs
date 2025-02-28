using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    public class SignalT : INotifyPropertyChanged
    {
        [JsonPropertyName("rsrp")]
        private long rsrp;
        [JsonPropertyName("rsrq")]
        private long rsrq;
        [JsonPropertyName("rssi")]
        private long rssi;
        [JsonPropertyName("network_type")]
        private string networkType;
        [JsonPropertyName("sim_county_iso")]
        private string simCountyIso;
        [JsonPropertyName("sim_operator")]
        private string simOperator;
        [JsonPropertyName("sim_operator_name")]
        private string simOperatorName;
        public event PropertyChangedEventHandler PropertyChanged;

        public long Rsrp
        {
            get { return rsrp; }
            set
            {
                rsrp = value;
                OnPropertyChanged("Rsrp");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public long Rsrq
        {
            get { return rsrq; }
            set
            {
                rsrq = value;
                OnPropertyChanged("Rsrq");
            }
        }
        public long Rssi
        {
            get { return rssi; }
            set
            {
                rssi = value;
                OnPropertyChanged("Rssi");
            }
        }
        public string NetworkType
        {
            get { return networkType; }
            set
            {
                networkType = value;
                OnPropertyChanged("NetworkType");
            }
        }
        public string SimOperatorName
        {
            get { return simOperatorName; }
            set
            {
                simOperatorName = value;
                OnPropertyChanged("SimOperatorName");
            }
        }
        public string SimCountyIso
        {
            get { return simCountyIso; }
            set
            {
                simCountyIso = value;
                OnPropertyChanged("SimCountyIso");
            }
        }
        public string SimOperator
        {
            get { return simOperator; }
            set
            {
                simOperator = value;
                OnPropertyChanged("SimOperator");
            }
        }

    }

    [Serializable]
    public class BatteryT : INotifyPropertyChanged
    {
        [JsonPropertyName("temperature")]
        private double temperature;
        [JsonPropertyName("level")]
        private double level;
        [JsonPropertyName("status")]
        private string status;
        [JsonPropertyName("charge")]
        private string charge;
        public event PropertyChangedEventHandler PropertyChanged;

        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged("Temperature");
            }
        }
        public double Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        public string Charge
        {
            get { return charge; }
            set
            {
                charge = value;
                OnPropertyChanged("Charge");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    [Serializable]
    public class PhonesT : INotifyPropertyChanged
    {
        [JsonPropertyName("time")]
        private string time;
        [JsonPropertyName("battery")]
        private Battery battery;
        [JsonPropertyName("signal")]
        private Signal signal;
        [JsonPropertyName("sms")]
        private uint sms;
        [JsonPropertyName("phone")]
        private uint phone;
        public event PropertyChangedEventHandler PropertyChanged;


        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }
        public Battery Battery
        {
            get { return battery; }
            set
            {
                battery = value;
                OnPropertyChanged("Battery");
            }
        }
        public Signal Signal
        {
            get { return signal; }
            set
            {
                signal = value;
                OnPropertyChanged("Signal");
            }
        }
        public uint Sms
        {
            get { return sms; }
            set
            {
                sms = value;
                OnPropertyChanged("Sms");
            }
        }
        public uint Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }
        /*public static (Phones, String) Connect(String adress, CommandSend commandSend, String param )
        {
            var sendCommand = new SendCommand();
            var sSend = sendCommand.Command(commandSend);
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(adress, 38300);
            }
            catch (Exception e)
            {
                tcpClient.Close();
                var error = $"Ошибка: {e.Message}!";
                return (null, error);
            }
            NetworkStream stream = tcpClient.GetStream();

            byte[] data = Encoding.UTF8.GetBytes("  " + sSend + "\n");
            stream.Write(data, 0, data.Length);

            var responseData = new byte[512];
            var message = new StringBuilder();
            int bytes; 
            do
            {
                bytes = stream.Read(responseData,0,512);
                message.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
            }
            while (bytes > 0); // пока данные есть в потоке 

            try
            {
                tcpClient.Close();
                Phones phones = JsonSerializer.Deserialize<Phones>(message.ToString());
                return (phones, message.ToString());
            }

            catch (Exception e)
            {
                tcpClient.Close();
                var error = $"Ошибка: {e.Message}!\n {message}";
                
                return (null, error);
            }
        }
        */
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class InfoLogT : INotifyPropertyChanged
    {
        private PhonesT info;
        private string json;
        public event PropertyChangedEventHandler PropertyChanged;


        public PhonesT Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        public string Json
        {
            get { return json; }
            set
            {
                json = value;
                OnPropertyChanged("Json");
            }
        }
        /*
        public static InfoLogT Connect(string address)
        {

          //  var (info, json) = Phones.Connect(address, CommandSend.INFO, "");
            return new InfoLogT { Info = info, Json = json };
        }
        */
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
