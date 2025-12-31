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

        [Header("Audio Settings")]
        [SerializeField] private AudioClip _typeSound; // 1文字ごとの音
        private AudioSource _audioSource;

        private bool _isTyping = false;
        private string _currentSentence = "";

        void Awake()
        {
            // AudioSourceを取得、なければ追加する
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
            _audioSource.playOnAwake = false;
        }

        IEnumerator Start()
        {
            yield return null;

            if (GameManager.Instance.userScriptManager == null)
            {
                Debug.LogError("UserScriptManagerが見つかりません");
                yield break;
            }

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

                    // --- 修正点：文字が表示されるたびに音を鳴らす ---
                    if (_typeSound != null && _audioSource != null)
                    {
                        _audioSource.PlayOneShot(_typeSound);
                    }

                    i++;
                    yield return new WaitForSeconds(_textSpeed);
                }
            }
            _isTyping = false;
        }
    }
}