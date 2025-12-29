using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovelGame
{
    public class GameManager : MonoBehaviour
    {
        // 別のクラスからGameManagerの変数などを使えるようにするためのもの。（変更はできない）
        public static GameManager Instance { get; private set; }

        void Awake()
        {
            // これで、別のクラスからGameManagerの変数などを使えるようになる。
            Instance = this;
        }
    }
}
