using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputer : MonoBehaviour, IInputProvider
{
    // ---------- Field
    private InputSystem_Actions _inputSystemActions;

    // ---------- R3
    private ReactiveProperty<Vector2> _move = new();
    public ReadOnlyReactiveProperty<Vector2> MoveButton => _move;

    private ReactiveProperty<bool> _leftMouseBtn = new();
    public ReadOnlyReactiveProperty<bool> LeftMouseBtn => _leftMouseBtn;

    private ReactiveProperty<float> _mouseMidBtn = new();
    public ReadOnlyReactiveProperty<float> MouseMidBtn => _mouseMidBtn;

    // ---------- UnityMessage
    private void Start()
    {
        _inputSystemActions = new InputSystem_Actions();

        _inputSystemActions.Enable();

        var playerActions = _inputSystemActions.Player;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                _move.OnNext(playerActions.Move.ReadValue<Vector2>());

                _leftMouseBtn.Value =
                    playerActions.ActivateCard.ReadValue<float>() == 1 ? true : false;

                _mouseMidBtn.Value = playerActions.Scroll.ReadValue<Vector2>().y;
            });
    }
}
