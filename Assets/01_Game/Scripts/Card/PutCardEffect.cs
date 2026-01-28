using System;
using System.Collections.Generic;
using R3;
using R3.Triggers;
using UnityEngine;

public class PutCardEffect : BaseCardEffect
{
    [SerializeField] private float _range = 20f;
    public override void ActivateCardEffect()
    {
        var camera = Camera.main;

        var centerRay = camera.ViewportPointToRay(camera.rect.center);

        // ƒJ[ƒh‚ÌŒø‰Ê‚ª”­“®‚·‚éêŠ
        Vector3 activatePos = new();

        if (Physics.Raycast(centerRay, out RaycastHit hit))
        {
            activatePos = hit.point;

            Debug.Log("[PutCardEffect] Hit Anything");
        }
        else
        {
            activatePos =
                camera.transform.position + camera.transform.forward * _range;

            Debug.Log("[PutCardEffect] No Hit");
        }

        gameObject.transform.localPosition = activatePos;
    }
}
