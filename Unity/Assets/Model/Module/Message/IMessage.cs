
using ETHotfix;

namespace ETModel
{
    public interface IMessage
    {
    }

    public interface IRequest : IMessage
    {
        int RpcId { get; set; }
    }
    //需要UserId请求的转发 要通过网关转发的消息必须继承这个
    public interface IUserRequest : IRequest
    {
        long UserId { get; set; }
    }

    //管理员后台操作的协议
    public interface IAdministratorRequest : IUserRequest
    {
        string Account { get; set; }
        string Password { get; set; }
    }

    //需要UserId请求的转发 需要获取用户在网关的SessionActorId的要继承这个
    public interface IUserActorRequest : IUserRequest
    {
        User User { get; set; }
        long SessionActorId { get; set; }
    }
    public interface IResponse : IMessage
    {
        int Error { get; set; }
        string Message { get; set; }
        int RpcId { get; set; }
    }

    public class ResponseMessage : IResponse
    {
        public int Error { get; set; }
        public string Message { get; set; }
        public int RpcId { get; set; }
    }
}