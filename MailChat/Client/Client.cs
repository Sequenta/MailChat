using System;
using MailChat.Messages;
using MailChat.WinAPI;
using MailChat.Mailslots;

namespace MailChat.Client
{
    class Client
    {
        private readonly Mailslot mailslot;
        private readonly string target;
        private readonly string nickName;

        public Client(string nickName, string mailslotName, string mailslotServer)
        {
            this.nickName = nickName;
            mailslot = new Mailslot(mailslotName);
            target = Mailslot.GetFullMailslotPath(mailslotServer, mailslotName);
        }

        public string NickName
        {
            get { return nickName; }
        }

        public uint Send(string data)
        {
            var message = new Message(nickName, data);
            var bytes = message.ToByte();
            return Send(bytes);
        }

        private uint Send(byte[] data)
        {
            uint bytesWritten = 0;

            try
            {
                mailslot.Handle = WinFunctions.CreateFile(target,
                                                         WinFunctions.FileDesiredAccess.GenericWrite,
                                                         0,
                                                         IntPtr.Zero,
                                                         WinFunctions.FileCreationDisposition.OpenExisting,
                                                         0,
                                                         IntPtr.Zero);

                WinFunctions.WriteFile(mailslot.Handle, data, (uint)data.Length, out bytesWritten, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                WinFunctions.ThrowException("Send failed.", ex);
            }
            finally
            {
                if (mailslot.IsValid())
                {
                    mailslot.Dispose();
                }
            }
            return bytesWritten;
        }
    }
}
