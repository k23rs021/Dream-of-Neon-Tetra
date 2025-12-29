using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [Header("遷移先のシーン名")]
    public string sceneName;

    [Header("フェード用の画像")]
    public Image fadeImage;

    [Header("フェードアウトの速さ")]
    public float fadeSpeed = 1.0f;

    // ボタンから呼び出す関数
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // フェードアウト処理を開始
            StartCoroutine(DoFadeOutAndLeave());
        }
        else
        {
            Debug.LogWarning("遷移先のシーン名が設定されていません。");
        }
    }

    IEnumerator DoFadeOutAndLeave()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Imageがセットされていません。");
            SceneManager.LoadScene(sceneName); // 画像がない場合は即座に遷移
            yield break;
        }

        // フェード用画像を表示状態にする
        fadeImage.gameObject.SetActive(true);
        float alpha = 0;

        // 徐々に黒くしていく
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null; // 1フレーム待機
        }

        // 完全に黒くなったらシーンを切り替える
        SceneManager.LoadScene(sceneName);
    }
}