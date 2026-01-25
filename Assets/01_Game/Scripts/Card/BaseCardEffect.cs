using UnityEngine;

public abstract class BaseCardEffect : MonoBehaviour
{
    public abstract void ActivateCardEffect();

    public virtual void ActivateCardEffect(Transform tr)
    {

    }
}
