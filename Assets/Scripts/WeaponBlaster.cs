using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlaster : Weapon
{
    [System.Serializable]
    class EnergySocket
    {
        [SerializeField] Transform socket;

        float totalEnergy;
        float energy;

        public void Start(float _energy)
        {
            totalEnergy = _energy;
            energy = _energy;
        }

        public void Update()
        {
            // 에너지가 몇인가에 따라 소캣의 y축을 움직인다.
            Vector3 local = socket.localPosition;
            local.y = (1f - (energy / totalEnergy)) * 0.1f;
            socket.localPosition = local;
        }

        // 에너지를 사용하겠다.
        public bool Use()
        {
            // 에너지가 없다면 false를 반환.
            if (energy <= 0.0f)
                return false;

            energy = Mathf.Clamp(energy - Time.deltaTime, 0f, totalEnergy);
            return true;
        }
        public bool Charge()
        {
            // 에너지가 가득 찼다면 false를 반환.
            if (energy >= totalEnergy)
                return false;

            energy = Mathf.Clamp(energy + Time.deltaTime, 0f, totalEnergy);
            return true;
        }
    }

    [SerializeField] ParticleSystem muzzleFlashFx;  // 총구 빛 이펙트.
    [SerializeField] AudioSource shotAudio;         // 발사 오디오 (SFX)
    [SerializeField] EnergySocket[] sockets;        // 피스톤 소캣 배열.
    [SerializeField] float rate;                    // 연사 속도.
    [SerializeField] float totalEnergy;             // 총 에너지.

    float nextFireTime;         // 다음 공격 가능 시간.
    float nextChageTime;        // 다음 충전 가능 시간.

    private void Start()    
    {
        // 총 에너지 양을 n등분하여 소켓에게 전달.
        float energy = totalEnergy / sockets.Length;
        foreach(var socket in sockets)
            socket.Start(energy);
    }

    private void Update()
    {
        // 충전이 가능한 시간이 되었다는 것을 의미.
        if (nextChageTime <= Time.time)
        {
            // 4번 소캣부터 거꾸로 충전해야함을 알려준다.
            // 충전을 했다면 true를 반환한다.
            for(int i = sockets.Length - 1; i >= 0; i--)
            {
                if (sockets[i].Charge())
                    break;
            }
        }

        // 모든 에너지 소켓의 Update함수 호출.
        foreach (var socket in sockets)
            socket.Update();
    }

    public override void Press(MOUSE mouse)
    {
        // 에너지 소비.
        bool isEmptyEnergy = true;              // 에너지가 바닥 났는가?
        foreach(var socket in sockets)          // 모든 소켓을 정방향으로 검색.
        {
            if(socket.Use())                    // n번째 소켓에게 에너지를 사용했는지 물어본다.
            {
                isEmptyEnergy = false;          // 사용했다면 에너지가 비어있다는 변수를 flase로 만든다.
                break;                          // 더 검색할 필요가 없으니 foreach문을 빠져나온다.
            }
        }

        nextChageTime = Time.time + 1f;

        // 연사 대기 시간이 남아있다면 사용하지 않는다.
        if (Time.time < nextFireTime || isEmptyEnergy)
            return;

        // 공격 가능 시간 갱신 (+ rate초)
        nextFireTime = Time.time + rate;

        // 투사체 생성 후 발사.
        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        projectile.transform.LookAt(GetCameraPoint());      // 특정 지점을 바라봐라.
        projectile.Fire(power, speed, mask);

        muzzleFlashFx.Play();
        shotAudio.Play();
    }

    public override void Release(MOUSE mouse)
    {

    }
}
