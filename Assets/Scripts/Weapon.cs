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

            // �������� ���̰� 1�ʿ� ���ļ� ȸ���ϰ� �ͱ� ������
            // ���� �������� N��� �ð� ���� N�� ���Ѵ�.
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
    [SerializeField] protected EnergySocket[] sockets;          // �ǽ��� ��Ĺ �迭.
    [SerializeField] protected GameObject weaponObject;         // �ѱ� ������Ʈ.
    [SerializeField] protected Transform muzzle;                // �ѱ�.
    [SerializeField] protected Projectile projectilePrefab;     // ����ü ������.

    [Header("UI")]
    [SerializeField] protected Crosshair crossHair;             // ũ�ν� ���.
    [SerializeField] protected WeaponUI weaponUi;               // �ѱ� UI.

    [Header("Info")]
    [SerializeField] protected float power;
    [SerializeField] protected float speed;
    [SerializeField] protected float MAX_ENERGY;
    [SerializeField] protected LayerMask mask;

    protected float nextRecoveryTime = 0f;   // ���� ���� �ð�.
    private bool isEquip = false;            // ��� ���� �����ΰ�?

    protected void Start()
    {
        // �� ������ ���� n����Ͽ� ���Ͽ��� ����.
        foreach (var socket in sockets)
            socket.Start(MAX_ENERGY / sockets.Length);
    }
    protected void Update()
    {
        UpdateCrosshair();

        // ���� UI���� ���� �������� �� �������� �����Ѵ�.
        float currentEnergy = sockets.Select(s => s.Energy).Sum();
        weaponUi.UpdateFill(currentEnergy, MAX_ENERGY);

        // ���Ϻ� Update�Լ� ȣ��.
        foreach (var socket in sockets)
            socket.Update();

        // ���Ϻ� ������ ȸ�� ����.
        if(nextRecoveryTime <= Time.time)
        {
            // ������ ���Ϻ��� ȸ���� ��Ų��. (�Ųٷ�)
            foreach(var socket in sockets.Reverse())
            {
                // Charge:bool
                // => �������� �� ���ִٸ� false ��ȯ.
                //    �������� ȸ���ߴٸ� true ��ȯ.
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

        GameObject hitTarget = GetCameraHitTarget();    // ī�޶� ���� Ÿ�� ����.
        if (hitTarget?.GetComponent<IHit>() != null)    // Ÿ���� �ְ� �ش� Ÿ���� IHit�� �����ߴٸ�?
            crossHair.Switch(true);
        else
            crossHair.Switch(false);
    }
    protected bool UseEnergy()
    {
        foreach (var socket in sockets)
        {
            // �ش� ������ �������� ����ߴٸ� true.
            if (socket.Use())
                return true;
        }

        return false;
    }

    public virtual void Press(MOUSE mouse)
    {
        nextRecoveryTime = Time.time + 1f;
    }
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
}
