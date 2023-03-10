using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum TYPE
    {
        Bullet,     // 직선.
        Grenade,    // 포물선.
    }


    [SerializeField] ParticleSystem sparkFx;

    const float MAX_DISTANCE = 100f;

    float power;
    float speed;
    LayerMask mask;
    TYPE type;

    Vector3 createPosition;     // 생성 지점.

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

    // 무언가랑 충돌했을 때.
    private void OnTriggerEnter(Collider other)
    {
        if (type != TYPE.Bullet)
            return;

        // GameObject.layer : layer목록 중 몇번째인지를 의미 (int정수)
        // LayerMask : 레이어 목록을 여러개 가지고 있을 수 있는 flag값 (int정수)

        // gameObject.layer의 값을 정수가 아닌 n번째 자릿수를 의미하는 flag로 변환한 후
        // LayerMask에 포함되어 있는지 &(앤드)연산을 통해 검출한다.
        if ((mask.value & 1 << other.gameObject.layer) > 0)
        {
            OnHit(other.gameObject);
        }
    }

    // FPS(프레임)기준으로 호출되는 함수.
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
            // 투사체가 최대 이동 거리를 넘어버리면 삭제.
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
    private void UpdateGrenade()
    {

    }

    private void OnHit(GameObject target)
    {
        ParticleSystem vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();

        // 충돌한 대상이 IHit을 구현하고 있다면 OnHit함수를 호출해 피격 처리.
        target.GetComponent<IHit>()?.OnHit(power);

        Destroy(gameObject);
    }
}
