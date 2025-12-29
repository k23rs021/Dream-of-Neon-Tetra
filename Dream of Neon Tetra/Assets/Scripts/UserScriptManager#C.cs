using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NovelGame
{
    public class UserScriptManager : MonoBehaviour
    {
        [SerializeField] TextAsset _textFile;

        // 文章中の文（ここでは１行ごと）を入れておくためのリスト
        List<string> _sentences = new List<string>();

        void Awake()
        {
            // テキストファイルの中身を、１行ずつリストに入れておく
            StringReader reader = new StringReader(_textFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                _sentences.Add(line);
            }
        }

        // 現在の行の文を取得する
        public string GetCurrentSentence()
        {
            return _sentences[GameManager.Instance.lineNumber];
        }
    }
}
