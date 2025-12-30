using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/Enemy")]
public class EnemyData : BaseData
{
    [SerializeField] private EnemyCore _enemyPrefab;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _atk;

    public EnemyCore EnemyPrefab => _enemyPrefab;
    public int MaxHp => _maxHp;
    public int Atk => _atk;
}
