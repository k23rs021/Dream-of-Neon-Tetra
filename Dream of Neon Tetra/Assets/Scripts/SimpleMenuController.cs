using UnityEngine;

namespace NovelGame
{
    // CanvasGroupを使ってメニューの開閉を管理する
    [RequireComponent(typeof(CanvasGroup))]
    public class SimpleMenuController : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private bool _isOpen = false;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            // 最初は閉じておく
            CloseMenu();
        }

        // メニューの開閉を切り替える（ボタンから呼び出す用）
        public void ToggleMenu()
        {
            if (_isOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        public void OpenMenu()
        {
            _isOpen = true;
            _canvasGroup.alpha = 1;           // 見えるようにする
            _canvasGroup.interactable = true; // メニュー内のボタンを押せるようにする
            _canvasGroup.blocksRaycasts = true; // 背後のクリックを遮断する

            // GameManagerのフラグを更新（テキスト進行を止める）
            if (GameManager.Instance != null)
            {
                GameManager.Instance.isMenuOpen = true;
            }
        }

        public void CloseMenu()
        {
            _isOpen = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            // GameManagerのフラグを更新（テキスト進行を再開）
            if (GameManager.Instance != null)
            {
                GameManager.Instance.isMenuOpen = false;
            }
        }
    }
}