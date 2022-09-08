using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] BoardState _currentState = BoardState.None;
    bool _onStone = false;
    bool _isPut = false;
    int _boardPosX = 0;
    int _boardPosZ = 0;
    List<Stone> _changeStones = new List<Stone>();

    public bool OnStone { get => _onStone; set => _onStone = value; }
    public int BoardPosX { get => _boardPosX; set => _boardPosX = value; }
    public int BoardPosZ { get => _boardPosZ; set => _boardPosZ = value; }
    public bool IsPut { get => _isPut; set => _isPut = value; }
    public BoardState CurrentState { get => _currentState; set => _currentState = value; }
    public List<Stone> ChangeStones { get => _changeStones; set => _changeStones = value; }
}

public enum BoardState
{
    None = 0,
    Black = 1,
    White = 2,
}
