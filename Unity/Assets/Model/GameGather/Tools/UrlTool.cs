using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
  public static class UrlTool
    {
        public static string GetUrlParameter(this string url,string parameterName)
        {
            string[] urls = url.Split('?');
            if (urls.Length < 2)
            {
                return string.Empty;
            }
            Dictionary<string,string> urlParmetersDic=new Dictionary<string, string>();
            string[] parames = urls[1].Split('&');
            for (int i = 0; i < parames.Length; i++)
            {
                string[] parameter = parames[i].Split('=');
                if (parameter.Length==2)
                {
                    urlParmetersDic.Add(parameter[0], parameter[1]);
                }
            }
            if (urlParmetersDic.ContainsKey(parameterName))
            {
                return urlParmetersDic[parameterName];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
