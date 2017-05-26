using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TexasHoldem.communication.Interfaces
{
    public interface ICommunicationHandler
    {
        List<Tuple<string, TcpClient>> GetReceivedMessages();
        TcpClient GetSocketById(int id);
        bool AddMsgToSend(string msg, int userId);
        List<String> GetMsgsToSend();
        void AddUserId(int id, TcpClient socket);
        void Start();
        void Close();
    }
}
