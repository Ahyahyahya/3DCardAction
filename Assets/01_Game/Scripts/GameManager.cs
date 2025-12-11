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

    private int _clearCnt = 0;

    public int ClearCnt => _clearCnt;

    private ReactiveProperty<GameState> _state = new(GameState.TITLE);
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
                        _clearCnt = 0;
                        break;
                    case GameState.SELECT:
                        _sg.GenerateStage();
                        ChangeGameState(GameState.BATTLE);
                        break;
                    case GameState.BATTLE:
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
        _state.Value = state;
    }
}
