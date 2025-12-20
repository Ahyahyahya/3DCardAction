using R3;
using UnityEngine;

public enum GameState
{
    TITLE,
    SELECT,
    BATTLE,
    CLEAR,
    EVENT,
    SHOP,
    RESULT
}

public class GameManager : MonoBehaviour
{
    // ---------- Singleton
    public static GameManager Instance;

    // ---------- Field
    [SerializeField] private StageGenerator _sg;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private TransitionEventer _transitionEventer;
    [SerializeField] private TransitionAnimator _transitionAnimator;

    private int _clearCnt = 0;

    public int ClearCnt => _clearCnt;

    [SerializeField] private SerializableReactiveProperty<GameState> _state = new(GameState.TITLE);
    public ReadOnlyReactiveProperty<GameState> State => _state;

    // ---------- UnityMessage
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _state
            .Subscribe(state =>
            {
                //Debug.Log($"[GameManager] ステート変更:{state[0]} => {state[1]}");

                Debug.Log($"[GameManager] 現在のステート:{state}");

                switch (state)
                {
                    case GameState.TITLE:
                        break;
                    case GameState.SELECT:
                        _mapGenerator.GenerateMap();
                        break;
                    case GameState.BATTLE:
                        _sg.GenerateStage();
                        break;
                    case GameState.CLEAR:
                        _clearCnt++;
                        break;
                    case GameState.EVENT:
                        break;
                    case GameState.SHOP:
                        break;
                    case GameState.RESULT:
                        break;
                }
            })
            .AddTo(this);
    }

    // ---------- Method
    public void ChangeGameState(GameState state)
    {
        _transitionEventer.OnTransitionHalf
            .Subscribe( _ =>
            {
                _state.Value = state;
            })
            .AddTo(this);

        _transitionAnimator.StartTransitionAnim();
    }
}
