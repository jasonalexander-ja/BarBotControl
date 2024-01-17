using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Exceptions;

public class I2cCommunicationException : WorkerExceptionBase
{
	public int I2cChannel { get; set; }
	public int I2cAddress { get; set; }
	public I2cCommunicationException(int i2cChannel, int i2cAddress, Exception innerException)
		: base($"Error opening I2C channel {i2cChannel}. ", innerException)
	{
		I2cChannel = i2cChannel;
		I2cAddress = i2cAddress;
	}
}
