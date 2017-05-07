using System;
using System.IO;
using System.Xml.Serialization;
namespace TexasHoldemShared.Parser
{
    public interface ICommMsgXmlParser
    {

        public static string SerializeMsg(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        public static T ParseString<T>(string xmlText)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

       

  //      string SerializeMsg(CommMessages.CommunicationMessage msg);
    //    CommMessages.CommunicationMessage ParseString(string msg); //TODO: parse to exact msg type (GameDataMessage for example)
    }
}
