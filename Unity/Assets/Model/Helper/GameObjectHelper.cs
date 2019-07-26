using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
	public static class GameObjectHelper
	{
	    public static Transform FindChild(this GameObject gameObject,string childName)
	    {
	       return gameObject.transform.Find(childName);
	    }
	    public static Transform FindChild(this Transform transform, string childName)
	    {
	        return transform.Find(childName);
	    }
        public static T Get<T>(this GameObject gameObject, string key) where T : class
		{
			try
			{
				return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
			}
			catch (Exception e)
			{
				throw new Exception($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}", e);
			}
		}
	    public static void SetText(this Button button,string content)
	    {
	        if (button==null||button.transform.childCount <= 0)
	        {
	            Log.Error("按钮"+button.name+"没有子节点Text");
                return;
            }
            
	        button.transform.GetChild(0).GetComponent<Text>().text = content;
	    }

	    public static void CreatorChildCount(this Transform transformParent,int count)
	    {
	        if (transformParent.childCount == 0)
	        {
                Log.Error("创建多个孩子 没有一个子节点作为预制体");
	            return;
	        }
	        for (int i = transformParent.childCount; i < count; i++)
	        {
	            GameObject.Instantiate(transformParent.GetChild(0).gameObject, transformParent);
	        }
	        for (int i = 0; i < count; i++)
	        {
	            transformParent.GetChild(i).gameObject.SetActive(true);
            }
	        for (int i = count; i < transformParent.childCount; i++)
	        {
	            transformParent.GetChild(i).gameObject.SetActive(false);
	        }
        }
	    public static void CreatorChildCount(this GameObject gameObjectParent, int count)
	    {
	        CreatorChildCount(gameObjectParent.transform, count);
	    }
        public static void CopyThisNum(this GameObject gameObject, int count)
	    {
	        gameObject.transform.parent.CreatorChildCount(count);
	    }
        public static void HideAllChild(this GameObject gameObject)
	    {
	        gameObject.transform.HideAllChild();

	    }
	    public static void HideAllChild(this Transform transform)
	    {
	        for (int i = 0; i < transform.childCount; i++)
	        {
	            transform.GetChild(i).gameObject.SetActive(false);
            }
	    }

	    public static void ShowAllChild(this Transform transform)
	    {
	        for (int i = 0; i < transform.childCount; i++)
	        {
	            transform.GetChild(i).gameObject.SetActive(true);
	        }
	    }
    }
}