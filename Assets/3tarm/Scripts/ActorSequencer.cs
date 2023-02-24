using UnityEngine;

/// <summary>キャラクターの登場・退場を管理するクラス </summary>
public class ActorSequencer : MonoBehaviour
{
    [ElementNames(new string[] { "0.Left", "1.Center", "2.Right" })]
    [SerializeField, Tooltip("キャラクターイラストを表示するクラスの配列")]
    Actor[] _actors = default;

    /// <summary>現在の表示している表情差分 </summary>
    int _currentFaceIndex = 0;


    private void Update()
    {
        if (Input.GetButtonDown("Jump")) //スキップ
        {
            _actors[(int)ActorImagePosition.Center].Skip();
            _actors[(int)ActorImagePosition.Left].Skip();
            _actors[(int)ActorImagePosition.Right].Skip();
        }

        if (Input.GetKeyDown(KeyCode.A))    //キャラクター登場
        {
            OneAppearance(Characters.UnityChan, FaceSprites.Anger, ActorImagePosition.Left, 3f);
            OneAppearance(Characters.Yuko, FaceSprites.Anger, ActorImagePosition.Center, 3f);
            OneAppearance(Characters.Misaki, FaceSprites.Anger, ActorImagePosition.Right, 3f);
        }

        if (Input.GetKeyDown(KeyCode.E))    //キャラクター退場
        {
            OneExit(ActorImagePosition.Left, 3f);
            OneExit(ActorImagePosition.Center, 3f);
            OneExit(ActorImagePosition.Right, 3f);
        }

        if (Input.GetKeyDown(KeyCode.F))    //表情変更
        {
            _currentFaceIndex++;
            _currentFaceIndex = _currentFaceIndex % _actors.Length;
            _actors[(int)ActorImagePosition.Center].UpdateFaceSprite((FaceSprites)_currentFaceIndex);
            _actors[(int)ActorImagePosition.Left].UpdateFaceSprite((FaceSprites)_currentFaceIndex);
            _actors[(int)ActorImagePosition.Right].UpdateFaceSprite((FaceSprites)_currentFaceIndex);
        }
    }

    /// <summary>キャラクターを一人登場させる処理</summary>
    /// <param name="character">登場人物</param>
    /// <param name="sprite">表示差分</param>
    /// <param name="position">登場位置</param>
    /// <param name="appearanceTime">登場にかかる時間</param>
    public void OneAppearance(Characters character, FaceSprites sprite, ActorImagePosition position, float appearanceTime)
    {
        var actor = _actors[(int)position];
        actor.SetFirstCharacterSprite(character, sprite);
        StartCoroutine(actor.FadeRun(FadeMode.FadeIn, appearanceTime));
    }

    /// <summary>キャラクターを一人退場させる処理</summary>
    /// <param name="position">退場させる位置</param>
    /// <param name="exitTime">退場にかかる時間</param>
    public void OneExit(ActorImagePosition position, float exitTime)
    {
        StartCoroutine(_actors[(int)position].FadeRun(FadeMode.FadeOut, exitTime));
    }

    /// <summary>複数のキャラクターを登場させる</summary>
    public void MultipleAppearances()
    {
        if (_actors[1].FadeResult)
        {
            Debug.Log("End");
        }
    }
}

/// <summary>キャラクターを登場させる為のパラメーター </summary>
public struct AppearanceInfo
{
    /// <summary>登場人物 </summary>
    public Characters _character;
    /// <summary>表情 </summary>
    public FaceSprites _face;
    /// <summary>登場位置 </summary>
    public ActorImagePosition _position;
    /// <summary>登場にかかる時間</summary>
    public float _appearanceTime;

    public AppearanceInfo(Characters character, FaceSprites face, ActorImagePosition position, float appearnceTime)
    {
        _character = character;
        _face = face;
        _position = position;
        _appearanceTime = appearnceTime;
    }
}

/// <summary>キャラクターの表示位置 </summary>
public enum ActorImagePosition
{
    Left = 0,
    Center = 1,
    Right = 2,
}

/// <summary>複数人登場の場合の登場方法</summary>
public enum MultipleAppearanceMode
{
    /// <summary>順番に登場させる </summary>
    Order,
    /// <summary>同時に登場させる </summary>
    Together
}
