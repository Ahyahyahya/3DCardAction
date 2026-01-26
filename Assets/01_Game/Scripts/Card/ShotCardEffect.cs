using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShotCardEffect : BaseCardEffect
{
    // ---------- Field
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _hit;
    [SerializeField] private GameObject _trail;

    private const float _createPosOffset = 3.0f;

    // ---------- Method
    public override void ActivateCardEffect()
    {
        var camera = Camera.main;

        var rb = GetComponent<Rigidbody>();

        var core = GetComponent<CardEffectCore>();

        // 発射位置調整
        transform.localPosition =
            camera.transform.localPosition + camera.transform.forward * _createPosOffset;

        // 発射角度調整
        transform.localRotation = camera.transform.localRotation;

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

        this.UpdateAsObservable()
            .ThrottleLast(TimeSpan.FromSeconds(core.CardData.LifeTime))
            .Subscribe(_ =>
            {
                Instantiate(
                        _hit,
                        transform.localPosition,
                        transform.localRotation);

                Destroy(gameObject);
            });
    }
}
