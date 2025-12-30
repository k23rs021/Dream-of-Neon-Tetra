using UnityEngine;
using TMPro;

namespace NovelGame
{
    public class LogViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _logTextPrefab;
        [SerializeField] private Transform _logContentParent;

        void Awake()
        {
            // ゲーム開始時、ログパネルを確実に非表示にする
            gameObject.SetActive(false);
        }

        // ログ画面を表示する（ログボタンからこれを呼ぶ）
        public void ShowLog()
        {
            // 自身のゲームオブジェクト（LogPanel）を表示する
            gameObject.SetActive(true);

            if (GameManager.Instance != null)
            {
                Debug.Log("現在のログ件数: " + GameManager.Instance.logList.Count);
            }

            // 一旦古い表示を消去
            foreach (Transform child in _logContentParent)
            {
                Destroy(child.gameObject);
            }

            // GameManagerの存在チェック（エラー防止）
            if (GameManager.Instance == null) return;

            // ログを生成
            foreach (string log in GameManager.Instance.logList)
            {
                GameObject obj = Instantiate(_logTextPrefab, _logContentParent);
                obj.GetComponent<TextMeshProUGUI>().text = log;
            }

            // スクロール位置を一番下にする処理
            Canvas.ForceUpdateCanvases();
            var scrollRect = GetComponentInParent<UnityEngine.UI.ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        // ログ画面を閉じる（ログ画面内の「戻る」ボタン用）
        public void HideLog()
        {
            gameObject.SetActive(false);
        }
        void Update()
        {
            // ログが表示されている間だけキー入力を受け付ける
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                HideLog();
            }
        }
    }
}