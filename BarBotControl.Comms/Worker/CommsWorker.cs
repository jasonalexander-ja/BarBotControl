using BarBotControl.Worker.Models;
using BarBotControl.Comms.Models;
using BarBotControl.Comms.Exceptions;
using System.Threading.Channels;
using Response = BarBotControl.Worker.Models.Response<BarBotControl.Comms.Models.ResponseType>;

namespace BarBotControl.Comms.Worker;

public static class CommsWorker
{
	public static WorkerConfig Config { get; set; } = new WorkerConfig();

    public static async Task Run(Request<RequestModel, ResponseType> request)
	{
		var sequenceItems = request.RequestBody.WorkerSequenceItems;
		await RunSequence(sequenceItems, request.ResponseWriter);
		try
        {
            await request.ResponseWriter.WriteAsync(
				new Response.WorkerMessage<ResponseType>(new ResponseType.End()));
        }
		catch
		{
			// User disconnected 
		}
	}

	private static async Task RunSequence(List<WorkerSequenceItem> sequenceItems, 
		ChannelWriter<Response> responseWriter)
	{
		foreach (var (seqItem, index) in sequenceItems.Select((s, i) => (s, i)))
		{
			try
			{
				await SendStatusUpdate(seqItem, responseWriter);
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
		ChannelWriter<Response> responseWriter)
	{
		var responseChannel = Channel.CreateUnbounded<ExceptionResponse>();
		try
		{
			await responseWriter.WriteAsync(new Response.WorkerMessage<ResponseType>(
                new ResponseType.WorkerException(ex, responseChannel.Writer)
			));
            return await responseChannel.Reader.ReadAsync();
        }
		catch
		{
			return ExceptionResponse.PerformSequenceHalt(Config.DefaultResetRoutine);
        }
	}

	private static async Task SendStatusUpdate(WorkerSequenceItem item,
        ChannelWriter<Response> responseWriter)
	{
		try
		{
			var statusUpdate = new Response.WorkerMessage<ResponseType>(
                new ResponseType.StatusUpdate(item));
            await responseWriter.WriteAsync(statusUpdate);
        }
		catch
        {
            // User disconnected 
        }
    }

	private static void Dispatch(WorkerSequenceItem item)
	{
		try
		{
            II2cWrapper i2c = Config.MockI2c 
				? new I2cMock(item.ModuleAddress, Config) 
				: new I2cWrapper(item.ModuleAddress, Config.I2cChannel);
			i2c.WriteByte(Convert.ToByte(item.Option));
			var result = TryRead(i2c);
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
			throw new I2cCommunicationException(item.Index, Config.I2cChannel, item.ModuleAddress, ex);
		}
	}

	private static int TryRead(II2cWrapper i2c)
	{
		while (true)
		{
			byte[] res = i2c.Read();
			if (res.FirstOrDefault((byte)0) == 1)
			{
				return res.LastOrDefault((byte)0);
			}
			Thread.Sleep(200);
		}
	}
}
