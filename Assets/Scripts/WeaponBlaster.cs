using UnityEngine;

public class WeaponBlaster : Weapon
{
    [SerializeField] ParticleSystem muzzleFlashFx;  // 총구 빛 이펙트.
    [SerializeField] AudioSource shotAudio;         // 발사 오디오 (SFX)    
    [SerializeField] float rate;                    // 연사 속도.

    float nextFireTime;         // 다음 공격 가능 시간.

    // 마우스 컨트롤.
    public override void Press(MOUSE mouse)
    {
        base.Press(mouse);

        // 에너지 소비.
        bool isUseEnergy = UseEnergy();

        // 연사 대기 시간이 남아있다면 사용하지 않는다.
        if (Time.time < nextFireTime || !isUseEnergy)
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
