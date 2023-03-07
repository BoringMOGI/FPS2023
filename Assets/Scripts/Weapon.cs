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

    public Vector3 GetCameraPoint()
    {
        Vector3 camPosition = Camera.main.transform.position;       // ī�޶��� ��ġ.
        Vector3 camForward = Camera.main.transform.forward;         // ī�޶��� ���� ����.

        // ��� ��Ʈ�� 1�� ä�� �ڿ� Player�� ����ϴ� ��Ʈ�� ���� ����.
        // ^(XOR) �� ���� ������ 1 �ٸ��� 0�̴�.
        int layer = int.MaxValue ^ (1 << LayerMask.NameToLayer("Player"));

        RaycastHit hit;                                           
        Vector3 point;
        if (Physics.Raycast(camPosition, camForward, out hit, 100f, layer))     // ���� �߻�.
        {
            // ī�޶��� �������� ���̸� �߻����� �� �浹�� ��ġ.
            point = hit.point;
        }
        else
        {
            // ī�޶� �������� 100��ŭ ������ ��ġ.
            point = camPosition + camForward * 100f;
        }

        return point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * 100f);
    }
}
