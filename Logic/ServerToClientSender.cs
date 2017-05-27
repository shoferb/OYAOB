using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Logic
{
    public class ServerToClientSender
    {
        public void SendMessageToClient(GameDataCommMessage gmData, List<int> idsToSend, bool useCommunication)
        {
            if (useCommunication)
            {
                foreach (int id in idsToSend)
                {
                    if (id == gmData.UserId && gmData.action == CommunicationMessage.ActionType.Join)
                    {
                        continue;
                    }
                    gmData.UserId = id; //id of the user to send 
                    gameServiceHandler.SendMessageToClientGameData(gmData);
                }
            }
        }

    }
}
