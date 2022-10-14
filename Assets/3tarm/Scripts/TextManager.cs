using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField, Header("文字を出力する間隔")]
    float _interval = 1f;
    [SerializeField, Tooltip("キャラクターの名前")]
    string _characterName = "";
    [SerializeField, Tooltip("表示する内容")]
    string _talkContent = "";
    [SerializeField, Tooltip("名前を表示するテキスト")] 
    TMP_Text _nameText = default;
    [SerializeField, Tooltip("会話内容を表示するテキスト")] 
    TMP_Text _talkText = default;

    /// <summary>出力した文字の添え字 </summary>
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
