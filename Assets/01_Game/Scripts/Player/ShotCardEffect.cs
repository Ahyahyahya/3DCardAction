using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShotCardEffect : BaseCardEffect
{
    public override void ActivateCardEffect()
    {
        var camera = Camera.main;

        var centerRay = camera.ViewportPointToRay(camera.rect.center);

        var shotDir = centerRay.direction;

        var rb = GetComponent<Rigidbody>();

        this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                rb.AddForce(shotDir * 5f);
            })
            .AddTo(gameObject);

        this.OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log("ìGÇ…É_ÉÅÅ[ÉWÇó^Ç¶ÇΩÇÊ");

                    Destroy(gameObject);
                }
            })
            .AddTo(gameObject);

        Destroy(gameObject, 3f);
    }
}
