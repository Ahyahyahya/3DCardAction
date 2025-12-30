using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ObservableCollections;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using System;
using Random = UnityEngine.Random;

public class CardHolder : MonoBehaviour
{
    // ---------- Field
    // 手札の枚数上限
    private const int _handCount = 3;

    private CardDataStore _cardDataStore;

    // カードを発動するためのエネルギー
    private ReactiveProperty<int> _currentEnergy = new();
    public ReadOnlyReactiveProperty<int> CurrentEnergy => _currentEnergy;

    private float _energyRecoverySec = 1f;

    private int _energyMax = 5;

    private int _activateCnt = 1;

    // 手札(IDでカードを識別する)
    private ObservableFixedSizeRingBuffer<int> _hand = new(_handCount);
    public ObservableFixedSizeRingBuffer<int> Hand => _hand;

    private ObservableFixedSizeRingBuffer<int> _newCards = new(_handCount);
    public ObservableFixedSizeRingBuffer<int> NewCards => _newCards;

    // 山札(IDでカードを識別する)
    private ObservableList<int> _deck = new();
    public ObservableList<int> Deck => _deck;

    // 墓地
    private ObservableList<int> _trash = new();
    public ObservableList<int> Trash => _trash;

    // デッキ構成
    private ObservableList<int> _allCards = new() { 0, 0, 0, 0, 0};
    public ObservableList<int> AllCards => _allCards;

    // ---------- UnityMessage
    private void Awake()
    {
        _cardDataStore = FindAnyObjectByType<CardDataStore>();
    }
    private void Start()
    {
        var inputer = GetComponent<PlayerInputer>();

        var gm = GameManager.Instance;

        inputer.Hand1Button
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .Where(input => input == true)
            .Subscribe(_ =>
            {
                PlayCard(0);
            })
            .AddTo(gameObject);

        inputer.Hand2Button
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .Where(input => input == true)
            .Subscribe(_ =>
            {
                PlayCard(1);
            })
            .AddTo(gameObject);

        inputer.Hand3Button
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .Where(input => input == true)
            .Subscribe(_ =>
            {
                PlayCard(2);
            })
            .AddTo(gameObject);

        gm.State
            .Subscribe(state =>
            {
                if (state == GameState.TITLE)
                {
                    _currentEnergy.Value = 0;

                    // デッキをシャッフル
                    ShuffleCardsIntoDeck(_allCards, false);

                    // 最初のドロー
                    for (int i = 0; i < _handCount; i++)
                    {
                        // 手札を初期化
                        _hand.AddLast(-1);

                        _newCards.AddLast(-1);

                        // カードを引く
                        DrawCard(i);
                    }

                }
                else if (state == GameState.CLEAR)
                {
                    for (int i = 0; i < _newCards.Count; i++)
                    {
                        _newCards[i] = Random.Range(0, _cardDataStore.GetCount);
                    }
                }
            })
            .AddTo(this);

        this.UpdateAsObservable()
            .Skip(1)
            .ThrottleFirst(TimeSpan.FromSeconds(_energyRecoverySec))
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .Where(_ => _currentEnergy.Value < _energyMax)
            .Subscribe(_ =>
            {
                _currentEnergy.Value++;
            });
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
    /// 山札に新しいカードを追加する
    /// </summary>
    /// <param name="cardId"></param>
    public void AddCardIntoDeck(int index)
    {
        _deck.Add(NewCards[index]);

        if (GameManager.Instance.State.CurrentValue == GameState.CLEAR)
        {
            GameManager.Instance.ChangeGameState(GameState.SELECT);
        }
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
    private async void PlayCard(int handIndex)
    {
        // 使ったカードのデータを取得
        var targetCard = _cardDataStore.FindWithId(_hand[handIndex]);

        if (_currentEnergy.Value < targetCard.Cost)
        {
            Debug.Log("[CardHolder] エネルギーが足りない！");
            return;
        }

        var activateCnt = _activateCnt;

        _activateCnt = 1;

        // 使ったカードのコスト分エネルギーを減らす
        _currentEnergy.Value -= targetCard.Cost;

        // 発動したカードを墓地へ
        _trash.Add(_hand[handIndex]);

        // カードを引く
        DrawCard(handIndex);

        for (int i = 0; i < activateCnt; i++)
        {
            // 効果発動
            Debug.Log($"[CardHolder] {targetCard.DataName + targetCard.Id} を発動！");

            targetCard.Activate();

            await UniTask.WaitForSeconds(0.5f);
        }
    }

    public void AddActivateCnt(int value)
    {
        _activateCnt += value;
    }
}
