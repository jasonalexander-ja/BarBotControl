using BarBotControl.Worker.Models;
using BarBotControl.Comms.Models;
using BarBotControl.Comms.Exceptions;
using System.Device.I2c;

namespace BarBotControl.Comms.Worker;

public static class Worker
{
	public static TimeSpan Timeout = TimeSpan.FromSeconds(3600);
	public static int Channel = 1;

	public static async Task Run(Request<RequestModel, ResponseModel> request)
	{
		var sequenceItems = request.RequestBody.WorkerSequenceItems;
		foreach (var (seqItem, index) in sequenceItems.Select((s, i) => (s, i)))
		{
			try
			{
				Dispatch(seqItem);
			}
			catch (WorkerExceptionBase ex)
			{ 
				
			}
		}
	}

	public static void Dispatch(WorkerSequenceItem item)
	{
		try
		{
			I2cDevice i2c = I2cDevice.Create(new I2cConnectionSettings(Channel, item.ModuleAddress));
			i2c.WriteByte(Convert.ToByte(item.Option));
			var result = TryRead(i2c, item);
			if (result != 0)
			{
				throw new ModuleReturnedException(item.ModuleAddress, item.Option, result, item.Index);
			}
		}
		catch (WorkerExceptionBase)
		{
			throw;
		}
		catch (IOException ex)
		{
			throw new I2cCommunicationException(Channel, item.ModuleAddress, ex);
		}
	}

	public static I2cDevice OpenI2cDevices(int address)
	{
		try
		{
			return I2cDevice.Create(new I2cConnectionSettings(Channel, address));
		}
		catch (IOException ex)
		{
			throw new OpeningI2cException(address, ex);
		}
	}

	public static int TryRead(I2cDevice i2c, WorkerSequenceItem item)
	{
		var timeOutTime = DateTime.Now + Timeout;
		while (true)
		{
			byte[] res = {0, 0};
			i2c.Read(res);
			if (res.FirstOrDefault((byte)0) == 1)
			{
				return res.LastOrDefault((byte)0);
			}
			Thread.Sleep(200);
			if (DateTime.Now > timeOutTime)
			{
				throw new ModuleTimedOutException(item.ModuleAddress, item.Option, item.Index);
			}
		}
	}
}
