using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class I2cMock : II2cWrapper
{
    public DateTime WrittenTime { get; set; }
    public int I2cAddress { get; set; }
    public int Value { get; set; }
    public int DelaySeconds { get; set; } = 1;
    public int Return { get; set; } = 0;
    public I2cMockSetting? I2cMockSetting { get; set; }
    public List<I2cMockSetting> I2cMockSettings { get; set; }

    public I2cMock(int address, WorkerConfig config)
    {
        I2cAddress = address;
        DelaySeconds = config.MockDelaySeconds;
        Return = config.MockReturn;
        I2cMockSettings = config.I2cMockSettings;
    }

    public byte[] Read()
    {
        int delaySeconds = I2cMockSetting is not null ? I2cMockSetting.DelaySeconds : DelaySeconds;
        int ret = I2cMockSetting?.Return ?? Return;
        if (DateTime.Now >= WrittenTime.AddSeconds(delaySeconds))
        {
            return new byte[] { 1, Convert.ToByte(ret) };
        }
        return new byte[] { 0, 0 };
    }

    public void WriteByte(int value)
    {
        WrittenTime = DateTime.Now;
        Value = value;
        I2cMockSetting = I2cMockSettings
            .FirstOrDefault(s => s.I2CAddress == I2cAddress && s.Value == Value, null);
    }
}
