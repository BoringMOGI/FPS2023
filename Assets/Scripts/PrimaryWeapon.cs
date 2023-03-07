using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : Weapon
{
    [SerializeField] float attackRate;      // 공격 속도(주기)
    [SerializeField] float maxEnergy;       // 최대 에너지.
    [SerializeField] float energy;          // 현재 에너지.

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
        // Time.time : 게임이 시작되고 지금까지 흐른 시간.
        // 투사체를 발사했을 때 현재 시간 + 공격 주기를 대입해 연속으로 나가지 않도록 처리.
        if(nextRateTime <= Time.time)
        {
            nextRateTime = Time.time + attackRate;      // 다음 공격 가능 시간 (현재 시간 + 공격 주기)
            Debug.Log("공격!");
        }
    }
}
