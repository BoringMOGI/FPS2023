using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ParticleSystem sparkFx;

    const float MAX_DISTANCE = 100f;

    float power;
    float speed;
    LayerMask mask;

    Vector3 createPosition;     // ���� ����.

    private void Start()
    {
        createPosition = transform.position;
    }

    public void Fire(float power, float speed, LayerMask mask)
    {
        this.power = power;
        this.speed = speed;
        this.mask = mask;
    }

    // ���𰡶� �浹���� ��.
    private void OnTriggerEnter(Collider other)
    {
        // GameObject.layer : layer��� �� ���°������ �ǹ� (int����)
        // LayerMask : ���̾� ����� ������ ������ ���� �� �ִ� flag�� (int����)

        // gameObject.layer�� ���� ������ �ƴ� n��° �ڸ����� �ǹ��ϴ� flag�� ��ȯ�� ��
        // LayerMask�� ���ԵǾ� �ִ��� &(�ص�)������ ���� �����Ѵ�.
        if((mask.value & 1 << other.gameObject.layer) > 0)
        {
            OnHit(other.gameObject);
        }
    }

    // FPS(������)�������� ȣ��Ǵ� �Լ�.
    void Update()
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

    private void OnHit(GameObject target)
    {
        ParticleSystem vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();
        Destroy(gameObject);
    }
}
