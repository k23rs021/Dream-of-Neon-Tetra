using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Image操作に必要

namespace NovelGame
{
    public class UserScriptManager : MonoBehaviour
    {
        [SerializeField] TextAsset _textFile;
        [SerializeField] string _nextSceneName;

        [Header("フェード設定")]
        [SerializeField] private Image _fadeImage;      // インスペクターで黒いパネルをアタッチ
        [SerializeField] private float _fadeDuration = 2.0f; // 何秒かけて暗くするか

        List<string> _sentences = new List<string>();
        private bool _isTransitioning = false; // 二重遷移防止

        void Awake()
        {
            StringReader reader = new StringReader(_textFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                _sentences.Add(line);
            }

            // 開始時はフェード用パネルを透明にしておく
            if (_fadeImage != null)
            {
                _fadeImage.gameObject.SetActive(true);
                Color c = _fadeImage.color;
                c.a = 0;
                _fadeImage.color = c;
            }
        }

        public string GetCurrentSentence()
        {
            if (GameManager.Instance.lineNumber >= _sentences.Count)
            {
                // まだ遷移を開始していなければフェードを開始
                if (!_isTransitioning)
                {
                    StartCoroutine(FadeAndTransition());
                }
                return null;
            }

            return _sentences[GameManager.Instance.lineNumber];
        }

        // 徐々に暗くしてシーン遷移するコルーチン
        private IEnumerator FadeAndTransition()
        {
            _isTransitioning = true;

            if (_fadeImage != null)
            {
                float timer = 0;
                while (timer < _fadeDuration)
                {
                    timer += Time.deltaTime;
                    float alpha = Mathf.Lerp(0, 1, timer / _fadeDuration);

                    Color c = _fadeImage.color;
                    c.a = alpha;
                    _fadeImage.color = c;

                    yield return null;
                }
            }

            // 暗くなりきったら遷移
            TransitionToNextScene();
        }

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

        public bool IsStatement(string sentence)
        {
            if (string.IsNullOrEmpty(sentence)) return false;
            return sentence[0] == '&';
        }

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