using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputHandler������Ʈ�� �־���Ѵٶ�� ����.
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    InputHandler input;

    void Start()
    {
        input = GetComponent<InputHandler>();
    }

    // Movement�� Update���� �������� ���� ���� ó���ϱ� ����
    // LateUpdate ���.
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
