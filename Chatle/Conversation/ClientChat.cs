using APSRede.Conversation.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace APSRede.Conversation
{
    public class ClientChat : IChatMessage
    {
        private TcpClient _tcpClient;
        private Stream _stream;
        public string ConnectedIpAddress { get; set; }

        public ClientChat(string ip, int port)
        {
            _tcpClient = new TcpClient(ip, port);
            _stream = _tcpClient.GetStream();
            ConnectedIpAddress = ip;
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
    }
}
