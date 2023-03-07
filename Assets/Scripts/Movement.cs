using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputHandler������Ʈ�� �־���Ѵٶ�� ����.
[RequireComponent(typeof(InputHandler))]
public class Movement : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] float moveSpeed;

    InputHandler input;
    CharacterController controller;
    float rotateY;

    private void Start()
    {
        input = GetComponent<InputHandler>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // �⺻ ������.
        Vector3 direction = (transform.right * input.moveDirection.x + transform.forward * input.moveDirection.z).normalized;
        controller.Move(direction * moveSpeed * Time.deltaTime);

        // ���� �¿� ȸ��.
        transform.Rotate(Vector3.up * input.mouseMove.x);

        // ���� ���� ȸ��.
        // = ����� �˰��ִ� (x,y,z)ȸ������ ������ ���Ϸ� ������ Quaternion���� ��ȯ.
        rotateY = Mathf.Clamp(rotateY + input.mouseMove.y, -70, 70);
        cam.localRotation = Quaternion.Euler(rotateY, 0, 0);
    }
}
