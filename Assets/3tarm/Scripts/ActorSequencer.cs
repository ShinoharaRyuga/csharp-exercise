using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�L�����N�^�[�̓o��E�ޏ���Ǘ�����N���X </summary>
public class ActorSequencer : MonoBehaviour
{
    [ElementNames(new string[] {"0.Left", "1.Center", "2.Right"})]
    [SerializeField, Tooltip("�L�����N�^�[�C���X�g��\������N���X�̔z��")]
    Actor[] _actors = default;

    private void Start()
    {
        Appearance(Characters.UnityChan, FaceSprites.Anger, ActorImagePosition.Center, 10f, () => Debug.Log("�o��"));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _actors[(int)ActorImagePosition.Center].Skip();
        }
    }

    /// <summary>�L�����N�^�[����l�o�ꂳ���鏈��</summary>
    /// <param name="character">�o��l��</param>
    /// <param name="sprite">�\������</param>
    /// <param name="position">�o��ʒu</param>
    /// <param name="appearanceTime">�o��ɂ����鎞��</param>
    /// <param name="endAction">�I������</param>
    public void Appearance(Characters character, FaceSprites sprite, ActorImagePosition position, float appearanceTime, Action endAction = null)
    {
        var actor = _actors[(int)position];
        actor.SetFirstCharacterSprite(character, sprite);
        StartCoroutine(actor.FadeIn(appearanceTime, endAction));
    }
}

/// <summary>�L�����N�^�[�̕\���ʒu </summary>
public enum ActorImagePosition
{
    Left = 0,
    Center = 1,
    Right = 2,
}
