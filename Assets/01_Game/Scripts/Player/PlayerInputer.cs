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

    private ReactiveProperty<bool> _hand1 = new();
    public ReadOnlyReactiveProperty<bool> Hand1Button => _hand1;

    private ReactiveProperty<bool> _hand2 = new();
    public ReadOnlyReactiveProperty<bool> Hand2Button => _hand2;

    private ReactiveProperty<bool> _hand3 = new();
    public ReadOnlyReactiveProperty<bool> Hand3Button => _hand3;

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

                _hand1.Value =
                    playerActions.Hand1.ReadValue<float>() == 1 ? true : false;

                _hand2.Value =
                    playerActions.Hand2.ReadValue<float>() == 1 ? true : false;

                _hand3.Value =
                    playerActions.Hand3.ReadValue<float>() == 1 ? true : false;

                _mouseMidBtn.Value = playerActions.Scroll.ReadValue<Vector2>().y;
            });
    }

    // ---------- InputSystem
    private void OnHand1(InputAction.CallbackContext context)
    {
        _hand1.Value = context.ReadValue<bool>();
    }
}
