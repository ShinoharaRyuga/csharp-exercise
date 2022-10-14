using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField, Header("�������o�͂���Ԋu")]
    float _interval = 1f;
    [SerializeField, Tooltip("�L�����N�^�[�̖��O")]
    string _characterName = "";
    [SerializeField, Tooltip("�\��������e")]
    string _talkContent = "";
    [SerializeField, Tooltip("���O��\������e�L�X�g")] 
    TMP_Text _nameText = default;
    [SerializeField, Tooltip("��b���e��\������e�L�X�g")] 
    TMP_Text _talkText = default;

    /// <summary>�o�͂��������̓Y���� </summary>
    int _currentIndex = 0;
    bool _isTalk = true;
    float _time = 0f;
    
    private void Start()
    {
        _nameText.text = _characterName;
    }

    private void Update()
    {
        if (_isTalk && _currentIndex < _talkContent.Length)
        {
            _time += Time.deltaTime;

            if (_interval <= _time)
            {
                _talkText.text += _talkContent[_currentIndex].ToString();
                _time = 0f;
                _currentIndex++;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _talkText.text = _talkContent;
            _isTalk = false;
        }
    }
}
