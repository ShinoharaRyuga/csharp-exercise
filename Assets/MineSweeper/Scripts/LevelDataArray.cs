using UnityEngine;

/// <summary>��Փx�f�[�^�̓Y������ </summary>
public class LevelDataArray : PropertyAttribute
{
    public readonly string[] _indexName;

    public LevelDataArray(string[] indexName)
    {
        _indexName = indexName;
    }
}
