using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Event/ChangeMoney")]
public class ChangeMoney : BaseEvent
{
    [SerializeField] private int _amount;

    public int Amount => _amount;

    public override void Execute()
    {
        PlayerDataProvider.Instance.PlusMoney(_amount);
    }
}
