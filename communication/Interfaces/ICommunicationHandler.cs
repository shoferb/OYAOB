using System.Collections.Generic;
using System.Threading.Tasks;

namespace TexasHoldem.communication.Interfaces
{
    interface ICommunicationHandler
    {
        List<string> GetReceivedMessages();
        bool AddMsgToSend(string msg, int userId);
        //void AcceptClients();
        Task<bool> Start();
        void Close();
    }
}
