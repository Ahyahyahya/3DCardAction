using System;
using System.Collections.Generic;
using R3;
using R3.Triggers;
using UnityEngine;

public class PutCardEffect : BaseCardEffect
{
    [SerializeField] private float _lifeTime = 1.0f;

    private List<IDamageble> _withinTargets = new();
    public override void ActivateCardEffect(CardData cardData)
    {
        var camera = Camera.main;

        var centerRay = camera.ViewportPointToRay(camera.rect.center);

        // カードの効果が発動する場所
        Vector3 activatePos = new();

        if (Physics.Raycast(centerRay, out RaycastHit hit))
        {
            activatePos = hit.point;
        }
        else
        {
            activatePos = camera.transform.localPosition;
        }

        this.OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (!collider.TryGetComponent<IDamageble>(out var damageble)) return;

                if (collider.gameObject == PlayerDataProvider.Instance.gameObject) return;

                _withinTargets.Add(damageble);
            })
            .AddTo(gameObject);

        this.OnTriggerExitAsObservable()
            .Subscribe(collider =>
            {
                if (!collider.TryGetComponent<IDamageble>(out var damageble)) return;

                _withinTargets.Remove(damageble);
            })
            .AddTo(gameObject);

        this.UpdateAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ =>
            {
                foreach (var target in _withinTargets)
                {
                    if (target == null) continue;

                    target.TakeDamage(cardData.Atk);

                    Debug.Log(cardData.Atk + "ダメージを与えた");
                }
            })
            .AddTo(gameObject);

        gameObject.transform.localPosition = activatePos;

        Destroy(gameObject, _lifeTime);
    }
}
