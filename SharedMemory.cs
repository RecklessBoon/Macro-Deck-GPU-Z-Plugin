using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RecklessBoon.MacroDeck.GPUZ
{
    public class SharedMemory
    {
        public EventHandler<EventArgs> OnRefreshStarted;
        public EventHandler<GPUZ_SH_MEM> OnMemUpdated;
        public EventHandler<GPUZ_RECORD> OnDataUpdated;
        public EventHandler<GPUZ_SENSOR_RECORD> OnSensorUpdated;
        public EventHandler<EventArgs> OnRefreshComplete;

        public GPUZ_SH_MEM Mem { get; set; }
        public List<GPUZ_RECORD> Data { get; set; }
        public List<GPUZ_SENSOR_RECORD> Sensors { get; set; }

        public SharedMemory()
        {
            _ = Refresh();
        }

        public async Task Refresh()
        {
            await Task.Run(() =>
            {
                OnRefreshStarted?.Invoke(this, EventArgs.Empty);

                using var mmf = MemoryMappedFile.OpenExisting("GPUZShMem");
                using var accessor = mmf.CreateViewAccessor();

                int shMemSize = Marshal.SizeOf(typeof(GPUZ_SH_MEM));
                int recordSize = Marshal.SizeOf(typeof(GPUZ_RECORD));
                int recordArraySize = recordSize * 128;
                int sensorSize = Marshal.SizeOf(typeof(GPUZ_SENSOR_RECORD));
                int sensorArraySize = sensorSize * 128;

                GPUZ_SH_MEM mem;
                Mem = default;
                Data = new List<GPUZ_RECORD>();
                Sensors = new List<GPUZ_SENSOR_RECORD>();

                // Meta data
                accessor.Read(0, out mem);
                Mem = mem;
                OnMemUpdated?.Invoke(this, mem);

                // System data
                for (var i = 0; i < recordArraySize; i += recordSize)
                {
                    GPUZ_RECORD record;
                    byte[] buffer = new byte[256];
                    accessor.ReadArray<byte>(shMemSize + i, buffer, 0, 256);
                    record.key = Encoding.Unicode.GetString(buffer).Trim('\0');
                    buffer = new byte[256];
                    accessor.ReadArray<byte>(shMemSize + i + 512, buffer, 0, 256);
                    record.value = Encoding.Unicode.GetString(buffer).Trim('\0');

                    if (String.IsNullOrWhiteSpace(record.key)) continue;

                    Data.Add(record);
                    OnDataUpdated?.Invoke(this, record);
                }

                // Sensor data
                for (var i = 0; i < sensorArraySize; i += sensorSize)
                {
                    GPUZ_SENSOR_RECORD record;
                    byte[] buffer = new byte[256];
                    var position = shMemSize + recordArraySize + i;

                    accessor.ReadArray<byte>(position, buffer, 0, 256);
                    position += 512;
                    record.name = Encoding.Unicode.GetString(buffer).Trim('\0');

                    buffer = new byte[8];
                    accessor.ReadArray<byte>(position, buffer, 0, 8);
                    position += 16;
                    record.unit = Encoding.Unicode.GetString(buffer).Trim('\0');

                    record.digits = accessor.ReadUInt32(position);
                    position += Marshal.SizeOf(typeof(UInt32));
                    record.value = accessor.ReadDouble(position);

                    if (String.IsNullOrWhiteSpace(record.name)) continue;

                    Sensors.Add(record);
                    OnSensorUpdated?.Invoke(this, record);
                }

                OnRefreshComplete?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
