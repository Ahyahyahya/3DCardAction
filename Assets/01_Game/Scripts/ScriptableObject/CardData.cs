using UnityEngine;

public enum Element
{
    None,
    Fire,
    Ice,
    Thunder,
    Wind,
    Poison,
}

[CreateAssetMenu(menuName = "ScriptableObject/Data/Card")]
public class CardData : BaseData
{
    [SerializeField] private Element _element;

    [SerializeField] private CardEffectCore _effectCore;

    [SerializeField] private int _atk;

    [SerializeField] private int _cost;

    [SerializeField] private int _price = 100;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private string _description;

    public Element Element => _element;
    public int Atk => _atk;
    public int Cost => _cost;
    public int Price => _price;
    public Sprite Sprite => _sprite;
    public string Description => _description;

    public void Activate()
    {
        var core = Instantiate(_effectCore, Vector3.zero, Quaternion.identity);

        core.Effect.ActivateCardEffect();
    }
}
