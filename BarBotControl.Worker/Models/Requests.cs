using System.Threading.Channels;

namespace BarBotControl.Worker.Models;

public class Request<TReq, TRes>
{
	public ChannelWriter<Response<TRes>> ResponseWriter { get; set; }
	public ChannelReader<bool> CancelChannel { get; set; }
	public string IdempotencyKey { get; set; } = string.Empty;
    public TReq RequestBody { get; set; }


	public Request(ChannelWriter<Response<TRes>> responseWriter, ChannelReader<bool> cancelChannel, string idpKey, TReq requestBody)
	{
		ResponseWriter = responseWriter;
		CancelChannel = cancelChannel;
        IdempotencyKey = idpKey;
        RequestBody = requestBody;
	}
}
