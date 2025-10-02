using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem2.ApplicationLayer.Common
{
    public static class SecureStringService
    {
        public static SecureString ToSecureString(string plainText)
        {
            if (plainText == null)
                throw new ArgumentNullException(nameof(plainText));

            SecureString secure = new SecureString();
            foreach (char c in plainText)
            {
                secure.AppendChar(c);
            }

            secure.MakeReadOnly();
            return secure;
        }

        public static string SecureStringToString(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException(nameof(secureString));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToBSTR(secureString);
                return Marshal.PtrToStringBSTR(unmanagedString) ?? string.Empty;
            }
            finally
            {
                if (unmanagedString != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(unmanagedString); // clear the memory
            }
        }

        public static bool AreSecureStringEqual(SecureString s1, SecureString s2)
        {
            if (s1 == null || s2 == null)
                return false;

            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = IntPtr.Zero;

            try
            {
                ptr1 = Marshal.SecureStringToBSTR(s1);
                ptr2 = Marshal.SecureStringToBSTR(s2);

                string str1 = Marshal.PtrToStringBSTR(ptr1) ?? string.Empty;
                string str2 = Marshal.PtrToStringBSTR(ptr2) ?? string.Empty;

                return str1 == str2;
            }
            finally
            {
                if (ptr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(ptr1);
                if (ptr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(ptr2);
            }
        }
    }

}
