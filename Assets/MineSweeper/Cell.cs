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
    Image _cellCover = default;
    /// <summary>周囲の地雷数 </summary>
    int _mineCount = 0;
    int _row = 0;
    int _column = 0;
    bool _isOpen = false;
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
    public bool IsMine => CellState == CellState.Mine;
    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }
    public Image CellCover { get => _cellCover; set => _cellCover = value; }

    void Start()
    {
        OnCellStateChanged();
         _cellCover = transform.GetChild(1).GetComponent<Image>();
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
        _cellCover.enabled = false;
        _view.enabled = true;
        _isOpen = true;
        if (CellState == CellState.Mine)
        {
            return true;
        }

        return false;
    }

    /// <summary>自身のセルを開けてステータスがカウントでなければ自身のセルを返す </summary>
    public Cell GetNextOpenCell()
    {
        _cellCover.enabled = false;
        _view.enabled = true;
        _isOpen = true;

        if (_cellState == CellState.Count)
        {
            return null;
        }

        return this;
    }

    /// <summary>地雷が見えるようにする </summary>
    /// <param name="isView"></param>
    public void SetView(ViewMode mode)
    {
        _view.enabled = false;

        switch (mode)
        {
            case ViewMode.All:
                _view.enabled = true;
                break;
            case ViewMode.MineOnly:
                if (_cellState == CellState.Mine)
                {
                    _view.enabled = true;
                }
                break;
            case ViewMode.Game:
                if (_isOpen)
                {
                    _view.enabled = true;
                }
                break;
        }
    }

    /// <summary>状態をリセットする </summary>
    public void ResetState()
    {
        CellState = CellState.None;
        _mineCount = 0;
        _isOpen = false;
    }
}

/// <summary>セルの状態</summary>
public enum CellState
{
    None = 0, // 空セル
    Count = 1, //周囲に地雷がある状態
    Mine = -1, // 地雷
}
