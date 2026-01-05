using R3;
using R3.Triggers;
using UnityEngine;

public class CardEffectCore : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private CardData _cardData;

    [SerializeField] private BaseCardEffect _effect;
    public CardData CardData => _cardData;
    public BaseCardEffect Effect => _effect;

    // ---------- UnityMessage
    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .Subscribe(colider =>
            {
                if (colider.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.TakeDamage(_cardData.Atk, _cardData.Element);
                }
            })
            .AddTo(this);
    }
}
