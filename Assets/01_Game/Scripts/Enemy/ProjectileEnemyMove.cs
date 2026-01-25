using System;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using System.Threading;
using UnityEngine;

public class ProjectileEnemyMove : BaseEnemyMove
{
    [SerializeField] private CardData _cardData;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _castTime = 3f;
    [SerializeField] private float _castInterval = 1f;

    private bool _isCasting = false;

    protected override void Movement()
    {
        this.UpdateAsObservable()
            .Where(_ => !_isCasting)
            .Where(_ => enemyCore.CanMove.CurrentValue)
            .Where(_ => gameManager.State.CurrentValue == GameState.BATTLE)
            .Subscribe(async _ =>
            {
                var playerDist = Vector3.Distance(
                    transform.position,
                    playerTransform.position);

                if (playerDist < _attackRange)
                {
                    var castTask = Cast(this.destroyCancellationToken);
                    if (await castTask.SuppressCancellationThrow()) return;
                }
                else
                {
                    // プレイヤーを追う
                    transform.localPosition = Vector3.MoveTowards(
                        transform.position,
                        playerTransform.position,
                        enemyCore.EnemyData.MoveSpeed * Time.deltaTime);
                }
            });
    }

    private async UniTask Cast(CancellationToken ct)
    {
        Debug.Log("キャスト開始");

        _isCasting = true;

        await UniTask.WaitForSeconds(
            _castTime,
            cancellationToken: ct);

        Debug.Log("キャスト終了");

        _cardData.Activate(transform);

        await UniTask.WaitForSeconds(
            _castInterval,
            cancellationToken: ct);

        _isCasting = false;

        Debug.Log("キャストインターバル終了");
    }
}
