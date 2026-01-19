using MasterFX;
using R3;
using R3.Triggers;
using System;
using UnityEngine;

public class CardEffectCore : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private CardData _cardData;

    [SerializeField] private BaseCardEffect _effect;
    public CardData CardData => _cardData;
    public BaseCardEffect Effect => _effect;

    private Ray _ray = new();

    // ---------- UnityMessage
    private void Start()
    {
        switch (_cardData.EffectType)
        {
            case EffectType.Shot:
            case EffectType.Front:

                this.OnTriggerEnterAsObservable()
                .Subscribe(colider =>
                {
                    if (colider.TryGetComponent<IDamageble>(out var damageble))
                    {
                        damageble.TakeDamage(_cardData.Atk, _cardData.Element);
                    }
                })
                .AddTo(this);

                break;
            case EffectType.Put:
                break;
            case EffectType.Laser:

                var laser = GetComponent<MLaser>();

                var radius = 3f;

                var runCnt = 0;

                this.UpdateAsObservable()
                    .ThrottleFirst(TimeSpan.FromSeconds(_cardData.DamageInterval))
                    .Subscribe(_ =>
                    {
                        runCnt++;

                        var hits = Physics.SphereCastAll(
                            transform.position,
                            radius,
                            transform.forward * laser.CurrentLaserDist.CurrentValue);

                        foreach (var hit in hits)
                        {
                            if (hit.collider.gameObject
                                == PlayerDataProvider.Instance.gameObject) continue;

                            if (hit.collider.TryGetComponent<IDamageble>(out var damageble))
                            {
                                damageble.TakeDamage(_cardData.Atk, _cardData.Element);
                            }
                        }

                        if (runCnt >= _cardData.DamageCnt)
                        {
                            Destroy(gameObject);
                        }
                    })
                    .AddTo(this);

                break;
            case EffectType.Self:
                break;

        }
    }
}
