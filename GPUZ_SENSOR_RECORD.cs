using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RecklessBoon.MacroDeck.GPUZ
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct GPUZ_SENSOR_RECORD
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string unit;
        public UInt32 digits;
        public double value;
    }
}
