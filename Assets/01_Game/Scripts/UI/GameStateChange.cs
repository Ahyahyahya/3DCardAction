using UnityEngine;

public class GameStateChange : MonoBehaviour
{
    [SerializeField] private GameState _gameState;

    public void ChangeGameState()
    {
        GameManager.Instance.ChangeGameState(_gameState);
    }
}
