using ObservableCollections;
using R3;
using UnityEngine;

public class PlayerDataProvider : MonoBehaviour
{
    // ---------- Singleton
    public static PlayerDataProvider Instance;

    // ---------- Field
    [SerializeField] private CardHolder _cardHolder;

    // ---------- Property
    public ReadOnlyReactiveProperty<int> CurrentEnergy => _cardHolder.CurrentEnergy;
    public ObservableFixedSizeRingBuffer<int> Hand => _cardHolder.Hand;

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
