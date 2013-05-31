using MailChat.Messages;
using MailChat.View;

namespace MailChat.Presenter
{
    public class ServerPresenter
    {
        private readonly IServerView view;
        private Server.Server server; 

        public ServerPresenter(IServerView view)
		{
			this.view = view;
		}

        private void ShowMessage(object sender, MessageEventArgs e)
        {
            view.ShowMessage(FormatMessage(e));
        }

        public void CreateServer()
        {
            server = new Server.Server(view.GetServerName());
            server.MessageReceived += ShowMessage;
            server.Start();
        }

        private static string FormatMessage(MessageEventArgs e)
        {
            return string.Format("{0}: {1}", e.Sender,e.MessageText );
        }

        public void ShutdownServer()
        {
            if(server == null)return;
            server.MessageReceived -= ShowMessage;
            server.Shutdown();
        }
    }
}