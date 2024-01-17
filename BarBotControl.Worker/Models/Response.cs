using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Worker.Models;

public class Response<T>
{
	public ResponseType Type { get; set; }
	public SchedulingMessage? SchedulingMessage { get; set; }
	public T? Message { get; set; }

	public bool IsWorkerMessage
	{
		get => Type == ResponseType.WorkerMessage;
	}
	public bool IsSchedulerMessage
	{
		get => Type == ResponseType.SchedulerMessage;
	}

	public static Response<T> SchedulerMessage(int pos)
	{
		return new Response<T>()
		{
			Type = ResponseType.SchedulerMessage,
			SchedulingMessage = new SchedulingMessage(pos)
		};
	}

	public static Response<T> WorkerMessage(T message)
	{
		return new Response<T>()
		{
			Type = ResponseType.WorkerMessage,
			Message = message
		};
	}
}

public enum ResponseType
{
	WorkerMessage,
	SchedulerMessage
}
