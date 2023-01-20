using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTalkContent : MonoBehaviour
{
    [SerializeField] 
    TextAsset _textAsset = default;
    List<string> _talkContent = new List<string>();
    Dictionary<string, List<string>> _allTalkContent = new Dictionary<string, List<string>>();

    private void Awake()
    {
        CreateTalkContent();
    }

    /// <summary>CSVからテキストデータを読み込む </summary>
    void CreateTalkContent()
    {
        var sr = new StringReader(_textAsset.text);
        var name = sr.ReadLine();

        while (true)
        {
            var line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                Debug.Log("finish");
                break;
            }

            _talkContent.Add(line);
        }

        _allTalkContent.Add(name, _talkContent);
    }
}
