using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum MOUSE
    {
        Left,
        Right,
    }

    [SerializeField] protected Transform muzzle;                // 총구.
    [SerializeField] protected Projectile projectilePrefab;     // 투사체 프리팹.

    [Header("Info")]
    [SerializeField] protected float power;
    [SerializeField] protected float speed;
    [SerializeField] protected LayerMask mask;

    // abstract함수 : 선언문만 존재하는 함수.
    public abstract void Press(MOUSE mouse);            // 마우스를 눌렀을때.
    public abstract void Release(MOUSE mouse);          // 마우스를 놓았을때.
}
