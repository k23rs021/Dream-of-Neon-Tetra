using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NovelGame
{
    public class MainTextController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _mainTextObject;
        [SerializeField] float _textSpeed = 0.05f;

        private bool _isTyping = false;
        private string _currentSentence = "";

        void Start()
        {
            _currentSentence = GameManager.Instance.userScriptManager.GetCurrentSentence();
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
            if (Input.GetMouseButtonUp(0))
            {
                if (_isTyping)
                {
                    StopAllCoroutines();
                    _mainTextObject.text = _currentSentence;
                    _isTyping = false;
                }
                else
                {
                    GoToTheNextLine();
                }
            }
        }

        public void GoToTheNextLine()
        {
            GameManager.Instance.lineNumber++;
            _currentSentence = GameManager.Instance.userScriptManager.GetCurrentSentence();

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

        private void HandleStatement()
        {
            GameManager.Instance.userScriptManager.ExecuteStatement(_currentSentence);
            GoToTheNextLine();
        }

        public void DisplayText()
        {
            StopAllCoroutines();
            StartCoroutine(TypeText());
        }

        // --- 修正したコルーチン ---
        IEnumerator TypeText()
        {
            _isTyping = true;
            _mainTextObject.text = "";

            int i = 0;
            while (i < _currentSentence.Length)
            {
                char c = _currentSentence[i];

                // タグ（<）の開始を検知した場合
                if (c == '<')
                {
                    int tagStartIndex = i;
                    // 閉じタグ（>）が見つかるまで一気に読み込む
                    while (i < _currentSentence.Length && _currentSentence[i] != '>')
                    {
                        i++;
                    }
                    // '>' 自体もインデックスに含める
                    i++;

                    // タグの部分を一度に流し込む（待ち時間なし）
                    _mainTextObject.text += _currentSentence.Substring(tagStartIndex, i - tagStartIndex);
                }
                else
                {
                    // 普通の文字なら1文字追加して待機
                    _mainTextObject.text += c;
                    i++;
                    yield return new WaitForSeconds(_textSpeed);
                }
            }

            _isTyping = false;
        }
    }
}