using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Models;

public class StatusUpdate
{
	public int SequenceIndex { get; set; }
	public int ModuleId { get; set; }
	public int OptionId { get; set; }
	public string Description { get; set; } = string.Empty;
	
	public StatusUpdate(int sequenceIndex, int moduleId, int optionId, string desc)
	{
		SequenceIndex = sequenceIndex;
		ModuleId = moduleId;
		OptionId = optionId;
		Description = desc;
	}
}
