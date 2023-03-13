using UnityEngine;

public class WeaponBlaster : Weapon
{
    [SerializeField] ParticleSystem muzzleFlashFx;  // �ѱ� �� ����Ʈ.
    [SerializeField] AudioSource shotAudio;         // �߻� ����� (SFX)    
    [SerializeField] float rate;                    // ���� �ӵ�.

    float nextFireTime;         // ���� ���� ���� �ð�.

    // ���콺 ��Ʈ��.
    public override void Press(MOUSE mouse)
    {
        base.Press(mouse);

        // ������ �Һ�.
        bool isUseEnergy = UseEnergy();

        // ���� ��� �ð��� �����ִٸ� ������� �ʴ´�.
        if (Time.time < nextFireTime || !isUseEnergy)
            return;

        // ���� ���� �ð� ���� (+ rate��)
        nextFireTime = Time.time + rate;

        // ����ü ���� �� �߻�.
        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        projectile.transform.LookAt(GetCameraPoint());      // Ư�� ������ �ٶ����.
        projectile.Fire(power, speed, mask);

        muzzleFlashFx.Play();
        shotAudio.Play();
    }
    public override void Release(MOUSE mouse)
    {

    }
}
