using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class ActorHelp
    {
        public static void SendeActor(long actorId, IActorMessage iActorMessage)
        {
            if (actorId == 0)
            {
                return;
            }
            ActorMessageSender actorSender = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(actorId);
            actorSender.Send(iActorMessage);
        }
    }
}
