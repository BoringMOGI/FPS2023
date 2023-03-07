using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    const float MAX_DISTANCE = 1000f;

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
            Debug.Log($"�浹��:{other.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // ����ü�� �ִ� �̵� �Ÿ��� �Ѿ������ ����.
        if(Vector3.Distance(createPosition, transform.position) >= MAX_DISTANCE)
        {
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
}
