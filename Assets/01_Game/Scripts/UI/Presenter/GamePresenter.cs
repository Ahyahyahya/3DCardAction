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
    [SerializeField] private SliderAnimation _castSlider;
    [SerializeField] private TextMeshProUGUI _cardNameTMP;
    [SerializeField] private TextMeshProUGUI _energyTMP;
    [SerializeField] private CardView[] _cards = new CardView[3];
    [SerializeField] private SlideCardsView _slideCardsView;

    // ---------- UnityMessage
    protected override void Start()
    {
        base.Start();

        // Model
        var playerDatas = PlayerDataProvider.Instance;
        var gameManager = GameManager.Instance;
        var cardDataStore = FindAnyObjectByType<CardDataStore>();

        // 現在のHPのゲージ比率を調整
        playerDatas.MaxHp
            .Subscribe(value =>
            {
                _hpSlider.SetValueWithAnimation((float)playerDatas.Hp.CurrentValue / value);
            })
            .AddTo(this);

        // HPのゲージを減らす
        playerDatas.Hp
            .Subscribe(value =>
            {
                _hpSlider.SetValueWithAnimation((float)value / playerDatas.MaxHp.CurrentValue);
            })
            .AddTo(this);

        // 手札が変わるたびにカードの情報を更新する
        playerDatas.Hand
            .ObserveReplace()
            .Subscribe(data =>
            {
                _cards[data.Index].SetCardData(cardDataStore.FindWithIndex(data.NewValue));
            })
            .AddTo(this);

        // 現在のエネルギー所持数によってテキストを変える
        playerDatas.CurrentEnergy
            .Subscribe(value =>
            {
                _energyTMP.text = value.ToString();
            })
            .AddTo(this);

        playerDatas.CurCardNum
            .Skip(1)
            .Subscribe(num =>
            {
                _cards[num].transform.SetAsLastSibling();

                _slideCardsView.Slide(num);
            })
            .AddTo(this);

        playerDatas.IsCasting
            .Subscribe(isCasting =>
            {
                if (isCasting)
                {
                    _castSlider.gameObject.SetActive(true);
                }
                else
                {
                    _castSlider.gameObject.SetActive(false);
                }

            })
            .AddTo(this);

        playerDatas.IsCastSuccess
            .Where(value => value == true)
            .Subscribe(_ =>
            {
                _castSlider.gameObject.SetActive(false);
            })
            .AddTo(this);

        playerDatas.CurCastTime
            .Subscribe(value =>
            {
                var targetCardId = playerDatas.Hand[playerDatas.CurCardNum.CurrentValue];

                var targetCard = cardDataStore.FindWithIndex(targetCardId);

                _cardNameTMP.text = targetCard.DataName;

                if (targetCard.CastTime == 0) return;

                _castSlider.SetValue(value / targetCard.CastTime);
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
