using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>キャラクターの登場・退場を管理するクラス </summary>
public class ActorSequencer : MonoBehaviour
{
    [ElementNames(new string[] {"0.Left", "1.Center", "2.Right"})]
    [SerializeField, Tooltip("キャラクターイラストを表示するクラスの配列")]
    Actor[] _actors = default;

    private void Start()
    {
        Appearance(Characters.UnityChan, FaceSprites.Anger, ActorImagePosition.Center, 10f, () => Debug.Log("登場"));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _actors[(int)ActorImagePosition.Center].Skip();
        }
    }

    /// <summary>キャラクターを一人登場させる処理</summary>
    /// <param name="character">登場人物</param>
    /// <param name="sprite">表示差分</param>
    /// <param name="position">登場位置</param>
    /// <param name="appearanceTime">登場にかかる時間</param>
    /// <param name="endAction">終了処理</param>
    public void Appearance(Characters character, FaceSprites sprite, ActorImagePosition position, float appearanceTime, Action endAction = null)
    {
        var actor = _actors[(int)position];
        actor.SetFirstCharacterSprite(character, sprite);
        StartCoroutine(actor.FadeIn(appearanceTime, endAction));
    }
}

/// <summary>キャラクターの表示位置 </summary>
public enum ActorImagePosition
{
    Left = 0,
    Center = 1,
    Right = 2,
}
