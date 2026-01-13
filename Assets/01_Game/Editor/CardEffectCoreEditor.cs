using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardEffectCore))]
public class CardEffectCoreEditor : Editor
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
        var core = (CardEffectCore)target;

        if (Instance == null)
        {
            Instance = CreateEditor(core.CardData);
        }

        // MonoBehaviorの変数表示
        base.OnInspectorGUI();

        // ScriptableObjectのステータスの描画
        Instance.DrawDefaultInspector();
    }
}
