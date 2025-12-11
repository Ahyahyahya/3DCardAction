using UnityEngine;
using R3;

public abstract class BasePresenter : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private GameState _targetGameState;

    // ---------- UnityMessage
    protected virtual void Start()
    {
        var gm = GameManager.Instance;

        // 対象のゲームステートの時のみUIを表示する
        gm.State
            .Subscribe(state =>
            {
                if (state == _targetGameState)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            })
            .AddTo(this);
    }
}
