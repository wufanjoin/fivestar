using System;
using System.IO;

namespace ETModel
{
	public class SessionCallbackComponent: Component
	{
		public Action<Session, byte, ushort, MemoryStream> MessageCallback;
		public Action<Session> DisposeCallback;

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
		    Session session = this.GetParent<Session>();
            base.Dispose();
		    Session session2 = this.GetParent<Session>();
            this.DisposeCallback?.Invoke(session);
		}
	}
}
