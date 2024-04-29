using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private Transform _pivot;

    private Coroutine _rotateAroundPivotCoroutine;

    public void SwitchToJump(bool direction = true)
    {
        _rotateAroundPivotCoroutine = StartCoroutine(RotateAroundPivot());
        _animationController.SwitchToJump(direction);
    }

    public void SwitchToIdle(PlayerPosition position)
    {
        StopCoroutine(_rotateAroundPivotCoroutine);
        transform.DORotate(new Vector3(0, 0, (float)position), 1f * Time.deltaTime);
        _animationController.SwitchToIdle();
    }

    public void SwitchToDeath()
    {   
        StopCoroutine(_rotateAroundPivotCoroutine);
        transform.rotation= Quaternion.identity;
   
        _animationController.SwitchToDeath();
    }

    private IEnumerator RotateAroundPivot()
    {
        Vector3 _directionToPivot;
        while (true)
        {
            _directionToPivot = _pivot.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, _directionToPivot.normalized);
            yield return null;
        }
    }
}