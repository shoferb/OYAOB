using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TexasHoldemShared.CommMessages;
using TexasHoldemShared.CommMessages.ClientToServer;
using TexasHoldemShared.CommMessages.ServerToClient;

namespace TexasHoldemShared.Parser
{
    public class ParserImplementation : ICommMsgXmlParser
    {
        public ParserImplementation() {}
       

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
                else if (msg.GetType() == typeof(ChatCommMessage))
                {
                    msgToRet = "f" + msgToRet;
                }

                else if(msg.GetType()==typeof(ResponeCommMessage)){
                     msgToRet = "g"+msgToRet;
                 }
                else if (msg.GetType() == typeof(SearchCommMessage))
                {
                    msgToRet = "h" + msgToRet;
                }
                else if (msg.GetType() == typeof(LoginResponeCommMessage))
                {
                    msgToRet = "i" + msgToRet;
                }
                else if (msg.GetType() == typeof(RegisterResponeCommMessage))
                {
                    msgToRet = "j" + msgToRet;
                }
                else if (msg.GetType() == typeof(SearchResponseCommMessage))
                {
                    msgToRet = "k" + msgToRet;
                }
                else if (msg.GetType() == typeof(CreatrNewRoomMessage))
                {
                    msgToRet = "l" + msgToRet;
                }
                else if (msg.GetType() == typeof(ChatResponceCommMessage))
                {
                    msgToRet = "m" + msgToRet;
                }
                return msgToRet;
            }
        }

        public CommMessages.CommunicationMessage ParseString(string msg)
        {
           if(msg.IndexOf('a')==0)
           {
               string XMLmsg = msg.Substring(1);
               return DeserializeActionCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('b') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return DeserializeEditCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('c') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return DeserializeLoginCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('d') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return DeserializeRegisterCommMessage(XMLmsg);
           }
           else if (msg.IndexOf('e') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return DeserializeGameDataCommMessage(XMLmsg);
           }
        
             else if (msg.IndexOf('f') == 0)
             {
                string XMLmsg = msg.Substring(1);
                return DeserializeChatCommMessage(XMLmsg);
              }

            else if (msg.IndexOf('g') == 0)
           {
               string XMLmsg = msg.Substring(1);
               return DeserializeResponeCommMessage(XMLmsg);
           }
            else if (msg.IndexOf('h') == 0)
            {
                string XMLmsg = msg.Substring(1);
                return DeserializeSearchCommMessage(XMLmsg);
            }
            else if (msg.IndexOf('i') == 0)
            {
                string XMLmsg = msg.Substring(1);
                return DeserializeLoginResponeCommMessage(XMLmsg);
            }
            else if (msg.IndexOf('j') == 0)
            {
                string XMLmsg = msg.Substring(1);
                return DeserializeRegisterResponeCommMessage(XMLmsg);
            }
            else if (msg.IndexOf('k') == 0)
            {
                string XMLmsg = msg.Substring(1);
                return DeserializeSearchResponseCommMessageSTC(XMLmsg);
            }
            else if (msg.IndexOf('l') == 0)
            {
                string XMLmsg = msg.Substring(1);
                return DeserializeCreatrNewRoomMessage(XMLmsg);
            }
            else if (msg.IndexOf('m') == 0)
            {
                string XMLmsg = msg.Substring(1);
                return deserializeChatResponceCommMessage(XMLmsg);
            }
            return null;
        }


        private ChatResponceCommMessage deserializeChatResponceCommMessage(string XmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(XmlText))
            {
                var serializer = new XmlSerializer(typeof(ChatResponceCommMessage));
                return (ChatResponceCommMessage)serializer.Deserialize(stringReader);
            }
        }


        private ChatCommMessage DeserializeChatCommMessage(string XmlText)
         {
           using (StringReader stringReader = new System.IO.StringReader(XmlText))
          {
            var serializer = new XmlSerializer(typeof(ChatCommMessage));
           return (ChatCommMessage)serializer.Deserialize(stringReader);
         }
                }

        private CreatrNewRoomMessage DeserializeCreatrNewRoomMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(CreatrNewRoomMessage));
                return (CreatrNewRoomMessage)serializer.Deserialize(stringReader);
            }
        }

        private SearchResponseCommMessage DeserializeSearchResponseCommMessageSTC(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(SearchResponseCommMessage));
                return (SearchResponseCommMessage)serializer.Deserialize(stringReader);
            }
        }

        private RegisterResponeCommMessage DeserializeRegisterResponeCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(RegisterResponeCommMessage));
                return (RegisterResponeCommMessage)serializer.Deserialize(stringReader);
            }
        }

        private SearchCommMessage DeserializeSearchCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(SearchCommMessage));
                return (SearchCommMessage)serializer.Deserialize(stringReader);
            }
        }

        private LoginResponeCommMessage DeserializeLoginResponeCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(LoginResponeCommMessage));
                return (LoginResponeCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private ActionCommMessage DeserializeActionCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(ActionCommMessage));
                return (ActionCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private EditCommMessage DeserializeEditCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(EditCommMessage));
                return (EditCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private LoginCommMessage DeserializeLoginCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(LoginCommMessage));
                return (LoginCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private RegisterCommMessage DeserializeRegisterCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(RegisterCommMessage));
                return (RegisterCommMessage)serializer.Deserialize(stringReader);
            }
        }
        private GameDataCommMessage DeserializeGameDataCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(GameDataCommMessage));
                return (GameDataCommMessage)serializer.Deserialize(stringReader);
            }
        }
       
        
        //todo - add handle to login responce and register responce
        private ResponeCommMessage DeserializeResponeCommMessage(string xmlText)
        {
            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(ResponeCommMessage));
                return (ResponeCommMessage)serializer.Deserialize(stringReader);
            }
        }
       
     
        
    }
}
