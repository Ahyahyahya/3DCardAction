using MasterFX;
using R3;
using UnityEngine;

public class LaserCardEffect : BaseCardEffect
{
    [SerializeField] private MLaser _laser;

    private float _createPosOffset = 3.0f;

    public override void ActivateCardEffect()
    {
        var camera = Camera.main;

        // î≠éÀà íuí≤êÆ
        transform.localPosition =
            camera.transform.localPosition + camera.transform.forward * _createPosOffset;

        // î≠éÀäpìxí≤êÆ
        transform.localRotation = camera.transform.localRotation;
    }
}
