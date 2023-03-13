using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputHandler������Ʈ�� �־���Ѵٶ�� ����.
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    public enum WEAPON
    {
        Blaster,
        Launcher,
    }

    [SerializeField] Weapon[] weapons;

    Weapon currentWeapon => weapons[(int)equipWeapon];

    WEAPON equipWeapon;
    InputHandler input;

    void Start()
    {
        input = GetComponent<InputHandler>();

        // ���۽� ��� ���⸦ �����Ѵ�.
        foreach (var weapon in weapons)
            weapon.UnEquip();

        // ���ʿ� �����͸� ����Ѵ�.
        ChangeWeapon(WEAPON.Blaster, true);
    }

    // Movement�� Update���� �������� ���� ���� ó���ϱ� ����
    // LateUpdate ���.
    void LateUpdate()
    {
        switch(input.mouseFire)
        {
            case MOUSE_STATE.Pressed:
                currentWeapon.Press(Weapon.MOUSE.Left);
                break;
            case MOUSE_STATE.Realease:
                currentWeapon.Release(Weapon.MOUSE.Left);
                break;
        }
        switch(input.mouseZoom)
        {
            case MOUSE_STATE.Pressed:
                currentWeapon.Press(Weapon.MOUSE.Right);
                break;
            case MOUSE_STATE.Realease:
                currentWeapon.Release(Weapon.MOUSE.Right);
                break;
        }

        switch(input.changeWeaponIndex)
        {
            case 1:
                ChangeWeapon((WEAPON)0);
                break;
            case 2:
                ChangeWeapon((WEAPON)1);
                break;
        }
    }

    private void ChangeWeapon(WEAPON type, bool isForce = false)
    {
        Weapon changeWeapon = weapons[(int)type];           // �ٲ� ������ ����.
        if(changeWeapon != currentWeapon || isForce)
        {
            currentWeapon.UnEquip();        // ���� ���� ����.
            equipWeapon = type;             // Ÿ�� ����.
            currentWeapon.Equip();          // ���� ���� ����.
        }
    }
}
