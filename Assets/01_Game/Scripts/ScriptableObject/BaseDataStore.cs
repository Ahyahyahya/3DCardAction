using UnityEngine;

public class BaseDataStore<T, U> : MonoBehaviour where T : BaseDataBase<U> where U : BaseData
{
    [SerializeField] protected T dataBase;

    protected T DataBase => dataBase;

    public U FindWithName(string name)
    {
        if (string.IsNullOrEmpty(name)) return default;

        return dataBase.DataList.Find(e => e.DataName == name);
    }

    public U FindWithName(int id)
    {
        return dataBase.DataList.Find(e => e.Id == id);
    }
}
