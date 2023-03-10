using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLauncher : Weapon
{
    // 런처의 작동 방식.
    // 1. 런처의 최대 에너지는 5다.
    // 2. 런처가 차징하는 최대 시간은 5다.    
    // 3. 차징하는 시간에 비례해 탄환이 날아가는 힘이 달라진다.
    // 4. 차징 시 남은 에너지가 부족하면 더 이상 차징할 수 없다.
    // 5. 에너지에 관계없이 마우스를 때면 투사체를 날린다.

    // 에너지를 회복하는 것 => recovery
    // 런처에서 파워를 모으는 것 => charge

    // 런처의 에너지에 관련된 변수.
    const float RECOVERY_TIME = 2f;     // 에너지는 2초에 걸쳐 회복된다.
    float nextRecoveryTime;             // 총을 쏜 후 에너지를 회복할 수 있는 시간.

    // 런처의 차징과 관련된 변수.
    const float MIN_CHARGE_TIME = 0.5f;
    const float MAX_CHARGE_TIME = 5.0f;
    float chargeTime;

    // 투사체를 날리는 힘.
    const float MAX_CHARGE_POWER = 20;

    protected new void Update()
    {
        base.Update();

        if(nextRecoveryTime <= Time.time)
        {
            // 이번 프레임동안 흐린 시간 비율 * 최대 에너지.
            float amount = (Time.deltaTime / RECOVERY_TIME) * maxEnergy;
            energy = Mathf.Clamp(energy + amount, 0f, maxEnergy);
        }
    }

    public override void Press(MOUSE mouse)
    {
        // 남은 에너지가 있어야 한다.
        if(energy > 0.0f)
        {
            // 차징 시간은 최소 0 최대 MAX까지다.
            chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0f, MAX_CHARGE_TIME);
            energy = Mathf.Clamp(energy - Time.deltaTime, 0f, maxEnergy);
        }
    }
    public override void Release(MOUSE mouse)
    {
        // 최소 차징 타임보다 커야한다.
        if (chargeTime >= MIN_CHARGE_TIME)
        {
            float chargeRatio = chargeTime / MAX_CHARGE_TIME;       // 차징 비율.
            float chargePower = MAX_CHARGE_POWER * chargeRatio;     // 차징 시간에 따른 파워(힘)

            // 투사체 생성 후 발사.
            Projectile projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
            projectile.transform.LookAt(GetCameraPoint());      // 특정 지점을 바라봐라.
            projectile.Fire(Projectile.TYPE.Grenade, power, speed, mask);
        }

        chargeTime = 0f;
    }
}
