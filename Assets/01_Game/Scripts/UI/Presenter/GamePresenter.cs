using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using TMPro;
using ObservableCollections;

public class GamePresenter : MonoBehaviour
{
    // ---------- View
    [SerializeField] private TextMeshProUGUI _energyTMP;
    [SerializeField] private CardView[] _cards = new CardView[3];

    // ---------- UnityMessage
    private void Start()
    {
        // Model
        var playerDatas = PlayerDataProvider.Instance;
        var cardDataStore = FindAnyObjectByType<CardDataStore>();

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
