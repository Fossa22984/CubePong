using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    private string _fpsFormat;
    private float _deltaTime = 0.0f;
    private float _msec = 0.0f;
    private float _fps = 0.0f;

    private void Awake() => _fpsFormat = _fpsText.text;

    private void Update() => _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;

    private void OnGUI()
    {
        _msec = _deltaTime * 1000.0f;
        _fps = 1.0f / _deltaTime;
        _fpsText.text = string.Format(_fpsFormat, _msec, _fps);
    }
}