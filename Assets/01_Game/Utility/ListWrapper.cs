using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListWrapper<T>
{
    [SerializeField] private List<T> list = new();
    public List<T> List => list;
}
