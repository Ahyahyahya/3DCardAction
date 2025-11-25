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

    // ---------- UnityMessage
    private void Start()
    {
        _inputSystemActions = new InputSystem_Actions();

        _inputSystemActions.Enable();

        var playerActions = _inputSystemActions.Player;

        this.UpdateAsObservable()
            .Select(x => playerActions.Move.ReadValue<Vector2>())
            .Subscribe(x =>{ _move.OnNext(x) ;})
            .AddTo(this);

        this.UpdateAsObservable()
            .Select(input => playerActions.Hand1.ReadValue<float>())
            .Subscribe(input =>
            {
                _hand1.Value = input == 1 ? true : false;
            });

        this.UpdateAsObservable()
            .Select(input => playerActions.Hand2.ReadValue<float>())
            .Subscribe(input =>
            {
                _hand2.Value = input == 1 ? true : false;
            });

        this.UpdateAsObservable()
            .Select(input => playerActions.Hand3.ReadValue<float>())
            .Subscribe(input =>
            {
                _hand3.Value = input == 1 ? true : false;
            });
    }

    // ---------- InputSystem
    private void OnHand1(InputAction.CallbackContext context)
    {
        _hand1.Value = context.ReadValue<bool>();
    }
}
