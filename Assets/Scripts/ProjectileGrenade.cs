using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGrenade : Projectile
{
    [SerializeField] TrailRenderer trilRenderer;
    [SerializeField] float minExpldeRange;
    [SerializeField] float maxExplodeRange;

    float energyRatio;    // ������ ����. (0 ~ 1)

    private void Start()
    {
        trilRenderer.enabled = false;
    }

    public override void Fire(float power, float speed, LayerMask mask)
    {
        this.power = power;
        this.mask = mask;

        energyRatio = speed / 20f;

        // ������ٵ��� AddForce�� �̿��� �������� speed��ŭ ������.
        createPosition = transform.position;
        rigid.AddForce(transform.forward * speed, ForceMode.Impulse);
        rigid.useGravity = true;
        isFire = true;

        trilRenderer.enabled = true;
    }

    protected override void OnHit(GameObject target)
    {
        if (!isFire)
            return;

        Vfx vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play(energyRatio * maxExplodeRange);


        // ���� ������ �ּ�~�ִ� ���̰� ���.
        // ������ ������ ���� ���� ������ ��������.
        float explodeRange = minExpldeRange + ((maxExplodeRange - minExpldeRange) * energyRatio);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange * energyRatio, mask);
        foreach(Collider collider in colliders)
        {
            IHit hit = collider.GetComponent<IHit>();
            if (hit != null)
                hit.OnHit(power);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minExpldeRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxExplodeRange);
    }
}
