namespace BarBotControl.Comms.Exceptions;

public class ModuleReturnedException : WorkerExceptionBase
{
	public int ModuleAddress { get; set; }
	public int Option { get; set; }
	public int ReturnedValue { get; set; }
	public int SequenceIndex { get; set; }

	public ModuleReturnedException(int moduleAddr, int option, int returnedValue, int sequenceIndex)
		: base($"Module at address `{moduleAddr}` returned an error `{returnedValue}` on option `{option}` at sequence index `{sequenceIndex}`. ")
	{
		ModuleAddress = moduleAddr;
		Option = option;
		ReturnedValue = returnedValue;
		SequenceIndex = sequenceIndex;
	}
}
