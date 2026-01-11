using ObservableCollections;
using R3;
using UnityEngine;

public class PlayerDataProvider : MonoBehaviour
{
    // ---------- Singleton
    public static PlayerDataProvider Instance;

    // ---------- Field
    [SerializeField] private PlayerCore _core;
    [SerializeField] private CardHolder _cardHolder;

    // ---------- Property
    public ReadOnlyReactiveProperty<int> Hp => _core.Hp;
    public ReadOnlyReactiveProperty<int> MaxHp => _core.MaxHp;
    public ReadOnlyReactiveProperty<int> CurrentEnergy => _cardHolder.CurrentEnergy;
    public ReadOnlyReactiveProperty<int> Money => _core.Money;
    public ObservableFixedSizeRingBuffer<int> Hand => _cardHolder.Hand;
    public ReadOnlyReactiveProperty<int> CurCardNum => _cardHolder.CurCardNum;
    public ObservableFixedSizeRingBuffer<int> NewCards => _cardHolder.NewCards;
    public ReadOnlyReactiveProperty<Node> CurrentNode => _core.CurrentNode;

    // ---------- Method
    public void AddCardIntoDeck(int index) => _cardHolder.AddCardIntoDeck(index);
    public void SetNode(Node node) => _core.SetNode(node);
    public void AddActivateCnt(int value) => _cardHolder.AddActivateCnt(value);
    public void PlusMoney(int value) => _core.PlusMoney(value);
    public void MinusMoney(int value) => _core.MinusMoney(value);

    // ---------- UnityMessage
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
