using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using R3;
using ObservableCollections;

public class EnemyManager : MonoBehaviour
{
    // ---------- Singleton
    public static EnemyManager Instance;

    // ---------- Field
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private PlayerDataProvider _playerData;

    private ObservableList<EnemyCore> _enemies = new();

    public ObservableList<EnemyCore> Enemies => _enemies;

    // ---------- UnityMessage
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // 敵のHPが0になったらリストから削除
        _enemies.ObserveAdd()
            .Subscribe(enemy =>
            {
                enemy.Value.Hp
                .Where(value => value <= 0)
                .Subscribe(value => _enemies.Remove(enemy.Value))
                .AddTo(this);
            })
            .AddTo(this);

        // 敵が全滅したら現在のノードタイプによってステート変更
        _enemies.ObserveRemove()
            .Where(_ => _enemies.Count == 0)
            .Subscribe(_ =>
            {
                if (_playerData.CurrentNode.CurrentValue.type == NodeType.Boss)
                {
                    _gameManager.ChangeGameState(GameState.RESULT);
                }
                else
                {
                    _gameManager.ChangeGameState(GameState.CLEAR);
                }
            })
            .AddTo(this);
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

    public void AddEnemy(EnemyCore enemy) => _enemies.Add(enemy);
}
