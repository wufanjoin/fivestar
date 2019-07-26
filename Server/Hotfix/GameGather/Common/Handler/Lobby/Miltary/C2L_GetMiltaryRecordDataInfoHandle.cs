using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetMiltaryRecordDataInfoHandle : AMRpcHandler<C2L_GetMiltaryRecordDataInfo, L2C_GetMiltaryRecordDataInfo>
    {
        protected override async void Run(Session session, C2L_GetMiltaryRecordDataInfo message, Action<L2C_GetMiltaryRecordDataInfo> reply)
        {
            L2C_GetMiltaryRecordDataInfo response = new L2C_GetMiltaryRecordDataInfo();
            try
            {
                response.RecordDataInfo = await MiltaryComponent.Ins.GetParticularMiltaryRecordData(message.DataId);

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
