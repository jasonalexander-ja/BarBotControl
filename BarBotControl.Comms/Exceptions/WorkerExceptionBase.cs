using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Exceptions;

public class WorkerExceptionBase : Exception
{
	public WorkerExceptionBase() : base() { }
	public WorkerExceptionBase(string? message) : base(message) { }
	public WorkerExceptionBase(string? message, Exception? innerException) : base(message, innerException) { }
}
