

namespace BarBotControl.Comms.Models;

public class ExceptionResponse
{
	public ExceptionResponseType Type { get; set; }
	public List<WorkerSequenceItem> SequenceItems { get; set; } = new List<WorkerSequenceItem>();


	public static ExceptionResponse Continue()
	{
		return new ExceptionResponse()
		{
			Type = ExceptionResponseType.Continue,
			SequenceItems = new List<WorkerSequenceItem>()
		};
	}

	public static ExceptionResponse PerformSequenceContinue(List<WorkerSequenceItem> sequenceItems)
	{
		return new ExceptionResponse()
		{
			Type = ExceptionResponseType.Continue,
			SequenceItems = sequenceItems
		};
	}
	public static ExceptionResponse Halt()
	{
		return new ExceptionResponse()
		{
			Type = ExceptionResponseType.Halt,
			SequenceItems = new List<WorkerSequenceItem>()
		};
	}

	public static ExceptionResponse PerformSequenceHalt(List<WorkerSequenceItem> sequenceItems)
	{
		return new ExceptionResponse()
		{
			Type = ExceptionResponseType.Halt,
			SequenceItems = sequenceItems
		};
	}
}

public enum ExceptionResponseType
{
	Continue,
	Halt
}
