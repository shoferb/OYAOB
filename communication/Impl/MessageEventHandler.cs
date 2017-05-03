using System.Collections.Generic;
using TexasHoldem.communication.Interfaces;

//takes msgs from msgQueue in CommHandler and deals with them using EventHandlers
namespace TexasHoldem.communication.Impl
{
    public class MessageEventHandler
    {
        //TODO: fill this class up

        private List<IEventHandler> _eventHandlers;

        public void HandleRawMsg(string data)
        {
        }
    }
}