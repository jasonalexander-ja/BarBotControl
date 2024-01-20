using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

internal interface II2cWrapper
{
    public byte[] Read();
    public void WriteByte(int value);
}
