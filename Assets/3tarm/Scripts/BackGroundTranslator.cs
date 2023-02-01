using UnityEngine;
using UnityEngine.UI;

/// <summary>�w�i�������s���N���X</summary>
public class BackGroundTranslator : MonoBehaviour
{
    /// <summary>�t�F�[�h���s���ׂ�color <see cref="_currentfrom"/> </summary>
    readonly Color CURRENT_BACKGROUND_COLOR = new Color(0, 0, 0, 0);
    /// <summary>�t�F�[�h���s���ׂ�color <see cref="_nextfrom"/> </summary>
    readonly Color NEXT_BACKGROUND_COLOR = new Color(1, 1, 1, 1);

    [SerializeField, Tooltip("�\�����̔w�i")]
    Image _currentImage = default;

    [SerializeField, Tooltip("�t�F�[�h�p�̃C���[�W")]
    Image _fadeImage = default;

    [SerializeField, Tooltip("�J�ڎ���")]
    float _duration = 1f;

    /// <summary>�o�ߎ��� </summary>
    float _elapsed = 0;
    /// <summary>Lerp�������s���Ă��� </summary>
    bool _isLerp = false;
    /// <summary>���ݕ\�����Ă���w�i�̐F </summary>
    Color _currentfrom;
    /// <summary>���ɕ\������w�i�̐F</summary>
    Color _nextfrom;

    public bool IsLerp { get => _isLerp; }

    void Update()
    {
        if (_fadeImage.sprite == null) { return; }

        _elapsed += Time.deltaTime;

        if (_elapsed < _duration)
        {
            _currentImage.color = Color.Lerp(_currentfrom, CURRENT_BACKGROUND_COLOR, _elapsed / _duration);
            _fadeImage.color = Color.Lerp(_nextfrom, NEXT_BACKGROUND_COLOR, _elapsed / _duration);
        }
        else if (_isLerp)
        {
            ResetProcess();
        }
    }

    /// <summary>�ēx�������s����悤�ɂ��� </summary>
    void ResetProcess()
    {
        _currentImage.sprite = _fadeImage.sprite;
        _fadeImage.sprite = null;                       //�����𑖂点�Ȃ��悤�ɂ����
        _fadeImage.color = CURRENT_BACKGROUND_COLOR;    //�����ɂ���
        _currentImage.color = NEXT_BACKGROUND_COLOR;
        _isLerp = false;
    }

    /// <summary>�w�i�����������J�n����</summary>
    /// <param name="backGround"></param>
    public void Play(Sprite backGround)
    {
        if (_fadeImage is null || _currentImage is null) { return; }

        _fadeImage.sprite = backGround;
        _currentfrom = _currentImage.color;
        _nextfrom = _fadeImage.color;
        _elapsed = 0;
        _isLerp = true;
    }

    public void Skip()
    {
        ResetProcess();
    }
}
