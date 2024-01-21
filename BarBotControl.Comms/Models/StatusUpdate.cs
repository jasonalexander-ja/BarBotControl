using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class StatusUpdate
{
	public int SequenceIndex { get; set; }
	public int ModuleAddress { get; set; }
	public int Option { get; set; }
	
	public StatusUpdate(int sequenceIndex, int moduleAddr, int option)
	{
		SequenceIndex = sequenceIndex;
		ModuleAddress = moduleAddr;
		Option = option;
	}

	public StatusUpdate(WorkerSequenceItem item)
	{
		SequenceIndex = item.Index;
        ModuleAddress = item.ModuleAddress;
        Option = item.Option;
    }
}
