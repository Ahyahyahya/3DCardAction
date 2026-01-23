using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Event/ChangeHP")]
public class ChangeHP : BaseEvent
{
    [SerializeField] private int _amount;
    public int Amount => _amount;
    public override void Execute()
    {
        PlayerDataProvider.Instance.TakeHeal(_amount);
    }

}
