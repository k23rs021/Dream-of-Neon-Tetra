using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ISelectHandler と IDeselectHandler を追加
public class ButtonHoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("右側に表示するオブジェクト")]
    public GameObject imageToDisplay;

    [Header("選択・ホバー時のボタン画像")]
    public Sprite hoveredButtonSprite;

    private Image buttonImage;
    private Sprite originalButtonSprite;

    void Start()
    {
        if (imageToDisplay != null)
        {
            imageToDisplay.SetActive(false);
        }

        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalButtonSprite = buttonImage.sprite;
        }
    }

    // --- 表示・画像切り替えの共通処理 ---
    private void ShowEffect()
    {
        if (imageToDisplay != null) imageToDisplay.SetActive(true);
        if (buttonImage != null && hoveredButtonSprite != null)
        {
            buttonImage.sprite = hoveredButtonSprite;
        }
    }

    private void HideEffect()
    {
        if (imageToDisplay != null) imageToDisplay.SetActive(false);
        if (buttonImage != null)
        {
            buttonImage.sprite = originalButtonSprite;
        }
    }

    // --- マウス操作用 ---
    public void OnPointerEnter(PointerEventData eventData) => ShowEffect();
    public void OnPointerExit(PointerEventData eventData) => HideEffect();

    // --- 十字キー・コントローラー操作用 ---
    public void OnSelect(BaseEventData eventData) => ShowEffect();
    public void OnDeselect(BaseEventData eventData) => HideEffect();
}