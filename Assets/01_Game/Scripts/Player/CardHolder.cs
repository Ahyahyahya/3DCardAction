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
using System.Threading;

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

    private ReactiveProperty<bool> _isCasting = new();
    public ReadOnlyReactiveProperty<bool> IsCasting => _isCasting;

    private ReactiveProperty<bool> _isCastSuccess = new();
    public ReadOnlyReactiveProperty<bool> IsCastSuccess => _isCastSuccess;

    private ReactiveProperty<float> _curCastTime = new();

    public ReadOnlyReactiveProperty<float> CurCastTime => _curCastTime;

    private ReactiveProperty<int> _curCardNum = new(1);
    public ReadOnlyReactiveProperty<int> CurCardNum => _curCardNum;

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

    [SerializeField] private List<int> _initialCards = new();
    // デッキ構成
    private ObservableList<int> _allCards = new();
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

        // 初期化
        foreach (var card in _initialCards)
        {
            _allCards.Add(card);
        }

        for (int i = 0; i < _handCount; i++)
        {
            // 手札を初期化
            _hand.AddLast(-1);

            _newCards.AddLast(-1);
        }

        inputer.LeftMouseBtn
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .Subscribe(input =>
            {
                _isCasting.Value = input;

                // PlayCard(_curCardNum.Value);
            })
            .AddTo(gameObject);

        // 発動するカードを選ぶ処理
        inputer.MouseMidBtn
            .Where(_ => gm.State.CurrentValue == GameState.BATTLE)
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(input =>
            {
                if (input > 0)
                {
                    if (_curCardNum.Value + 1 >= Hand.Count)
                    {
                        _curCardNum.Value = 0;
                    }
                    else
                    {
                        _curCardNum.Value++;
                    }
                }
                else if (input < 0)
                {
                    if (_curCardNum.Value - 1 < 0)
                    {
                        _curCardNum.Value = Hand.Count - 1;
                    }
                    else
                    {
                        _curCardNum.Value--;
                    }
                }
            })
            .AddTo(gameObject);

        gm.State
            .Subscribe(state =>
            {
                if (state == GameState.BATTLE)
                {
                    _currentEnergy.Value = 0;

                    // デッキをシャッフル
                    ShuffleCardsIntoDeck(_allCards, false);

                    // 最初のドロー
                    for (int i = 0; i < _handCount; i++)
                    {
                        // カードを引く
                        DrawCard(i);
                    }

                }
                else if (state == GameState.CLEAR)
                {
                    for (int i = 0; i < _newCards.Count; i++)
                    {
                        _newCards[i] = Random.Range(0, _cardDataStore.GetCount);

                        Debug.Log($"[CardHolder] 新カード{i}番" + _cardDataStore.FindWithIndex(_newCards[i]).DataName);
                    }
                }
                else if (state == GameState.TITLE)
                {
                    _allCards.Clear();

                    foreach (var initialCard in _initialCards)
                    {
                        _allCards.Add(initialCard);
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

        CheckCastLoop(destroyCancellationToken).Forget();
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
        // 山札をリセット
        _deck.Clear();

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
    public void AddCardIntoDeck(int cardId)
    {
        _allCards.Add(cardId);

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
    }

    /// <summary>
    /// カードを発動する
    /// </summary>
    /// <param name="handIndex">発動する手札の要素番号</param>
    private async void PlayCard(int handIndex)
    {
        // 使ったカードのデータを取得
        var targetCard = _cardDataStore.FindWithIndex(_hand[handIndex]);

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
            targetCard.Activate();

            await UniTask.WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// キャスト時間分長押ししているか確認する
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask CheckCastLoop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (_isCasting.Value)
            {
                if (!_isCastSuccess.Value)
                {
                    _curCastTime.Value += Time.deltaTime;

                    if (_curCastTime.Value >= _cardDataStore.FindWithIndex(_hand[_curCardNum.Value]).CastTime)
                    {
                        PlayCard(_curCardNum.Value);

                        _isCastSuccess.Value = true;
                    }
                }
            }
            else
            {
                _curCastTime.Value = 0;

                _isCastSuccess.Value = false;
            }

            await UniTask.Yield();
        }
    }

    public void AddActivateCnt(int value)
    {
        _activateCnt += value;
    }
}
