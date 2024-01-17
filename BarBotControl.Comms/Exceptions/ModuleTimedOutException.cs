using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Exceptions;

public class ModuleTimedOutException : WorkerExceptionBase
{
	public int ModuleAddress { get; set; }
	public int Option { get; set; }
	public int SequenceIndex { get; set; }

	public ModuleTimedOutException(int moduleAddr, int option, int sequenceIndex)
		: base($"Module at address `{moduleAddr}` timed out on `{option}` at sequence index `{sequenceIndex}`. ")
	{
		ModuleAddress = moduleAddr;
		Option = option;
		SequenceIndex = sequenceIndex;
	}
}
