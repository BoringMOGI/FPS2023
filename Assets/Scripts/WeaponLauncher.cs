using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLauncher : Weapon
{
    enum SE
    {
        Off = -1,
        Start,
        Loop,
        Release,
    }

    [Header("Audio")]
    [SerializeField] AudioSource[] chargeSes;

    Projectile projectileDisk;       // ������ ��ũ.

    // ��ó�� �������� ���õ� ����.
    float MIN_CHARGE_TIME;  // �ּ� ��¡ Ÿ���� ������ ���� 1���� 10% �з� ��ŭ�̴�.
    float MAX_CHARGE_TIME;  // �ִ� ��¡ Ÿ���� ������ ���� 1�� �з� ��ŭ�̴�.
    float chargeTime;       // ���� ��¡�� �ð�.

    protected new void Start()
    {
        base.Start();

        MAX_CHARGE_TIME = MAX_ENERGY / sockets.Length;
        MIN_CHARGE_TIME = MAX_CHARGE_TIME * 0.4f;
    }


    private void PlaySe(SE type)
    {
        // ��û�� type SE�� ����ϰ� �������� ����.
        for(int i = 0; i<chargeSes.Length; i++)
        {
            if (type == (SE)i)
            {
                // ���� ������� �ƴ� ��쿡�� ����� �� �ִ�.
                if (!chargeSes[i].isPlaying)
                    chargeSes[i].Play();
            }
            else
            {
                chargeSes[i].Stop();
            }
        }
    }
    private bool IsPlayingSE(SE type)
    {
        return chargeSes[(int)type].isPlaying;
    }

    public override void Press(MOUSE mouse)
    {
        base.Press(mouse);

        // ���ʿ� ����ü�� ������ �����Ѵ�.
        if (projectileDisk == null)
        {
            projectileDisk = Instantiate(projectilePrefab, muzzle);     // �ѱ�(muzzle) �Ʒ��� ����.
            projectileDisk.transform.localPosition = Vector3.zero;      // ���� ������ �ʱ�ȭ.
            projectileDisk.transform.localScale = Vector3.zero;         // ���� ������ �ʱ�ȭ.

            PlaySe(SE.Start);
        }

        // ���� Ÿ���� �ִ� ����Ÿ�� �̻��̸� �� �̻� ������ �� ����.
        // �� �̻� ����� �������� ���� ���.
        if (chargeTime >= MAX_CHARGE_TIME || !UseEnergy())
        {
            PlaySe(SE.Loop);
            return;
        }

        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0f, MAX_CHARGE_TIME);

        float scale = chargeTime / MAX_CHARGE_TIME;     // 0.0 ~ 1.0 ���̰�.
        projectileDisk.transform.localScale = new Vector3(scale, scale, scale);
    }
    public override void Release(MOUSE mouse)
    {
        // Disk�� �ֻ��� ��ü�� �����.
        projectileDisk.transform.parent = null;

        // �ּ� ��¡ Ÿ�Ӻ��� Ŀ���Ѵ�.
        if (chargeTime >= MIN_CHARGE_TIME)
        {
            float chargeRatio = chargeTime / MAX_CHARGE_TIME;       // ��¡ ���� (0f ~ 1f)
            float chargePower = speed * chargeRatio;                // ��¡ �ð��� ����� ��

            // źȯ�� �߻�.
            projectileDisk.Fire(power, chargePower, mask);

            // ���� ����Ʈ ���.            
            PlaySe(SE.Release);
        }
        else
        {
            // �������� �ּҸ� ���� ���� �߻��� �� ���� ������ �����Ѵ�.
            PlaySe(SE.Off);
            Destroy(projectileDisk.gameObject);
        }

        chargeTime = 0f;
        projectileDisk = null;
    }
}
