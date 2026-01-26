using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // ---------- SerializeField
    [SerializeField] private List<Transform> _spawnTransfroms = new();

    // ---------- UnityMessage
    private void Start()
    {
        var enemyManager = EnemyManager.Instance;

        var enemyDataStore = FindAnyObjectByType<EnemyDataStore>();

        foreach(var tr in _spawnTransfroms)
        {
            var enemyId = Random.Range(0, enemyDataStore.GetCount);

            var targetEnemy = enemyDataStore.FindWithIndex(enemyId).EnemyPrefab;

            var clone = Instantiate(
                targetEnemy,
                tr.position,
                Quaternion.identity,
                enemyManager.transform);

            enemyManager.AddEnemy(clone);
        }
    }

    // ---------- Method
    [ContextMenu("AddChildrenIntoList")]
    public void GetChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (_spawnTransfroms.Contains(transform.GetChild(i))) continue;

            _spawnTransfroms.Add(transform.GetChild(i));
        }
    }
}
