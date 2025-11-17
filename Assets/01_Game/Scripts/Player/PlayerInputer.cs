using R3;
using R3.Triggers;
using UnityEngine;

public class PlayerInputer : MonoBehaviour, IInputProvider
{
    // ---------- Field
    private InputSystem_Actions _input;

    // ---------- R3
    private ReactiveProperty<Vector2> _move = new();
    public ReadOnlyReactiveProperty<Vector2> MoveButton => _move;

    // ---------- UnityMessage
    private void Start()
    {
        _input = new InputSystem_Actions();

        _input.Enable();

        this.UpdateAsObservable()
            .Select(x => _input.Player.Move.ReadValue<Vector2>())
            .Subscribe(x =>{ _move.OnNext(x) ;})
            .AddTo(this);
    }
}
