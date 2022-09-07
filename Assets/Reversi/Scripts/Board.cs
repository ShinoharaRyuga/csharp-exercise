using UnityEngine;

public class Board : MonoBehaviour
{
    bool _onStone = false;
    bool _isPut = false;
    int _boardPosX = 0;
    int _boardPosZ = 0;

    public bool OnStone { get => _onStone; set => _onStone = value; }
    public int BoardPosX { get => _boardPosX; set => _boardPosX = value; }
    public int BoardPosZ { get => _boardPosZ; set => _boardPosZ = value; }
    public bool IsPut { get => _isPut; set => _isPut = value; }
}
