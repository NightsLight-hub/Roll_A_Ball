using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtl : MonoBehaviour
{
    // player speed
    public float moveSpeed = 2f;
    public float rotateSpeed = 0.2f;

    // player rigidbody
    private Rigidbody rb;
    private float inputMoveX, inputMoveY;
    private Vector2 inputLook;
    private bool hideCurosr = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.forward = new Vector3(-1, 0, 0);
    }

    void OnCtrl()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    void OnFire()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnLook(InputValue lookValue)
    {
        inputLook = lookValue.Get<Vector2>();
        //Debug.LogFormat("lookValue {0}", inputLook);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movement = movementValue.Get<Vector2>();
        inputMoveX = movement.x;
        inputMoveY = movement.y;
        //Debug.LogFormat("x {0}, y {1}", inputMoveX, inputMoveY);
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        // get player forward
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        // 朝向方向前进 moveY， 水平走moveX
        Vector3 movement = forward * inputMoveY + transform.right * inputMoveX;

        Vector3 target = transform.position + movement * moveSpeed * Time.deltaTime;
        //Debug.LogFormat("target {0}", target);
        // change player rigidbody position
        rb.MovePosition(target);
    }

    private void rotate()
    {
        transform.Rotate(-inputLook.y * rotateSpeed, 0, 0, Space.Self);
        transform.Rotate(0, inputLook.x * rotateSpeed, 0, Space.World);
        Vector3 eular = Quaternion.LookRotation(transform.forward).eulerAngles;
        Debug.LogFormat("eular {0}", eular);
        if (eular.x > 45 && eular.x < 315)
        {
            // 限制上下旋转角度
            transform.Rotate(inputLook.y * rotateSpeed, 0, 0, Space.Self);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rotate();
    }
}
