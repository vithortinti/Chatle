using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSRede.Conversation.Interfaces
{
    public interface IChatMessage
    {
        string ConnectedIpAddress { get; }
        void SendMessage(string message);
        string WaitMessage();
    }
}
