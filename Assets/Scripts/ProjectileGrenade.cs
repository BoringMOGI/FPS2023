using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGrenade : Projectile
{
    [SerializeField] float explodeRange;

    protected new void Update()
    {
        base.Update();

        // �߻� ���°� �ƴϸ� return.
        if (!isFire)
            return;

        // ��ź ����ü�� �������� �׷����ϱ� ������ �߷� ���ӵ��� �����ش�.
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
