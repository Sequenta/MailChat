using System;
using System.Globalization;
using System.Windows.Forms;
using MailChat.Presenter;
using MailChat.View;

namespace MailChat
{
    public partial class Form1 : Form,IServerView,IClientView
    {
        delegate void SetTextCallback(string text);
        private ServerPresenter serverPresenter;
        private ClientPresenter clientPresenter;
        private ConnectionEventArgs eventArgs;
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        #region IServerPresenter members

        public ServerPresenter Presenter
        {
            get { return serverPresenter ?? (serverPresenter = new ServerPresenter(this)); }
        }

        public string GetServerName()
        {
            return eventArgs.ServerName;
        }

        public void CreateServer()
        {
            Presenter.CreateServer();
        }

        public void ShowMessage(string message)
        {
            SetText(Environment.NewLine + dateTimeLabel.Text + " " + message);
        }

        #endregion

        #region IClientPresenter members

        public string GetMailslotName()
        {
            return eventArgs.MailslotName;
        }

        public string GetNickname()
        {
            return eventArgs.Nickname;
        }

        public string SendMessage()
        {
            return messageText.Text;
        }

        ClientPresenter IView<ClientPresenter>.Presenter
        {
            get { return clientPresenter ?? (clientPresenter = new ClientPresenter(this)); }
        }

        #endregion

        private void SetText(string text)
        {
            if (textLog.InvokeRequired)
            {
                SetTextCallback d = SetText;
                Invoke(d, new object[] { text });
            }
            else
            {
                textLog.Text += text;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimeLabel.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientSettings.Connect += GetConnectionSettings;
            var settings = new ClientSettings();
            settings.ShowDialog();
            ClientSettings.Connect -= GetConnectionSettings;
            if (clientPresenter == null)
            {
                clientPresenter = new ClientPresenter(this);
            }
            if (eventArgs == null) return;
            clientPresenter.Connect();
            textLog.Text += string.Format("#Connected to {0} at {1}", eventArgs.MailslotName, dateTimeLabel.Text);
        }

        private void GetConnectionSettings(object sender, ConnectionEventArgs e)
        {
            eventArgs = e;
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerSettings.Create += GetConnectionSettings;
            var settings = new ServerSettings();
            settings.ShowDialog();
            ServerSettings.Create -= GetConnectionSettings;
            if(eventArgs == null)return;
            Presenter.CreateServer();
            textLog.Enabled = false;
            messageText.Enabled = false;
            sendButton.Enabled = false;
            textLog.Text += string.Format("#Server {0} started at {1}", eventArgs.MailslotName, dateTimeLabel.Text);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (clientPresenter == null)return;
            clientPresenter.SendMessage(SendMessage());
            messageText.Text = string.Empty;
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ShutdownServer();
            textLog.Text += string.Format("#Server {0} shutdown at {1}", eventArgs.MailslotName, dateTimeLabel.Text);
            textLog.Enabled = true;
            messageText.Enabled = true;
            sendButton.Enabled = true;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(clientPresenter == null)return;
            textLog.Text += string.Format("#Disconnected from {0} at {1}", eventArgs.MailslotName, dateTimeLabel.Text);
            clientPresenter.Disconnect();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }
    }
}
