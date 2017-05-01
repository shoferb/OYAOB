namespace TexasHoldem.communication.Reactor.Interfaces
{
    interface IReactor
    {
        void AcceptClients();
        void Start();
        void Close();
    }
}
