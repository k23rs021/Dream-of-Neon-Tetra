using UnityEngine;

using System.Collections;
using System.Collections.Generic;
namespace NovelGame
{
    public class GameManager : MonoBehaviour
    {
        // 別のクラスからGameManagerの変数などを使えるようにするためのもの。（変更はできない）
        public static GameManager Instance { get; private set; }

        public UserScriptManager userScriptManager;
        public MainTextController mainTextController;
        public ImageManager imageManager;
        public bool isMenuOpen = false;

        // ユーザスクリプトの、今の行の数値。クリック（タップ）のたびに1ずつ増える。
        [System.NonSerialized] public int lineNumber;

        void Awake()
        {
            // これで、別のクラスからGameManagerの変数などを使えるようになる。
            Instance = this;

            lineNumber = 0;
        }

        public List<string> logList = new List<string>(); // ログを保存するリスト
        public const int MaxLogCount = 50; // 最大保存件数（お好みで）

        public void AddLog(string sentence)
        {
            logList.Add(sentence);
            // 溜まりすぎたら古いものから消す
            if (logList.Count > MaxLogCount)
            {
                logList.RemoveAt(0);
            }
        }
    }

}