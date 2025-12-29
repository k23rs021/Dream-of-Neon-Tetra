using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("右側に表示するオブジェクト")]
    public GameObject imageToDisplay;

    [Header("ホバー時のボタン画像")]
    public Sprite hoveredButtonSprite;

    private Image buttonImage; // ボタン自身のImageコンポーネント用
    private Sprite originalButtonSprite; // 元のボタン画像を保存用

    void Start()
    {
        // 1. 右側の画像を非表示にする
        if (imageToDisplay != null)
        {
            imageToDisplay.SetActive(false);
        }

        // 2. ボタン自身のImageコンポーネントを取得し、元の画像を覚えておく
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalButtonSprite = buttonImage.sprite;
        }
    }

    // マウスがボタンに入ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 右側の画像を表示
        if (imageToDisplay != null)
        {
            imageToDisplay.SetActive(true);
        }

        // ボタンの画像をホバー用に切り替え
        if (buttonImage != null && hoveredButtonSprite != null)
        {
            buttonImage.sprite = hoveredButtonSprite;
        }
    }

    // マウスがボタンから出たとき
    public void OnPointerExit(PointerEventData eventData)
    {
        // 右側の画像を非表示
        if (imageToDisplay != null)
        {
            imageToDisplay.SetActive(false);
        }

        // ボタンの画像を元に戻す
        if (buttonImage != null)
        {
            buttonImage.sprite = originalButtonSprite;
        }
    }
}