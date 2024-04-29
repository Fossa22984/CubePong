using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<Transform> _pathPoints;
    [SerializeField] private float _duration = 2f;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _timeScaleLimit =9f;
    private Sequence _sequences;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySfx(SfxType.Death);
        GameManager.Instance.SwitchStateTo(GameState.GameOverMenu);
    }

    public void KillAnimation()
    {
        _sequences.Kill();
    }

    public void ChangeEnemy(float TimeScale, Color color)
    {
        _sequences.timeScale =Mathf.Clamp(TimeScale,1f, _timeScaleLimit);
        _spriteRenderer.color = color;
    }


    public void MoveOnPath()
    {
        transform.DOScale(1f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(_pathPoints.First().transform.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _sequences = DOTween.Sequence();

                for (int i = 1; i < _pathPoints.Count; i++)
                {
                    _sequences.Append(transform.DOMove(_pathPoints[i].transform.position, _duration)).SetEase(Ease.Linear)
                        .Append(transform.DORotate(new Vector3(0, 0, _pathPoints[i].transform.rotation.eulerAngles.z), 0.5f));
                }

                _sequences.SetLoops(-1, LoopType.Yoyo);
            });
        });
    }
}