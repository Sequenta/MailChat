using System;
using System.Collections.Generic;
using System.Text;

namespace MailChat.Messages
{
    public class Message
    {
         private readonly string sender;
         public string Sender
         {
             get { return sender; }
         }

         private readonly string messageText;
         public string MessageText
         {
             get { return messageText; }
         }

         public Message(string sender, string message)
         {
             this.sender = sender;
             messageText = message;
         }

         public Message(byte[] data)
         {
            var nameLen = BitConverter.ToInt32(data, 0);
            sender = nameLen > 0 ? Encoding.UTF8.GetString(data, 4, nameLen) : null;
            messageText = Encoding.UTF8.GetString(data, nameLen + 4, data.Length - nameLen - 4);
        }

        public byte[] ToByte()
        {
            var result = new List<byte>();

            //Длина имени.
            result.AddRange(Sender != null ? BitConverter.GetBytes(Sender.Length) : BitConverter.GetBytes(0));

            //Имя.
            if (Sender != null)
                result.AddRange(Encoding.UTF8.GetBytes(Sender));

            //Сообщение.
            if (MessageText != null)
                result.AddRange(Encoding.UTF8.GetBytes(MessageText));
            
            return result.ToArray();
        } 
    }
}