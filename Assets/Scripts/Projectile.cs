using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    const float MAX_DISTANCE = 1000f;

    float power;
    float speed;
    LayerMask mask;

    Vector3 createPosition;     // 생성 지점.

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

    // 무언가랑 충돌했을 때.
    private void OnTriggerEnter(Collider other)
    {
        // GameObject.layer : layer목록 중 몇번째인지를 의미 (int정수)
        // LayerMask : 레이어 목록을 여러개 가지고 있을 수 있는 flag값 (int정수)

        // gameObject.layer의 값을 정수가 아닌 n번째 자릿수를 의미하는 flag로 변환한 후
        // LayerMask에 포함되어 있는지 &(앤드)연산을 통해 검출한다.
        if((mask.value & 1 << other.gameObject.layer) > 0)
        {
            Debug.Log($"충돌함:{other.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 투사체가 최대 이동 거리를 넘어버리면 삭제.
        if(Vector3.Distance(createPosition, transform.position) >= MAX_DISTANCE)
        {
            Destroy(gameObject);
        }
        else
        {
            // Translate는 내 기준 z축으로 forward 방향이 정면.
            // position은 월드 기준 정면이 정면.
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //transform.position += (transform.forward * speed * Time.deltaTime);
        }
    }
}
