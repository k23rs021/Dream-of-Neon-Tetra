using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneChanger : MonoBehaviour
{
    [Header("遷移先のシーン名")]
    public string sceneName;

    // ボタンから呼び出す関数
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("遷移先のシーン名が設定されていません。");
        }
    }
}