using MailChat.Presenter;

namespace MailChat.View
{
    public interface IServerView : IView<ServerPresenter>
    {
        string GetServerName();
        void CreateServer();
        void ShowMessage(string message);
    }
}