using APSRede.Conversation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace APSRede.Conversation
{
    public class HostChat : IChatMessage
    {
        private TcpListener _tcpListener;
        private TcpClient _tcpClient;
        private Stream _stream;
        public string IpAddress { get; private set; }
        public int Port { get; private set; }
        public string ConnectedIpAddress { get; set; }

        public HostChat(int port)
        {
            IpAddress = GetInternIp();
            IPAddress ip = IPAddress.Parse(IpAddress);
            _tcpListener = new TcpListener(ip, port);
            Port = port;
        }

        public IPAddress Start()
        {
            _tcpListener.Start();
            var client = _tcpListener.AcceptTcpClient();

            IPAddress connectedClient = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
            ConnectedIpAddress = connectedClient.ToString();
            _stream = client.GetStream();

            return connectedClient;
        }

        public void SendMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            _stream.Write(messageBytes, 0, messageBytes.Length);
        }

        public string WaitMessage()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            string messageReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return messageReceived;
        }

        private string GetInternIp()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(hostName);

            return entry.AddressList[entry.AddressList.Count() - 1].ToString();
        }
    }
}
