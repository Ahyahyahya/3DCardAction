using R3;
using R3.Triggers;
using UnityEngine;

public class EnemyNormalMove : MonoBehaviour
{
    [SerializeField] private int _atk = 10;
    [SerializeField] private float _moveSpeed = 3f;

    private void Start()
    {
        var playerTr = PlayerDataProvider.Instance.transform;

        this.UpdateAsObservable()
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
                    damageble.TakeDamage(_atk);
                }
            })
            .AddTo(this.gameObject);
    }
}
