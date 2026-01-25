using R3;
using R3.Triggers;
using UnityEngine;

public abstract class BaseEnemyMove : MonoBehaviour
{
    protected Transform playerTransform;
    protected EnemyCore enemyCore;
    protected GameManager gameManager;

    private void Start()
    {
        playerTransform = PlayerDataProvider.Instance.transform;
        gameManager = GameManager.Instance;
        enemyCore = GetComponent<EnemyCore>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.LookAt(playerTransform.position);
            })
            .AddTo(this);


        Movement();
    }

    protected abstract void Movement();
}
