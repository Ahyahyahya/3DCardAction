using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using System;

public class ShopPresenter : BasePresenter
{
    // ---------- Field
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private List<GameObject> _cards = new();

    // ---------- UnityMessage
    protected override void Start()
    {
        base.Start();

        var playerData = PlayerDataProvider.Instance;
        var cardDataStore = FindAnyObjectByType<CardDataStore>();
        var cardViews = new List<CardView>();

        for (int i = 0; i < _cards.Count; i++)
        {
            cardViews.Add(_cards[i].GetComponent<CardView>());
        }

        _shopManager.ShopCards
            .ObserveReplace()
            .Subscribe(data =>
            {
                _cards[data.Index].SetActive(true);

                cardViews[data.Index].SetCardData(cardDataStore.FindWithId(data.NewValue));
            })
            .AddTo(this);

        foreach (var card in _cards)
        {
            var btn = card.GetComponent<CustomButton>();

            btn.OnButtonClicked
                .Subscribe(_ =>
                {
                    var targetCardData = cardDataStore.FindWithId(
                        _shopManager.ShopCards[_cards.IndexOf(btn.gameObject)]);

                    if (playerData.Money.CurrentValue < targetCardData.Price) return;

                    playerData.AddCardIntoDeck(targetCardData.Id);
                    playerData.MinusMoney(targetCardData.Price);
                    btn.gameObject.SetActive(false);
                })
                .AddTo(this);
        }
    }
}
