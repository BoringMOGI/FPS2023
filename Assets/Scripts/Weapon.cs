using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [System.Serializable]
    protected class EnergySocket
    {
        [SerializeField] Transform socket;

        float totalEnergy;
        float energy;

        public float Energy => energy;

        public void Start(float _energy)
        {
            totalEnergy = _energy;
            energy = _energy;
        }

        public void Update()
        {
            // 에너지가 몇인가에 따라 소캣의 y축을 움직인다.
            Vector3 local = socket.localPosition;
            local.y = (1f - (energy / totalEnergy)) * 0.1f;
            socket.localPosition = local;
        }

        // 에너지를 사용하겠다.
        public bool Use()
        {
            // 에너지가 없다면 false를 반환.
            if (energy <= 0.0f)
                return false;

            energy = Mathf.Clamp(energy - Time.deltaTime, 0f, totalEnergy);
            return true;
        }
        public bool Charge()
        {
            // 에너지가 가득 찼다면 false를 반환.
            if (energy >= totalEnergy)
                return false;

            // 에너지가 몇이건 1초에 걸쳐서 회복하고 싶기 때문에
            // 만약 에너지가 N라면 시간 값에 N을 곱한다.
            energy = Mathf.Clamp(energy + Time.deltaTime * totalEnergy, 0f, totalEnergy);
            return true;
        }
    }

    public enum MOUSE
    {
        Left,
        Right,
    }

    [Header("Weapon")]
    [SerializeField] protected EnergySocket[] sockets;          // 피스톤 소캣 배열.
    [SerializeField] protected GameObject weaponObject;         // 총기 오브젝트.
    [SerializeField] protected Transform muzzle;                // 총구.
    [SerializeField] protected Projectile projectilePrefab;     // 투사체 프리팹.

    [Header("UI")]
    [SerializeField] protected Crosshair crossHair;             // 크로스 헤어.
    [SerializeField] protected WeaponUI weaponUi;               // 총기 UI.

    [Header("Info")]
    [SerializeField] protected float power;
    [SerializeField] protected float speed;
    [SerializeField] protected float MAX_ENERGY;
    [SerializeField] protected LayerMask mask;

    protected float nextRecoveryTime = 0f;   // 충전 가능 시간.
    private bool isEquip = false;            // 장비 중인 무기인가?

    protected void Start()
    {
        // 총 에너지 양을 n등분하여 소켓에게 전달.
        foreach (var socket in sockets)
            socket.Start(MAX_ENERGY / sockets.Length);
    }
    protected void Update()
    {
        UpdateCrosshair();

        // 무기 UI에게 현재 에너지와 총 에너지를 전달한다.
        float currentEnergy = sockets.Select(s => s.Energy).Sum();
        weaponUi.UpdateFill(currentEnergy, MAX_ENERGY);

        // 소켓별 Update함수 호출.
        foreach (var socket in sockets)
            socket.Update();

        // 소켓별 에너지 회복 로직.
        if(nextRecoveryTime <= Time.time)
        {
            // 마지막 소켓부터 회복을 시킨다. (거꾸로)
            foreach(var socket in sockets.Reverse())
            {
                // Charge:bool
                // => 에너지가 꽉 차있다면 false 반환.
                //    에너지를 회복했다면 true 반환.
                if (socket.Charge())
                    break;
            }
        }
    }

    public void Equip()
    {
        crossHair.gameObject.SetActive(true);
        weaponUi.SwitchSelected(true);
        weaponObject.SetActive(true);
        isEquip = true;
    }
    public void UnEquip() 
    {
        crossHair.gameObject.SetActive(false);
        weaponUi.SwitchSelected(false);
        weaponObject.SetActive(false);
        isEquip = false;
    }

    protected void UpdateCrosshair()
    {
        if (!isEquip)
            return;

        GameObject hitTarget = GetCameraHitTarget();    // 카메라 정면 타겟 대입.
        if (hitTarget?.GetComponent<IHit>() != null)    // 타겟이 있고 해당 타겟이 IHit을 구현했다면?
            crossHair.Switch(true);
        else
            crossHair.Switch(false);
    }
    protected bool UseEnergy()
    {
        foreach (var socket in sockets)
        {
            // 해당 소켓의 에너지를 사용했다면 true.
            if (socket.Use())
                return true;
        }

        return false;
    }

    public virtual void Press(MOUSE mouse)
    {
        nextRecoveryTime = Time.time + 1f;
    }
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
}
