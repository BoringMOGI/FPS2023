using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOUSE_STATE
{
    None,           // 아무것도 아님.
    Down,           // 누른 순간.
    Pressed,        // 누르고 있을 때.
    Realease,       // 땠을 때.
}

public class InputHandler : MonoBehaviour
{
    // 단축키 목록
    // 이동 : 상,하,좌,우
    // 발사 : 마우스 좌측
    // 확대(줌) : 마우스 우측
    // 점프 : 스페이스
    // 달리기 : Shift

    KeyCode moveUp;
    KeyCode moveDown;
    KeyCode moveLeft;
    KeyCode moveRight;
    KeyCode fire;
    KeyCode zoom;
    KeyCode jump;
    KeyCode run;

    KeyCode changeWeapon1;
    KeyCode changeWeapon2;
    KeyCode changeWeapon3;

    private void Awake()
    {
        // PlayerPrefs : 윈도우에 레지스트리 값으로 기본 자료형을 저장
        // GetInt(string key, int defaultValue) : int
        moveUp = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_UP", (int)KeyCode.W);
        moveDown = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_DOWN", (int)KeyCode.S);
        moveLeft = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_LEFT", (int)KeyCode.A);
        moveRight = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_RIGHT", (int)KeyCode.D);

        fire = (KeyCode)PlayerPrefs.GetInt("KEY_FIRE", (int)KeyCode.Mouse0);
        zoom = (KeyCode)PlayerPrefs.GetInt("KEY_ZOOM", (int)KeyCode.Mouse1);
        jump = (KeyCode)PlayerPrefs.GetInt("KEY_JUMP", (int)KeyCode.Space);
        run = (KeyCode)PlayerPrefs.GetInt("KEY_RUN", (int)KeyCode.LeftShift);

        changeWeapon1 = (KeyCode)PlayerPrefs.GetInt("KEY_CHANGE_WEAPON_1", (int)KeyCode.Alpha1);
        changeWeapon2 = (KeyCode)PlayerPrefs.GetInt("KEY_CHANGE_WEAPON_2", (int)KeyCode.Alpha2);
        changeWeapon3 = (KeyCode)PlayerPrefs.GetInt("KEY_CHANGE_WEAPON_3", (int)KeyCode.Alpha3);

        // 마우스 커서 가리기 & 고정.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool isLock;
    public Vector3 moveDirection;
    public Vector2 mouseMove;
    public bool isJump;
    public bool isRun;
    public MOUSE_STATE mouseFire;
    public MOUSE_STATE mouseZoom;
    public int changeWeaponIndex;


    // 만약에 설정창에서 키보드에 단축키에 대한 교체가 있다.
    private void Update()
    {
        moveDirection = Vector3.zero;
        mouseMove = Vector2.zero;
        isJump = false;
        isRun = false;
        mouseFire = MOUSE_STATE.None;
        mouseZoom = MOUSE_STATE.None;

        if (!isLock)
        {
            if (Input.GetKey(moveLeft))
                moveDirection.x -= 1;
            if (Input.GetKey(moveRight))
                moveDirection.x += 1;

            if (Input.GetKey(moveUp))
                moveDirection.z += 1;
            else if (Input.GetKey(moveDown))
                moveDirection.z -= 1;

            mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            isJump = Input.GetKey(jump);
            isRun = Input.GetKey(run);

            // 좌측 키(발사)
            if (Input.GetKeyDown(fire))
                mouseFire = MOUSE_STATE.Down;
            else if (Input.GetKey(fire))
                mouseFire = MOUSE_STATE.Pressed;
            else if (Input.GetKeyUp(fire))
                mouseFire = MOUSE_STATE.Realease;

            // 우측 키(줌)
            if (Input.GetKeyDown(zoom))
                mouseZoom = MOUSE_STATE.Down;
            else if (Input.GetKey(zoom))
                mouseZoom = MOUSE_STATE.Pressed;
            else if (Input.GetKeyUp(zoom))
                mouseZoom = MOUSE_STATE.Realease;

            // 무기 변경 키.
            if (Input.GetKeyDown(changeWeapon1))
                changeWeaponIndex = 1;
            else if (Input.GetKeyDown(changeWeapon2))
                changeWeaponIndex = 2;
            else if (Input.GetKeyDown(changeWeapon3))
                changeWeaponIndex = 3;
            else
                changeWeaponIndex = 0;

        }

        moveDirection.Normalize();
    }
}
