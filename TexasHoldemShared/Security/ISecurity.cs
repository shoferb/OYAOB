namespace TexasHoldemShared.Security
{
    public interface ISecurity
    {
        long GenerateNewSessionId(); //unique session id

        //TODO: add methods for encryption here
    }
}
