using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NovelGame
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("16個のアイコン用Image（子要素のIcon）を順番に入れる")]
        [SerializeField] private List<Image> _slotIcons;

        [Header("全アイテムのデータ登録")]
        [SerializeField] private List<ItemData> _allItemData;

        [System.Serializable]
        public struct ItemData
        {
            public string itemName; // GameManagerに登録する名前（例：古い日記）
            public Sprite itemIcon; // 表示したい画像
        }

        // 画面が開くたびに実行される
        void OnEnable()
        {
            UpdateInventory();
        }

        public void UpdateInventory()
        {
            // 1. まず16個すべてのアイコンを非表示（透明）にする
            foreach (var icon in _slotIcons)
            {
                icon.enabled = false;
            }

            // 2. GameManagerのリストにある分だけ画像を表示する
            for (int i = 0; i < GameManager.Instance.inventoryList.Count; i++)
            {
                if (i >= _slotIcons.Count) break; // 16個を超えたら無視

                string currentItemName = GameManager.Instance.inventoryList[i];

                // 全データの中から、名前が一致するデータを探す
                ItemData data = _allItemData.Find(x => x.itemName == currentItemName);

                if (data.itemIcon != null)
                {
                    _slotIcons[i].sprite = data.itemIcon;
                    _slotIcons[i].enabled = true; // 画像を表示
                }
            }
        }
    }
}