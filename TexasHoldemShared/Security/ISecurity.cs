namespace TexasHoldemShared.Security
{
    public interface ISecurity
    {
        long GenerateNewSessionId(); //unique session id

        byte[] Encrypt(string data);
        string EncryptToString(string data);
        string Decrypt(byte[] data);
    }
}
