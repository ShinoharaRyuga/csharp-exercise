using UnityEngine;
using UnityEngine.UI;

/// <summary> ライフゲームで使用するセルを管理するクラス </summary>
public class LifeGameCell : MonoBehaviour
{
    [SerializeField, Tooltip("現在の状態")] LifeGameCellState _currnetState = LifeGameCellState.Dead;
    int _row = 0;
    int _col = 0;
    /// <summary>次世代のセル状態</summary>
    LifeGameCellState _nextState = LifeGameCellState.Dead;
    Image _cellImage => GetComponent<Image>();

    public LifeGameCellState State 
    {
        get { return _currnetState; }
        set
        {
            _currnetState = value;
            ChangeColor();
        }
    }

    /// <summary>セルが生きている </summary>
    public bool IsLive => State == LifeGameCellState.Alive;
    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }
    public LifeGameCellState NextState { get => _nextState; set => _nextState = value; }

    private void OnValidate()
    {
        ChangeColor();
    }

    /// <summary>次世代のセル状態を決める </summary>
    /// <param name="aliveCount">周囲八マスで生きているセルの数</param>
    public void SetNextCellState(int aliveCount)
    {
        if (!IsLive)   
        {
            if (aliveCount == 3) //誕生
            {
                _nextState = LifeGameCellState.Alive;
            }
        }
        else
        {
            if (aliveCount == 2 || aliveCount == 3) //生存
            {
                _nextState= LifeGameCellState.Alive;
            }

            if (aliveCount <= 1)   //過疎
            {
                _nextState = LifeGameCellState.Dead;
            }

            if (4 <= aliveCount)   //過密
            {
                _nextState = LifeGameCellState.Dead;
            }
        }
    }

    /// <summary>現在のセル状態を次世代のセル状態にする </summary>
    public void ChangeStep()
    {
        _currnetState = _nextState;
        ChangeColor();
    }

    /// <summary>状態によって色を変える</summary>
    void ChangeColor()
    {
        switch(_currnetState)
        {
            case LifeGameCellState.Alive:
                _cellImage.color = Color.green;
                break;
            case LifeGameCellState.Dead:
                _cellImage.color = Color.white;
                break;
        }
    }
}

public enum LifeGameCellState
{
    Alive,
    Dead
}
