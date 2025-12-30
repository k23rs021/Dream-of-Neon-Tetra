using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動の検知に必要
using System.Collections;
using System.Collections.Generic;

namespace NovelGame
{
    public class GameManager : MonoBehaviour
    {
        // シングルトン実体
        public static GameManager Instance { get; private set; }

        [Header("Managers")]
        public UserScriptManager userScriptManager;
        public MainTextController mainTextController;
        public ImageManager imageManager;

        [Header("Status")]
        public bool isMenuOpen = false;
        // ユーザスクリプトの今の行数。
        [System.NonSerialized] public int lineNumber;

        [Header("Log Settings")]
        public List<string> logList = new List<string>();
        public const int MaxLogCount = 50;

        void Awake()
        {
            // --- シングルトン & DontDestroyOnLoad ---
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // シーンが読み込まれたときに OnSceneLoaded を実行するように登録
                SceneManager.sceneLoaded += OnSceneLoaded;

                lineNumber = 0;
            }
            else
            {
                // 二重生成された場合は新しい方を削除
                Destroy(gameObject);
            }
        }

        // --- シーンが切り替わった直後に実行される処理 ---
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            lineNumber = 0;
            // 1. 各マネージャーの参照を、新しいシーン内のものに更新する
            // シーンを跨ぐと以前のシーンのオブジェクトへの参照は無効になるため、探し直す必要があります
            userScriptManager = FindObjectOfType<UserScriptManager>();
            mainTextController = FindObjectOfType<MainTextController>();
            imageManager = FindObjectOfType<ImageManager>();

            // 2. ImageManager の画像生成親（Canvas）を再設定する
            if (imageManager != null)
            {
                imageManager.BindParentInNewScene();
            }

            // メニューが開いたままシーン移動したときのためのリセット
            isMenuOpen = false;

            Debug.Log($"シーン {scene.name} のマネージャー参照を更新しました。");
        }

        private void OnDestroy()
        {
            // オブジェクトが破棄されるとき、イベント登録を解除（メモリリーク防止）
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// ログに文章を追加する
        /// </summary>
        /// <param name="sentence">保存する文章</param>
        public void AddLog(string sentence)
        {
            // 空の文章や、命令文（&で始まるもの）はログに含めない
            if (string.IsNullOrEmpty(sentence) || sentence.StartsWith("&"))
            {
                return;
            }

            logList.Add(sentence);

            // 溜まりすぎたら古いものから消す
            if (logList.Count > MaxLogCount)
            {
                logList.RemoveAt(0);
            }
        }

        /// <summary>
        /// ログを全消去する
        /// </summary>
        public void ClearLog()
        {
            logList.Clear();
        }
    }
}