using System;
using System.Collections.Generic;
using R3;
using R3.Triggers;
using UnityEngine;

public class PutCardEffect : BaseCardEffect
{
    private List<GameObject> _withinTargets = new();
    public override void ActivateCardEffect()
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
                if (!collider.CompareTag("Enemy")) return;

                _withinTargets.Add(collider.gameObject);
            })
            .AddTo(gameObject);

        this.OnTriggerExitAsObservable()
            .Subscribe(collider =>
            {
                if (!collider.CompareTag("Enemy")) return;

                _withinTargets.Remove(collider.gameObject);
            })
            .AddTo(gameObject);

        this.UpdateAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ =>
            {
                foreach (var target in _withinTargets)
                {
                    Debug.Log(target.name + "にダメージを与えた");
                }
            })
            .AddTo(gameObject);

        gameObject.transform.localPosition = activatePos;

        Destroy(gameObject, 5f);
    }
}
