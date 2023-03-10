using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum TYPE
    {
        Bullet,     // ����.
        Grenade,    // ������.
    }


    [SerializeField] ParticleSystem sparkFx;

    const float MAX_DISTANCE = 100f;

    float power;
    float speed;
    LayerMask mask;
    TYPE type;

    Vector3 createPosition;     // ���� ����.

    private void Start()
    {
        createPosition = transform.position;
    }

    public void Fire(TYPE type, float power, float speed, LayerMask mask)
    {
        this.type = type;
        this.power = power;
        this.speed = speed;
        this.mask = mask;
    }

    // ���𰡶� �浹���� ��.
    private void OnTriggerEnter(Collider other)
    {
        if (type != TYPE.Bullet)
            return;

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
    void Update()
    {
        switch(type)
        {
            case TYPE.Bullet:
                UpdateBullet();
                break;
            case TYPE.Grenade:
                UpdateGrenade();
                break;
        }
    }

    private void UpdateBullet()
    {
        RaycastHit hit;
        Vector3 nextPoint = transform.position + transform.forward * speed * Time.deltaTime;
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
            // Translate�� �� ���� z������ forward ������ ����.
            // position�� ���� ���� ������ ����.
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //transform.position += (transform.forward * speed * Time.deltaTime);
        }
    }
    private void UpdateGrenade()
    {

    }

    private void OnHit(GameObject target)
    {
        ParticleSystem vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();

        // �浹�� ����� IHit�� �����ϰ� �ִٸ� OnHit�Լ��� ȣ���� �ǰ� ó��.
        target.GetComponent<IHit>()?.OnHit(power);

        Destroy(gameObject);
    }
}
