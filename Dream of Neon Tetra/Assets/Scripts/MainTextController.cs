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

        // void Start ではなく IEnumerator Start に書き換える
        IEnumerator Start()
        {
            // 1フレーム待つことで、GameManager側の参照更新(OnSceneLoaded)を確実に待つ
            yield return null;

            if (GameManager.Instance.userScriptManager == null)
            {
                Debug.LogError("UserScriptManagerが見つかりません");
                yield break;
            }

            _currentSentence = GameManager.Instance.userScriptManager.GetCurrentSentence();

            // 最初の行が命令文（背景設定など）なら処理する
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
                // --- 修正点：UI判定ではなくフラグ判定に変更 ---
                // メニューまたはログ画面が開いている時は、クリックを無視する
                if (GameManager.Instance.isMenuOpen)
                {
                    return;
                }

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

        IEnumerator TypeText()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddLog(_currentSentence);
            }

            _isTyping = true;
            _mainTextObject.text = "";

            int i = 0;
            while (i < _currentSentence.Length)
            {
                char c = _currentSentence[i];

                if (c == '<')
                {
                    int tagStartIndex = i;
                    while (i < _currentSentence.Length && _currentSentence[i] != '>')
                    {
                        i++;
                    }
                    i++;
                    _mainTextObject.text += _currentSentence.Substring(tagStartIndex, i - tagStartIndex);
                }
                else
                {
                    _mainTextObject.text += c;
                    i++;
                    yield return new WaitForSeconds(_textSpeed);
                }
            }
            _isTyping = false;
        }
    }
}