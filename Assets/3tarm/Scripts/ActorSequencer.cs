using UnityEngine;

/// <summary>�L�����N�^�[�̓o��E�ޏ���Ǘ�����N���X </summary>
public class ActorSequencer : MonoBehaviour
{
    [ElementNames(new string[] { "0.Left", "1.Center", "2.Right" })]
    [SerializeField, Tooltip("�L�����N�^�[�C���X�g��\������N���X�̔z��")]
    Actor[] _actors = default;

    /// <summary>���݂̕\�����Ă���\��� </summary>
    int _currentFaceIndex = 0;


    private void Update()
    {
        if (Input.GetButtonDown("Jump")) //�X�L�b�v
        {
            _actors[(int)ActorImagePosition.Center].Skip();
            _actors[(int)ActorImagePosition.Left].Skip();
            _actors[(int)ActorImagePosition.Right].Skip();
        }

        if (Input.GetKeyDown(KeyCode.A))    //�L�����N�^�[�o��
        {
            OneAppearance(Characters.UnityChan, FaceSprites.Anger, ActorImagePosition.Left, 3f);
            OneAppearance(Characters.Yuko, FaceSprites.Anger, ActorImagePosition.Center, 3f);
            OneAppearance(Characters.Misaki, FaceSprites.Anger, ActorImagePosition.Right, 3f);
        }

        if (Input.GetKeyDown(KeyCode.E))    //�L�����N�^�[�ޏ�
        {
            OneExit(ActorImagePosition.Left, 3f);
            OneExit(ActorImagePosition.Center, 3f);
            OneExit(ActorImagePosition.Right, 3f);
        }

        if (Input.GetKeyDown(KeyCode.F))    //�\��ύX
        {
            _currentFaceIndex++;
            _currentFaceIndex = _currentFaceIndex % _actors.Length;
            _actors[(int)ActorImagePosition.Center].UpdateFaceSprite((FaceSprites)_currentFaceIndex);
            _actors[(int)ActorImagePosition.Left].UpdateFaceSprite((FaceSprites)_currentFaceIndex);
            _actors[(int)ActorImagePosition.Right].UpdateFaceSprite((FaceSprites)_currentFaceIndex);
        }
    }

    /// <summary>�L�����N�^�[����l�o�ꂳ���鏈��</summary>
    /// <param name="character">�o��l��</param>
    /// <param name="sprite">�\������</param>
    /// <param name="position">�o��ʒu</param>
    /// <param name="appearanceTime">�o��ɂ����鎞��</param>
    public void OneAppearance(Characters character, FaceSprites sprite, ActorImagePosition position, float appearanceTime)
    {
        var actor = _actors[(int)position];
        actor.SetFirstCharacterSprite(character, sprite);
        StartCoroutine(actor.FadeRun(FadeMode.FadeIn, appearanceTime));
    }

    /// <summary>�L�����N�^�[����l�ޏꂳ���鏈��</summary>
    /// <param name="position">�ޏꂳ����ʒu</param>
    /// <param name="exitTime">�ޏ�ɂ����鎞��</param>
    public void OneExit(ActorImagePosition position, float exitTime)
    {
        StartCoroutine(_actors[(int)position].FadeRun(FadeMode.FadeOut, exitTime));
    }

    /// <summary>�����̃L�����N�^�[��o�ꂳ����</summary>
    public void MultipleAppearances()
    {
        if (_actors[1].FadeResult)
        {
            Debug.Log("End");
        }
    }
}

/// <summary>�L�����N�^�[��o�ꂳ����ׂ̃p�����[�^�[ </summary>
public struct AppearanceInfo
{
    /// <summary>�o��l�� </summary>
    public Characters _character;
    /// <summary>�\�� </summary>
    public FaceSprites _face;
    /// <summary>�o��ʒu </summary>
    public ActorImagePosition _position;
    /// <summary>�o��ɂ����鎞��</summary>
    public float _appearanceTime;

    public AppearanceInfo(Characters character, FaceSprites face, ActorImagePosition position, float appearnceTime)
    {
        _character = character;
        _face = face;
        _position = position;
        _appearanceTime = appearnceTime;
    }
}

/// <summary>�L�����N�^�[�̕\���ʒu </summary>
public enum ActorImagePosition
{
    Left = 0,
    Center = 1,
    Right = 2,
}

/// <summary>�����l�o��̏ꍇ�̓o����@</summary>
public enum MultipleAppearanceMode
{
    /// <summary>���Ԃɓo�ꂳ���� </summary>
    Order,
    /// <summary>�����ɓo�ꂳ���� </summary>
    Together
}
