using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/Card")]
public class CardData : BaseData
{
    [SerializeField] private BaseCardEffect _cardEffect;

    [SerializeField] private int _cost;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private string _description;
    public int Cost => _cost;
    public Sprite Sprite => _sprite;
    public string Description => _description;

    public void Activate()
    {
        if (_cardEffect == null) return;

        var playerTr = PlayerDataProvider.Instance.transform;

        var effect = Instantiate(
            _cardEffect,
            playerTr.localPosition,
            playerTr.localRotation);

        effect.ActivateCardEffect();
    }
}
