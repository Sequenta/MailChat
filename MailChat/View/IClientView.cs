using MailChat.Presenter;

namespace MailChat.View
{
	public interface IClientView : IView<ClientPresenter>
	{
        string GetServerName();
	    string GetMailslotName();
	    string GetNickname();
	    string SendMessage();
	}
}