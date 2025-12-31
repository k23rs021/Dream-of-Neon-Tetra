using UnityEngine;
using UnityEngine.EventSystems;

namespace NovelGame
{
    // ISubmitHandler を追加して、Enterキーなどの決定操作を検知できるようにする
    public class SimpleButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
    {
        [Header("SEの設定")]
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] private AudioClip _clickSound;

        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GameObject.Find("SEPlayer")?.GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }
        }

        // --- 共通の音再生メソッド ---
        private void PlayHoverSound()
        {
            if (_hoverSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_hoverSound);
            }
        }

        private void PlayClickSound()
        {
            if (_clickSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_clickSound);
            }
        }

        // --- ホバー・選択時の処理 ---
        public void OnPointerEnter(PointerEventData eventData) => PlayHoverSound();
        public void OnSelect(BaseEventData eventData) => PlayHoverSound();

        // --- マウスによるクリック時の処理 ---
        public void OnPointerClick(PointerEventData eventData) => PlayClickSound();

        // --- キーボード(Enter)やコントローラー(決定ボタン)による処理 ---
        public void OnSubmit(BaseEventData eventData)
        {
            PlayClickSound();
        }
    }
}