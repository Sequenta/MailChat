using System;
using System.Windows.Forms;

namespace MailChat
{
    public partial class ServerSettings : Form
    {
        public static event ConnectionEventHandler Create;
        public ServerSettings()
        {
            InitializeComponent();
        }

        protected virtual void OnCreate(ConnectionEventArgs e)
        {
            var handler = Create;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            //TODO: check
            OnCreate(new ConnectionEventArgs("",serverNameText.Text,""));
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
