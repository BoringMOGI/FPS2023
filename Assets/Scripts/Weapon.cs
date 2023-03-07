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

    [SerializeField] protected Transform muzzle;                // �ѱ�.
    [SerializeField] protected Projectile projectilePrefab;     // ����ü ������.

    [Header("Info")]
    [SerializeField] protected float power;
    [SerializeField] protected float speed;
    [SerializeField] protected LayerMask mask;

    // abstract�Լ� : ���𹮸� �����ϴ� �Լ�.
    public abstract void Press(MOUSE mouse);            // ���콺�� ��������.
    public abstract void Release(MOUSE mouse);          // ���콺�� ��������.
}
