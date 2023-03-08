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
    [SerializeField] protected WeaponUI weaponUi;               // �ѱ� UI.

    [Header("Info")]
    [SerializeField] protected float power;
    [SerializeField] protected float speed;
    [SerializeField] protected LayerMask mask;

    // abstract�Լ� : ���𹮸� �����ϴ� �Լ�.
    public abstract void Press(MOUSE mouse);            // ���콺�� ��������.
    public abstract void Release(MOUSE mouse);          // ���콺�� ��������.

    // 1. ī�޶� �ٶ󺸴� ���� ���ο��� �浹�� ��ġ
    // 2. ī�޶� �ٶ󺸴� ���� ���ο��� �浹�� ������Ʈ.

    private bool RaycastCamera(out RaycastHit hit)
    {
        Vector3 camPosition = Camera.main.transform.position;       // ī�޶��� ��ġ.
        Vector3 camForward = Camera.main.transform.forward;         // ī�޶��� ���� ����.

        // ��� ��Ʈ�� 1�� ä�� �ڿ� Player�� ����ϴ� ��Ʈ�� ���� ����.
        // ^(XOR) �� ���� ������ 1 �ٸ��� 0�̴�.
        int layer = int.MaxValue;
        layer ^= (1 << LayerMask.NameToLayer("Player"));
        layer ^= (1 << LayerMask.NameToLayer("Ignore Raycast"));

        return Physics.Raycast(camPosition, camForward, out hit, 100f, layer);
    }

    public GameObject GetCameraHitTarget()
    {
        RaycastHit hit;
        if(RaycastCamera(out hit))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
    public Vector3 GetCameraPoint()
    {
        RaycastHit hit;
        if(RaycastCamera(out hit))
        {
            return hit.point;
        }
        else
        {
            Vector3 camPosition = Camera.main.transform.position;       // ī�޶��� ��ġ.
            Vector3 camForward = Camera.main.transform.forward;         // ī�޶��� ���� ����.
            return camPosition + camForward * 100f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * 100f);
    }
}
