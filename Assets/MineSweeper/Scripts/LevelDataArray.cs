using UnityEngine;

/// <summary>難易度データの添え字名 </summary>
public class LevelDataArray : PropertyAttribute
{
    public readonly string[] _indexName;

    public LevelDataArray(string[] indexName)
    {
        _indexName = indexName;
    }
}
