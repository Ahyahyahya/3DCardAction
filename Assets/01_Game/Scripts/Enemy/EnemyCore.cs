using R3;
using R3.Triggers;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyCore : MonoBehaviour, IDamageble
{
    // ---------- Field
    [SerializeField] private EnemyData _enemyData;
    public EnemyData EnemyData => _enemyData;

    private ReactiveProperty<int> _hp = new(100);
    public ReadOnlyReactiveProperty<int> Hp => _hp;

    private ReactiveProperty<bool> _canMoved = new(true);
    public ReadOnlyReactiveProperty<bool> CanMove => _canMoved;

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
    public void TakeDamage(int damage, Element element)
    {
        _hp.Value -= damage;

        Debug.Log($"[EnemyCore] ダメージ: {damage}   属性: {element.ToString()}");

        switch (element)
        {
            case Element.Fire:

                var burnDamage = Mathf.Ceil(damage * 0.01f);

                this.UpdateAsObservable()
                    .ThrottleLast(TimeSpan.FromSeconds(3f))
                    .Take(5)
                    .Subscribe(_ =>
                    {
                        Debug.Log($"[EnemyCore] {(int)burnDamage}火傷ダメージ");

                        _hp.Value -= (int)burnDamage;
                    });

                break;
            case Element.Ice:

                _canMoved.Value = false;

                this.UpdateAsObservable()
                    .Take(1)
                    .Delay(TimeSpan.FromSeconds(3f))
                    .Subscribe(_ => _canMoved.Value = true);

                break;
            case Element.Thunder:
                break;
            case Element.Wind:
                break;
            case Element.Poison:

                var poisonDamage = Mathf.Ceil(_enemyData.MaxHp * 0.01f);

                this.UpdateAsObservable()
                    .ThrottleLast(TimeSpan.FromSeconds(3f))
                    .Take(5)
                    .Subscribe(_ =>
                    {
                        Debug.Log($"[EnemyCore] {(int)poisonDamage}毒ダメージ");

                        _hp.Value -= (int)poisonDamage;
                    });

                break;
        }
    }
}
