using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    [SerializeField] private DrawLine _dl;
    [SerializeField] private float _weight;

    public void GenerateLine(Vector2 pos1, Vector2 pos2)
    {
        var line = Instantiate(_dl, transform);
        line.SetLine(pos1, pos2, _weight);
    }
}
