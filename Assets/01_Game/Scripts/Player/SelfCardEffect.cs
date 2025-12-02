using R3.Triggers;
using UnityEngine;

public class SelfCardEffect : BaseCardEffect
{
    public override void ActivateCardEffect()
    {
        Destroy(gameObject, 3f);
    }
}
