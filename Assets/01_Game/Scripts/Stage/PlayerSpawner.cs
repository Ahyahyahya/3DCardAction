using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // ---------- UnityMessage
    private void Start()
    {
        var playerData = PlayerDataProvider.Instance;

        playerData.transform.localPosition = transform.position;
    }
}
