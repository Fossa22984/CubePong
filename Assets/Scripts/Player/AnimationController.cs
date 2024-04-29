using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private string _idleAnimation;
    [SerializeField] private string _jumpLeftAnimation;
    [SerializeField] private string _jumpRightAnimation;
    [SerializeField] private string _deathAnimation;

    public void SwitchToIdle()=> _animator.Play(_idleAnimation);
    public void SwitchToDeath() => _animator.Play(_deathAnimation);
    public void SwitchToJump(bool flip)
    {
        if (flip) _animator.Play(_jumpRightAnimation);
        else _animator.Play(_jumpLeftAnimation);
    }
}