using System.Runtime.InteropServices;

namespace RecklessBoon.MacroDeck.GPUZ
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct GPUZ_RECORD
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string key;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string value;
    }
}
