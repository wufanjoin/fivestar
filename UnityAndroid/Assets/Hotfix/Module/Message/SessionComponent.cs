using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class SessionComponentAwakeSystem : AwakeSystem<SessionComponent>
	{
		public override void Awake(SessionComponent self)
		{
			self.Awake();
		}
	}

	public class SessionComponent: Component
	{
		public static SessionComponent Instance;

		public Session Session;

		public void Awake()
		{
			Instance = this;
		}

	    public void Send(IMessage message)
	    {
	        Session.Send(message);
        }

        public ETTask<IResponse> Call(IRequest request)
	    {
	       return  Session.Call(request);
	    }
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		    if (Session != null)
		    {
		        this.Session.Dispose();
		        this.Session = null;
            }
			Instance = null;
		}
	}
}
