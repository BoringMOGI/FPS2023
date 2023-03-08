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
    [SerializeField] protected WeaponUI weaponUi;               // 총기 UI.

    [Header("Info")]
    [SerializeField] protected float power;
    [SerializeField] protected float speed;
    [SerializeField] protected LayerMask mask;

    // abstract함수 : 선언문만 존재하는 함수.
    public abstract void Press(MOUSE mouse);            // 마우스를 눌렀을때.
    public abstract void Release(MOUSE mouse);          // 마우스를 놓았을때.

    // 1. 카메라가 바라보는 정면 라인에서 충돌한 위치
    // 2. 카메라가 바라보는 정면 라인에서 충돌한 오브젝트.

    private bool RaycastCamera(out RaycastHit hit)
    {
        Vector3 camPosition = Camera.main.transform.position;       // 카메라의 위치.
        Vector3 camForward = Camera.main.transform.forward;         // 카메라의 정면 방향.

        // 모든 비트를 1로 채운 뒤에 Player를 담당하는 비트의 값만 뺀다.
        // ^(XOR) 두 값이 같으면 1 다르면 0이다.
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
            Vector3 camPosition = Camera.main.transform.position;       // 카메라의 위치.
            Vector3 camForward = Camera.main.transform.forward;         // 카메라의 정면 방향.
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
