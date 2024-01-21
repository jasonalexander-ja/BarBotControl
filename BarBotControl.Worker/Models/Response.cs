using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Worker.Models;

public class Response<To>
{
	private Response() { }

    public class WorkerMessage<T> : Response<T>
    {
        public T Message { get; set; }
        public WorkerMessage(T message)
        {
            Message = message;
        }
    }

    public class SchedulerLimmit<T> : Response<T> { }

    public class AlreadyScheduled<T> : Response<T> { }

    public class SchedulerMessage<T> : Response<T>
    {
        public int Position { get; set; }
        public SchedulerMessage(int position)
        {
            Position = position;
        }
    }
}
