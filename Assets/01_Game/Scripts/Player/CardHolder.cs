using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ObservableCollections;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using R3;

public class CardHolder : MonoBehaviour
{
    // ---------- Field
    // 手札の枚数上限
    private const int _handCount = 3;

    private CardDataStore _cardDataStore;

    // 手札(IDでカードを識別する)
    private ObservableFixedSizeRingBuffer<int> _hand = new(_handCount);
    public ObservableFixedSizeRingBuffer<int> Hand => _hand;

    // 山札(IDでカードを識別する)
    private ObservableList<int> _deck = new();
    public ObservableList<int> Deck => _deck;

    // 墓地
    private ObservableList<int> _trash = new();
    public ObservableList<int> Trash => _trash;

    // デッキ構成
    private ObservableList<int> _allCards = new() { 0, 0, 0, 0, 0, 1, 1, 1, 2, 2};
    public ObservableList<int> AllCards => _allCards;

    // ---------- UnityMessage
    private void Awake()
    {
        _cardDataStore = FindAnyObjectByType<CardDataStore>();

    }
    private void Start()
    {
        // デッキをシャッフル
        ShuffleCardsIntoDeck(_allCards, false);

        // 最初のドロー
        for (int i = 0; i < _handCount; i++)
        {
            // 手札を初期化
            _hand.AddLast(-1);

            // カードを引く
            DrawCard(i);
        }

        var inputer = GetComponent<PlayerInputer>();

        inputer.Hand1Button
            .Where(input => input == true)
            .Subscribe(_ =>
            {
                PlayCard(1);
            })
            .AddTo(gameObject);

        inputer.Hand2Button
            .Where(input => input == true)
            .Subscribe(_ =>
            {
                PlayCard(2);
            })
            .AddTo(gameObject);

        inputer.Hand3Button
            .Where(input => input == true)
            .Subscribe(_ =>
            {
                PlayCard(3);
            })
            .AddTo(gameObject);
    }

    // ---------- Method
    /// <summary>
    /// 対象のカード達をシャッフルして山札に入れる
    /// </summary>
    /// <param name="targetCards">対象のカード達</param>
    /// <param name="isClear">対象のカード達を消すか</param>
    private void ShuffleCardsIntoDeck(
        ObservableList<int> targetCards,
        bool isClear = true)
    {
        Debug.Log("[CardHolder] 山札をシャッフル！");

        // 対象のカード達のカードをランダムに山札に格納するための一時的変数
        var tmpCards = new List<int>(targetCards);

        // 対象のカード達を山札にランダムに格納する(シャッフル)
        while (tmpCards.Count > 0)
        {
            // 山札に加えるカードを決定
            var targetIndex = Random.Range(0, tmpCards.Count);

            // 山札に加える
            _deck.Add(tmpCards[targetIndex]);

            // 加えたカードを消す
            tmpCards.RemoveAt(targetIndex);
        }

        if (isClear) { targetCards.Clear(); }
    }

    /// <summary>
    /// 山札の最初からカードを手札に加える
    /// </summary>
    /// <param name="handIndex">カードを加える場所</param>
    private void DrawCard(int handIndex)
    {
        // 山札の最初からカードを手札に加える
        _hand[handIndex] = _deck.First();

        // 手札に加えたカードを山札から消す
        _deck.RemoveAt(0);

        // 加えたあと山札がないならシャッフルする
        if (_deck.Count == 0)
        {
            ShuffleCardsIntoDeck(_trash);
        }
        Debug.Log($"[CardHolder] 残り山札: {_deck.Count}");
    }

    /// <summary>
    /// カードを発動する
    /// </summary>
    /// <param name="handIndex">発動する手札の要素番号</param>
    private void PlayCard(int handIndex)
    {
        var targetCard = _cardDataStore.FindWithId(_hand[handIndex]);

        Debug.Log($"[CardHolder] {targetCard.DataName + targetCard.Id} を発動！");

        // 発動したカードを墓地へ
        _trash.Add(_hand[handIndex]);

        // カードを引く
        DrawCard(handIndex);
    }
}
