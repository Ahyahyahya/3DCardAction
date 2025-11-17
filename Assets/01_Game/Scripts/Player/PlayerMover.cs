using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerMover : MonoBehaviour
{
    // ---------- UnityMessage
    private void Start()
    {
        var inputer = GetComponent<PlayerInputer>();
        var rb = GetComponent<Rigidbody>();

        inputer.MoveButton
            .Subscribe(input =>
            {
                Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

                Vector3 moveForward = cameraForward * input.y + Camera.main.transform.right * input.x;

                rb.linearVelocity = moveForward * 5f + new Vector3(0, rb.linearVelocity.y, 0);

                transform.rotation = Quaternion.LookRotation(cameraForward);
            })
            .AddTo(gameObject);
    }
}
