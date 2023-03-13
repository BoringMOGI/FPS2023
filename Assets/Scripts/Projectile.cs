using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected ParticleSystem sparkFx;        // �浹 ����Ʈ.

    protected const float MAX_DISTANCE = 100f;    // �ִ� �Ÿ�.
    protected const float GRAVITY = -9.81f;       // �߷�.

    protected float power;              // ������.
    protected float speed;              // �ӵ�.
    protected LayerMask mask;           // ���̾� ����ũ.
    protected Vector3 createPosition;   // ���� ����.
    protected Vector3 velocity;         // �߷� ���ӵ�.

    protected bool isFire;              // �߻� ����.

    public void Fire(float power, float speed, LayerMask mask)
    {
        createPosition = transform.position;

        this.power = power;
        this.speed = speed;
        this.mask = mask;

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

        RaycastHit hit;

        // ���� ��ġ�� ���� ��� + �߷� ���ӵ�.
        // �׷��� �Ϲ� Bullet�� �߷°��� ���� �ʴ´�.
        Vector3 nextPoint = transform.position + transform.forward * speed * Time.deltaTime;
        nextPoint += velocity;

        if (Physics.Linecast(transform.position, nextPoint, out hit, mask))
        {
            OnHit(hit.collider.gameObject);
        }
        else if (Vector3.Distance(createPosition, transform.position) >= MAX_DISTANCE)
        {
            // ����ü�� �ִ� �̵� �Ÿ��� �Ѿ������ ����.
            Destroy(gameObject);
        }
        else
        {
            transform.LookAt(nextPoint);            // �������� ������ ��ġ�� �ٶ󺻴�.
            transform.position = nextPoint;         // ���� �� ���� ��ġ�� �����Ѵ�.
        }
    }

    protected virtual void OnHit(GameObject target)
    {
        ParticleSystem vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();

        // �浹�� ����� IHit�� �����ϰ� �ִٸ� OnHit�Լ��� ȣ���� �ǰ� ó��.
        target.GetComponent<IHit>()?.OnHit(power);

        Destroy(gameObject);
    }
}
