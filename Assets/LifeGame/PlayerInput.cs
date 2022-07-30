using UnityEngine;

/// <summary>プレイヤーの入力を受け取り選択されたセルの状態を変更する </summary>
public class PlayerInput : MonoBehaviour
{
    void Update()
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (Input.GetButtonDown("Fire1") && hit)    //セルを選択する
        {
            var cell = hit.collider.GetComponent<LifeGameCell>();

            if (cell.State == LifeGameCellState.Live)
            {
                cell.State = LifeGameCellState.Die;
            }
            else
            {
                cell.State = LifeGameCellState.Live;
            }
        }
    }
}
