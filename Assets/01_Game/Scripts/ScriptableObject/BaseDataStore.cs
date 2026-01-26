using UnityEngine;

public class BaseDataStore<T, U> : MonoBehaviour where T : BaseDataBase<U> where U : BaseData
{
    [SerializeField] protected T dataBase;

    protected T DataBase => dataBase;

    public int GetCount => dataBase.DataList.Count;

    public U FindWithName(string name)
    {
        if (string.IsNullOrEmpty(name)) return default;

        return dataBase.DataList.Find(e => e.DataName == name);
    }

    public U FindWithId(int id)
    {
        return dataBase.DataList.Find(e => e.Id == id);
    }

    public U FindWithIndex(int index)
    {
        return dataBase.DataList[index];
    }
}
