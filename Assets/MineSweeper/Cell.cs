using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Z���̏�Ԃ��Ǘ�����@
/// ���g�̃Z�����J���� 
/// </summary>
public class Cell : MonoBehaviour
{
    [SerializeField, Tooltip("�n���̐���\������e�L�X�g")] Text _view = null;
    [SerializeField, Tooltip("�Z���̏��")] CellState _cellState = CellState.None;
    Image _cellCover = default;
    /// <summary>���͂̒n���� </summary>
    int _mineCount = 0;
    int _row = 0;
    int _column = 0;
    bool _isOpen = false;
    /// <summary>���g�Ɋ��������Ă��邩�ǂ��� </summary>
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

    /// <summary>�Z���̏�Ԃɂ���ĐF��ύX���� </summary>
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

    /// <summary>���g�̃Z�����J���� </summary>
    /// <returns>true=���g���n�� false=�n���ł͂Ȃ�</returns>
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

    /// <summary>���g�̃Z�����J���ăX�e�[�^�X���J�E���g�łȂ���Ύ��g�̃Z����Ԃ� </summary>
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

    /// <summary>�n����������悤�ɂ��� </summary>
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

    /// <summary>��Ԃ����Z�b�g���� </summary>
    public void ResetState()
    {
        CellState = CellState.None;
        _mineCount = 0;
        _isOpen = false;
    }
}

/// <summary>�Z���̏��</summary>
public enum CellState
{
    None = 0, // ��Z��
    Count = 1, //���͂ɒn����������
    Mine = -1, // �n��
}
