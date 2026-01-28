using ModestTree;
using ObservableCollections;
using R3;
using System;
using UnityEngine;

public class ClearPresenter : BasePresenter
{
    // ---------- Views
    [SerializeField] private GameObject[] _cards = new GameObject[3];

    // ---------- UnityMessage
    protected override void Start()
    {
        base.Start();

        var playerData = PlayerDataProvider.Instance;
        var cardDataStore = FindAnyObjectByType<CardDataStore>();
        var cardViews = new CardView[3];
        for ( int i = 0; i < _cards.Length; i++ )
        {
            cardViews[i] = _cards[i].GetComponent<CardView>();
        }

        playerData.NewCards
            .ObserveReplace()
            .Subscribe(data =>
            {
                cardViews[data.Index].SetCardData(cardDataStore.FindWithIndex(data.NewValue));
            })
            .AddTo(this);

        foreach (var card in _cards)
        {
            var btn = card.GetComponent<CustomButton>();

            btn.OnButtonClicked
                .Subscribe(_ =>
                {
                    var targetCardData = cardDataStore.FindWithIndex(
                        playerData.NewCards[_cards.IndexOf(btn.gameObject)]);

                    playerData.AddCardIntoDeck(targetCardData.Id);
                })
                .AddTo(this);
        }
    }
}
