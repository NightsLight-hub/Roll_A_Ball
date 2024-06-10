using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerCtl : MonoBehaviour
{
    // player speed
    public float moveSpeed = 2f;
    public float rotateSpeed = 0.2f;

    // player rigidbody
    //private Rigidbody rb;
    private Vector3 originPosition;
    private float inputMoveX, inputMoveY;
    private Vector2 inputLook;
    private CharacterController cc;
    private Vector3 lastMoveForward;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //transform.forward = new Vector3(0, 0, -1);
        originPosition = transform.position;
        cc = GetComponent<CharacterController>();
        //cc = rb.GetComponent<CharacterController>();
    }

    void OnCtrl()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Confined : CursorLockMode.Locked;
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

    private void LateUpdate()
    {
        rotate();
    }

    private void move()
    {
        if (inputMoveX == 0 && inputMoveY == 0)
        {
            return;
        }

        // get player forward
        Vector3 forward = new Vector3(inputMoveY, 0, -inputMoveX);
        forward = transform.TransformDirection(forward);
        lastMoveForward = forward * Time.fixedDeltaTime * moveSpeed;
        // cc move
        cc.Move(lastMoveForward);
        //Debug.LogFormat("after move, position is {0}", transform.position);

        // 限制角色在特定区域
        Vector3 newPosition = transform.position;
        newPosition.z = Mathf.Clamp(newPosition.z, -10, 10);
        transform.position = newPosition;
        //Debug.LogFormat("after correct, position is {0}", transform.position);


        //// 朝向方向前进 moveY， 水平走moveX
        //Vector3 movement = forward * inputMoveY + transform.right * inputMoveX;
        //Vector3 target = transform.position + movement * moveSpeed * Time.fixedDeltaTime;

        //// change player rigidbody position
        //rb.MovePosition(target);
        //Debug.LogFormat("Player move to target {0}", target);
    }



    private void rotate()
    {
        if (inputLook == Vector2.zero)
        {
            return;
        }

        transform.Rotate(0, 0, inputLook.y * rotateSpeed, Space.Self);
        transform.Rotate(0, inputLook.x * rotateSpeed, 0, Space.World);
        Vector3 eular = Quaternion.LookRotation(transform.forward).eulerAngles;
        //Debug.LogFormat("eular {0}", eular);
        if (eular.x > 45 && eular.x < 315)
        {
            // 限制上下旋转角度
            transform.Rotate(inputLook.y * rotateSpeed, 0, 0, Space.Self);
        }
    }
}
