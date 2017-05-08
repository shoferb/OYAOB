using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.Parser
{
    class parserImplementation : ICommMsgXmlParser
    {


        public string SerializeMsg(CommMessages.CommunicationMessage msg)
        {

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(msg.GetType());
                serializer.Serialize(stringwriter, msg);
                string msgToRet = stringwriter.ToString();
                 if(msg.GetType()==typeof(ActionCommMessage)){
                     msgToRet = "a"+msgToRet;
                 }
                else if(msg.GetType()==typeof(EditCommMessage)){
                     msgToRet = "b"+msgToRet;
                 }
                else if(msg.GetType()==typeof(LoginCommMessage)){
                     msgToRet = "c"+msgToRet;
                 }
                else if(msg.GetType()==typeof(RegisterCommMessage)){
                     msgToRet = "d"+msgToRet;
                 }
                else if(msg.GetType()==typeof(GameDataCommMessage)){
                     msgToRet = "e"+msgToRet;
                 }
                else if(msg.GetType()==typeof(MoveOptionsCommMessage)){
                     msgToRet = "f"+msgToRet;
                 }
                else if(msg.GetType()==typeof(ResponeCommMessage)){
                     msgToRet = "g"+msgToRet;
                 }
                return msgToRet;
            }
        }

        public CommMessages.CommunicationMessage ParseString(string msg)
        {
           if(msg.IndexOf('a')==0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeActionCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('b') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeEditCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('c') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeLoginCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('d') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeRegisterCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('e') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeGameDataCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('f') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeMoveOptionsCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('g') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return deserializeResponeCommMessage(XMLmsg);
           }
            return null;
        }

        private ActionCommMessage deserializeActionCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(ActionCommMessage));
                return (ActionCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private EditCommMessage deserializeEditCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(EditCommMessage));
                return (EditCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private LoginCommMessage deserializeLoginCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(LoginCommMessage));
                return (LoginCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private RegisterCommMessage deserializeRegisterCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(RegisterCommMessage));
                return (RegisterCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private GameDataCommMessage deserializeGameDataCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(GameDataCommMessage));
                return (GameDataCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private MoveOptionsCommMessage deserializeMoveOptionsCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(MoveOptionsCommMessage));
                return (MoveOptionsCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private ResponeCommMessage deserializeResponeCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(ResponeCommMessage));
                return (ResponeCommMessage)serializer.Deserialize(stringReader);
            }
        }
       
     
        
    }
}
