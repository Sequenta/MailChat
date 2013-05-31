using System;
using System.Windows.Forms;

namespace MailChat
{
    public partial class ClientSettings : Form
    {
        public static event ConnectionEventHandler Connect;
        public ClientSettings()
        {
            InitializeComponent();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nickname - имя пользователя" + Environment.NewLine +
                            "Server name - имя сервера (локальный, если не задано)" + Environment.NewLine +
                            "Mailslot name - имя канала", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        protected virtual void OnConnect(ConnectionEventArgs e)
        {
            var handler = Connect;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(ValidateSettings())
            {
                OnConnect(new ConnectionEventArgs(mailslotName.Text,serverName.Text,nickText.Text));
                Close();
            }
        }

        private bool ValidateSettings()
        {
            if(mailslotName.Text  == string.Empty ||nickText.Text == string.Empty)
            {
                MessageBox.Show("Заполните обязательные поля:" + Environment.NewLine +
                                " -Nickname" + Environment.NewLine +
                                " -Mailslot name", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if(serverName.Text == string.Empty)
                serverName.Text = ".";
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
