using UnityEngine;

public class FrontCardEffect : BaseCardEffect
{
    [SerializeField] private Vector3 _rotOffset;

    [SerializeField] private float _createPosOffset = 3f;

    public override void ActivateCardEffect()
    {
        var cameraTr = Camera.main.transform;

        var playerData = PlayerDataProvider.Instance;

        // î≠éÀà íuí≤êÆ
        transform.localPosition =
            cameraTr.localPosition + cameraTr.forward * _createPosOffset;

        // î≠éÀäpìxí≤êÆ
        transform.localRotation = Quaternion.LookRotation(playerData.transform.forward);

        transform.eulerAngles += _rotOffset;

        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(
            ray,
            out var hit,
            100f,
            LayerMask.GetMask("Ground")))
        {
            Debug.Log("AAA");

            transform.localPosition = new Vector3(
                transform.localPosition.x,
                hit.point.y,
                transform.localPosition.z);
        }
    }
}
