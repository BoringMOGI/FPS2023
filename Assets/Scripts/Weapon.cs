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

    public Vector3 GetCameraPoint()
    {
        Vector3 camPosition = Camera.main.transform.position;       // 카메라의 위치.
        Vector3 camForward = Camera.main.transform.forward;         // 카메라의 정면 방향.

        // 모든 비트를 1로 채운 뒤에 Player를 담당하는 비트의 값만 뺀다.
        // ^(XOR) 두 값이 같으면 1 다르면 0이다.
        int layer = int.MaxValue ^ (1 << LayerMask.NameToLayer("Player"));

        RaycastHit hit;                                           
        Vector3 point;
        if (Physics.Raycast(camPosition, camForward, out hit, 100f, layer))     // 레이 발사.
        {
            // 카메라의 정면으로 레이를 발사했을 때 충돌한 위치.
            point = hit.point;
        }
        else
        {
            // 카메라 정면으로 100만큼 움직인 위치.
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
