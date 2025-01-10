using BarBotControl.Comms.Exceptions;
using System.Threading.Channels;


namespace BarBotControl.Comms.Models;

public class ResponseType
{
	private ResponseType() { }

	public class StatusUpdate : ResponseType
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

    public class ConfirmContinue : ResponseType
    {
        public ChannelWriter<bool> ResponseSender { get; set; }
        public string Message { get; set; }

        public ConfirmContinue(string message, ChannelWriter<bool> responseSender) 
        {
            Message = message;
            ResponseSender = responseSender;
        }
    }

    public class WorkerException : ResponseType
    {
        public WorkerExceptionBase WorkerExceptionBase { get; set; }
        public ChannelWriter<ExceptionResponse> ExceptionResponseSender { get; set; }

		public WorkerException(
			WorkerExceptionBase workerExceptionBase,
			ChannelWriter<ExceptionResponse> exceptionResponseSender)
		{
			WorkerExceptionBase = workerExceptionBase;
			ExceptionResponseSender = exceptionResponseSender;
        }
    }

	public class End : ResponseType { }
}
