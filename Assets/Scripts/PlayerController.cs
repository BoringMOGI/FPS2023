using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputHandler컴포넌트가 있어야한다라고 제한.
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

        // 시작시 모든 무기를 해제한다.
        foreach (var weapon in weapons)
            weapon.UnEquip();

        // 최초에 블래스터를 장비한다.
        ChangeWeapon(WEAPON.Blaster, true);
    }

    // Movement의 Update에서 움직임이 끝난 이후 처리하기 위해
    // LateUpdate 사용.
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
        Weapon changeWeapon = weapons[(int)type];           // 바뀔 예정인 무기.
        if(changeWeapon != currentWeapon || isForce)
        {
            currentWeapon.UnEquip();        // 이전 무기 해제.
            equipWeapon = type;             // 타입 변경.
            currentWeapon.Equip();          // 현재 무기 장착.
        }
    }
}
