using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ydav2024_wpf
{
    public class AndroidConnect
    {
        protected static (String, String, String) ConnectBase(String adress, CommandSend commandSend, String param)
        {
            var sendCommand = new SendCommand();
            var json = new StringBuilder();
            var sSend = sendCommand.Command(commandSend, param);
            TcpClient tcpClient = new TcpClient();
            tcpClient.SendTimeout = 1;
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
                return (json.ToString(), json.ToString(), null);
            }

            catch (Exception e)
            {
                tcpClient.Close();
                return (null, json.ToString(), $"Ошибка: {e.Message}!");
            }
        }
    }
}
