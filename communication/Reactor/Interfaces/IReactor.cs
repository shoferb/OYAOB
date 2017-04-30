namespace TexasHoldem.communication.Reactor.Interfaces
{
    interface IReactor
    {
        void RegisterHandler(IEventHandler eventHandler);
        void RemoveHandler(IEventHandler eventHandler);
        void AcceptClients();
        void Start();
        void Close();
    }
}
