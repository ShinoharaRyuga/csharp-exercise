using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] StoneColor _currentState = StoneColor.Black;
    /// <summary>自身が置かれたマス目 </summary>
    Board _putBoard = null;

    public StoneColor CurrentState { get => _currentState; set => _currentState = value; }
    public Board PutBoard { get => _putBoard; set => _putBoard = value; }

    void ChangeStoneState()
    {
        if (_currentState == StoneColor.White)
        {
            transform.Rotate(180, 0, 0);
        }
        else if (_currentState == StoneColor.Black)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}

public enum StoneColor
{
    Black,
    White,
}
