using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected ParticleSystem sparkFx;        // 충돌 이펙트.

    protected const float MAX_DISTANCE = 100f;    // 최대 거리.
    protected const float GRAVITY = -9.81f;       // 중력.

    protected float power;              // 데미지.
    protected float speed;              // 속도.
    protected LayerMask mask;           // 레이어 마스크.
    protected Vector3 createPosition;   // 생성 지점.
    protected Vector3 velocity;         // 중력 가속도.

    protected bool isFire;              // 발사 여부.

    public void Fire(float power, float speed, LayerMask mask)
    {
        createPosition = transform.position;

        this.power = power;
        this.speed = speed;
        this.mask = mask;

        isFire = true;
    }

    // 무언가랑 충돌했을 때.
    private void OnTriggerEnter(Collider other)
    {
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
    protected void Update()
    {
        if (!isFire)
            return;

        RaycastHit hit;

        // 다음 위치는 나의 운동량 + 중력 가속도.
        // 그러나 일반 Bullet은 중력값을 받지 않는다.
        Vector3 nextPoint = transform.position + transform.forward * speed * Time.deltaTime;
        nextPoint += velocity;

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
            transform.LookAt(nextPoint);            // 다음으로 움직일 위치를 바라본다.
            transform.position = nextPoint;         // 이후 내 실제 위치를 변경한다.
        }
    }

    protected virtual void OnHit(GameObject target)
    {
        ParticleSystem vfx = Instantiate(sparkFx, transform.position, transform.rotation);
        vfx.Play();

        // 충돌한 대상이 IHit을 구현하고 있다면 OnHit함수를 호출해 피격 처리.
        target.GetComponent<IHit>()?.OnHit(power);

        Destroy(gameObject);
    }
}
