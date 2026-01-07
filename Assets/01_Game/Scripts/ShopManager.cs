using ObservableCollections;
using R3;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // ----------- Field
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private CardDataStore _cardDataStore;

    private const int _shopCardCnt = 10;

    private ObservableFixedSizeRingBuffer<int> _shopCards = new(_shopCardCnt);
    public ObservableFixedSizeRingBuffer<int> ShopCards => _shopCards;

    // ---------- UnityMessage
    private void Start()
    {
        for (int i = 0; i < _shopCardCnt; i++)
        {
            _shopCards.AddLast(-1);
        }

        _gameManager.State
            .Where(state => state == GameState.SHOP)
            .Subscribe(_ =>
            {
                for (int i = 0; i < _shopCardCnt; i++)
                {
                    _shopCards[i] = (Random.Range(0, _cardDataStore.GetCount));
                }
            })
            .AddTo(this);
    }
}
