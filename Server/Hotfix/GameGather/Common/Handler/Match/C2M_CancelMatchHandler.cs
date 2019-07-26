//using System;
//using System.Collections.Generic;
//using ETModel;
//using Google.Protobuf.Collections;

//namespace ETHotfix
//{
//    [MessageHandler(AppType.Match)]
//    public class C2M_CancelMatchHandler : AMRpcHandler<C2M_CancelMatch, M2C_CancelMatch>
//    {
//        protected override void Run(Session session, C2M_CancelMatch message, Action<M2C_CancelMatch> reply)
//        {
//            M2C_CancelMatch response = new M2C_CancelMatch();
//            try
//            {
//                Game.Scene.GetComponent<MatchRoomComponent>().RmoveQueueUser(message.UserId);
//                reply(response);
//            }
//            catch (Exception e)
//            {
//                ReplyError(response, e, reply);
//            }
//        }

//        public bool tsasdc(User s)
//        {
//            return true;
//        }
//    }
//}