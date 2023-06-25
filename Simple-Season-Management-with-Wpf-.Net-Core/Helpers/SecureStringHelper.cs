using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Helpers
{
    public static class SecureStringHelper
    {
        public static bool CompareSecureStrings(SecureString s1, SecureString s2)
        {
            if (s1.Length != s2.Length)
                return false;

            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = IntPtr.Zero;

            try
            {
                ptr1 = Marshal.SecureStringToGlobalAllocUnicode(s1);
                ptr2 = Marshal.SecureStringToGlobalAllocUnicode(s2);
                for (int i = 0; i < s1.Length; i++)
                {
                    char c1 = (char)Marshal.ReadInt16(ptr1, i * 2);
                    char c2 = (char)Marshal.ReadInt16(ptr2, i * 2);
                    if (c1 != c2)
                        return false;
                }
                return true;
            }
            finally
            {
                if (ptr1 != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr1);
                if (ptr2 != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr2);
            }
        }
    }
}
