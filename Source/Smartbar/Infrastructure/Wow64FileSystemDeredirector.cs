namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using System.Runtime.InteropServices;

    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean Wow64RevertWow64FsRedirection(IntPtr ptr);
    }
}
