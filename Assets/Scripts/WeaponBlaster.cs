using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlaster : Weapon
{
    [SerializeField] ParticleSystem muzzleFlashFx;  // �ѱ� �� ����Ʈ.
    [SerializeField] float rate;                    // ���� �ӵ�.

    float delayTime;

    private void Update()
    {
        delayTime = Mathf.Clamp(delayTime - Time.deltaTime, 0f, rate);
    }

    public override void Press(MOUSE mouse)
    {
        // ���� ��� �ð��� �����ִٸ� ������� �ʴ´�.
        if (delayTime > 0f)
            return;

        delayTime = rate;

        // ����ü ���� �� �߻�.
        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        projectile.Fire(power, speed, mask);
        muzzleFlashFx.Play();
    }

    public override void Release(MOUSE mouse)
    {
       
    }
}
