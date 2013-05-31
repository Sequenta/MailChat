using System;
using MailChat.View;

namespace MailChat.Presenter
{
	public class ClientPresenter:IDisposable
	{
		private readonly IClientView view;
	    private Client.Client client;

        public ClientPresenter(IClientView view)
		{
			this.view = view;
		}

        public void Connect()
        {
            client = new Client.Client(view.GetNickname(),
                                       view.GetMailslotName(),
                                       view.GetServerName());
        }

        public void SendMessage(string message)
        {
            client.Send(message);
        }

	    public void Disconnect()
	    {
	        client = null;
	    }

	    public void Dispose()
	    {
	        
	    }
	}
}