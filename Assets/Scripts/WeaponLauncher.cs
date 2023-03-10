using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLauncher : Weapon
{
    // ��ó�� �۵� ���.
    // 1. ��ó�� �ִ� �������� 5��.
    // 2. ��ó�� ��¡�ϴ� �ִ� �ð��� 5��.    
    // 3. ��¡�ϴ� �ð��� ����� źȯ�� ���ư��� ���� �޶�����.
    // 4. ��¡ �� ���� �������� �����ϸ� �� �̻� ��¡�� �� ����.
    // 5. �������� ������� ���콺�� ���� ����ü�� ������.

    // �������� ȸ���ϴ� �� => recovery
    // ��ó���� �Ŀ��� ������ �� => charge

    // ��ó�� �������� ���õ� ����.
    const float RECOVERY_TIME = 2f;     // �������� 2�ʿ� ���� ȸ���ȴ�.
    float nextRecoveryTime;             // ���� �� �� �������� ȸ���� �� �ִ� �ð�.

    // ��ó�� ��¡�� ���õ� ����.
    const float MIN_CHARGE_TIME = 0.5f;
    const float MAX_CHARGE_TIME = 5.0f;
    float chargeTime;

    // ����ü�� ������ ��.
    const float MAX_CHARGE_POWER = 20;

    protected new void Update()
    {
        base.Update();

        if(nextRecoveryTime <= Time.time)
        {
            // �̹� �����ӵ��� �帰 �ð� ���� * �ִ� ������.
            float amount = (Time.deltaTime / RECOVERY_TIME) * maxEnergy;
            energy = Mathf.Clamp(energy + amount, 0f, maxEnergy);
        }
    }

    public override void Press(MOUSE mouse)
    {
        // ���� �������� �־�� �Ѵ�.
        if(energy > 0.0f)
        {
            // ��¡ �ð��� �ּ� 0 �ִ� MAX������.
            chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0f, MAX_CHARGE_TIME);
            energy = Mathf.Clamp(energy - Time.deltaTime, 0f, maxEnergy);
        }
    }
    public override void Release(MOUSE mouse)
    {
        // �ּ� ��¡ Ÿ�Ӻ��� Ŀ���Ѵ�.
        if (chargeTime >= MIN_CHARGE_TIME)
        {
            float chargeRatio = chargeTime / MAX_CHARGE_TIME;       // ��¡ ����.
            float chargePower = MAX_CHARGE_POWER * chargeRatio;     // ��¡ �ð��� ���� �Ŀ�(��)

            // ����ü ���� �� �߻�.
            Projectile projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
            projectile.transform.LookAt(GetCameraPoint());      // Ư�� ������ �ٶ����.
            projectile.Fire(Projectile.TYPE.Grenade, power, speed, mask);
        }

        chargeTime = 0f;
    }
}
