using BarBotControl.Comms.Exceptions;
using System.Threading.Channels;


namespace BarBotControl.Comms.Models;

public class ResponseModel
{
	public ResponseModelType ResponseType { get; set; }
	public WorkerExceptionBase? WorkerExceptionBase { get; set; }
	public StatusUpdate? StatusUpdate { get; set; }
	public ChannelWriter<ExceptionResponse>? ExceptionResponseSender { get; set; }

	public static ResponseModel ExceptionResponse(
		WorkerExceptionBase workerExceptionBase, 
		ChannelWriter<ExceptionResponse> exceptionResponseSender)
	{
		return new ResponseModel() 
		{
			ResponseType = ResponseModelType.Exception,
			WorkerExceptionBase = workerExceptionBase,
			ExceptionResponseSender = exceptionResponseSender
		};
	}

	public static ResponseModel StatusResponse(StatusUpdate statusUpdate)
	{
		return new ResponseModel()
		{
			ResponseType = ResponseModelType.Status,
			StatusUpdate = statusUpdate
		};
	}
}

public enum ResponseModelType
{
	Status,
	Exception
}
