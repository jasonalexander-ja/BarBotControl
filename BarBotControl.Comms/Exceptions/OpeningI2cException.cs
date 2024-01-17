using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Comms.Exceptions;

public class OpeningI2cException : WorkerExceptionBase
{
	public int I2cChannel { get; set; }
	public OpeningI2cException(int i2cChannel, Exception innerException) 
		: base($"Error opening I2C channel {i2cChannel}. ", innerException)
	{
		I2cChannel = i2cChannel;
	}
}
