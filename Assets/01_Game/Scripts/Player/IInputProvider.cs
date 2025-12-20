using R3;
using UnityEngine;

public interface IInputProvider
{
    public ReadOnlyReactiveProperty<Vector2> MoveButton { get; }
    public ReadOnlyReactiveProperty<bool> Hand1Button { get; }
    public ReadOnlyReactiveProperty<bool> Hand2Button { get; }
    public ReadOnlyReactiveProperty<bool> Hand3Button { get; }
    public ReadOnlyReactiveProperty<float> MouseMidBtn { get; }
}
