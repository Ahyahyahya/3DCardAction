using R3;
using UnityEngine;

public interface IInputProvider
{
    public ReadOnlyReactiveProperty<Vector2> MoveButton { get; }
    public ReadOnlyReactiveProperty<bool> LeftMouseBtn { get; }
    public ReadOnlyReactiveProperty<float> MouseMidBtn { get; }
}
