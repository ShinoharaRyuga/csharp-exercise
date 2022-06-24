using UnityEngine;

/// <summary>縦、横の位置と自分の色の情報を持っている </summary>
public class CellInformation : MonoBehaviour
{
    int _row = 0;
    int _column = 0;
    /// <summary>true=白　false=黒 </summary>
    bool _color = true;
    public int Row { get => _row; }
    public int Column { get => _column; }
    public bool Color { get => _color; set => _color = value; }

    public void SetPosition(int row, int column)
    {
        _row = row;
        _column = column;
    }
}
