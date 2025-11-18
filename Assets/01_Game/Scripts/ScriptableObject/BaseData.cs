using UnityEngine;

public abstract class BaseData : ScriptableObject
{
    [SerializeField] private string dataName;

    [SerializeField] private int id;

    public string DataName
    {
        get => dataName;
        set => dataName = value;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }
}
