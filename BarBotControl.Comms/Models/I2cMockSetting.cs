using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class I2cMockSetting
{
    public int I2CAddress { get; set; }
    public int Value { get; set; }
    public int DelaySeconds { get; set; }
    public int Return { get; set; }
}
