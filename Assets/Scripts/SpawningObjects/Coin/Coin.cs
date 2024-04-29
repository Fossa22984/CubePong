using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action SpawnNewCoinEvent;
    public event Action<int> ChangeScoreEvent;

    [SerializeField] private TypeCoin _typeCoin;
    [SerializeField] private int _value;

    private Tween _animation;

    public bool CheckSubscription(Action action) =>
        SpawnNewCoinEvent != null && SpawnNewCoinEvent.GetInvocationList().Contains(action);

    public void PreparationBeforeSpawn(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _animation.Play();
    }

    private void Start() => StartAnimation();

    private void StartAnimation() =>
       _animation = transform.DOScale(0.5f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySfx(SfxType.GetCoin);
        PoolManager.PutObject(gameObject);

        _animation.Pause();
        gameObject.SetActive(false);

        ChangeScoreEvent?.Invoke(_value);
        SpawnNewCoinEvent?.Invoke();
    }

    ~Coin()
    {
        if (SpawnNewCoinEvent != null)
        {
            foreach (Delegate d in SpawnNewCoinEvent.GetInvocationList())
                SpawnNewCoinEvent -= (Action)d;
        }

        if (ChangeScoreEvent != null)
        {
            foreach (Delegate d in ChangeScoreEvent.GetInvocationList())
                ChangeScoreEvent -= (Action<int>)d;
        }
    }
}