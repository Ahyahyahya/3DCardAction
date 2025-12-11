using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class EnemyManager : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private List<EnemyCore> _enemies = new();

    // ---------- UnityMessage
    private void Start()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Hp
                .Where(value => value <= 0)
                .Subscribe(_ =>
                {
                    _enemies.Remove(enemy);

                    if (_enemies.Count > 0) return;

                    if (GameManager.Instance.ClearCnt < 2)
                    {
                        GameManager.Instance.ChangeGameState(GameState.CLEAR);
                    }
                    else
                    {
                        GameManager.Instance.ChangeGameState(GameState.RESULT);
                    }
                })
                .AddTo(this);
        }
    }

    // ---------- Method
    [ContextMenu("FindEnemies")]
    private void FindEnemiesInScene()
    {
        var enemies = FindObjectsByType<EnemyCore>(FindObjectsSortMode.None);

        foreach(var enemy in enemies)
        {
            _enemies.Add(enemy);
        }
    }
}
