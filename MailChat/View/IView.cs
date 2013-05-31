namespace MailChat.View
{
	public interface IView<out TPresenter>
	{
	    TPresenter Presenter { get; }
	}
}