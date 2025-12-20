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
    public ObservableFixedSizeRingBuffer<int> Hand => _cardHolder.Hand;
    public ObservableFixedSizeRingBuffer<int> NewCards => _cardHolder.NewCards;
    public ReadOnlyReactiveProperty<Node> CurrentNode => _core.CurrentNode;

    // ---------- Method
    public void AddCardIntoDeck(int index) => _cardHolder.AddCardIntoDeck(index);
    public void SetNode(Node node) => _core.SetNode(node);

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
