using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class AIUserFactory
    {

        private static int _AIUserUseIndex = 0;

        private static int _IconNameUseIndex = 0;
        public static async  Task<User> Create(int minBeansNum)
        {
            if (_AiMatchUsers == null)
            {
                await InitAIUsers();
            }
            if (++_AIUserUseIndex >= _AIUsers.Count)
            {
                _AIUserUseIndex = 0;
            }
            User userAi = _AIUsers[_AIUserUseIndex];
            _IconNameUseIndex++;
            if (_IconNameUseIndex >= _AINames.Count|| _IconNameUseIndex >= _AIIcons.Count)
            {
                _IconNameUseIndex = 0;
            }
            userAi.Icon = _AIIcons[_IconNameUseIndex];
            userAi.Name = _AINames[_IconNameUseIndex];
            int oughtBeansNum = minBeansNum + RandomTool.Random(1, 10) * (minBeansNum / 10);
            int aiAddBeansNum =(int)(oughtBeansNum - userAi.Beans);
            userAi.Beans = oughtBeansNum;
            for (int i = 0; i < _AiMatchUsers.Count; i++)
            {
                if (_AiMatchUsers[i].UserId == userAi.UserId)
                {
                    _AiMatchUsers[i].BeansTotalResult += aiAddBeansNum;
                    await Game.Scene.GetComponent<DBProxyComponent>().Save(_AiMatchUsers[i]);
                  
                    break;
                }
            }
            return UserFactory.AIUserCopy(userAi);
        }

        private static List<MatchAIUser> _AiMatchUsers;//AI配置
        private static List<User> _AIUsers=new List<User>();//可用的AIUser

        public static async Task InitAIUsers()
        {
            _AiMatchUsers = await Game.Scene.GetComponent<DBProxyComponent>().Query<MatchAIUser>(AI => true);
            if (_AiMatchUsers.Count <= 0)
            {
               await SaveDefaultAIUser();
            }
            for (int i = 0; i < _AiMatchUsers.Count; i++)
            {
                List<User> users=await Game.Scene.GetComponent<DBProxyComponent>().Query<User>(user => user.UserId== _AiMatchUsers[i].UserId);
                _AIUsers.AddRange(users);
            }
        }

        public static async Task SaveDefaultAIUser()
        {
            for (int i = 1; i < 3; i++)
            {
                MatchAIUser matchAiUser = ComponentFactory.Create<MatchAIUser>();
                matchAiUser.UserId = i;
                matchAiUser.BeansTotalResult = 0;
                await Game.Scene.GetComponent<DBProxyComponent>().Save(matchAiUser);
                _AiMatchUsers.Add(matchAiUser);
                await UserFactory.AICreatUser(matchAiUser.UserId);
            }
        }

        public static List<string> _AINames=new List<string>()
        {
            "陈大胡子",
            "北方有孤城",
            "Euphoria",
            "今日光辉",
            "中意",
            "爷的霸气",
            "故事.",
            "华锅",
            "静静是谁",
            "Lee-西子",
            "梦想成真",
            "默默",
            "平淡如水",
            "如风过境",
            "书生夺命剑",
        };
        public static List<string> _AIIcons=new List<string>()
        {
            "https://ss0.bdstatic.com/94oJfD_bAAcT8t7mm9GUKT-xh_/timg?image&quality=100&size=b4000_4000&sec=1560007602&di=49deb87c9ae669ae5049d3642be91b13&src=http://5b0988e595225.cdn.sohucs.com/q_70,c_zoom,w_640/images/20190226/84cd65d72d2048b4b2175ff8b8ed8368.jpeg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560017750825&di=4288c551bec31f46788602b94a10c11e&imgtype=0&src=http%3A%2F%2Fimg0.pconline.com.cn%2Fpconline%2F1507%2F18%2F6716136_05_thumb.jpg",
            "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=4056021011,3048875021&fm=26&gp=0.jpg",
            "https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1925913628,3438169534&fm=26&gp=0.jpg",
            "https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1441866181,3761987185&fm=26&gp=0.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560017908293&di=f0239c49d26d13ad0113dc9b1ee2979f&imgtype=0&src=http%3A%2F%2Ftc.sinaimg.cn%2Fmaxwidth.2048%2Ftc.service.weibo.com%2Fp%2Fmmbiz_qpic_cn%2F95696ca130b2d60d4e1e0b55f439fdf2.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018146370&di=ed1f7f3f76148687b2c57216e818b6cd&imgtype=0&src=http%3A%2F%2Fimgnews.mumayi.com%2Ffile%2F2019%2F05%2F06%2F6cfd1bb22c62eb07b3a30f3f14f3c87f.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018169952&di=358660a89dbf644095caa45bfe400b3d&imgtype=0&src=http%3A%2F%2F5b0988e595225.cdn.sohucs.com%2Fq_70%2Cc_zoom%2Cw_640%2Fimages%2F20190507%2F42db09e92ec543d19e6b5d63337a8fc9.jpeg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018227685&di=292167602e53504cbc8dda112d366882&imgtype=0&src=http%3A%2F%2Fpic4.zhimg.com%2F50%2Fv2-9b96ff3929c486edc1c9a99aedba6bb4_hd.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018256813&di=563d93d54862685b18de5592ddcdd52f&imgtype=0&src=http%3A%2F%2F5b0988e595225.cdn.sohucs.com%2Fq_70%2Cc_zoom%2Cw_640%2Fimages%2F20190430%2F21cf98246a74493d81e8e262b4aecfb6.jpeg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018311109&di=8689e8832282f3a0208eb3d7c650deba&imgtype=0&src=http%3A%2F%2Fm.360buyimg.com%2Fpop%2Fjfs%2Ft24118%2F305%2F1773056458%2F44415%2Fa14fdcd%2F5b697ad4N76813d2a.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018335026&di=0e5ce74380f9c489b0976d5fc7fff28d&imgtype=0&src=http%3A%2F%2Fpic1.zhimg.com%2F50%2Fv2-7df232c7ec973cef194dc5f8ea738374_hd.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018447142&di=8ad13f92de8d834a44a04b1d0bee2495&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201412%2F08%2F20141208100321_sa5N3.jpeg",
            "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=3175426795,521728447&fm=26&gp=0.jpg",
            "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1560018500054&di=1da5855c69cacd10e21335b02c0bc70c&imgtype=0&src=http%3A%2F%2Fimg.name2012.com%2Fuploads%2Fallimg%2F13-10%2F29-021309_515.jpg",
        };
    }
}
