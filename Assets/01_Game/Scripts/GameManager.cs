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
    private ReactiveProperty<GameState> _state = new(GameState.TITLE);
    public ReadOnlyReactiveProperty<GameState> State => _state;

    // ---------- UnityMessage
    private void Start()
    {
        _state
            .Chunk(2, 1)
            .Subscribe(state =>
            {
                Debug.Log($"[GameManager] ステート変更:{state[0]} => {state[1]}");

                switch (state[1])
                { 
                    case GameState.TITLE:
                        break;
                    case GameState.SELECT:
                        break;
                    case GameState.BATTLE:
                        break;
                    case GameState.CLEAR:
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
