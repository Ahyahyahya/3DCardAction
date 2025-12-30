using ObservableCollections;
using R3;
using TMPro;
using UnityEngine;

public class DamageDispacher : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private EnemyManager _enemyManager;

    [SerializeField] private GamePresenter _gamePresenter;

    [SerializeField] private TextMeshProUGUI _damageTMP;

    // ---------- UnityMessage
    private void Start()
    {
        _enemyManager.Enemies.ObserveAdd()
            .Subscribe(enemy =>
            {
                enemy.Value.Hp
                .Pairwise()
                .Where(hps => hps.Current < hps.Previous)
                .Subscribe(hps =>
                {
                    var damage = hps.Previous - hps.Current;

                    Dispatch(
                        enemy.Value.transform.position - Camera.main.transform.forward * 2f,
                        damage);
                })
                .AddTo(this);
            })
            .AddTo(this);
    }

    // ---------- Method
    public void Dispatch(Vector3 pos, int damage)
    {
        var clone = Instantiate(
            _damageTMP,
            pos,
            Quaternion.identity);

        _gamePresenter.CreateDamage(clone, damage);
    }
}
