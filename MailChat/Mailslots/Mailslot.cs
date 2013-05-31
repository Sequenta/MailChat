using System;
using System.Runtime.InteropServices;
using MailChat.WinAPI;

namespace MailChat.Mailslots
{
    public class Mailslot : IDisposable
    {    
        public static readonly IntPtr InvalidHandle = new IntPtr(-1);
   
        private string mailslotName = string.Empty;
        public string Name
        {
            get { return mailslotName; }
            set { mailslotName = GetLocalMailslotPath(value); }
        }

        private IntPtr mailslotHandle;
        public IntPtr Handle
        {
            get { return mailslotHandle; }
            set 
                { 
                    mailslotHandle = value;
                    if (!IsValid())
                    {
                        WinFunctions.ThrowException("Не удалось создать mailslot");
                    }
                }
        }

        public Mailslot(String name)
        {
            Name = name;
        }


        public WinFunctions.MailslotInfo GetMailslotInfo()
        {
            return GetMailslotInfo(mailslotHandle);
        }

        public static WinFunctions.MailslotInfo GetMailslotInfo(IntPtr mailslotHandle)
        {
            var info = new WinFunctions.MailslotInfo();
            var result = WinFunctions.GetMailslotInfo(
                                                      mailslotHandle,
                                                      ref info.MaxMessageSize,
                                                      ref info.NextMessageSize,
                                                      ref info.MessageCount,
                                                      ref info.ReadTimeout
                                                      );

            if (!result)
            {
                WinFunctions.ThrowException("Не удалось вызвать API функцию GetMailslotInfo");
            }

            return info;
        }

        public bool IsValid()
        {
            return mailslotHandle != InvalidHandle;
        }

        public static string GetLocalMailslotPath(string name)
        {
            return GetFullMailslotPath(".", name);
        }

        public static string GetFullMailslotPath(string serverName, string name)
        {
            return string.Format(@"\\{0}\mailslot\{1}", serverName, name);
        }

        private void CloseMailslot()
        {
            if (!IsValid()) return;
            if (!WinFunctions.CloseHandle(mailslotHandle))
            {
                WinFunctions.ThrowException("Не удалось вызвать API функцию CloseHandle");
            }
            mailslotHandle = InvalidHandle;
        }

        public void Dispose()
        {
            CloseMailslot();
        }
    }
}