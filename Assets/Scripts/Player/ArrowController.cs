using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private Transform _pivotPoint;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _startAngle = 0f;
    [SerializeField] private float _maxAngle = 45f;
    [SerializeField] private float _rotationSpeed = 100f;

    private float _currentAngle;

    public void ChangeArrowVisibility(bool visibility) => _spriteRenderer.enabled = visibility;
    public Vector3 GetArrowLocalPosition() => transform.localPosition;
    public bool GetArrowDirection() => _rotationSpeed <= 0 ? true : false;

    public void ChangeStartAngle(PlayerPosition state)
    {
        StopAllCoroutines();
        _startAngle = (float)state;

        transform.RotateAround(_pivotPoint.position, Vector3.forward, _startAngle - transform.rotation.eulerAngles.z);
        StartCoroutine(Rotation());
    }

    private IEnumerator Rotation()
    {
        ChangeArrowVisibility(true);

        while (true)
        {
            transform.RotateAround(_pivotPoint.position, Vector3.forward, _rotationSpeed * Time.deltaTime);
            _currentAngle = transform.rotation.eulerAngles.z;

            if (_startAngle != 180f)
                _currentAngle = _currentAngle > 180f ? _currentAngle - 360f : _currentAngle;

            _currentAngle = Mathf.Clamp(_currentAngle, _startAngle - _maxAngle, _startAngle + _maxAngle);
            ChangeDirection();

            yield return null;
        }
    }

    private void ChangeDirection()
    {
        if (_currentAngle >= _startAngle + _maxAngle)
            _rotationSpeed = Mathf.Abs(_rotationSpeed) * -1f;

        else if (_currentAngle <= _startAngle - _maxAngle)
            _rotationSpeed = Mathf.Abs(_rotationSpeed);
    }
}