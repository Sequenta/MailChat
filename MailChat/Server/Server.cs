using System;
using System.Threading;
using MailChat.Messages;
using MailChat.WinAPI;
using MailChat.Mailslots;

namespace MailChat.Server
{
    class Server:IDisposable
    {
        
        private readonly Mailslot mailslot;
        private Thread receiverThread;
        public event MessageEventHandler MessageReceived;
        private bool IsRunning;

        public Server(string name)
        {
           mailslot = new Mailslot(name);
           mailslot.Handle = WinFunctions.CreateMailslot(mailslot.Name, 0, 0, IntPtr.Zero);
        }

        public uint Read(out byte[] data)
        {
            if (!mailslot.IsValid()) throw new Exception("Mailslot handle is invalid.");

            uint bytesRead = 0;
            try
            {
                var info = mailslot.GetMailslotInfo();
                if (info.MessageCount == 0)
                {
                    data = new byte[] { };
                    return 0;
                }
                data = new byte[info.NextMessageSize];
                WinFunctions.ReadFile(mailslot.Handle, data, (uint)data.Length, out bytesRead, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                if (!IsRunning)
                {
                    data = new byte[] {};
                    return bytesRead;
                }
                data = new byte[] { };
                WinFunctions.ThrowException("Read failed.", ex);
            }
            return bytesRead;
        }

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            var handler = MessageReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Read()
        {
            while (IsRunning)
            {
                byte[] data;
                Read(out data);
                if (data.Length != 0)
                {
                    OnMessageReceived(new MessageEventArgs(new Message(data)));
                }
            }
        }

        public void Start()
        {
            receiverThread = new Thread(Read);
            receiverThread.Start();
            IsRunning = true;
        }

        public void Shutdown()
        {
            IsRunning = false;
            Dispose();
        }

        public void Dispose()
        {
            receiverThread.Abort();
            mailslot.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
