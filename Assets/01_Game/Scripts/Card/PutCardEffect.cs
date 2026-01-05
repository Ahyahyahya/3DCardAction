using System;
using System.Collections.Generic;
using R3;
using R3.Triggers;
using UnityEngine;

public class PutCardEffect : BaseCardEffect
{
    [SerializeField] private float _lifeTime = 1.0f;
    public override void ActivateCardEffect()
    {
        var camera = Camera.main;

        var centerRay = camera.ViewportPointToRay(camera.rect.center);

        // ƒJ[ƒh‚ÌŒø‰Ê‚ª”­“®‚·‚éêŠ
        Vector3 activatePos = new();

        if (Physics.Raycast(centerRay, out RaycastHit hit))
        {
            activatePos = hit.point;
        }
        else
        {
            activatePos = camera.transform.localPosition;
        }

        gameObject.transform.localPosition = activatePos;

        Destroy(gameObject, _lifeTime);
    }
}
