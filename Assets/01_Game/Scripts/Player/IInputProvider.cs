using R3;
using UnityEngine;

public interface IInputProvider
{
    public ReadOnlyReactiveProperty<Vector2> MoveButton { get; }
}
