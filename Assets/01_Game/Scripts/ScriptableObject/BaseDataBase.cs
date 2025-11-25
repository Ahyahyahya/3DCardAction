using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDataBase<T> : ScriptableObject where T : BaseData
{
    [SerializeField] private List<T> dataList = new();

    public List<T> DataList => dataList;
}
