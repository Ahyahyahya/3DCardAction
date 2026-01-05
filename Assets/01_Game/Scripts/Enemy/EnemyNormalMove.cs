using R3;
using R3.Triggers;
using UnityEngine;

public class EnemyNormalMove : MonoBehaviour
{
    [SerializeField] private EnemyCore _core;
    [SerializeField] private int _atk = 10;
    [SerializeField] private Element _attackElement;
    [SerializeField] private float _moveSpeed = 3f;

    private void Start()
    {
        var playerTr = PlayerDataProvider.Instance.transform;

        var gm = GameManager.Instance;

        this.UpdateAsObservable()
            .Where(_ => _core.CanMove.CurrentValue)
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .Subscribe(_ =>
            {
                // ƒvƒŒƒCƒ„[‚ð’Ç‚¤
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    playerTr.transform.localPosition,
                    _moveSpeed * Time.deltaTime);
            });

        this.OnCollisionEnterAsObservable()
            .Subscribe(collision =>
            {
                if (collision.gameObject.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.TakeDamage(_atk, _attackElement);
                }
            })
            .AddTo(this.gameObject);
    }
}
