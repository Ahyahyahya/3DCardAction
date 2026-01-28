using NUnit.Framework.Constraints;
using R3.Triggers;
using UnityEngine;

public enum BuffType
{
    AddActivateCnt,
    NoCast
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
            case BuffType.NoCast:
                playerData.SetNoCast();
                break;
        }
        Destroy(gameObject, 3f);
    }
}
