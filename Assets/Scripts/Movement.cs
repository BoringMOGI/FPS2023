using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputHandler컴포넌트가 있어야한다라고 제한.
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
        // 기본 움직임.
        Vector3 direction = (transform.right * input.moveDirection.x + transform.forward * input.moveDirection.z).normalized;
        controller.Move(direction * moveSpeed * Time.deltaTime);

        // 시점 좌우 회전.
        transform.Rotate(Vector3.up * input.mouseMove.x);

        // 시점 상하 회전.
        // = 사람이 알고있는 (x,y,z)회적축을 가지는 오일러 각도를 Quaternion으로 변환.
        rotateY = Mathf.Clamp(rotateY + input.mouseMove.y, -70, 70);
        cam.localRotation = Quaternion.Euler(rotateY, 0, 0);
    }
}
