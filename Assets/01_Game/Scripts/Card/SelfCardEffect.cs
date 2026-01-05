using NUnit.Framework.Constraints;
using R3.Triggers;
using UnityEngine;

public enum BuffType
{
    AddActivateCnt
}

public class SelfCardEffect : BaseCardEffect
{
    [SerializeField] private BuffType _type;
    [SerializeField] private int _amount;
    public override void ActivateCardEffect()
    {
        var playerData = PlayerDataProvider.Instance;

        transform.localPosition = Camera.main.transform.localPosition;

        switch(_type)
        {
            case BuffType.AddActivateCnt:
                playerData.AddActivateCnt(_amount);
                break;
        }
        Destroy(gameObject, 3f);
    }
}
