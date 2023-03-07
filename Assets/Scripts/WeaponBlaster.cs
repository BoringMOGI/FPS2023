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
            // �������� ���ΰ��� ���� ��Ĺ�� y���� �����δ�.
            Vector3 local = socket.localPosition;
            local.y = (1f - (energy / totalEnergy)) * 0.1f;
            socket.localPosition = local;
        }

        // �������� ����ϰڴ�.
        public bool Use()
        {
            // �������� ���ٸ� false�� ��ȯ.
            if (energy <= 0.0f)
                return false;

            energy = Mathf.Clamp(energy - Time.deltaTime, 0f, totalEnergy);
            return true;
        }
        public bool Charge()
        {
            // �������� ���� á�ٸ� false�� ��ȯ.
            if (energy >= totalEnergy)
                return false;

            energy = Mathf.Clamp(energy + Time.deltaTime, 0f, totalEnergy);
            return true;
        }
    }

    [SerializeField] ParticleSystem muzzleFlashFx;  // �ѱ� �� ����Ʈ.
    [SerializeField] AudioSource shotAudio;         // �߻� ����� (SFX)
    [SerializeField] EnergySocket[] sockets;        // �ǽ��� ��Ĺ �迭.
    [SerializeField] float rate;                    // ���� �ӵ�.
    [SerializeField] float totalEnergy;             // �� ������.

    float nextFireTime;         // ���� ���� ���� �ð�.
    float nextChageTime;        // ���� ���� ���� �ð�.

    private void Start()    
    {
        // �� ������ ���� n����Ͽ� ���Ͽ��� ����.
        float energy = totalEnergy / sockets.Length;
        foreach(var socket in sockets)
            socket.Start(energy);
    }

    private void Update()
    {
        // ������ ������ �ð��� �Ǿ��ٴ� ���� �ǹ�.
        if (nextChageTime <= Time.time)
        {
            // 4�� ��Ĺ���� �Ųٷ� �����ؾ����� �˷��ش�.
            // ������ �ߴٸ� true�� ��ȯ�Ѵ�.
            for(int i = sockets.Length - 1; i >= 0; i--)
            {
                if (sockets[i].Charge())
                    break;
            }
        }

        // ��� ������ ������ Update�Լ� ȣ��.
        foreach (var socket in sockets)
            socket.Update();
    }

    public override void Press(MOUSE mouse)
    {
        // ������ �Һ�.
        bool isEmptyEnergy = true;              // �������� �ٴ� ���°�?
        foreach(var socket in sockets)          // ��� ������ ���������� �˻�.
        {
            if(socket.Use())                    // n��° ���Ͽ��� �������� ����ߴ��� �����.
            {
                isEmptyEnergy = false;          // ����ߴٸ� �������� ����ִٴ� ������ flase�� �����.
                break;                          // �� �˻��� �ʿ䰡 ������ foreach���� �������´�.
            }
        }

        nextChageTime = Time.time + 1f;

        // ���� ��� �ð��� �����ִٸ� ������� �ʴ´�.
        if (Time.time < nextFireTime || isEmptyEnergy)
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
