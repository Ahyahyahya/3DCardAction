using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private GameObject _stage;

    private GameObject _currentStage;

    // ---------- Method
    public void GenerateStage()
    {
        if (_currentStage != null)
        {
            Destroy(_currentStage);
        }

        _currentStage = Instantiate(_stage);
    }
}
