using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private CoinManager _coinManager;

    [SerializeField] private int _changingLevelEmeny = 10;
    private int _currentlevelEnemy = 0;

    void Start() => _coinManager.ChangeScoreEvent += UpdateScore;

    public void MoveOnPath()
    {
        foreach (Enemy enemy in _enemies) 
         enemy.MoveOnPath(); 
    }
    public void StopEnemy()
    {
        foreach (Enemy enemy in _enemies)
        { enemy.KillAnimation(); }
    }

    private void UpdateScore(int value)
    {
        int res = value / 10;
        if (res > _currentlevelEnemy)
        {
            _currentlevelEnemy = res;
            var newColor = Random.ColorHSV();
            foreach (Enemy enemy in _enemies)
                enemy.ChangeEnemy(res * 1.5f, newColor);
        }
    }
}
