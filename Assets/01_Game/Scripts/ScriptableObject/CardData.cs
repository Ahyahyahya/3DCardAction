using UnityEngine;

public enum EffectType
{
    Shot,
    Front,
    Put,
    Laser,
    Self
}

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
    #region Common Field
    [Header("Common Field")]
    [SerializeField] private EffectType _effectType;

    [SerializeField] private Element _element;

    [SerializeField] private CardEffectCore _effectCore;

    [SerializeField] private int _cost;

    [SerializeField] private int _atk;

    [SerializeField] private float _castTime;

    [SerializeField] private int _price = 100;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private string _description;

    public EffectType EffectType => _effectType;
    public CardEffectCore Core => _effectCore;
    public Element Element => _element;
    public int Cost => _cost;
    public int Atk => _atk;
    public float CastTime => _castTime;
    public int Price => _price;
    public Sprite Sprite => _sprite;
    public string Description => _description;
    #endregion

    #region Shot Field
    [Header("Shot Field")]
    [SerializeField] private float _moveSpeed = 1.0f;
    public float MoveSpeed => _moveSpeed;
    #endregion

    #region Put & Laser

    [Header("Put & Laser")]
    [SerializeField] private int _damageCnt;

    [SerializeField] private float _damageInterval;
    public int DamageCnt => _damageCnt;
    public float DamageInterval => _damageInterval;

    #endregion



    public void Activate()
    {
        var core = Instantiate(_effectCore, Vector3.zero, Quaternion.identity);

        core.Effect.ActivateCardEffect();
    }
}
