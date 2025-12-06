using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private List<EnemyCore> _enemies = new();

    // ---------- Method
    [ContextMenu("FindEnemies")]
    private void FindEnemiesInScene()
    {
        var enemies = FindObjectsByType<EnemyCore>(FindObjectsSortMode.None);

        foreach(var enemy in enemies)
        {
            _enemies.Add(enemy);
        }
    }
}
