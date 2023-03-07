using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputHandler컴포넌트가 있어야한다라고 제한.
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    InputHandler input;

    void Start()
    {
        input = GetComponent<InputHandler>();
    }

    // Movement의 Update에서 움직임이 끝난 이후 처리하기 위해
    // LateUpdate 사용.
    void LateUpdate()
    {
        switch(input.mouseFire)
        {
            case MOUSE_STATE.Pressed:
                weapon.Press(Weapon.MOUSE.Left);
                break;
            case MOUSE_STATE.Realease:
                weapon.Release(Weapon.MOUSE.Left);
                break;
        }

        switch(input.mouseZoom)
        {
            case MOUSE_STATE.Pressed:
                weapon.Press(Weapon.MOUSE.Right);
                break;
            case MOUSE_STATE.Realease:
                weapon.Release(Weapon.MOUSE.Right);
                break;
        }
    }
}
