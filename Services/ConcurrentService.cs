using System.Threading.Channels;
using BarBotControl.Models;

namespace BarBotControl.Services;


public class ConcurrentService : IAsyncDisposable
{
    public Thread WorkerThread { get; set; }
    public Channel<RequestMsg> WorkerChannel { get; set; }

    public ConcurrentService() 
    {
        WorkerChannel = Channel.CreateUnbounded<RequestMsg>();
        WorkerThread = new Thread(async () => await Worker(WorkerChannel.Reader));
        WorkerThread.Start();
    }

    public async Task<ResponseMsg> SendMessageAwaitResponse(string message)
    {
        var responseChannel = Channel.CreateUnbounded<ResponseMsg>();
        await WorkerChannel.Writer.WriteAsync(new RequestMsg(message, responseChannel.Writer));
        return await responseChannel.Reader.ReadAsync();
    }

    public async Task Worker(ChannelReader<RequestMsg> channelReader)
    {
        while (true)
        {
            var message = await channelReader.ReadAsync();
            if (message == null || message.Shutdown)
            {
                break;
            }
            var response = new ResponseMsg()
            {
                ResponseMsgType = ResponseMsgType.Message,
                Message = $"Received: {message.Info}"
            };
            await message.Response.WriteAsync(response);
            Thread.Sleep(15000);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await WorkerChannel.Writer.WriteAsync(new RequestMsg("", Channel.CreateUnbounded<ResponseMsg>().Writer, true));
        WorkerThread.Join();
    }
}


public class RequestMsg
{
    public bool Shutdown { get; set; }
    public ChannelWriter<ResponseMsg> Response { get; set; }
    public string Info { get; set; } = string.Empty;

    public RequestMsg(string info, ChannelWriter<ResponseMsg> sender, bool shutdown = false)
    {
        Response = sender;
        Info = info;
        Shutdown = shutdown;
    }
}


public enum ResponseMsgType
{
    Error,
    Message,
    Queue
}


public class ResponseMsg
{
    public int QueuePosition { get; set; }
    public int ErrorId { get; set; }
    public ResponseMsgType ResponseMsgType { get; set; }
    public string Message { get; set; } = string.Empty;
}
