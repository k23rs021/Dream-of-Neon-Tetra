using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement; // シーン遷移に必要

namespace NovelGame
{
    public class UserScriptManager : MonoBehaviour
    {
        [SerializeField] TextAsset _textFile;
        [SerializeField] string _nextSceneName; // インスペクターで遷移先のシーン名を入力

        // 文章中の文（ここでは１行ごと）を入れておくためのリスト
        List<string> _sentences = new List<string>();

        void Awake()
        {
            // テキストファイルの中身を、１行ずつリストに入れておく
            StringReader reader = new StringReader(_textFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                // 空行を無視したい場合は if (!string.IsNullOrWhiteSpace(line)) を使う
                _sentences.Add(line);
            }
        }

        // 現在の行の文を取得する
        public string GetCurrentSentence()
        {
            // インデックスがリストの範囲外（全ての行を読み終えた後）かチェック
            if (GameManager.Instance.lineNumber >= _sentences.Count)
            {
                // 全て読み終えていたらシーン遷移を実行
                TransitionToNextScene();
                return null;
            }

            return _sentences[GameManager.Instance.lineNumber];
        }

        // シーン遷移の実行
        private void TransitionToNextScene()
        {
            if (!string.IsNullOrEmpty(_nextSceneName))
            {
                SceneManager.LoadScene(_nextSceneName);
            }
            else
            {
                Debug.LogWarning("次のシーン名が設定されていません。");
            }
        }

        // 文が命令かどうか
        public bool IsStatement(string sentence)
        {
            // sentence が null の場合のチェックを追加（遷移時のエラー防止）
            if (string.IsNullOrEmpty(sentence)) return false;

            if (sentence[0] == '&')
            {
                return true;
            }
            return false;
        }

        // 命令を実行する
        public void ExecuteStatement(string sentence)
        {
            if (string.IsNullOrEmpty(sentence)) return;

            string[] words = sentence.Split(' ');
            switch (words[0])
            {
                case "&img":
                    GameManager.Instance.imageManager.PutImage(words[1], words[2]);
                    break;
            }
        }
    }
}