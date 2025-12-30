using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ボタン制御に必要

namespace NovelGame
{
    public class SaveManager : MonoBehaviour
    {
        [Header("ロードボタン (タイトル画面でのみ設定)")]
        [SerializeField] private Button _loadButton;

        private string _savePath;

        void Awake()
        {
            // 保存先のパスを設定
            _savePath = Application.persistentDataPath + "/savedata.json";
        }

        void Start()
        {
            // 開始時にセーブデータの有無を確認し、ロードボタンの有効・無効を切り替える
            CheckSaveDataExists();
        }

        /// <summary>
        /// セーブデータの有無を確認し、ボタンのインタラクティブ状態を更新する
        /// </summary>
        public void CheckSaveDataExists()
        {
            if (_loadButton != null)
            {
                _loadButton.interactable = File.Exists(_savePath);
            }
        }

        /// <summary>
        /// 現在の状態をセーブする（メニューのセーブボタン用）
        /// </summary>
        public void Save()
        {
            try
            {
                SaveData data = new SaveData();
                data.lineNumber = GameManager.Instance.lineNumber;
                data.sceneName = SceneManager.GetActiveScene().name;

                string json = JsonUtility.ToJson(data);
                File.WriteAllText(_savePath, json);

                Debug.Log($"<color=green>保存成功:</color> {_savePath}");

                // セーブ直後、もしロードボタンがあれば有効化する
                CheckSaveDataExists();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"セーブ失敗: {e.Message}");
            }
        }

        /// <summary>
        /// セーブデータをロードしてシーンを切り替える（タイトルやメニューのロードボタン用）
        /// </summary>
        public void Load()
        {
            if (!File.Exists(_savePath))
            {
                Debug.LogWarning("セーブデータが見つかりません。");
                return;
            }

            try
            {
                string json = File.ReadAllText(_savePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                // GameManagerにロードした行番号をセット
                // (GameManagerが DontDestroyOnLoad である必要があります)
                GameManager.Instance.lineNumber = data.lineNumber;

                // 保存されていたシーンへ遷移
                SceneManager.LoadScene(data.sceneName);

                Debug.Log($"<color=blue>ロード成功:</color> {data.sceneName} の {data.lineNumber}行目から開始します。");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"ロード失敗: {e.Message}");
            }
        }

        /// <summary>
        /// デバッグ用：セーブデータを削除する（必要に応じて使用）
        /// </summary>
        [ContextMenu("Delete Save Data")]
        public void DeleteSaveData()
        {
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
                CheckSaveDataExists();
                Debug.Log("セーブデータを削除しました。");
            }
        }
    }

    /// <summary>
    /// セーブデータの構造
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public int lineNumber;
        public string sceneName;
    }
}