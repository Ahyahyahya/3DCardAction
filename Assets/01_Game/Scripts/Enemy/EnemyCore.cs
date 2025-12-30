using R3;
using UnityEngine;

public class EnemyCore : MonoBehaviour, IDamageble
{
    // ---------- Field
    [SerializeField] private EnemyData _enemyData;
    public EnemyData EnemyData => _enemyData;

    private ReactiveProperty<int> _hp = new(100);
    public ReadOnlyReactiveProperty<int> Hp => _hp;

    // ---------- UnityMessage
    private void Start()
    {
        // ステータスの初期化
        _hp.Value = _enemyData.MaxHp;

        _hp
            .Where(value => value <= 0)
            .Subscribe(_ =>
            {
                Destroy(gameObject);
            })
            .AddTo(this);
    }

    // ---------- Interface
    public void TakeDamage(int damage)
    {
        _hp.Value -= damage;
    }
}
