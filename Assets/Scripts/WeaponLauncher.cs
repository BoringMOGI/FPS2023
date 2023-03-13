using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLauncher : Weapon
{
    Projectile projectileDisk;       // 에너지 디스크.

    // 런처의 에너지에 관련된 변수.
    float MIN_CHARGE_TIME;  // 최소 차징 타임은 에너지 소켓 1개의 10% 분량 만큼이다.
    float MAX_CHARGE_TIME;  // 최대 차징 타임은 에너지 소켓 1개 분량 만큼이다.
    float chargeTime;       // 실제 차징한 시간.

    protected new void Start()
    {
        base.Start();

        MAX_CHARGE_TIME = MAX_ENERGY / sockets.Length;
        MIN_CHARGE_TIME = MAX_CHARGE_TIME * 0.4f;
    }

    public override void Press(MOUSE mouse)
    {
        base.Press(mouse);

        // 최초에 투사체가 없으면 생성한다.
        if (projectileDisk == null)
        {
            projectileDisk = Instantiate(projectilePrefab, muzzle);     // 총구(muzzle) 아래로 생성.
            projectileDisk.transform.localPosition = Vector3.zero;      // 로컬 포지션 초기화.
            projectileDisk.transform.localScale = Vector3.zero;         // 로컬 스케일 초기화.
        }

        // 차지 타임이 최대 차지타임 이상이면 더 이상 차지할 수 없다.
        // 더 이상 사용할 에너지가 없을 경우.
        if (chargeTime >= MAX_CHARGE_TIME || !UseEnergy())
            return;

        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0f, MAX_CHARGE_TIME);

        float scale = chargeTime / MAX_CHARGE_TIME;     // 0.0 ~ 1.0 사이값.
        projectileDisk.transform.localScale = new Vector3(scale, scale, scale);
    }
    public override void Release(MOUSE mouse)
    {
        // Disk를 최상위 객체로 만든다.
        projectileDisk.transform.parent = null;

        // 최소 차징 타임보다 커야한다.
        if (chargeTime >= MIN_CHARGE_TIME)
        {
            float chargeRatio = chargeTime / MAX_CHARGE_TIME;       // 차징 비율 (0f ~ 1f)
            float chargePower = speed * chargeRatio;                // 차징 시간에 비례한 힘

            // 탄환을 발사.
            projectileDisk.Fire(power, chargePower, mask);
        }
        else
        {
            // 에너지가 최소를 넘지 못해 발사할 수 없기 때문에 삭제한다.
            Destroy(projectileDisk.gameObject);
        }

        chargeTime = 0f;
        projectileDisk = null;
    }
}
