using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlaster : Weapon
{
    [SerializeField] ParticleSystem muzzleFlashFx;  // 총구 빛 이펙트.
    [SerializeField] float rate;                    // 연사 속도.

    float delayTime;

    private void Update()
    {
        delayTime = Mathf.Clamp(delayTime - Time.deltaTime, 0f, rate);
    }

    public override void Press(MOUSE mouse)
    {
        // 연사 대기 시간이 남아있다면 사용하지 않는다.
        if (delayTime > 0f)
            return;

        delayTime = rate;

        // 투사체 생성 후 발사.
        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        projectile.Fire(power, speed, mask);
        muzzleFlashFx.Play();
    }

    public override void Release(MOUSE mouse)
    {
       
    }
}
