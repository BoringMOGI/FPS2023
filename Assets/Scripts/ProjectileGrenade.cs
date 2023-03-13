using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGrenade : Projectile
{
    [SerializeField] float explodeRange;

    protected new void Update()
    {
        base.Update();

        // 발사 상태가 아니면 return.
        if (!isFire)
            return;

        // 폭탄 투사체는 포물선을 그려야하기 때문에 중력 가속도를 더해준다.
        velocity.y += GRAVITY * Time.deltaTime;
    }
    protected override void OnHit(GameObject target)
    {
        if (!isFire)
            return;

        ParticleSystem vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange, mask);
        foreach(Collider collider in colliders)
        {
            IHit hit = collider.GetComponent<IHit>();
            if (hit != null)
                hit.OnHit(power);
        }

        Destroy(gameObject);
    }
}
