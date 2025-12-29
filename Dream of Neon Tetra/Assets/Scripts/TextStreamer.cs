using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

namespace NovelGame
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextStreamer : MonoBehaviour
    {
        [Header("読み込むテキストファイル")]
        [SerializeField] private TextAsset _textFile;

        private TextMeshProUGUI _uiText;
        private List<string> _sentences = new List<string>();
        private int _currentLineIndex = 0;
        private int _linesInCurrentPage = 0;

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
                DisplayNextStep();
            }
        }

        private void DisplayNextStep()
        {
            if (_currentLineIndex >= _sentences.Count) return;

            if (_linesInCurrentPage >= 2)
            {
                _uiText.text = "";
                _linesInCurrentPage = 0;
            }

            string targetLine = _sentences[_currentLineIndex];

            if (targetLine.StartsWith("&"))
            {
                _currentLineIndex++;
                DisplayNextStep();
                return;
            }

            // --- ここを修正 ---
            if (_linesInCurrentPage == 0)
            {
                _uiText.text = targetLine;
            }
            else
            {
                // \n を2つ繋げることで、1行分の空行（スペース）を作ります
                _uiText.text += "\n\n" + targetLine;
            }
            // ------------------

            _linesInCurrentPage++;
            _currentLineIndex++;
        }
    }
}