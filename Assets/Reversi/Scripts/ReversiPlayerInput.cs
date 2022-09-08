using UnityEngine;

public class ReversiPlayerInput : MonoBehaviour
{
    const float GENERATE_POS_Y = 0.16f;
    ReversiGameManager _gameManager => GetComponent<ReversiGameManager>();

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.TryGetComponent(out Board board))
            {
                if (!board.OnStone && board.IsPut)
                {
                    var generatePos = new Vector3(board.transform.position.x, GENERATE_POS_Y, board.transform.position.z);
                    _gameManager.StoneGenerator(generatePos, board);
                    board.OnStone = true;
                }
            }
        }

        if (Input.GetButtonDown("Fire2") && Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.TryGetComponent(out Board board))
            {
                Debug.Log($"Put {board.IsPut}");
                Debug.Log($"OnStone{board.OnStone}");
                Debug.Log($"X{board.BoardPosX}");
                Debug.Log($"Z{board.BoardPosZ}");
            }

            if (hit.collider.gameObject.TryGetComponent(out Stone stone))
            {
                Debug.Log($"color {stone.CurrentState}");
            }
        }
    }
}
