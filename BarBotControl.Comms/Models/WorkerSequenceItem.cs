using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class WorkerSequenceItem
{
	public int Index { get; set; }
	public int ModuleAddress { get; set; }
	public int Option { get; set; }
}
