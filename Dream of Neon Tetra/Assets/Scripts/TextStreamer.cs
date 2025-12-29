using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement; // シーン遷移に必要

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

        private TextMeshProUGUI _uiText;
        private List<string> _sentences = new List<string>();
        private int _currentLineIndex = 0;
        private int _linesInCurrentPage = 0;

        private bool _isTyping = false;
        private string _baseText = "";
        private string _fullVisibleText = "";

        void Awake()
        {
            _uiText = GetComponent<TextMeshProUGUI>();
            _uiText.text = "";

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
            // --- 修正箇所：全行表示後の処理 ---
            if (_currentLineIndex >= _sentences.Count)
            {
                if (!string.IsNullOrEmpty(_nextSceneName))
                {
                    SceneManager.LoadScene(_nextSceneName);
                }
                else
                {
                    Debug.LogWarning("次のシーン名が設定されていません。");
                }
                return;
            }
            // --------------------------------

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
    }
}