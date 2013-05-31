using System;

namespace MailChat
{
    public delegate void ConnectionEventHandler(object sender,ConnectionEventArgs e);
    public class ConnectionEventArgs : EventArgs
    {
        private readonly string nickname;
        private readonly string serverName;
        private readonly string mailslotName;
        public ConnectionEventArgs(string mailslotName, string serverName, string nickname)
        {
            this.mailslotName = mailslotName;
            this.serverName = serverName;
            this.nickname = nickname;
        }

        public string Nickname
        {
            get { return nickname; }
        }

        public string ServerName
        {
            get { return serverName; }
        }

        public string MailslotName
        {
            get { return mailslotName; }
        }
    }
}