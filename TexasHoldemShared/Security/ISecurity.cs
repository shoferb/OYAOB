namespace TexasHoldemShared.Security
{
    public interface ISecurity
    {
        long GenerateNewSessionId(); //unique session id

        byte[] Encrypt(byte[] data);
        string Decrypt(byte[] data);
    }
}
