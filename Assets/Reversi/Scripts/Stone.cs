using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] BoardState _currentState = BoardState.Black;
    /// <summary>自身が置かれたマス目 </summary>
    Board _putBoard = null;

    public BoardState CurrentState { get => _currentState; set => _currentState = value; }
    public Board PutBoard { get => _putBoard; set => _putBoard = value; }

    public void ChangeStoneState()
    {
        if (_currentState == BoardState.White)
        {
            transform.Rotate(180, 0, 0);
        }
        else if (_currentState == BoardState.Black)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}

