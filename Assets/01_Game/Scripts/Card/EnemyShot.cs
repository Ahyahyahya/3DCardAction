using R3;
using R3.Triggers;
using UnityEngine;

public class EnemyShot : BaseCardEffect
{
    // ---------- Field
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _hit;
    [SerializeField] private GameObject _trail;

    private const float _createPosOffset = 3.0f;

    // ---------- Method
    public override void ActivateCardEffect()
    {

    }

    public override void ActivateCardEffect(Transform tr)
    {
        var camera = Camera.main;

        var rb = GetComponent<Rigidbody>();

        var core = GetComponent<CardEffectCore>();

        // 発射位置調整
        transform.localPosition =
            tr.position + tr.forward * _createPosOffset + tr.up * 1.5f;

        // 発射角度調整
        transform.localRotation = tr.localRotation;

        if (_muzzle != null)
        {
            // 生成エフェクト
            Instantiate(
            _muzzle,
            transform.localPosition,
            transform.localRotation);
        }

        if (_trail != null)
        {
            // 発射物のトレイル
            var trail = Instantiate(
                _trail,
                transform.localPosition,
                transform.localRotation,
                transform);
        }


        this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                rb.AddForce(transform.forward * core.CardData.MoveSpeed);
            })
            .AddTo(gameObject);

        this.OnTriggerEnterAsObservable()
            .Where(collider => !collider.GetComponent<BaseCardEffect>())
            .Subscribe(collider =>
            {
                Instantiate(
                        _hit,
                        transform.localPosition,
                        transform.localRotation);

                Destroy(gameObject);
            })
            .AddTo(gameObject);

        Destroy(gameObject, 3f);

    }
}
