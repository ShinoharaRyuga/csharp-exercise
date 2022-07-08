using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// セルの状態を管理する　
/// 自身のセルを開ける 
/// </summary>
public class Cell : MonoBehaviour
{
    [SerializeField, Tooltip("地雷の数を表示するテキスト")] Text _view = null;
    [SerializeField, Tooltip("セルの状態")] CellState _cellState = CellState.None;
    Image _cellImage => GetComponent<Image>();
    /// <summary>周囲の地雷数 </summary>
    int _mineCount = 0;
    int _row = 0;
    int _column = 0;
    bool _isView = false;
    bool _isOpen = false;
    bool _isMine = false;
    /// <summary>自身に旗が立っているかどうか </summary>
    bool _isFlag = false;

    public CellState CellState
    {
        get => _cellState;
        set
        {
            _cellState = value;
            OnCellStateChanged();
        }
    }

    
    public int MineCount { get => _mineCount; set => _mineCount = value; }
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public bool IsFlag { get => _isFlag; set => _isFlag = value; }
    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }

    void Start()
    {
        OnCellStateChanged();
    }

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    /// <summary>セルの状態によって色を変更する </summary>
    public void OnCellStateChanged()
    {
        if (_view == null) return;

        if (_cellState == CellState.Mine)
        {
            _view.text = "M";
            _view.color = Color.red;
        }
        else if (_cellState == CellState.None)
        {
            _view.text = "";
        }
        else
        {
            _view.text = _mineCount.ToString();
            _view.color = Color.blue;
        }
    }

    /// <summary>自身のセルを開ける </summary>
    /// <returns>true=自身が地雷 false=地雷ではない</returns>
    public bool OpenCell()
    {
        _cellImage.enabled = false;
        _view.enabled = true;

        if (CellState == CellState.Mine)
        {
            return true;
        }

        _isOpen = true;

        return false;
    }

    public void OpenCell(Cell cell)
    {
        cell._cellImage.enabled = false;
        cell.enabled = true;
        _isOpen = true;
    }

    /// <summary>地雷が見えるようにする </summary>
    /// <param name="isView"></param>
    public void SetView(bool isView)
    {
        _view.enabled = isView;
    }

    /// <summary>状態をリセットする </summary>
    public void ResetState()
    {
        CellState = CellState.None;
        _mineCount = 0;
    }
}

/// <summary>セルの状態</summary>
public enum CellState
{
    None = 0, // 空セル
    Count = 1, //周囲に地雷がある状態
    Mine = -1, // 地雷
}
