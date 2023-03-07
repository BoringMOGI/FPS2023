using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOUSE_STATE
{
    None,           // �ƹ��͵� �ƴ�.
    Down,           // ���� ����.
    Pressed,        // ������ ���� ��.
    Realease,       // ���� ��.
}

public class InputHandler : MonoBehaviour
{
    // ����Ű ���
    // �̵� : ��,��,��,��
    // �߻� : ���콺 ����
    // Ȯ��(��) : ���콺 ����
    // ���� : �����̽�
    // �޸��� : Shift

    KeyCode moveUp;
    KeyCode moveDown;
    KeyCode moveLeft;
    KeyCode moveRight;
    KeyCode fire;
    KeyCode zoom;
    KeyCode jump;
    KeyCode run;

    private void Awake()
    {
        // PlayerPrefs : �����쿡 ������Ʈ�� ������ �⺻ �ڷ����� ����
        // GetInt(string key, int defaultValue) : int
        moveUp = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_UP", (int)KeyCode.W);
        moveDown = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_DOWN", (int)KeyCode.S);
        moveLeft = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_LEFT", (int)KeyCode.A);
        moveRight = (KeyCode)PlayerPrefs.GetInt("KEY_MOVE_RIGHT", (int)KeyCode.D);

        fire = (KeyCode)PlayerPrefs.GetInt("KEY_FIRE", (int)KeyCode.Mouse0);
        zoom = (KeyCode)PlayerPrefs.GetInt("KEY_ZOOM", (int)KeyCode.Mouse1);
        jump = (KeyCode)PlayerPrefs.GetInt("KEY_JUMP", (int)KeyCode.Space);
        run = (KeyCode)PlayerPrefs.GetInt("KEY_RUN", (int)KeyCode.LeftShift);

        // ���콺 Ŀ�� ������ & ����.
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


    // ���࿡ ����â���� Ű���忡 ����Ű�� ���� ��ü�� �ִ�.
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

            // ���� Ű(�߻�)
            if (Input.GetKeyDown(fire))
                mouseFire = MOUSE_STATE.Down;
            else if (Input.GetKey(fire))
                mouseFire = MOUSE_STATE.Pressed;
            else if (Input.GetKeyUp(fire))
                mouseFire = MOUSE_STATE.Realease;

            // ���� Ű(��)
            if (Input.GetKeyDown(zoom))
                mouseZoom = MOUSE_STATE.Down;
            else if (Input.GetKey(zoom))
                mouseZoom = MOUSE_STATE.Pressed;
            else if (Input.GetKeyUp(zoom))
                mouseZoom = MOUSE_STATE.Realease;
        }

        moveDirection.Normalize();
    }
}
