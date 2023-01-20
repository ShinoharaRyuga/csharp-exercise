using TMPro;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textUi = default;

    [SerializeField]
    private string _message = "";

    [SerializeField]
    private float _speed = 1.0f;

    private float _elapsed = 0; // ������\�����Ă���̌o�ߎ���
    private float _interval; // �������̑҂�����

    // _message �t�B�[���h����\�����錻�݂̕����C���f�b�N�X�B
    // �����w���Ă��Ȃ��ꍇ�� -1 �Ƃ���B
    private int _currentIndex = -1;

    public bool IsPrinting
    {
        get
        {
            if (_message is null or { Length: 0 })
            {
                return false;
            }
            return _currentIndex < _message.Length - 1;
        }
    }


    private void Start()
    {
        ShowMessage(_message);
    }

    private void Update()
    {
        if (_textUi is null || _message is null || _currentIndex + 1 >= _message.Length) { return; }

        _elapsed += Time.deltaTime;
        if (_elapsed > _interval)
        {
            _elapsed = 0;
            _currentIndex++;
            _textUi.text += _message[_currentIndex];
        }
    }

    /// <summary>
    /// �w��̃��b�Z�[�W��\������B
    /// </summary>
    /// <param name="message">�e�L�X�g�Ƃ��ĕ\�����郁�b�Z�[�W�B</param>
    public void ShowMessage(string message)
    {
        if (_textUi is null) { return; }
        _textUi.text = "";

        _currentIndex = -1;
        _message = message;

        _elapsed = 0;
        if (_message is null or { Length: 0 }) { _interval = 0; }
        else { _interval = _speed / _message.Length; }
    }

    public void Skip()
    {
        if (_message is null) { return; }

        _currentIndex = _message.Length;
        _textUi.text = _message;
    }
}