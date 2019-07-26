using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
  public class ExpressionAnim
  {
      public GameObject gameObject;
      public Sprite[] _Sprites;
      public Image _Image;

      public void Init(Image image, Sprite[] sprites)
      {
          gameObject = image.gameObject;
          _Image = image;
            Show(sprites);
      }

      public void SetParent(Transform parent)
      {
          gameObject.transform.SetParent(parent);
      }
      public void Show(Sprite[] sprites)
      {
          _Sprites = sprites;
          gameObject.SetActive(true);
          ExecuteAnim();
      }

      private bool _isAnimBeing = false;
      public async void ExecuteAnim()
      {
          if (_isAnimBeing)
          {
              return;
          }
          _isAnimBeing = true;
          int index = 0;
          while (gameObject.activeInHierarchy)
          {
              if (index >= _Sprites.Length)
              {
                  index = 0;
              }
              _Image.sprite = _Sprites[index++];
              await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(200);
          }
          _isAnimBeing = false;
        }
      public void LocalPositionZero()
      {
          gameObject.transform.localPosition = Vector3.zero;
      }
        public void Destroy()
      {
          gameObject.SetActive(false);
          ExpressionAnimPool.Ins.DestroyExpression(this);
      }


  }
}
