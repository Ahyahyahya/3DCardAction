using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using TMPro;

public class GamePresenter : MonoBehaviour
{
    // ---------- View
    [SerializeField] private TextMeshProUGUI _energyTMP;

    // ---------- UnityMessage
    private void Start()
    {
        // Model
        var playerDatas = PlayerDataProvider.Instance;

        // 現在のエネルギー所持数によってテキストを変える
        playerDatas.CurrentEnergy
            .Subscribe(value =>
            {
                _energyTMP.text = value.ToString();
            })
            .AddTo(this);
    }
}
