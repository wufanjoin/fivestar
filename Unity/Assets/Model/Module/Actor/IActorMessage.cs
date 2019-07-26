using Google.Protobuf.Collections;

namespace ETModel
{
	// 不需要返回消息
	public interface IActorMessage: IMessage
	{
		long ActorId { get; set; }
	}

    //这条消息包含某一玩家的手牌信息 需要特殊处理 不需要返回消息
    public interface IHandActorMessage : IActorMessage
    {
        int SeatIndex { get; set; }
        RepeatedField<int> Hands { get; set; }
    }

    public interface IActorRequest : IRequest
	{
		long ActorId { get; set; }
	}

	public interface IActorResponse : IResponse
	{
	}

	public interface IFrameMessage : IMessage
	{
		long Id { get; set; }
	}
}