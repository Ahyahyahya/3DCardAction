using R3;
using UnityEngine;

public class PlayerCore : MonoBehaviour, IDamageble
{
    // ---------- Field
    [SerializeField]
    private SerializableReactiveProperty<int> _hp = new(100);
    public ReadOnlyReactiveProperty<int> Hp => _hp;

    // ---------- Interface
    public void TakeDamage(int damage)
    {
        _hp.Value -= damage;
    }
}
