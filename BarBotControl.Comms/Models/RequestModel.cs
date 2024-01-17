using BarBotControl.Comms.Exceptions;

namespace BarBotControl.Comms.Models;

public class RequestModel
{
	public List<WorkerSequenceItem> WorkerSequenceItems { get; set; } = new List<WorkerSequenceItem>();

	public RequestModel(List<WorkerSequenceItem> workerSequenceItems)
	{
		WorkerSequenceItems = workerSequenceItems;
	}
}
