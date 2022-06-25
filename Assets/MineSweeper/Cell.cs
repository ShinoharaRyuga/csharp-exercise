using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] Text _view = null;
    [SerializeField] CellState _cellState = CellState.None;
    Image _cellImage => GetComponent<Image>();
    int _mineCount = 0;
    bool _isView = false;
    bool _isOpen = false;
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

    void Start()
    {
        OnCellStateChanged();
    }

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    void OnCellStateChanged()
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

    public void SetView(bool isView)
    {
        _view.enabled = isView;
    }
}



public enum CellState
{
    None = 0, // ‹óƒZƒ‹
    Count = 1, //üˆÍ‚É’n—‹‚ª‚ ‚éó‘Ô
    Mine = -1, // ’n—‹
}
