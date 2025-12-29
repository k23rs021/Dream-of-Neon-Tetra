using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace NovelGame
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextStreamer : MonoBehaviour
    {
        [Header("読み込むテキストファイル")]
        [SerializeField] private TextAsset _textFile;

        [Header("文字を表示する速さ")]
        [SerializeField] private float _textSpeed = 0.05f;

        private TextMeshProUGUI _uiText;
        private List<string> _sentences = new List<string>();
        private int _currentLineIndex = 0;
        private int _linesInCurrentPage = 0;

        private bool _isTyping = false;
        private string _baseText = "";      // すでに表示が完了している文字
        private string _fullVisibleText = ""; // 現在のページの完成形

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
                    _baseText = _fullVisibleText; // スキップ時もベーステキストを更新
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
            if (_currentLineIndex >= _sentences.Count) return;

            // 2行表示済みならクリア
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

            // タイピングすべき新しい文字列を特定する
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

            // 新しい部分だけをタイピングするコルーチンを開始
            StartCoroutine(TypeNewSegment(newTextSegment));

            _linesInCurrentPage++;
            _currentLineIndex++;
        }

        IEnumerator TypeNewSegment(string segment)
        {
            _isTyping = true;

            // 現在表示されているテキストをベースに、1文字ずつ追加
            foreach (char c in segment)
            {
                _uiText.text += c;
                yield return new WaitForSeconds(_textSpeed);
            }

            // 表示が終わった状態を保存
            _baseText = _uiText.text;
            _isTyping = false;
        }
    }
}