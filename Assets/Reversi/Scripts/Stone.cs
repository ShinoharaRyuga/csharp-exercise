using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] StoneColor _currentState = StoneColor.Black;

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
