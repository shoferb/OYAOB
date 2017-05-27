namespace TexasHoldemShared.Security
{
    interface ISecurity
    {
        long GenerateNewSessionId(); //unique session id

        //TODO: add methods for encryption here
    }
}
