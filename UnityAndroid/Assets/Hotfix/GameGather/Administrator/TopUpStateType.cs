using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 充值状态
/// </summary>
namespace ETHotfix
{
    public class TopUpStateType
    {
        public const int None = 0;
        public const int NoPay = 1;//没有支付
        public const int AlreadyPay = 2;//已经支付了
        public const int RepairOrder = 3;//已经补单了
    }
}
