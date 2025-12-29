using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NovelGame
{
    public class MainTextController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _mainTextObject;
        [SerializeField] float _textSpeed = 0.05f; // 文字を表示する速さ

        private bool _isTyping = false; // 現在文字を表示中か
        private string _currentSentence = ""; // 現在表示すべき全文

        void Start()
        {
            // 最初の行を取得
            _currentSentence = GameManager.Instance.userScriptManager.GetCurrentSentence();

            // 命令文（&）なら実行して次の行へ
            if (GameManager.Instance.userScriptManager.IsStatement(_currentSentence))
            {
                HandleStatement();
            }
            else
            {
                StartCoroutine(TypeText());
            }
        }

        void Update()
        {
            // クリックされたときの処理
            if (Input.GetMouseButtonUp(0))
            {
                if (_isTyping)
                {
                    // タイピング中にクリック：一瞬で全文表示
                    StopAllCoroutines();
                    _mainTextObject.text = _currentSentence;
                    _isTyping = false;
                }
                else
                {
                    // 表示完了後にクリック：次の行へ
                    GoToTheNextLine();
                }
            }
        }

        // 次の行へ移動
        public void GoToTheNextLine()
        {
            GameManager.Instance.lineNumber++;
            _currentSentence = GameManager.Instance.userScriptManager.GetCurrentSentence();

            // 文がなくなったら終了（エラー防止）
            if (string.IsNullOrEmpty(_currentSentence)) return;

            if (GameManager.Instance.userScriptManager.IsStatement(_currentSentence))
            {
                HandleStatement();
            }
            else
            {
                DisplayText();
            }
        }

        // 命令文を再帰的に処理する
        private void HandleStatement()
        {
            GameManager.Instance.userScriptManager.ExecuteStatement(_currentSentence);
            GoToTheNextLine();
        }

        // テキスト表示開始
        public void DisplayText()
        {
            StopAllCoroutines();
            StartCoroutine(TypeText());
        }

        // 一文字ずつ表示するコルーチン
        IEnumerator TypeText()
        {
            _isTyping = true;
            _mainTextObject.text = "";

            foreach (char c in _currentSentence)
            {
                _mainTextObject.text += c;
                yield return new WaitForSeconds(_textSpeed);
            }

            _isTyping = false;
        }
    }
}