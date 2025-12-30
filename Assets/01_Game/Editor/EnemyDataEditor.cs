using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyCore))]
public class EnemyDataEditor : Editor
{
    private Editor Instance;

    private void OnEnable()
    {
        // インスタンスをリセット
        Instance = null;
    }

    public override void OnInspectorGUI()
    {
        // 検査対象のコンポーネント
        EnemyCore core = (EnemyCore)target;

        if (Instance == null)
        {
            Instance = CreateEditor(core.EnemyData);
        }

        // MonoBehaviorの変数表示
        base.OnInspectorGUI();

        // ScriptableObjectのステータスの描画
        Instance.DrawDefaultInspector();
    }
}
