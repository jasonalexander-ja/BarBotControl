using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class WorkerConfig
{
    public List<WorkerSequenceItem> DefaultResetRoutine { get; set; } = new List<WorkerSequenceItem>();
    public bool MockI2c { get; set; } = true;
    public int TimeoutSeconds { get; set; } = 3600;
    public int I2cChannel { get; set; } = 1;
    public int MockDelaySeconds { get; set; } = 1000;
    public int MockReturn { get; set; }
    public List<I2cMockSetting> I2cMockSettings { get; set; } = new List<I2cMockSetting>();
}
