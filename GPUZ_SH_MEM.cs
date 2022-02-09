using System;
using System.Runtime.InteropServices;

namespace RecklessBoon.MacroDeck.GPUZ
{

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct GPUZ_SH_MEM
    {
        public UInt32 version;
        public volatile Int32 busy;
        public UInt32 lastUpdate;
    }
}
