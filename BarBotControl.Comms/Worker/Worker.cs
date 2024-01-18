using BarBotControl.Worker.Models;
using BarBotControl.Comms.Models;
using BarBotControl.Comms.Exceptions;
using System.Device.I2c;
using System.Threading.Channels;

namespace BarBotControl.Comms.Worker;

public static class Worker
{
	public static TimeSpan Timeout = TimeSpan.FromSeconds(3600);
	public static int I2CChannel = 1;

	public static async Task Run(Request<RequestModel, ResponseModel> request)
	{
		var sequenceItems = request.RequestBody.WorkerSequenceItems;
		await RunSequence(sequenceItems, request.ResponseWriter);
		request.ResponseWriter.Complete();
	}

	private static async Task RunSequence(List<WorkerSequenceItem> sequenceItems, 
		ChannelWriter<Response<ResponseModel>> responseWriter)
	{
		foreach (var (seqItem, index) in sequenceItems.Select((s, i) => (s, i)))
		{
			try
			{
				Dispatch(seqItem);
			}
			catch (WorkerExceptionBase ex)
			{
				var response = await HandleException(ex, responseWriter);
				await RunSequence(response.SequenceItems, responseWriter);
				if (response.Type == ExceptionResponseType.Halt) 
					break;
			}
		}
	}

	private static async Task<ExceptionResponse> HandleException(
		WorkerExceptionBase ex,
		ChannelWriter<Response<ResponseModel>> responseWriter)
	{
		var responseChannel = Channel.CreateUnbounded<ExceptionResponse>();
		await responseWriter.WriteAsync(Response<ResponseModel>.WorkerMessage(
			ResponseModel.ExceptionResponse(ex, responseChannel.Writer)
		));
		return await responseChannel.Reader.ReadAsync();
	}

	private static void Dispatch(WorkerSequenceItem item)
	{
		try
		{
			I2cDevice i2c = I2cDevice.Create(new I2cConnectionSettings(I2CChannel, item.ModuleAddress));
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
			throw new I2cCommunicationException(I2CChannel, item.ModuleAddress, ex);
		}
	}

	private static I2cDevice OpenI2cDevices(int address)
	{
		try
		{
			return I2cDevice.Create(new I2cConnectionSettings(I2CChannel, address));
		}
		catch (IOException ex)
		{
			throw new OpeningI2cException(address, ex);
		}
	}

	private static int TryRead(I2cDevice i2c, WorkerSequenceItem item)
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
