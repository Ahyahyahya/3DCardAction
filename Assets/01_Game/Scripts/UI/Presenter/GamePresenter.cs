using Cysharp.Threading.Tasks;
using DG.Tweening;
using ObservableCollections;
using R3;
using System;
using TMPro;
using UnityEngine;

public class GamePresenter : BasePresenter
{
    // ---------- Model
    [SerializeField] private PlayerInputer _inputer;

    // ---------- View
    [SerializeField] private SliderAnimation _hpSlider;
    [SerializeField] private TextMeshProUGUI _energyTMP;
    [SerializeField] private CardView[] _cards = new CardView[3];
    [SerializeField] private SlideCardsView _slideCardsView;

    // ---------- UnityMessage
    protected override void Start()
    {
        base.Start();

        // Model
        var playerDatas = PlayerDataProvider.Instance;
        var gameManager =GameManager.Instance;
        var cardDataStore = FindAnyObjectByType<CardDataStore>();

        // 現在のHPのゲージ比率を調整
        playerDatas.MaxHp
            .Subscribe(value =>
            {
                _hpSlider.SetValue((float)playerDatas.Hp.CurrentValue / value);
            })
            .AddTo(this);

        // HPのゲージを減らす
        playerDatas.Hp
            .Subscribe(value =>
            {
                _hpSlider.SetValue((float)value / playerDatas.MaxHp.CurrentValue);
            })
            .AddTo(this);

        // 手札が変わるたびにカードの情報を更新する
        playerDatas.Hand
            .ObserveReplace()
            .Subscribe(data =>
            {
                _cards[data.Index].SetCardData(cardDataStore.FindWithId(data.NewValue));
            })
            .AddTo(this);

        // 現在のエネルギー所持数によってテキストを変える
        playerDatas.CurrentEnergy
            .Subscribe(value =>
            {
                _energyTMP.text = value.ToString();
            })
            .AddTo(this);

        //_inputer.MouseMidBtn
        //    .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
        //    .Where(_ => gameManager.State.CurrentValue == GameState.BATTLE)
        //    .Subscribe(input =>
        //    {
        //        if (input > 0)
        //        {
        //            _slideCardsView.SlideOut();
        //        }
        //        else if (input < 0)
        //        {
        //            _slideCardsView.SlideIn();
        //        }
        //    })
        //    .AddTo(this);

        playerDatas.CurCardNum
            .Skip(1)
            .Subscribe(num =>
            {
                _cards[num].transform.SetAsLastSibling();

                _slideCardsView.Slide(num);
            })
            .AddTo(this);
    }

    public void CreateDamage(TextMeshProUGUI damageTMP, int damage)
    {
        var textTr = damageTMP.transform;

        damageTMP.text = damage.ToString();

        textTr.rotation = Camera.main.transform.rotation;

        textTr.DOMoveY(
            textTr.position.y + 1f,
            1f)
            .SetLink(damageTMP.gameObject)
            .OnComplete(() => Destroy(damageTMP.gameObject));

        damageTMP.DOFade(0f, 1f)
            .SetLink(damageTMP.gameObject);
    }
}
