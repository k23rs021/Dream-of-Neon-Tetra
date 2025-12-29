using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // マウス検知に必要

namespace NovelGame
{
    [RequireComponent(typeof(Image))]
    public class HoverImageController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;

        void Awake()
        {
            _image = GetComponent<Image>();

            // 最初は透明（または非表示）にしておく
            SetImageAlpha(0);
        }

        // マウスが上に乗った時
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetImageAlpha(1);
        }

        // マウスが離れた時
        public void OnPointerExit(PointerEventData eventData)
        {
            SetImageAlpha(0);
        }

        // アルファ値を変更する共通メソッド
        private void SetImageAlpha(float alpha)
        {
            if (_image != null)
            {
                Color c = _image.color;
                c.a = alpha;
                _image.color = c;
            }
        }
    }
}