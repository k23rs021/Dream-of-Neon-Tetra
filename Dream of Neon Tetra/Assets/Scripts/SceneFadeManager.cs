using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFadeManager : MonoBehaviour
{
    [Header("フェード用の画像")]
    public Image fadeImage;

    [Header("フェードの速さ")]
    public float fadeSpeed = 1.0f;

    [Header("遷移先のシーン名")]
    public string nextSceneName;

    void Start()
    {
        // シーン開始時に自動でフェードインを開始
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 1);
            StartCoroutine(DoFadeIn());
        }
    }

    // シーン開始時の処理
    IEnumerator DoFadeIn()
    {
        float alpha = 1.0f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    // ボタンから呼び出す関数（エンターやクリック用）
    public void OnClickChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(DoFadeOutAndLoad());
        }
    }

    // 遷移時の処理
    IEnumerator DoFadeOutAndLoad()
    {
        fadeImage.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}