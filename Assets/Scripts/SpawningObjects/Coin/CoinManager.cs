using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public event Action<int> ChangeScoreEvent;

    [SerializeField] private List<Coin> _coins;
    [SerializeField] private List<float> _spawnWeights;
    [SerializeField] private BoxCollider2D _spawnArea;

    private int _score = 0;

    public void SpawnRandomCoin()
    {
        var randomValue = UnityEngine.Random.Range(0f, _spawnWeights.Sum());
        var weightSum = 0f;
        for (int i = 0; i < _coins.Count; i++)
        {
            weightSum += _spawnWeights[i];
            if (randomValue <= weightSum)
            {
                var coinFromPool = PoolManager.GetObject(_coins[i].gameObject);
                var coin = coinFromPool.GetComponent<Coin>();

                if (!coin.CheckSubscription(SpawnRandomCoin))
                {
                    coin.SpawnNewCoinEvent += SpawnRandomCoin;
                    coin.ChangeScoreEvent += UpdateScore;
                }

                coin.PreparationBeforeSpawn(GetRandomPointInCollider(_spawnArea));
                break;
            }
        }
    }

    private void UpdateScore(int value)
    {
        _score += value;
        ChangeScoreEvent?.Invoke(_score);
    }

    private Vector2 GetRandomPointInCollider(BoxCollider2D boxCollider)
    {
        return new Vector2(UnityEngine.Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
            UnityEngine.Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y));
    }
}