using UnityEngine;
using TMPro;
using System.Collections;

namespace NovelGame
{
    public class TextJuice : MonoBehaviour
    {
        private TMP_Text _textComponent;

        [Header("心拍の設定")]
        [SerializeField] private float _beatSpeed = 2.0f; // 速さ
        [SerializeField] private float _beatScale = 0.15f; // 膨らむ大きさ

        void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        void Start()
        {
            // 文章の更新を待ってから開始
            StartCoroutine(AnimateVertex());
        }

        IEnumerator AnimateVertex()
        {
            while (true)
            {
                _textComponent.ForceMeshUpdate();
                TMP_TextInfo textInfo = _textComponent.textInfo;

                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                    // 表示されていない、または「文字（Character）」である場合はスキップ
                    // スプライトは Character ではないため、これで判定できます
                    if (!charInfo.isVisible || charInfo.elementType != TMP_TextElementType.Sprite)
                    {
                        // もし上記でもエラーが出る場合は、下の行を試してください
                        // if (!charInfo.isVisible || charInfo.character != 0) continue;
                        continue;
                    }

                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;
                    Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;

                    // 心拍の計算
                    float sin = Mathf.Sin(Time.time * _beatSpeed * Mathf.PI);
                    float pulse = Mathf.Pow(Mathf.Max(0, sin), 4) * _beatScale;

                    // 中心点の計算
                    Vector3 charCenter = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                    // 頂点配列への参照を取得
                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    for (int j = 0; j < 4; j++)
                    {
                        Vector3 origin = sourceVertices[vertexIndex + j];
                        Vector3 dist = origin - charCenter;
                        destinationVertices[vertexIndex + j] = origin + (dist * pulse);
                    }
                }

                // メッシュの更新
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    _textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return null;
            }
        }
    }
}