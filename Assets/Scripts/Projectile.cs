using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Vfx sparkFx;        // �浹 ����Ʈ.
    [SerializeField] protected Rigidbody rigid;    // ������ٵ�.

    protected const float MAX_DISTANCE = 100f;    // �ִ� �Ÿ�.

    protected float power;              // ������.
    protected LayerMask mask;           // ���̾� ����ũ.
    protected Vector3 createPosition;   // ���� ����.

    protected bool isFire;              // �߻� ����.

    public virtual void Fire(float power, float speed, LayerMask mask)
    {
        this.power = power;
        this.mask = mask;

        createPosition = transform.position;          // ���� ��ġ.
        rigid.velocity = transform.forward * speed;   // ���� ȸ�� ���� �������� ��� ��Ѵ�. (�߷� X)
        rigid.useGravity = false;
        isFire = true;
    }

    // ���𰡶� �浹���� ��.
    private void OnTriggerEnter(Collider other)
    {
        // GameObject.layer : layer��� �� ���°������ �ǹ� (int����)
        // LayerMask : ���̾� ����� ������ ������ ���� �� �ִ� flag�� (int����)

        // gameObject.layer�� ���� ������ �ƴ� n��° �ڸ����� �ǹ��ϴ� flag�� ��ȯ�� ��
        // LayerMask�� ���ԵǾ� �ִ��� &(�ص�)������ ���� �����Ѵ�.
        if ((mask.value & 1 << other.gameObject.layer) > 0)
        {
            OnHit(other.gameObject);
        }
    }

    // FPS(������)�������� ȣ��Ǵ� �Լ�.
    protected void Update()
    {
        if (!isFire)
            return;

        // ����ü�� �ִ� �̵� �Ÿ��� �Ѿ������ ����.
        if (Vector3.Distance(createPosition, transform.position) >= MAX_DISTANCE)
            Destroy(gameObject);
    }

    protected virtual void OnHit(GameObject target)
    {
        Vfx vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();

        // �浹�� ����� IHit�� �����ϰ� �ִٸ� OnHit�Լ��� ȣ���� �ǰ� ó��.
        target.GetComponent<IHit>()?.OnHit(power);
        Destroy(gameObject);
    }
}
