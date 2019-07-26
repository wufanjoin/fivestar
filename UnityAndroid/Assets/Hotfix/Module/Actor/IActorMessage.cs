// 不要在这个文件加[ProtoInclude]跟[BsonKnowType]标签,加到InnerMessage.cs或者OuterMessage.cs里面去

using Google.Protobuf.Collections;

namespace ETHotfix
{
	public interface IActorMessage: IRequest
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