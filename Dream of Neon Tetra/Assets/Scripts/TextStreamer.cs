using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI操作に必要

namespace NovelGame
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextStreamer : MonoBehaviour
    {
        [Header("読み込むテキストファイル")]
        [SerializeField] private TextAsset _textFile;

        [Header("文字を表示する速さ")]
        [SerializeField] private float _textSpeed = 0.05f;

        [Header("次に遷移するシーン名")]
        [SerializeField] private string _nextSceneName;

        [Header("フェード用の暗幕パネル (Image)")]
        [SerializeField] private Image _fadePanel;
        [SerializeField] private float _fadeDuration = 1.5f;

        private TextMeshProUGUI _uiText;
        private List<string> _sentences = new List<string>();
        private int _currentLineIndex = 0;
        private int _linesInCurrentPage = 0;

        private bool _isTyping = false;
        private bool _isExiting = false; // 遷移中フラグ
        private string _baseText = "";
        private string _fullVisibleText = "";

        void Awake()
        {
            _uiText = GetComponent<TextMeshProUGUI>();
            _uiText.text = "";

            // フェードパネルの初期化（透明にしておく）
            if (_fadePanel != null)
            {
                Color c = _fadePanel.color;
                c.a = 0;
                _fadePanel.color = c;
                _fadePanel.gameObject.SetActive(false);
            }

            if (_textFile != null)
            {
                StringReader reader = new StringReader(_textFile.text);
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line)) _sentences.Add(line);
                }
            }
        }

        void Update()
        {
            if (_isExiting) return; // 遷移中は入力を受け付けない

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                if (_isTyping)
                {
                    StopAllCoroutines();
                    _uiText.text = _fullVisibleText;
                    _baseText = _fullVisibleText;
                    _isTyping = false;
                }
                else
                {
                    DisplayNextStep();
                }
            }
        }

        private void DisplayNextStep()
        {
            // 全行表示後の処理
            if (_currentLineIndex >= _sentences.Count)
            {
                StartCoroutine(FadeAndExit());
                return;
            }

            if (_linesInCurrentPage >= 2)
            {
                _uiText.text = "";
                _baseText = "";
                _fullVisibleText = "";
                _linesInCurrentPage = 0;
            }

            string targetLine = _sentences[_currentLineIndex];

            if (targetLine.StartsWith("&"))
            {
                _currentLineIndex++;
                DisplayNextStep();
                return;
            }

            string newTextSegment = "";
            if (_linesInCurrentPage == 0)
            {
                newTextSegment = targetLine;
                _fullVisibleText = targetLine;
            }
            else
            {
                newTextSegment = "\n\n" + targetLine;
                _fullVisibleText = _baseText + newTextSegment;
            }

            StartCoroutine(TypeNewSegment(newTextSegment));

            _linesInCurrentPage++;
            _currentLineIndex++;
        }

        IEnumerator TypeNewSegment(string segment)
        {
            _isTyping = true;
            foreach (char c in segment)
            {
                _uiText.text += c;
                yield return new WaitForSeconds(_textSpeed);
            }
            _baseText = _uiText.text;
            _isTyping = false;
        }

        // 徐々に暗くしてシーン遷移するコルーチン
        IEnumerator FadeAndExit()
        {
            _isExiting = true;

            if (_fadePanel != null)
            {
                _fadePanel.gameObject.SetActive(true);
                float timer = 0;
                Color c = _fadePanel.color;

                while (timer < _fadeDuration)
                {
                    timer += Time.deltaTime;
                    c.a = Mathf.Lerp(0, 1, timer / _fadeDuration); // 0から1へ変化
                    _fadePanel.color = c;
                    yield return null;
                }
            }

            if (!string.IsNullOrEmpty(_nextSceneName))
            {
                SceneManager.LoadScene(_nextSceneName);
            }
        }
    }
}