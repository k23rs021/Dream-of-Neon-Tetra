using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshProを使う場合

namespace NovelGame
{
    public class TextDisplayController : MonoBehaviour
    {
        [SerializeField] UserScriptManager _scriptManager;
        [SerializeField] TextMeshProUGUI _uiText; // 表示先のテキスト

        private string _currentText = ""; // 現在表示中の文字列
        private int _displayedLinesInPage = 0; // 現在のページに表示された行数(0, 1, 2)

        /// <summary>
        /// GameManagerなどのUpdateからクリック時にこれを呼ぶ
        /// </summary>
        public void OnClickNext()
        {
            // 2行表示されている状態でクリックされたら、クリアして次の行へ
            if (_displayedLinesInPage >= 2)
            {
                ClearText();
                // クリアした直後に1行目を出したい場合は、ここで続けて表示処理を行う
            }

            // 次の有効な文章（命令でない行）が見つかるまで読み進める
            while (GameManager.Instance.lineNumber < GetTotalLines())
            {
                string sentence = _scriptManager.GetCurrentSentence();

                // 命令文の場合
                if (_scriptManager.IsStatement(sentence))
                {
                    _scriptManager.ExecuteStatement(sentence);
                    GameManager.Instance.lineNumber++;
                    // 命令は画面にカウントしないので、ループを回して次の行へ
                    continue;
                }

                // 通常の文章の場合
                DisplaySentence(sentence);
                GameManager.Instance.lineNumber++;
                break; // 1行表示したのでループを抜ける
            }
        }

        private void DisplaySentence(string sentence)
        {
            if (_displayedLinesInPage == 0)
            {
                _currentText = sentence;
            }
            else
            {
                _currentText += "\n" + sentence;
            }

            _uiText.text = _currentText;
            _displayedLinesInPage++;
        }

        private void ClearText()
        {
            _currentText = "";
            _uiText.text = "";
            _displayedLinesInPage = 0;
        }

        // UserScriptManagerのリストにアクセスできないため、
        // 必要に応じてUserScriptManager側にListのCountを返すメソッドを追加してください
        private int GetTotalLines()
        {
            // ここでは簡易的に大きな数を入れていますが、
            // 実際は _scriptManager 内の _sentences.Count を参照するのが理想です
            return 9999;
        }
    }
}