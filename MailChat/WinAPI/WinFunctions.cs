using System;
using System.Runtime.InteropServices;

namespace MailChat.WinAPI
{
    public class WinFunctions
    {
        internal enum FileDesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
        }

        internal enum FileShareMode : uint
        {
            NoShare = 0,
            ShareRead = 1,
            ShareWrite = 2,
            ShareDelete = 4,
        }

        internal enum FileCreationDisposition : uint
        {
            CreateNew = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5,
        }

        internal enum FileAttributes : uint
        {
            ReadOnly = 1,
            Hidden = 2,
            System = 4,
            Archived = 0x20,
            Normal = 0x80,
            Temporary = 0x100,
        }

        public struct MailslotInfo
        {
            public uint MaxMessageSize;
            public uint NextMessageSize;
            public uint MessageCount;
            public uint ReadTimeout;
        }

        internal static void ThrowException(string message, Exception innerException = null)
        {
            message = string.Format("{0} Win32 error code {1}",message, Marshal.GetLastWin32Error());
            var exception = new Exception(message,innerException);
            throw exception;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateMailslot(string mailslotName, 
                                                     uint maxMessageSize, 
                                                     uint readTimeout, 
                                                     IntPtr securityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetMailslotInfo(
            IntPtr mailslotHandle, 
            ref uint maxMessageSize,
            ref uint nextSize,
            ref uint messageCount,
            ref uint readTimeout);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern IntPtr CreateFile(
            string fileName,
            FileDesiredAccess desiredAccess,
            FileShareMode shareMode, 
            IntPtr securityAttributes,
            FileCreationDisposition creationDisposition,
            FileAttributes flagsAndAttributes, 
            IntPtr templateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadFile(
            IntPtr fileHandle,
            [Out] byte[] buffer,
            uint numberOfBytesToRead,
            out uint numberOfBytesRead,
            IntPtr overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteFile(
            IntPtr fileHandle,
            byte[] buffer,
            uint numberOfBytesToWrite,
            out uint numberOfBytesWritten,
            IntPtr overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);
    }
}
