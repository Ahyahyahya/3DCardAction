using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
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
    public override void ActivateCardEffect(CardData cardData)
    {
        var camera = Camera.main;

        var rb = GetComponent<Rigidbody>();

        // 発射位置調整
        transform.localPosition =
            camera.transform.localPosition + camera.transform.forward * _createPosOffset;

        // 発射角度調整
        transform.localRotation = camera.transform.localRotation;

        // 生成エフェクト
        Instantiate(
            _muzzle,
            transform.localPosition,
            transform.localRotation);

        // 発射物のトレイル
        var trail = Instantiate(
            _trail,
            transform.localPosition,
            transform.localRotation,
            transform);

        this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                rb.AddForce(transform.forward * 5f);
            })
            .AddTo(gameObject);

        this.OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (collider.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.TakeDamage(cardData.Atk);

                    Debug.Log(collider.name + $"に{cardData.Atk}ダメージを与えたよ");
                }

                if (collider.GetComponent<BaseCardEffect>()) return;

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
