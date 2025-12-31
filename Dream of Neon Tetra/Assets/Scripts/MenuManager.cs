using UnityEngine;

namespace NovelGame
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _menuRootPanel;  // メニュー画面
        [SerializeField] private GameObject _inventoryPanel; // 持ち物画面

        // 1. メニュー内の「持ち物」ボタンを押した時
        public void OpenInventory()
        {
            _inventoryPanel.SetActive(true);  // 持ち物画面を出す
            _menuRootPanel.SetActive(false);   // メニュー画面を隠す
        }

        // 2. 持ち物画面の「戻る（◀）」ボタンを押した時
        public void CloseInventory()
        {
            _inventoryPanel.SetActive(false); // 持ち物画面を隠す
            _menuRootPanel.SetActive(true);    // メニュー画面を出す
        }

        // 3. 全てを閉じてゲーム画面に戻る時（念のため）
        public void CloseAll()
        {
            _menuRootPanel.SetActive(false);
            _inventoryPanel.SetActive(false);
            GameManager.Instance.isMenuOpen = false; // クリック制限を解除
        }
    }
}