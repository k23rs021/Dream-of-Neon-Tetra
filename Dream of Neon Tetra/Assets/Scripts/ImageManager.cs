using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NovelGame
{
    public class ImageManager : MonoBehaviour
    {
        [SerializeField] Sprite _background1;
        [SerializeField] GameObject _backgroundObject;
        [SerializeField] GameObject _imagePrefab;

        // テキストファイルから、文字列でSpriteやGameObjectを扱えるようにするための辞書
        Dictionary<string, Sprite> _textToSprite;
        Dictionary<string, GameObject> _textToParentObject;

        // 操作したいPrefabを指定できるようにするための辞書
        Dictionary<string, GameObject> _textToSpriteObject;

        void Awake()
        {
            _textToSprite = new Dictionary<string, Sprite>();
            _textToSprite.Add("background1", _background1);

            _textToParentObject = new Dictionary<string, GameObject>();
            _textToParentObject.Add("backgroundObject", _backgroundObject);

            _textToSpriteObject = new Dictionary<string, GameObject>();
        }

        // 画像を配置する
        public void PutImage(string imageName, string parentObjectName)
        {
            Sprite image = _textToSprite[imageName];
            GameObject parentObject = _textToParentObject[parentObjectName];

            Vector2 position = new Vector2(0, 0);
            Quaternion rotation = Quaternion.identity;
            Transform parent = parentObject.transform;
            GameObject item = Instantiate(_imagePrefab, position, rotation, parent);
            item.GetComponent<Image>().sprite = image;

            _textToSpriteObject.Add(imageName, item);
        }
        // 画像を生成する親（Canvas内のオブジェクトなど）
        [SerializeField] private Transform _imageParent;

        // --- シーン遷移後に新しいシーンの親をセットするためのメソッド ---
        public void BindParentInNewScene()
        {
            // 名前で検索する場合（例: シーンごとに "ImageParent" という名前の空オブジェクトがある前提）
            GameObject parentObj = GameObject.Find("ImageParent");
            if (parentObj != null)
            {
                _imageParent = parentObj.transform;
                Debug.Log("新しいシーンの ImageParent を紐付けました。");
            }
            else
            {
                Debug.LogWarning("新しいシーンに ImageParent が見つかりません。");
            }
        }

        // 既存の画像表示メソッド（例）
        public void ShowImage(string imageName)
        {
            if (_imageParent == null)
            {
                // もし null ならここで探し直す
                BindParentInNewScene();
            }

            // 画像生成処理...
            // Instantiate(imagePrefab, _imageParent);
        }
    }
}