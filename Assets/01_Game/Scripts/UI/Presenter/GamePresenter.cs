using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using TMPro;
using UnityEngine;

public class GamePresenter : BasePresenter
{
    // ---------- View
    [SerializeField] private SliderAnimation _hpSlider;
    [SerializeField] private TextMeshProUGUI _energyTMP;
    [SerializeField] private CardView[] _cards = new CardView[3];

    // ---------- UnityMessage
    protected override void Start()
    {
        base.Start();

        // Model
        var playerDatas = PlayerDataProvider.Instance;
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
    }
}
