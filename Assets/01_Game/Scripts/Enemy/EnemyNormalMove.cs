using R3;
using R3.Triggers;
using UnityEngine;

[RequireComponent(typeof(EnemyCore))]
public class EnemyNormalMove : BaseEnemyMove
{
    protected override void Movement()
    {
        this.UpdateAsObservable()
            .Where(_ => enemyCore.CanMove.CurrentValue)
            .Where(_ => gameManager.State.CurrentValue == GameState.BATTLE)
            .Subscribe(_ =>
            {
                // ƒvƒŒƒCƒ„[‚ð’Ç‚¤
                transform.localPosition = Vector3.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    enemyCore.EnemyData.MoveSpeed * Time.deltaTime);
            });

        this.OnCollisionEnterAsObservable()
            .Subscribe(collision =>
            {
                if (collision.gameObject.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.TakeDamage(
                        enemyCore.EnemyData.Atk,
                        enemyCore.EnemyData.AttackElement);
                }
            })
            .AddTo(this.gameObject);
    }
}
