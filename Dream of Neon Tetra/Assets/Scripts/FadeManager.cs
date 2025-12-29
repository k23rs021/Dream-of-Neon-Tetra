using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;      // フェード用の画像
    public float fadeSpeed = 1.0f; // フェードの速さ

    void Start()
    {
        // 起動時は画像を真っ黒（不透明）にする
        fadeImage.color = new Color(0, 0, 0, 1);

        // フェードイン（徐々に透明にする）を開始
        StartCoroutine(DoFadeIn());
    }

    IEnumerator DoFadeIn()
    {
        float alpha = 1.0f;

        while (alpha > 0)
        {
            // 時間経過に合わせてアルファ値を減らす
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null; // 1フレーム待機
        }

        // 完全に透明になったら、下のボタンなどを触れるように非アクティブ化する
        fadeImage.gameObject.SetActive(false);
    }
}