using System;

namespace MailChat.Messages
{
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);
    public class MessageEventArgs : EventArgs
    {
        private readonly Message message;
        public MessageEventArgs(Message message)
        {
            this.message = message;
        }

        public string Sender
        {
            get { return message.Sender; }
        }

        public string MessageText
        {
            get { return message.MessageText; }
        }
    }
}