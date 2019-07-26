using System;
using System.Collections.Generic;
using ETHotfix;
using UnityEngine;

public static class ItemCreaterHelper
{
    public readonly static Dictionary<GameObject,List<InitBaseItem>> goBaseIetms=new Dictionary<GameObject, List<InitBaseItem>>();

    public static T AddItemIfHaveInit<T, B>(this GameObject gameObject,B b) where T : InitBaseItem
    {
        if (!goBaseIetms.ContainsKey(gameObject))
        {
            T initBaseItem = ComponentFactory.Create<T, GameObject, B>(gameObject, b);
            goBaseIetms.Add(gameObject, new List<InitBaseItem>() { initBaseItem });
            return initBaseItem;
        }
        else
        {
            foreach (var initbaseItem in goBaseIetms[gameObject])
            {
                Type type = typeof(T);
                if (type == initbaseItem.GetType())
                {
                    Game.EventSystem.Awake(initbaseItem, gameObject, b);
                    return initbaseItem as T;
                }
            }
            T initBaseItem = ComponentFactory.Create<T, GameObject, B>(gameObject, b);
            goBaseIetms[gameObject].Add(initBaseItem);
            return initBaseItem;
        }
    }

    public static List<T> CreatorChildAndAddItem<T, B>(this Transform parentTransform, IList<B> dataArray) where T : InitBaseItem
    {
        List<T> tList=new List<T>();
        ETModel.GameObjectHelper.CreatorChildCount(parentTransform,dataArray.Count);
        for (int i = 0; i < dataArray.Count; i++)
        {
            T t=parentTransform.GetChild(i).AddItemIfHaveInit<T, B>(dataArray[i]);
            tList.Add(t);
        }
        return tList;
    }

    public static T AddItemIfHaveInit<T, B>(this Transform transform, B b) where T : InitBaseItem
    {
      return  transform.gameObject.AddItemIfHaveInit<T, B>(b);
    }

    public static T AddItem<T>(this GameObject gameObject) where T : InitBaseItem ,new()
    {
        T initBaseItem =new T();
        initBaseItem.Init(gameObject);
        return initBaseItem;
    }
}

