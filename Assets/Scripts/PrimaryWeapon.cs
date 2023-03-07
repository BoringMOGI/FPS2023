using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : Weapon
{
    [SerializeField] float attackRate;      // ���� �ӵ�(�ֱ�)
    [SerializeField] float maxEnergy;       // �ִ� ������.
    [SerializeField] float energy;          // ���� ������.

    float nextRateTime;

    public override void Press(MOUSE mouse)
    {
        if(mouse == MOUSE.Left)
        {
            Fire();
        }
        else if(mouse == MOUSE.Right)
        {

        }
    }

    public override void Release(MOUSE mouse)
    {
        
    }

    private void Fire()
    {
        // Time.time : ������ ���۵ǰ� ���ݱ��� �帥 �ð�.
        // ����ü�� �߻����� �� ���� �ð� + ���� �ֱ⸦ ������ �������� ������ �ʵ��� ó��.
        if(nextRateTime <= Time.time)
        {
            nextRateTime = Time.time + attackRate;      // ���� ���� ���� �ð� (���� �ð� + ���� �ֱ�)
            Debug.Log("����!");
        }
    }
}
