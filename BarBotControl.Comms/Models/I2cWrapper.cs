using BarBotControl.Comms.Exceptions;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class I2cWrapper : II2cWrapper
{
    I2cDevice i2c { get; set; }

    public I2cWrapper(int address, int i2CChannel)
    {
        try
        {
            i2c = I2cDevice.Create(new I2cConnectionSettings(i2CChannel, address));
        }
        catch (IOException ex)
        {
            throw new OpeningI2cException(address, ex);
        }
    }

    public byte[] Read()
    {
        byte[] res = { 0, 0 };
        i2c.Read(res);
        return res;
    }
    public void WriteByte(int value)
    {
        i2c.WriteByte(Convert.ToByte(value));
    }
}
